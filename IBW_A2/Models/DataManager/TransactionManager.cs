using System.Collections.Generic;
using System.Linq;
using IBW.Model;
using IBW.Data;
using WebApi.Models.Repository;

namespace WebApi.Models.DataManager
{
    // reference MovieManager.cs from week 9 tutorial 
    public class TransactionManager : IDataRepo<Transaction, int>
    {
        private readonly IBWContext _context;

        public TransactionManager(IBWContext context)
        {
            _context = context;
        }

        public Transaction Get(int id)
        {
            return _context.Transactions.Find(id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions.ToList();
        }

        public IEnumerable<Transaction> GetCustomerTran(int id)
        {
            var transactions = (from b in _context.Accounts
                                join t in _context.Transactions
                                on b.AccountNumber equals t.AccountNumber
                                where b.CustomerID == id
                                select t);

            return transactions;
        }


        public int Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return transaction.TransactionID;
        }

        public int Delete(int id)
        {
            _context.Transactions.Remove(_context.Transactions.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();

            return id;
        }
    }
}
