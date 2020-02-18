using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Admin.Web.Helper;
using Admin.Models;

namespace IBW.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers/Index
        [Route("Customers/List")]
        public async Task<IActionResult> Index(string searchString)
        {
            var response = await BankApi.InitializeClient().GetAsync("api/Customers");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            //Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            //Deserializing the response recieved from web api and storing into a list.
            var customers = JsonConvert.DeserializeObject<List<Customer>>(result);

            var customers1  = from m in customers
                             select m;

            if(!String.IsNullOrEmpty(searchString))
            {
                customers1 = customers1.Where(s => s.CustomerName.Contains(searchString));
            }

            return View(customers1.ToList());
        }


        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [Route("Customers/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = BankApi.InitializeClient().PostAsync("api/Customers", content).Result;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }

            return View(customer);
        }


        // GET: Customer Profile
        [Route("Customers/Profile")]
        public async Task<IActionResult> Profile(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await BankApi.InitializeClient().GetAsync($"api/Customers/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<Customer>(result);

            return View(customer);
        }


        // GET: Customers/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await BankApi.InitializeClient().GetAsync($"api/Customers/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<Customer>(result);

            return View(customer);
        }

        [Route("Customers/Block")]
        public async Task<IActionResult> Block(int id) 
        {
            var response = await BankApi.InitializeClient().GetAsync($"api/Logins/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var login = JsonConvert.DeserializeObject<Login>(result);
            login.Block = true;
            login.ModifyDate = DateTime.UtcNow;
            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            response = BankApi.InitializeClient().PutAsync("api/Logins", content).Result;
            return RedirectToAction(nameof(Index));

        }

        // POST: Customers/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Customer customer)
        {
            if (id != customer.CustomerID)
                return NotFound();

            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = BankApi.InitializeClient().PutAsync("api/Customers", content).Result;

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Profile), new { id = customer.CustomerID });
            }
            return View(customer);
        }

        // GET: Customers/Delete/1
        [Route("Customers/Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await BankApi.InitializeClient().GetAsync($"api/Customers/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<Customer>(result);

            return View(customer);
        }

        // POST: Customers/Delete/1
        [Route("Customers/Remove")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = BankApi.InitializeClient().DeleteAsync($"api/Customers/{id}").Result;

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return NotFound();
        }
    }
}
