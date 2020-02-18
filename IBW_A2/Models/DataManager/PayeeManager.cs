using IBW.Model;
using System.Collections.Generic;
using System.Linq;
using IBW.Data;
using WebApi.Models.Repository;

namespace WebApi.Models.DataManager
{
    // reference MovieManager.cs from week 9 tutorial 
    public class PayeeManager : IDataRepo<Payee, int>
    {
        private readonly IBWContext _context;

        public PayeeManager(IBWContext context)
        {
            _context = context;
        }

        public Payee Get(int id)
        {
            return _context.Payees.Find(id);
        }

        public IEnumerable<Payee> GetAll()
        {
            return _context.Payees.ToList();
        }

        public int Add(Payee payee)
        {
            _context.Payees.Add(payee);
            _context.SaveChanges();

            return payee.PayeeID;
        }

        public int Delete(int id)
        {
            _context.Payees.Remove(_context.Payees.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Payee payee)
        {
            _context.Update(payee);
            _context.SaveChanges();

            return id;
        }
    }
}
