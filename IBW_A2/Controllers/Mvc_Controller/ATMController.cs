using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBW.Attributes;
using IBW.Data;
using IBW.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBW.Utilities;

namespace IBW.Controllers
{
    [Authorize]
    public class ATMController : Controller
    {
        // GET: /<controller>/
        private readonly IBWContext _context;

        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        public ATMController(IBWContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customers.Include(x => x.Accounts).
              FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            var accounts = customer.Accounts;
            ViewBag.AccountNumber = new SelectList(accounts, "AccountNumber", "AccountNumber");
            // set the account view bag to account number

            List<TransactionType> transactionTypes = Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>().ToList();
            transactionTypes.Remove(TransactionType.ServiceCharge);
            // set transaction view bag
            ViewBag.TransactionType = new SelectList(transactionTypes);
            List<Account> DesAccounts = _context.Accounts.ToList();
            DesAccounts.Insert(0, new Account
            {
                AccountNumber = null
            });
            //set distination account view bag
            ViewBag.DestinationAccountNumber = new SelectList(DesAccounts, "AccountNumber", "AccountNumber");
            return View();

        }
        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("TransactionType,AccountNumber,DestinationAccountNumber,Amount,Comment")] Transaction transaction)
        {
            var account = await _context.Accounts.FindAsync(transaction.AccountNumber);
            transaction.TransactionTimeUtc = DateTime.UtcNow;
            // validation for destination account
            if (TransferExtensionUtilities.InvalidDepo(transaction))
                ModelState.AddModelError(nameof(Transaction.DestinationAccountNumber), "when you are not transfering, the destination account number should be empty!");
            if (TransferExtensionUtilities.InvalidTransfer(transaction))
                ModelState.AddModelError(nameof(Transaction.DestinationAccountNumber), "when you transfer, your destination can't be nothing");
            if (TransferExtensionUtilities.IsSameAcc(transaction))
                ModelState.AddModelError(nameof(Transaction.DestinationAccountNumber), "Can't transfer to same account.");
            //validation for amount
            if (!TransferExtensionUtilities.checkBalance(transaction, account))
                ModelState.AddModelError(nameof(Transaction.Amount), String.Format("your balance is not enough! Balance:{0} ", account.Balance));
            if(MiscellaneousExtensionUtilities.HasMoreThanTwoDecimalPlaces(transaction.Amount))
                ModelState.AddModelError(nameof(Transaction.Amount), "your should only have 2 decimal places in your amount.");

            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                if (!transaction.TransactionType.Equals(TransactionType.Deposit))
                    _context.Add(TransactionBuilder.BuildTransaction(transaction));
                updateAmount(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction("TransactionSuccess", "ATM");
            }

            ViewData["AccountNumber"] = new SelectList(_context.Accounts, "AccountNumber", "AccountNumber", transaction.AccountNumber);
            return await Index();
        }

        public IActionResult TransactionSuccess() => View();
        
        // this is for changing the amount of the account
        public void updateAmount(Transaction transaction) 
        {
            var account = _context.Accounts.Find(transaction.AccountNumber);
            if (transaction.TransactionType.Equals(TransactionType.Deposit)) 
                account.Balance += transaction.Amount;
            if (transaction.TransactionType.Equals(TransactionType.Withdraw))
                account.Balance -= (transaction.Amount + .1m);
            if (transaction.TransactionType.Equals(TransactionType.Transfer)) 
            {
                var desAccount = _context.Accounts.Find(transaction.DestinationAccountNumber);
                account.Balance -= (transaction.Amount + .2m);
                desAccount.Balance += transaction.Amount;
                desAccount.ModifyDate = DateTime.UtcNow;
            }
            account.ModifyDate = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }
}

