using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Admin.Models;
using Admin.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace Admin.Controllers
{
    
    public class BillPaysController : Controller
    {
        [Route("BillPays/List")]
        public async Task<IActionResult> Index(string searchString)
            {
                var response = await BankApi.InitializeClient().GetAsync("api/BillPays");

             /*   if (!response.IsSuccessStatusCode)
                    throw new Exception();*/

                //Storing the response details recieved from web api.
                var result = response.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into a list.
                var BillPays = JsonConvert.DeserializeObject<List<BillPay>>(result);

               
                return View(BillPays.ToList());
            }

        [Route("BillPays/Reverse")]
        public async Task<IActionResult> Reverse(int id)
        {
            var response = await BankApi.InitializeClient().GetAsync($"api/BillPays/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var billPay = JsonConvert.DeserializeObject<BillPay>(result);
            billPay.Block = !billPay.Block;            
            var content = new StringContent(JsonConvert.SerializeObject(billPay), Encoding.UTF8, "application/json");
            response = BankApi.InitializeClient().PutAsync("api/BillPays", content).Result;
            return RedirectToAction(nameof(Index));

        }

    }
}