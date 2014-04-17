using KendoUI.Northwind.Dashboard.Controllers;
using KendoUI.Northwind.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Data.SqlClient;

namespace KendoUI.Northwind.Dashboard.Controllers
{
    public class TeamController : Controller
    {
        public ActionResult TeamEfficiency()
        {
            return View();
        }

        public ActionResult EmployeesList([DataSourceRequest]DataSourceRequest request)
        {
            var employees = new NorthwindEntities().Employees.Select(e => new EmployeeViewModel
            {
                EmployeeID = e.EmployeeID,
                FirstName = e.FirstName,
                LastName = e.LastName,
                EmployeeName = e.FirstName + " " + e.LastName,
                Notes = e.Notes,
                Title = e.Title,
            }).OrderBy(e => e.FirstName);

            return Json(employees.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeSales([DataSourceRequest]DataSourceRequest request)
        {
            var northwind = new NorthwindEntities();
            var sales = northwind.Orders.Join(northwind.Customers, c => c.CustomerID, o => o.CustomerID, (o, c) => new { Order = o, Customer = c }).ToList().Select(o => new SaleViewModel
            {
                SaleID = o.Order.OrderID,
                EmployeeID = o.Order.EmployeeID,
                Title = o.Customer.CompanyName,
                Start = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc),
                End = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc).AddHours(1),
                IsAllDay = false
            });

            return Json(sales.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeAndTeamSales(int EmployeeID, DateTime fromDate, DateTime endDate)
        {
            var northwind = new NorthwindEntities();
            var result = northwind.Database.SqlQuery<SalesStatsViewModel>("SalesAmounts @EmployeeID", new SqlParameter("@EmployeeID", EmployeeID)).ToList().Where(d => d.Date >= fromDate && d.Date <= endDate);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeAverageSales(int EmployeeID, DateTime fromDate, DateTime endDate)
        {
            var northwind = new NorthwindEntities();
            var sales = northwind.Orders.Where(w => w.EmployeeID == EmployeeID)
                .Join(northwind.Order_Details, orders => orders.OrderID, orderDetails => orderDetails.OrderID, (orders, orderDetails) => new { Order = orders, OrderDetails = orderDetails })
                .Where(d => d.Order.OrderDate >= fromDate && d.Order.OrderDate <= endDate).ToList()
                .Select(o => new SalesStatsViewModel
                { 
                    Date = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc),
                    EmployeeSales = (double)((o.OrderDetails.Quantity * o.OrderDetails.UnitPrice) - (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice * (decimal)o.OrderDetails.Discount)),
                });
            var result = from s in sales
                group s by new { Date = new DateTime(s.Date.Year, s.Date.Month, 1) } into temp
                select new
                {
                    EmployeeSales = temp.Sum(s => s.EmployeeSales),
                    Date = temp.Key.Date
                };
            return Json(result, JsonRequestBehavior.AllowGet); 
        }

        public ActionResult EmployeeQuarterSales(int EmployeeID, DateTime endDate)
        {
            DateTime fromDate = endDate.AddMonths(-3);
            var northwind = new NorthwindEntities();
            var sales = northwind.Orders.Where(w => w.EmployeeID == EmployeeID)
                .Join(northwind.Order_Details, orders => orders.OrderID, orderDetails => orderDetails.OrderID, (orders, orderDetails) => new { Order = orders, OrderDetails = orderDetails })
                .Where(d => d.Order.OrderDate >= fromDate && d.Order.OrderDate <= endDate).ToList()
                .Select(o => new QuarterToDateSalesViewModel
                {
                    Current = (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice) - (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice * (decimal)o.OrderDetails.Discount)
                });
            //Generate the target based on team's average sales?
            var result = new List<QuarterToDateSalesViewModel>() { 
                     new QuarterToDateSalesViewModel {Current = sales.Sum(s=>s.Current), Target = 15000, OrderDate = endDate}
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
