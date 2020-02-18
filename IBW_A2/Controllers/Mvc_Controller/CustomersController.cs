using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IBW.Data;
using IBW.Model;
using Microsoft.AspNetCore.Http;

namespace IBW.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IBWContext _context;

        public CustomersController(IBWContext context) => _context = context;


        // GET: Customer Profile
        public async Task<IActionResult> Profile()
        {
            var id = HttpContext.Session.GetInt32(nameof(Customer.CustomerID));

            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
                return NotFound();
            
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,CustomerName,Address,City,PostCode,TFN,State,Phone")] Customer customer)
        {
            if (id != customer.CustomerID)
                return NotFound();
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
                        return NotFound(); 
                    else
                        throw;         
                }
                // after all the actions have been finished, redirect back to profile page
                return RedirectToAction(nameof(Profile), new { id = customer.CustomerID });
            }
            return View(customer);
        }
        private bool CustomerExists(int id) => _context.Customers.Any(e => e.CustomerID == id);
    }
}
