using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Admin.Web.Helper
{
    public class BankApi
    {
        private const string ApiBaseUri = "https://localhost:44310";

        public static HttpClient InitializeClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(ApiBaseUri) };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
