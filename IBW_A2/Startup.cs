using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IBW.Data;
using System.Globalization;
using WebApi.Models.DataManager;
using IBW.MiddleWare;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;

namespace IBW
{
    public class Startup
    {
        private const string _enableCrossOriginRequestsKey = "EnableCrossOriginRequests";
        public Startup(IConfiguration configuration) => Configuration = configuration;

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
               options.AddPolicy(_enableCrossOriginRequestsKey, builder => builder.AllowAnyOrigin()));

            services.AddDbContext<IBWContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(nameof(IBWContext)));
                // Enable lazy loading.
                //options.UseLazyLoadingProxies();
            });

            services.AddTransient<AccountManager>();
            services.AddTransient<CustomerManager>();
            services.AddTransient<LoginManager>();
            services.AddTransient<TransactionManager>();
            services.AddTransient<BillPayManager>();
            services.AddTransient<PayeeManager>();
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(30);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSession();
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.CurrencySymbol = "$";
            

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/error/500");
                // The default HSTS value is 30 days.
                // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseHttpsRedirect();
            app.UseCors(_enableCrossOriginRequestsKey);
            app.Use(async (context, next) =>
            {
                await next.Invoke();

                //After going down the pipeline check if we 404'd. 
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await context.Response.WriteAsync("Oops 404 - We can't find the page you're looking for!");
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapAreaControllerRoute(
                    "admin",
                    "Admin/{controller=Expire}/{action=Expire}",
                    "Admin/{controller=Block}/{action=Block}");
                endpoints.MapControllerRoute(
                    "default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}