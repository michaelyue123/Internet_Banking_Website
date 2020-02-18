using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    // reference Week 7 tutorial Transaction.cs
    public enum TransactionType
    {
        Deposit = 1,
        Withdraw = 2,
        Transfer = 3,
        ServiceCharge = 4
    }
    public class Transaction 
    {    
        public int TransactionID { get; set; }
 
        public TransactionType TransactionType { get; set; }

        public int AccountNumber { get; set; }
       
        public int? DAN { get; set; }
      
        public decimal Amount { get; set; }
 
        public string Comment { get; set; }

        public DateTime TransactionTimeUtc { get; set; }

        public Account Account { get; set; }
    }    
}





