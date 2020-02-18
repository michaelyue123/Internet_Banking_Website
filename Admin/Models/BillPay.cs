using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    public enum Period
    {
        M = 1,
        Q = 2,
        Y = 3,
        O = 4
    }
    public class BillPay
    {
   
        [Display(Name = "BillPayID")]
        public int BillPayID { get; set; }

        [Required(ErrorMessage = "Account Number is required")]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Required(ErrorMessage = "PayeeID is required")]
        [Display(Name = "PayeeID")]
        public int PayeeID { get; set; }

        [Required(ErrorMessage = "Money amount is required")]
        [Display(Name = "Money")]
        [DataType(DataType.Currency)]
        [Range(0.01, (Double)Decimal.MaxValue, ErrorMessage = "you should at least pay 1 cent and can't excess the max value!")]

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Schedule date is required, " +
            "hint: next schedule data for transaction to occur")]
        [Display(Name = "Schedule Date")]
        public DateTime ScheduleDate { get; set; }

        [Required(ErrorMessage = "Period is required")]
        [Display(Name = "Period")]
        public Period Period { get; set; }

        public Account Account { get; set; }
        public Payee payee { get; set; }

        public Boolean Block { get; set; } = false;
    }
}
