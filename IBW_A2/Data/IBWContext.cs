using Microsoft.EntityFrameworkCore;
using IBW.Model;

namespace IBW.Data
{
    public class IBWContext : DbContext
    {
        public IBWContext(DbContextOptions<IBWContext> options) : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Payee> Payees { get; set; }
        public DbSet<BillPay> BillPays { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Login>().HasCheckConstraint("CH_Login_LoginID", "len(UserID) = 8").
                HasCheckConstraint("CH_Login_PasswordHash", "len(PasswordHash) = 64");

            builder.Entity<Customer>().HasOne(x => x.Login).WithOne(x => x.Customer).HasForeignKey<Login>(x => x.CustomerID);

            builder.Entity<Account>().HasCheckConstraint("CH_Account_Balance", "Balance >= 0");

            builder.Entity<Transaction>().
                HasOne(x => x.Account).WithMany(x => x.Transactions).HasForeignKey(x => x.AccountNumber);

            builder.Entity<Transaction>().HasCheckConstraint("CH_Transaction_Amount", "Amount > 0");

            builder.Entity<Account>().
                HasOne(x => x.Customer).WithMany(x => x.Accounts).HasForeignKey(x => x.CustomerID);

            builder.Entity<BillPay>().
                HasOne(x => x.Account).WithMany(x => x.BillPays).HasForeignKey(x => x.AccountNumber);

            builder.Entity<BillPay>().
                HasOne(x => x.payee).WithMany(x => x.BillPays).HasForeignKey(x => x.PayeeID);

        }
    }
}
