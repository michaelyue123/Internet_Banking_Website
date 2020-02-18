using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBW.Model
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

        [Required(ErrorMessage = "Transaction ID is required")]
        [Display(Name = "ID")]
        public int TransactionID { get; set; }

        [Required(ErrorMessage = "Transaction Type is required")]
        [Display(Name = "Type")]  
        [EnumDataType(typeof(TransactionType))]
        public TransactionType TransactionType { get; set; }

        [Required(ErrorMessage = "Account Number is required")]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Display(Name = "DAN")]
        public int? DestinationAccountNumber { get; set; }

        [Display(Name = "Money")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Range(0.01, (Double)Decimal.MaxValue, ErrorMessage = "The money field must be at least 1 cent and can't excess the max value!")]
        public decimal Amount { get; set; }

        [Display(Name = "Comment")]
        [StringLength(255)]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Modify Date is required")]
        [Display(Name = "TransactionTimeUtc")]
        public DateTime TransactionTimeUtc { get; set; }

        public virtual Account Account { get; set; }

  

    }
    
}





