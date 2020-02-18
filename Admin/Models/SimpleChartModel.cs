using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public class SimpleReportViewModel
    {
        public string DimensionOne { get; set; }
        public int Quantity { get; set; }
    }

    public class DecimalViewModel
    {
        public string DimensionOne { get; set; }
        public Decimal Amount { get; set; }
    }
    public class StackedViewModel
    {
        public string StackedDimensionOne { get; set; }
        public List<SimpleReportViewModel> LstData { get; set; }
    }
}
