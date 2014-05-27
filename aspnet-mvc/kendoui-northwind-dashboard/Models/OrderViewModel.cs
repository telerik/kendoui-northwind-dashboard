using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KendoUI.Northwind.Dashboard.Models
{
    public class OrderViewModel
    {
        [ScaffoldColumn(false)]
        public int OrderID { get; set; }

        [UIHint("CustomGridForeignKey")]
        public string CustomerID { get; set; }

        [ScaffoldColumn(false)]
        public string ContactName { get; set; }

        public decimal? Freight { get; set; }

        public string ShipAddress { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ShippedDate { get; set; }

        [UIHint("ShipCountry")]
        public string ShipCountry { get; set; }

        public string ShipCity { get; set; }

        public string ShipName { get; set; }

        [UIHint("CustomGridForeignKey")]
        public int? EmployeeID { get; set; }

        [UIHint("CustomGridForeignKey")]
        public int? ShipVia { get; set; }

        public string ShipPostalCode { get; set; }
    }
}
