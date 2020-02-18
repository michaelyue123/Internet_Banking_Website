using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using IBW.Model;

namespace IBW.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var Context = new IBWContext(serviceProvider.GetRequiredService<DbContextOptions<IBWContext>>());
            // Look for customers.
            if (Context.BillPays.Any())
                return; // DB has already been seeded.

            const string format = "dd/MM/yyyy hh:mm:ss tt";
            /* Context.Customers.AddRange(
                new Customer
                {
                    CustomerID = 2100,
                    CustomerName = "Matthew Bolger",
                    Address = "123 Fake Street",
                    City = "Melbourne",
                    PostCode = "3000",
                    Phone = "(61)-403208239",
                    State = "VIC"        
                },
                new Customer
                {
                    CustomerID = 2200,
                    CustomerName = "Rodney Cocker",
                    Address = "456 Real Road",
                    City = "Melbourne",
                    PostCode = "3005",
                    Phone = "(61)-403208237",
                    State = "VIC"
                },
                new Customer
                {
                    CustomerID = 2300,
                    CustomerName = "Shekhar Kalra",
                    Phone = "(61)-403218239",
                    State = "VIC"
                });*/

          /* Context.Logins.AddRange(
                 new Login
                 {
                     UserID = "12345678",
                     CustomerID = 2100,
                     PasswordHash = "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV+I8d2",
                     ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null),
                     Block = false
                 },
                 new Login
                 {
                     UserID = "38074569",
                     CustomerID = 2200,
                     PasswordHash = "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04",
                     ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null),
                     Block = false
                 },
                 new Login
                 {
                     UserID = "17963428",
                     CustomerID = 2300,
                     PasswordHash = "LuiVJWbY4A3y1SilhMU5P00K54cGEvClx5Y+xWHq7VpyIUe5fe7m+WeI0iwid7GE",
                     ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null),
                     Block = false
                 }) ; 
          
            Context.Accounts.AddRange(
                new Account
                {
                    AccountNumber = 4100,
                    AccountType = AccountType.Saving,
                    CustomerID = 2100,
                    ModifyDate = DateTime.UtcNow,
                    Balance = 100.00m
                    
                },
                new Account
                {
                    AccountNumber = 4101,
                    AccountType = AccountType.Checking,
                    CustomerID = 2100,
                    ModifyDate = DateTime.UtcNow,
                    Balance = 500
                   
                },
                new Account
                {
                    AccountNumber = 4200,
                    AccountType = AccountType.Saving,
                    CustomerID = 2200,
                    ModifyDate = DateTime.UtcNow,
                    Balance = 500.95m
                   
                },
                new Account
                {
                    AccountNumber = 4300,
                    AccountType = AccountType.Checking,
                    CustomerID = 2300,
                    ModifyDate = DateTime.UtcNow,
                    Balance = 1250.50m

                });
            
            const string openingBalance = "Opening balance";
           
            Context.Transactions.AddRange(
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 3000,
                    Comment = openingBalance,
                    TransactionTimeUtc = DateTime.ParseExact("19/07/2019 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    AccountNumber = 4100,
                    Amount = 1000,
                    Comment = openingBalance,
                    TransactionTimeUtc = DateTime.ParseExact("19/08/2019 08:00:00 PM", format, null)
                },

                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    AccountNumber = 4100,
                    Amount = 500,
                    Comment = openingBalance,
                    TransactionTimeUtc = DateTime.ParseExact("19/09/2019 08:00:00 PM", format, null)
                },

                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    AccountNumber = 4100,
                    Amount = 600,
                    Comment = openingBalance,
                    TransactionTimeUtc = DateTime.ParseExact("19/10/2019 08:00:00 PM", format, null)
                },

                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    AccountNumber = 4100,
                    Amount = 700,
                    Comment = openingBalance,
                    TransactionTimeUtc = DateTime.ParseExact("19/11/2019 08:00:00 PM", format, null)
                },

                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    AccountNumber = 4100,
                    Amount = 200,
                    Comment = openingBalance,
                    TransactionTimeUtc = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4101,
                    Amount = 500,
                    Comment = openingBalance,
                    TransactionTimeUtc = DateTime.ParseExact("19/12/2019 08:30:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4200,
                    Amount = 500.95m,
                    Comment = openingBalance,
                    TransactionTimeUtc = DateTime.ParseExact("19/12/2019 09:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4300,
                    Amount = 1250.50m,
                    Comment = "Opening balance",
                    TransactionTimeUtc = DateTime.ParseExact("19/12/2019 10:00:00 PM", format, null)
                });

            Context.Payees.AddRange(
                new Payee
                {
                PayeeName = "Telstra",
                Address = "242 Exhibition St, Melbourne",
                City="Melbourne",
                PostCode="3053",
                TFN = "12338923898",
                State = "VIC",
                Phone = "(61)-403207266"
                },

                new Payee
                {
                    PayeeName = "Origin",
                    Address = "321 Exhibition St",
                    City = "Melbourne",
                    PostCode = "3000",
                    TFN = "12332923898",
                    State = "VIC",
                    Phone = "(61)-403207269"
                },

                new Payee
                {
                    PayeeName = "JB-HIFI",
                    Address = "321 latino St",
                    City = "Sydney",
                    PostCode = "2009",
                    TFN = "11332923898",
                    State = "NSW",
                    Phone = "(61)-409207269"
                }
            );*/

          Context.BillPays.AddRange(
                new BillPay
                {
                    AccountNumber = 4100,
                    PayeeID = 1002,
                    Amount = 4.3m,
                    ScheduleDate = DateTime.ParseExact("31/01/2020 10:00:00 AM", format, null),
                    Period = Period.Y,
                    Block =false
                }
                ); 
                
            Context.SaveChanges();
        }
    }
}
