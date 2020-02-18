using IBW.Model;
using System.Collections.Generic;
using System.Linq;
using IBW.Data;
using WebApi.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models.DataManager
{
    // reference MovieManager.cs from week 9 tutorial 
    public class BillPayManager : IDataRepo<BillPay, int>
    {
        private readonly IBWContext _context;

        public BillPayManager(IBWContext context)
        {
            _context = context;
        }

        public BillPay Get(int id)
        {
            return _context.BillPays.Find(id);
        }

        public IEnumerable<BillPay> GetAll()
        {
            return _context.BillPays.Include(b=>b.payee).ToList();
        }

        public int Add(BillPay billPay)
        {
            _context.BillPays.Add(billPay);
            _context.SaveChanges();

            return billPay.BillPayID;
        }

        public int Delete(int id)
        {
            _context.BillPays.Remove(_context.BillPays.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, BillPay billPay)
        {
            _context.Update(billPay);
            _context.SaveChanges();

            return id;
        }
    }
}
