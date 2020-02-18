using IBW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBW.Data;
using WebApi.Models.Repository;

namespace WebApi.Models.DataManager
{
    // reference MovieManager.cs from week 9 tutorial 

    public class LoginManager : IDataRepo<Login, int>
    {
        private readonly IBWContext _context;

        public LoginManager(IBWContext context)
        {
            _context = context;
        }

        public Login Get(int id)
        {
            return _context.Logins.Find(id);
        }

        public IEnumerable<Login> GetAll()
        {
            return _context.Logins.ToList();
        }

        public int Add(Login login)
        {
            _context.Logins.Add(login);
            _context.SaveChanges();

            return login.CustomerID;
        }

        public int Delete(int id)
        {
            _context.Logins.Remove(_context.Logins.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Login login)
        {
            _context.Update(login);
            _context.SaveChanges();

            return id;
        }
    }
}
