using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KendoUI.Northwind.Dashboard.Models
{
    public class CustomerCustomerDemo
    {
        [Key, Column(Order = 0)]
        public string CustomerID { get; set; }

        [Key, Column(Order = 1)]
        public string CustomerTypeID { get; set; }

        public Customer Customer { get; set; }
        public CustomerDemographic CustomerDemographic { get; set; }
    }
}