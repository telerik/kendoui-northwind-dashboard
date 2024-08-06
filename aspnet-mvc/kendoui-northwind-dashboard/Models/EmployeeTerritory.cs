using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KendoUI.Northwind.Dashboard.Models
{
    public class EmployeeTerritory
    {
        [Key, Column(Order = 0)]
        public int EmployeeID { get; set; }

        [Key, Column(Order = 1)]
        public string TerritoryID { get; set; }

        public Employee Employee { get; set; }
        public Territory Territory { get; set; }
    }
}