using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;

namespace KendoUI.Northwind.Dashboard.Models
{

    public class MonthValuesViewModel
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }

    public class MarketShareViewModel
    {
        public string Country { get; set; }
        public double Sales { get; set; }
    }


    public class TopSellingProductsViewModel
    {
        public string ProductName { get; set; } 
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }

    public class SalesStatsViewModel
    {
        public DateTime Date { get; set; }
        public double? EmployeeSales { get; set; }
        public double? TotalSales { get; set; } 
    }

    public class QuarterToDateSalesViewModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Current { get; set; }
        public decimal Target { get; set; }
    }

    public class SaleViewModel : ISchedulerEvent
    {
        public int SaleID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        private DateTime start;
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value.ToUniversalTime();
            }
        }

        public string StartTimezone { get; set; }

        private DateTime end;
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value.ToUniversalTime();
            }
        }

        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public int? RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsAllDay { get; set; }
        public int? EmployeeID{ get; set; }
    }
}
