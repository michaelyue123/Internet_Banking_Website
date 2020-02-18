using System;
using System.ComponentModel.DataAnnotations;


namespace IBW.Model
{
    // reference Week 7 tutorial Login.cs

    public class Login
    {
        [Required(ErrorMessage = "LoginID is required"), StringLength(50)]
        [Display(Name = "LoginID")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "CustomerID is required")]
        [Display(Name = "CustomerID")]
        [Key]
        public int CustomerID { get; set; }

        public virtual Customer Customer { get; set; }

        public string PasswordHash { get; set; }

        [Display(Name = "Modify Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        public DateTime ModifyDate { get; set; }

        public Boolean Block { get; set; } = false;

    }
}
