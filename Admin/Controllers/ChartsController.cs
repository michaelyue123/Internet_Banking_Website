using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Admin.Controllers
{
    public class ChartsController : Controller
    {
        // GET: Charts/PieAsync
       
        public IActionResult Pie()
        {
            String chart = HttpContext.Session.GetString("selectedTransactions");
            // Deserializing the response recieved from web api and storing into a list.
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(chart);
            var TypeGroups = from transaction in transactions
                             group transaction by transaction.TransactionType;

            var pieList = new List<SimpleReportViewModel>();

            foreach (var group in TypeGroups)
            {
                pieList.Add(new SimpleReportViewModel
                {
                    DimensionOne = group.Key.ToString(),
                    Quantity = group.Count()
                });
            }

            return View(pieList);

        }

        // GET: Charts/BarAsync
        public IActionResult Bar()
        {
            String chart = HttpContext.Session.GetString("selectedTransactions");
            var lstModel = new List<SimpleReportViewModel>();
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(chart);
            var OderedTransaction = transactions.OrderBy(t => t.TransactionTimeUtc).GroupBy(t => t.TransactionTimeUtc.Date);
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            foreach (var group in OderedTransaction)
            {
                lstModel.Add(
                    new SimpleReportViewModel
                    {
                        DimensionOne = (group.Key).ToShortDateString(),
                        Quantity = group.Count()
                    });
            }

            return View(lstModel);
        }

        // GET: Charts/LineAsync
        public IActionResult Line()
        {
            String chart = HttpContext.Session.GetString("selectedTransactions");
            var lstModel = new List<DecimalViewModel>();
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(chart);
            var selectedTransactions = from transaction in transactions
                                       where (transaction.TransactionType != TransactionType.Deposit)
                                       select transaction;
            var OderedTransaction = selectedTransactions.GroupBy(t => t.TransactionTimeUtc.Date).OrderBy(t => t.Key);

            foreach (var group in OderedTransaction)
            {
                lstModel.Add(
                    new DecimalViewModel
                    {
                        DimensionOne = (group.Key).ToShortDateString(),
                        Amount = group.Sum(x => x.Amount)
                    });
            }


            return View(lstModel);
        }
    }  
}



