using Admin.Models;
using Admin.Web.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    public static class CustomerHelper
    {
        public static async Task<bool> CheckBlockAsync(int id) {
            var response = await BankApi.InitializeClient().GetAsync($"api/Logins/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var login = JsonConvert.DeserializeObject<Login>(result);
            return login.Block;
        }
    }
}
