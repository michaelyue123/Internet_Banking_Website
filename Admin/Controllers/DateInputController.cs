using Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Admin.Controllers
{
    public class DateInputController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: DateInput/Create
        public IActionResult DateInput(int id) {

            TempData["customerId"] = id;
            return View();       
                }
  

        [HttpPost]
        public IActionResult DateInput([Bind("Date1, Date2")] DateInput dateInput)
        {
         
            if(dateInput.Date1.CompareTo(dateInput.Date2) >= 0)
            {
                ModelState.AddModelError(nameof(dateInput.Date1),"DateTime entered is incorrect!,The second date needs to happen after the first date!");
                return View(new DateInput());
            }

            HttpContext.Session.SetString("Date1",dateInput.Date1.ToString());
            HttpContext.Session.SetString("Date2", dateInput.Date2.ToString());


            return RedirectToAction("ViewTransaction", "Transactions");
        }

    }
}