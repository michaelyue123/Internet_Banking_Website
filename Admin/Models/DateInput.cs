using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    public class DateInput
    {
        [Required(ErrorMessage = "DateTime is required")]
        [Display(Name = "Date1")]
        public DateTime Date1 { get; set; }

        [Required(ErrorMessage = "DateTime is required")]
        [Display(Name = "Date2")]
        public DateTime Date2 { get; set; }
    }
}
