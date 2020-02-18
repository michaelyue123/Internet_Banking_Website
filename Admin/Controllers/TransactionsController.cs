using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Models;
using Admin.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Admin.Controllers
{

    public class TransactionsController : Controller
    {
        String sessionKey = "selectedTransactions";
        // GET: Transactions/ViewTransaction
         
        public async Task<IActionResult> ViewTransaction()
        {
            var id = TempData["customerId"];
            DateTime date1 = DateTime.Parse(HttpContext.Session.GetString("Date1")) ;
            DateTime date2 = DateTime.Parse(HttpContext.Session.GetString("Date2"));
            string s = string.Format("api/Transactions/{0}", id);

            var response = await BankApi.InitializeClient().GetAsync(s);

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(result);
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            foreach (Transaction transaction in transactions)
            { 
                transaction.TransactionTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(transaction.TransactionTimeUtc, cstZone);
            }

            var selectedTransactions = from transaction in transactions
                                       where transaction.TransactionTimeUtc.CompareTo(date1) >= 0 && transaction.TransactionTimeUtc.CompareTo(date2) <= 0
                                       select transaction;
            String jsonString  = JsonConvert.SerializeObject(selectedTransactions);
            HttpContext.Session.SetString(sessionKey, jsonString);
           



            return View(selectedTransactions);
        }
    }
}