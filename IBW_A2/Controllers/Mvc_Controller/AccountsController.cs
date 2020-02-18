using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBW.Data;
using IBW.Model;
using X.PagedList;
using Microsoft.AspNetCore.Http;

namespace IBW.Controllers
{

    public class AccountsController : Controller
    {
        private readonly IBWContext _context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        private const string AccountTypeSessionKey = "_AccountTypeSessionKey";
        //session key to restore user information.
        public AccountsController(IBWContext context) => _context = context;

        //AccountType accountType = AccountType.Saving;

        //GET: AccountType
        public IActionResult Index()
        {
            var accountType = Enum.GetValues(typeof(AccountType)).Cast<AccountType>().ToList();
            ViewBag.AccountType = new SelectList(accountType);
            //make user choose from account types
            return View();
        }
        [HttpPost]
        public IActionResult Index(AccountType type)
        {
            // Selects only accounts with transactions using Raw SQL.   
            HttpContext.Session.SetInt32(AccountTypeSessionKey, (int)type);
            //accountType = type;
            return RedirectToAction(nameof(Statement));
        }

        public async Task<IActionResult> Statement(int? page = 1)
        {
            var customer = await _context.Customers.Include(x => x.Accounts).FirstOrDefaultAsync<Customer>(x => x.CustomerID == CustomerID);

            var type = (AccountType)HttpContext.Session.GetInt32(AccountTypeSessionKey);

            ViewBag.Customer = customer;

            // Page the transaction maximum of 4 per page.
            const int pageSize = 4;
            var pagedList = await (from b in _context.Accounts
                                   join t in _context.Transactions
                                   on b.AccountNumber equals t.AccountNumber
                                   where b.CustomerID == CustomerID && b.AccountType == type
                                   select t
                ).
                ToPagedListAsync(page, pageSize);

            return View(pagedList);
        }
    }
}