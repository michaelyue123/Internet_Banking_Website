using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IBW.Model
{
    public class Payee
    {
        [Required(ErrorMessage = "Payee ID is required")]
        [Display(Name = "Payee ID")]
        [Range(1000,9999)]
        public int PayeeID { get; set; }

        [Required(ErrorMessage = "Payee Name is required")]
        [Display(Name = "Payee Name")]
        public string PayeeName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "PostCode")]
        public string PostCode { get; set; }

        [Display(Name = "TFN")]
        public string TFN { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone")]
        [StringLength(15)]
        public string Phone { get; set; }

        public  List<BillPay> BillPays { get; set; }
    }
}
