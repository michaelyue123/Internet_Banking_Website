using IBW.Data;
using IBW.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace IBW.MiddleWare
{
    public class HttpsRedirectMiddleware
    {

        private readonly RequestDelegate _next;
        private const string RedirectKey = "redirectKey";

        public HttpsRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IBWContext dbcontext)
        {
            var request = context.Request.Path.Value;
            String path = request.ToLower();
            if (path.Contains("login") || path.Contains("api") || path == "/" || path.Contains("block") || path.Contains("expire"))
                await _next.Invoke(context);

            else if (context.Session.GetInt32(nameof(Customer.CustomerID)) != null) {
                Login login = dbcontext.Logins.Find(context.Session.GetInt32(nameof(Customer.CustomerID)).Value);
                if (login.Block)
                {
                    context.Response.Redirect($"https://localhost:44310/Block/Block");
                    context.Session.Clear();
                }
                else
                    await _next.Invoke(context);

            }
            else
            {
                context.Response.Redirect($"https://localhost:44310/Expire/Expire");
            }

         }
    }

    public static class RedirectMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpsRedirect(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpsRedirectMiddleware>();
        }
}
}
