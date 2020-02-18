using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Admin.Models
{
    // reference Week 7 tutorial Account.cs

    public enum AccountType
    {
        Checking = 1,
        Saving = 2
    }

    public class Account
    {
        [Required(ErrorMessage = "Account number is required")]
        [Display(Name = "Account Number")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Nullable<int> AccountNumber { get; set; }

        [Required(ErrorMessage = "Account Type is required")]
        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }

        [Required(ErrorMessage = "CustomerID is required")]
        //[Range(0, 4, ErrorMessage = "Can only be between 0 .. 4")]
        [Display(Name = "CustomerID")]
        public int CustomerID { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Customer Customer { get; set; }

        //public decimal Balance { get; set; }

        [Display(Name = "Modify Date")]
       //[Range(typeof(DateTime), "1/1/2020", "9/1/2020",
       // ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime ModifyDate { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        
        public decimal Balance { get; set; }

        public virtual List<Transaction> Transactions { get; set; }
        public virtual List<BillPay> BillPays { get; set; }
    }
}
