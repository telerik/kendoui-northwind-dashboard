using KendoUI.Northwind.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace KendoUI.Northwind.Dashboard.Controllers
{
    public class TeamEfficiencyController : ApiController
    {
        //
        // GET: /TeamEfficiency/
        public List<EmployeeViewModel> GetEmployeesList()
        {
            var employees = new NorthwindEntities().Employees.Select(e => new EmployeeViewModel
            {
                EmployeeID = e.EmployeeID,
                FirstName = e.FirstName,
                LastName = e.LastName,
                EmployeeName = e.FirstName + " " + e.LastName,
                Notes = e.Notes,
                Title = e.Title,
                HomePhone = e.HomePhone
            }).OrderBy(e => e.FirstName);

            return employees.ToList();
        }

        public List<QuarterToDateSalesViewModel> GetEmployeeQuarterSales(int EmployeeID, DateTime endDate)
        {
            DateTime startDate = endDate.AddMonths(-3);
            var northwind = new NorthwindEntities();
            var sales = northwind.Orders.Where(w => w.EmployeeID == EmployeeID)
                .Join(northwind.Order_Details, orders => orders.OrderID, orderDetails => orderDetails.OrderID, (orders, orderDetails) => new { Order = orders, OrderDetails = orderDetails })
                .Where(d => d.Order.OrderDate >= startDate && d.Order.OrderDate <= endDate).ToList()
                .Select(o => new QuarterToDateSalesViewModel
                {
                    Current = (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice) - (o.OrderDetails.Quantity * o.OrderDetails.UnitPrice * (decimal)o.OrderDetails.Discount)
                });
            var result = new List<QuarterToDateSalesViewModel>() { 
                     new QuarterToDateSalesViewModel {Current = sales.Sum(s=>s.Current), Target = 15000, OrderDate = endDate}
            };
            return result.ToList();
        }

        public List<MonthlySalesByEmployee_Result> GetEmployeeAverageSales(int EmployeeID, DateTime startDate, DateTime endDate)
        {
            var northwind = new NorthwindEntities();
            var result = northwind.MonthlySalesByEmployee(EmployeeID).Where(d => d.Date >= startDate && d.Date <= endDate);

            return result.ToList();
        }

        public List<SalesAmounts_Result> GetEmployeeAndTeamSales(int EmployeeID, DateTime startDate, DateTime endDate)
        {
            var northwind = new NorthwindEntities();
            var result = northwind.SalesAmounts(EmployeeID).Where(d => d.Date >= startDate && d.Date <= endDate);
            return result.ToList();
        }

        public List<SaleViewModel> EmployeeSales()
        {
            var northwind = new NorthwindEntities();
            var data = northwind.Orders.Join(northwind.Customers, c => c.CustomerID, o => o.CustomerID, (o, c) => new { Order = o, Customer = c }).ToList();

            var sales = data.Select(o => new SaleViewModel
            {
                SaleID = o.Order.OrderID,
                EmployeeID = o.Order.EmployeeID,
                Title = o.Customer.CompanyName,
                Start = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc),
                End = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc).AddHours(1),
                IsAllDay = false
            });

            return sales.ToList();
        }

        public List<SaleViewModel> GetEmployeeSales()
        {
            var northwind = new NorthwindEntities();
            var data = northwind.Orders.Join(northwind.Customers, c => c.CustomerID, o => o.CustomerID, (o, c) => new { Order = o, Customer = c }).ToList();

            var sales = data.Select(o => new SaleViewModel
            {
                SaleID = o.Order.OrderID,
                EmployeeID = o.Order.EmployeeID,
                Title = o.Customer.CompanyName,
                Start = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc),
                End = DateTime.SpecifyKind((DateTime)o.Order.OrderDate, DateTimeKind.Utc).AddHours(1),
                IsAllDay = false
            });
            return sales.ToList();
        }


    }
}
