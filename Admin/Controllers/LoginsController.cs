using Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Admin.Controllers
{
    public class LoginsController : Controller
    {
        // GET: Logins/Index
        public IActionResult Login() => View();

        [Route("Logins/Login")]
        [HttpPost]
        public IActionResult Login(string userID, string password)
        {
            if (userID == null || password == null)
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View();
            }

            if (userID != "admin" || password != "admin")
            {
                ModelState.AddModelError("LoginFailed", "Invalid userID or password!");
                return View();
            }

            HttpContext.Session.SetString(nameof(Models.Login.UserID), userID);
            HttpContext.Session.SetString(nameof(Models.Login.PasswordHash), password);

            return RedirectToAction("Index", "Home");
        }

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logout customer.
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}