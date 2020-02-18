using IBW.Model;
using System.Collections.Generic;
using System.Linq;
using IBW.Data;
using WebApi.Models.Repository;

namespace WebApi.Models.DataManager
{
    // reference MovieManager.cs from week 9 tutorial 
    public class AccountManager : IDataRepo<Account, int>
    {
        private readonly IBWContext _context;

        public AccountManager(IBWContext context)
        {
            _context = context;
        }

        public Account Get(int id)
        {
            return _context.Accounts.Find(id);
        }

        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts.ToList();
        }

        public int Add(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account.CustomerID;
        }

        public int Delete(int id)
        {
            _context.Accounts.Remove(_context.Accounts.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Account account)
        {
            _context.Update(account);
            _context.SaveChanges();

            return id;
        }
    }
}
