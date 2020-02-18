using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IBW.Data;
using IBW.Model;
using SimpleHashing;
using Microsoft.AspNetCore.Http;

// this code reference to week 7's tutorial code
namespace IBW.Controllers
{
    [Route("/IBW/SecureLogin")]
    public class LoginController : Controller
    {
       
        private readonly IBWContext _context;

        public LoginController(IBWContext context)
        {
            _context = context;
        }
        public IActionResult Login() {
            HttpContext.Session.SetInt32("Block", 0);
            return View();
        }
        // GET: Logins

        [HttpPost]
        public async Task<IActionResult> Login(string UserID, string password)
        {
            var login = await _context.Logins.Include(x => x.Customer).
            FirstOrDefaultAsync(x => x.UserID == UserID);


            if (login.Block)
            {
                ModelState.AddModelError("LoginFailed", "Your account has been locked.");
                HttpContext.Session.SetInt32("Block", 0);
                return View(new Login { UserID = UserID });
            }


            if (login == null || !PBKDF2.Verify(login.PasswordHash, password))
            {
                HttpContext.Session.SetInt32("Block", HttpContext.Session.GetInt32("Block").Value + 1);
                ModelState.AddModelError("LoginFailed", String.Format("Login failed, please try again. left attempt: {0}", 3- HttpContext.Session.GetInt32("Block").Value));
                if (HttpContext.Session.GetInt32("Block").Value == 3) {
                    login.Block = true;
                    login.ModifyDate = DateTime.UtcNow;
                    _context.Add(login);
                    _context.Update(login);
                    await _context.SaveChangesAsync();
                }
                return View(new Login { UserID = UserID });
               
            }


            // Login customer.
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.CustomerName), login.Customer.CustomerName);
            HttpContext.Session.SetString(nameof(Model.Login.UserID), UserID);

            return RedirectToAction("Index", "ATM");
        }


        // change password method
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string password1, string password2)
        {
            var userID = HttpContext.Session.GetString(nameof(Model.Login.UserID));

            var login = await _context.Logins.FirstOrDefaultAsync(x => x.UserID == userID);

            login.PasswordHash = PBKDF2.Hash(password1);

            login.ModifyDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction("Profile", "Customers", null);
        }

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logout customer.
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }
    }
}
