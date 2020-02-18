using IBW.Data;
using IBW.Model;
using IBW.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Timer = System.Threading.Timer;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace IBW.Controllers
{
    public  class BillPayUpdater :  IHostedService
    {
        //ServiceProvider service = IServiceProvider.
        private Timer _timer;
        private readonly IServiceProvider serviceProvider;

       

        public BillPayUpdater(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
           
        }

        //DI to call Iservice
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, 0, 30000);
            _timer = new Timer(UnBlock, null, 0, 3000);
            // update bill pay every 30s
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //New Timer does not have a stop. 
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        //update bill pay
        async void DoWork(Object state)
        {
            var Context = new IBWContext(serviceProvider.CreateScope().ServiceProvider.GetRequiredService<DbContextOptions<IBWContext>>());
            await UpdateBillPay(Context);
        }

        async void UnBlock(Object state)
        { 
            var Context = new IBWContext(serviceProvider.CreateScope().ServiceProvider.GetRequiredService<DbContextOptions<IBWContext>>());
            await UpdateBlock(Context);
        }

 

        async Task UpdateBlock(IBWContext _context)
        {
            IList<Login> logins = await _context.Logins.ToListAsync();
            foreach (Login login in logins)
            {
                if (login.ModifyDate.AddSeconds(30).CompareTo(DateTime.UtcNow) <= 0)
                {
                    login.Block = false;
                    _context.Add(login);
                    _context.Update(login);
                }

                await _context.SaveChangesAsync();
            }
        }

        //bill pay method
        async Task UpdateBillPay(IBWContext _context )
        {
           
            IList<BillPay> billpays =await _context.BillPays.ToListAsync();
            foreach (BillPay billpay in billpays)
            {
                // check date
                if (billpay.ScheduleDate.CompareTo(DateTime.UtcNow)<=0 && !billpay.Block)
                {
                    Account account = _context.Accounts.Find(billpay.AccountNumber);
                    Transaction transaction = TransactionBuilder.BuildTransaction(billpay);
                    Transaction service = TransactionBuilder.BuildTransaction(transaction);
                    // check if there is enough balance
                    if (TransferExtensionUtilities.checkBalance(transaction, account))
                    {
                        var transactions = _context.Transactions;
                        transactions.Add(transaction);
                        transactions.Add(service);
                        int id = billpay.BillPayID;
                        int accId =billpay.AccountNumber;
                        await UpDateAcc(accId, billpay.Amount, _context);
                        await _context.SaveChangesAsync();
                        switch (billpay.Period)
                        {
                            case Period.M:
                                await AddMonthAsync(id, _context);
                                break;
                            case Period.Y:
                                await AddYearAsync(id, _context);
                                break;
                            case Period.Q:
                                await AddQuaterAsync(id, _context);
                                break;
                            case Period.O:
                                await DeleteOnce(id, _context);
                                break;
                        }
                    }
                }
            }
        }

        //if bill pay is once delete it after transaction
        private async Task DeleteOnce(int id, IBWContext _context)
        {
            var billpay = _context.BillPays.Find(id);
            _context.Remove(billpay);
            await _context.SaveChangesAsync();
        }

     //if it is not once change the schedule date after transaction
        private async Task AddQuaterAsync(int id, IBWContext _context)
        {
            var billpay = _context.BillPays.Find(id);
            billpay.ScheduleDate = billpay.ScheduleDate.AddMonths(3);
            _context.Add(billpay);
            _context.Update(billpay);
            await _context.SaveChangesAsync();
        }

        public async Task AddMonthAsync(int id, IBWContext _context) {
            var billpay = _context.BillPays.Find(id);
            billpay.ScheduleDate = billpay.ScheduleDate.AddMonths(1);
            _context.Add(billpay);
            _context.Update(billpay);
            await _context.SaveChangesAsync();
        }

        public async Task AddYearAsync(int id, IBWContext _context)
        {
            var billpay = _context.BillPays.Find(id);
            billpay.ScheduleDate = billpay.ScheduleDate.AddYears(1);
            _context.Add(billpay);
            _context.Update(billpay);
            await _context.SaveChangesAsync();
        }

        //update acount after the bill is paied
        public async Task UpDateAcc(int id,decimal amount, IBWContext _context)
        {
            var account = _context.Accounts.Find(id);
            account.Balance -= (amount +.2m);
            account.ModifyDate = DateTime.UtcNow;
            _context.Add(account);
            _context.Update(account);
            await _context.SaveChangesAsync();
        }    
    }
}
