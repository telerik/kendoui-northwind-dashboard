using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KendoUI.Northwind.Dashboard.Models
{
    public class CountryOrderViewModel
    {
        public DateTime? Date { get; set; }
        public int Value { get; set; }
    }
}