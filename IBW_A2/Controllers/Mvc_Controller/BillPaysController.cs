using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBW.Data;
using IBW.Model;
using Microsoft.AspNetCore.Http;
using IBW.Utilities;

namespace IBW.Controllers
{
    public class BillPaysController : Controller
    {
        private readonly IBWContext _context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        
        public BillPaysController(IBWContext context)
        {
            _context = context;
        }
        // GET: BillPays
        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customers.Include(x => x.Accounts).
             FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

            var iBWContext = (from b in _context.BillPays
                             join c in _context.Accounts
                             on b.AccountNumber equals c.AccountNumber
                             where c.CustomerID == CustomerID
                             select b).Include(x=>x.payee);

            return View(await iBWContext.ToListAsync());
        }

        // GET: BillPays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPay = await _context.BillPays
                .Include(b => b.Account)
                .Include(b => b.payee)
                .FirstOrDefaultAsync(m => m.BillPayID == id);
            if (billPay == null)
            {
                return NotFound();
            }

            return View(billPay);
        }

        // GET: BillPays/Create
        public IActionResult Create()
        {
            List<Period> periods = Enum.GetValues(typeof(Period)).Cast<Period>().ToList();        
            // set transaction view bag
            ViewBag.Periods = new SelectList(periods);
            ViewData["AccountNumber"] = new SelectList(_context.Accounts.Where(x => x.CustomerID ==  CustomerID), "AccountNumber", "AccountNumber");
            ViewData["PayeeID"] = new SelectList(_context.Payees, "PayeeID", "PayeeName");
            return View();
        }

        // POST: BillPays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillPayID,AccountNumber,PayeeID,Amount,ScheduleDate,Period")] BillPay billPay)
        {
            if (MiscellaneousExtensionUtilities.HasMoreThanTwoDecimalPlaces(billPay.Amount))
                ModelState.AddModelError(nameof(Transaction.Amount), "your should only have 2 decimal places in your amount.");
            if (ModelState.IsValid)
            {
                billPay.ScheduleDate = TimeZoneInfo.ConvertTimeToUtc(billPay.ScheduleDate);
                _context.Add(billPay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountNumber"] = new SelectList(_context.Accounts, "AccountNumber", "AccountNumber", billPay.AccountNumber);
            ViewData["PayeeID"] = new SelectList(_context.Payees, "PayeeID", "PayeeName", billPay.PayeeID);
            return View();
        }

        // GET: BillPays/Edit/5
        public async Task<IActionResult> Modify(int? id)
        {
            if (id == null)
                return NotFound();
            
            var billPay = await _context.BillPays.FindAsync(id);
            if (billPay.Block)
            {
                HttpContext.Session.SetInt32("Block", 1);
            }

            else 
            {
                HttpContext.Session.SetInt32("Block", 0);
            }
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            billPay.ScheduleDate = TimeZoneInfo.ConvertTimeFromUtc(billPay.ScheduleDate, cstZone);
            if (billPay == null)
            {
                return NotFound();
            }
            List<Period> periods = Enum.GetValues(typeof(Period)).Cast<Period>().ToList();
            // set transaction view bag
            ViewBag.Periods = new SelectList(periods);
            ViewData["AccountNumber"] = new SelectList(_context.Accounts.Where(x => x.CustomerID == CustomerID), "AccountNumber", "AccountNumber", billPay.AccountNumber);
            ViewData["PayeeID"] = new SelectList(_context.Payees, "PayeeID", "PayeeName", billPay.PayeeID);
            return View(billPay);
        }

        // POST: BillPays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modify(int id, [Bind("BillPayID,AccountNumber,PayeeID,Amount,ScheduleDate,Period,Block")] BillPay billPay)
        {
            if (id != billPay.BillPayID)
                return NotFound();
            if (HttpContext.Session.GetInt32("Block").Value == 1)
                billPay.Block = true;
            
            if (MiscellaneousExtensionUtilities.HasMoreThanTwoDecimalPlaces(billPay.Amount))
                ModelState.AddModelError(nameof(Transaction.Amount), "your should only have 2 decimal places in your amount.");

            if (ModelState.IsValid)
            {
                try
                {
                    billPay.ScheduleDate = TimeZoneInfo.ConvertTimeToUtc(billPay.ScheduleDate);
                    _context.Add(billPay);
                    _context.Update(billPay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillPayExists(billPay.BillPayID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            List<Period> periods = Enum.GetValues(typeof(Period)).Cast<Period>().ToList();
            // set transaction view bag
            ViewBag.Periods = new SelectList(periods);
            ViewData["AccountNumber"] = new SelectList(_context.Accounts, "AccountNumber", "AccountNumber", billPay.AccountNumber);
            ViewData["PayeeID"] = new SelectList(_context.Payees, "PayeeID", "PayeeName", billPay.PayeeID);
            return View(billPay);
        }
        // GET: BillPays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            
            var billPay = await _context.BillPays
                .Include(b => b.Account)
                .Include(b => b.payee)
                .FirstOrDefaultAsync(m => m.BillPayID == id);

            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            billPay.ScheduleDate = TimeZoneInfo.ConvertTimeFromUtc(billPay.ScheduleDate, cstZone);

            if (billPay == null)
            {
                return NotFound();
            }

            return View(billPay);
        }

        // POST: BillPays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billPay = await _context.BillPays.FindAsync(id);
            _context.BillPays.Remove(billPay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool BillPayExists(int id)
        {
            return _context.BillPays.Any(e => e.BillPayID == id);
        }
    }
}
