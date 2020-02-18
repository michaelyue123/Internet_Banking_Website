using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IBW_A2.Controllers
{
    public class BlockController : Controller
    {
        public IActionResult Block()
        {
            return View();
        }
    }
}