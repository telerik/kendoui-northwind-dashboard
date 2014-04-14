using KendoUI.Northwind.Dashboard.Controllers;
using KendoUI.Northwind.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace KendoUI.Northwind.Dashboard.Controllers
{
    public class EmployeesController : Controller
    {
        public ActionResult TeamEfficiency()
        {
            return View();
        }

        public ActionResult Employees_Read([DataSourceRequest]DataSourceRequest request)
        {
            var employees = new NorthwindEntities().Employees.Select(e => new EmployeeViewModel
            {
                EmployeeID = e.EmployeeID,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Notes = e.Notes,
                Title = e.Title,
            }).OrderBy(e => e.FirstName);

            return Json(employees.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalesResources_Read([DataSourceRequest]DataSourceRequest request)
        {
            var employees = new NorthwindEntities().Employees.Select(e => new EmployeeViewModel
            {
                EmployeeID = e.EmployeeID,
                EmployeeName = e.FirstName + " " + e.LastName
            }).OrderBy(e => e.EmployeeName);

            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sales_Read([DataSourceRequest]DataSourceRequest request)
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
            //var allSales = northwind.Orders
            //    .Join(northwind.Order_Details, orders => orders.OrderID, orderDetails => orderDetails.OrderID, (orders, orderDetails) => new { Order = orders, OrderDetails = orderDetails }).ToList()
            //    .Select(o => new SaleStatsViewModel
            //    {
            //        OrderID = o.Order.OrderID,
            //        OrderDate = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc),
            //        OrderAmount = null,
            //        TotalAmount = (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice) - (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice * (decimal)o.OrderDetails.Discount),

            //    }).Where(d => d.OrderDate >= fromDate && d.OrderDate <= endDate); ;

            //var employeeSales = northwind.Orders.Where(w => w.EmployeeID == EmployeeID)
            //    .Join(northwind.Order_Details, orders => orders.OrderID, orderDetails => orderDetails.OrderID, (orders, orderDetails) => new { Order = orders, OrderDetails = orderDetails }).ToList()
            //    .Select(o => new SaleStatsViewModel
            //    {
            //        OrderID = o.Order.OrderID,
            //        OrderDate = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc),
            //        OrderAmount = (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice) - (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice * (decimal)o.OrderDetails.Discount),
            //        TotalAmount = null
            //    }).Where(d => d.OrderDate >= fromDate && d.OrderDate <= endDate).GroupBy(x => x.OrderID);

            //var result = allSales.Join(employeeSales, all => all.OrderID, emp => emp.OrderID, (all, emp) => new { All = all, Employee = emp }).ToList()
            //    .Select(o => new SaleStatsViewModel
            //    {
            //        OrderID = o.All.OrderID,
            //        OrderDate = o.All.OrderDate,
            //        TotalAmount = o.All.TotalAmount,
            //        OrderAmount = o.Employee.OrderAmount
            //    });

            //var result = allSales.Union(employeeSales).ToList()
            //    .Select(o => new SaleStatsViewModel
            //    {
            //        //OrderID = o.OrderID,
            //        OrderDate = o.OrderDate,
            //        TotalAmount = o.TotalAmount,
            //        OrderAmount = o.OrderAmount
            //    });


            var allSales = northwind.Database.SqlQuery<SaleStatsViewModel>(@"SELECT EmployeeSales.Date AS OrderDate, EmployeeSales.Amount AS OrderAmount, AllSales.Amount AS TotalAmount FROM (
                SELECT Orders.EmployeeID, Orders.OrderDate AS Date, SUM(Details.Amount) AS Amount FROM Orders 
                INNER JOIN (
                    SELECT Orders.OrderID, sum((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Amount FROM [Order Details] AS Details
                    INNER JOIN Orders ON Orders.OrderID = Details.OrderID
                    GROUP BY Orders.OrderID
                ) AS Details  ON Orders.OrderID = Details.OrderID 
                WHERE Orders.EmployeeID = 1
                GROUP BY Orders.OrderDate, EmployeeID
            ) AS EmployeeSales
            LEFT OUTER JOIN 
            (
                SELECT Orders.OrderDate AS Date, SUM(Details.Amount) AS Amount FROM Orders 
                INNER JOIN (
                    SELECT Orders.OrderID, sum((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Amount FROM [Order Details] AS Details
                    INNER JOIN Orders ON Orders.OrderID = Details.OrderID
                    GROUP BY Orders.OrderID
                ) AS Details  ON Orders.OrderID = Details.OrderID 
                GROUP BY Orders.OrderDate
            ) AS AllSales ON AllSales.Date = EmployeeSales.Date
            GROUP BY EmployeeSales.Date, EmployeeSales.Amount, AllSales.Amount")
            .ToList();
            //var employeeSales = allSales.Where(w => w.EmployeeID == EmployeeID).ToList();

            var result = allSales.Select(o => new SaleStatsViewModel
                { 
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    OrderAmount = o.OrderAmount
                });//.Where(d => d.OrderDate >= fromDate && d.OrderDate <= endDate);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeSales(int EmployeeID, DateTime fromDate, DateTime endDate)
        {
            var northwind = new NorthwindEntities();
            var employeeSales = northwind.Orders.Where(w => w.EmployeeID == EmployeeID)
                .Join(northwind.Order_Details, orders => orders.OrderID, orderDetails => orderDetails.OrderID, (orders, orderDetails) => new { Order = orders, OrderDetails = orderDetails }).ToList()
                .Select(o => new SaleStatsViewModel
                {
                    OrderID = o.Order.OrderID,
                    OrderDate = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc),
                    OrderAmount = 1,//(o.OrderDetails.Quantity * o.OrderDetails.UnitPrice) - (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice * (decimal)o.OrderDetails.Discount),
                }).Where(d => d.OrderDate >= fromDate && d.OrderDate <= endDate ); 
            return Json(employeeSales, JsonRequestBehavior.AllowGet); 
        }

        public ActionResult EmployeeQuarterSales(int EmployeeID, DateTime endDate)
        {
            DateTime fromDate = endDate.AddMonths(-3);
            var northwind = new NorthwindEntities();
            var employeeSales = northwind.Orders.Where(w => w.EmployeeID == EmployeeID)
                .Join(northwind.Order_Details, orders => orders.OrderID, orderDetails => orderDetails.OrderID, (orders, orderDetails) => new { Order = orders, OrderDetails = orderDetails }).ToList()
                .Select(o => new QuarterToDateSalesViewModel
                { 
                    OrderDate = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc),
                    Current = 1,//(o.OrderDetails.Quantity * o.OrderDetails.UnitPrice) - (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice * (decimal)o.OrderDetails.Discount),
                    Target = 3000
                }).Where(d => d.OrderDate >= fromDate && d.OrderDate <= endDate);
            return Json(employeeSales, JsonRequestBehavior.AllowGet);
        }

    }
}
