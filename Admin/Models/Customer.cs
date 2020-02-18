using Admin.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    // reference Week 7 tutorial Customer.cs
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "CustomerID")]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Customer Name is required")]
        [Display(Name = "Customer Name")]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Invalid input! Only letters are allowed!")]
        public string CustomerName { get; set; }

        [Display(Name = "Address")]
        [StringLength(50)]
        [RegularExpression(@"^\d+\s[A-z]+\s[A-z]+$", ErrorMessage = "Invalid input! Address is made up by number + street name!")]
        public string Address { get; set; }

        [Display(Name = "City")]
        [StringLength(40)]
        [RegularExpression(@"^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$", ErrorMessage = "Invalid input! Only letters are allowed!")]
        public string City { get; set; }

        [Display(Name = "PostCode")]
        [StringLength(4)]
        [RegularExpression("^[3053|3000|3011|3016|3002|3006|3012|3018|3003|3008|3013|3019|3004|3010|3015|3020]{4}$", ErrorMessage = "Invalid input! PostCode should only be length of 4 numbers!")]
        public string PostCode { get; set; }

        [Display(Name = "TFN")]
        [StringLength(11)]
        [RegularExpression(@"^(\d *?){11}$", ErrorMessage = "Invalid input! Tax File Number needs to be 11 numbers!")]
        public string TFN { get; set; }

        [Display(Name = "State")]
        [StringLength(3)]
        [RegularExpression("^[VIC|NSW|TAS|QLD]{3}$", ErrorMessage = "Invalid input! State needs to be 3 uppercase letters!")]
        public string State { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone")]
        [StringLength(15, MinimumLength = 14)]
        [RegularExpression(@"^(\+?\(61\)|\(\+?61\)|\+?61|\(0[1-9]\)|0[1-9])?( ?-?[0-9]){7,9}$", ErrorMessage = "Invalid input! Phone number needs to start with (61)- or 04!")]
        public string Phone { get; set; }

        public  List<Account> Accounts { get; set; }

        public Login Login { get; set; }

    }
}
