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
        public ActionResult Employees()
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

        public ActionResult SalesByEmployee(int EmployeeID)
        {
            var northwind = new NorthwindEntities();
            var employeeSales = northwind.Orders.Where(w => w.EmployeeID == EmployeeID)
                .Join(northwind.Order_Details, orders => orders.OrderID, orderDetails => orderDetails.OrderID, (orders, orderDetails) => new { Order = orders, OrderDetails = orderDetails }).ToList()
                .Select(o => new SaleStatsViewModel
                {
                    OrderID = o.Order.OrderID,
                    OrderDate = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc),
                    OrderAmount = (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice) - (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice * (decimal)o.OrderDetails.Discount),
                });
            var allSales = northwind.Orders
                .Join(northwind.Order_Details, orders => orders.OrderID, orderDetails => orderDetails.OrderID, (orders, orderDetails) => new { Order = orders, OrderDetails = orderDetails }).ToList()
                .Select(o => new SaleStatsViewModel
                {
                    OrderID = o.Order.OrderID,
                    OrderDate = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc),
                    TotalAmount = (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice) - (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice * (decimal)o.OrderDetails.Discount),
                });
            //var result = allSales.Join(employeeSales, all => all.OrderID, emp => emp.OrderID, (all, emp) => new { All = all, Employee = emp }).ToList()
            //    .Select(o => new SaleStatsViewModel
            //    {
            //        OrderID = o.All.OrderID,
            //        OrderDate = o.All.OrderDate,
            //        TotalAmount = o.All.TotalAmount,
            //        OrderAmount = o.Employee.OrderAmount
            //    });
            var result = allSales.Union(employeeSales).ToList()
                .Select(o => new SaleStatsViewModel
                {
                    OrderID = o.OrderID,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    OrderAmount = o.OrderAmount
                });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
