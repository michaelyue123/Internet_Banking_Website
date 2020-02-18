using System.Collections.Generic;
using System.Linq;
using IBW.Model;
using IBW.Data;
using WebApi.Models.Repository;

namespace WebApi.Models.DataManager
{
    // reference MovieManager.cs from week 9 tutorial 

    public class CustomerManager : IDataRepo<Customer, int>
    {
        private readonly IBWContext _context;

        public CustomerManager(IBWContext context)
        {
            _context = context;
        }

        public Customer Get(int id)
        {
            return _context.Customers.Find(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public int Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer.CustomerID;
        }

        public int Delete(int id)
        {
            _context.Customers.Remove(_context.Customers.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Customer customer)
        {
            _context.Update(customer);
            _context.SaveChanges();

            return id;
        }
    }
}
