using KendoUI.Northwind.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendoUI.Northwind.Dashboard.Controllers
{
    public class RegionalSalesController : Controller
    {
        public ActionResult TopSellingProducts(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();

            var result = northwind.CountryTopProducts(Country, FromDate.ToString("yyyyMMdd"), ToDate.ToString("yyyyMMdd"));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountryCompanies(string Country)
        {
            var northwind = new NorthwindEntities();
            var companies = northwind.Customers.Select(customer => new CustomerViewModel
            {
                CompanyName = customer.CompanyName,
                Country = customer.Country
            }).Where(x => x.Country == Country);
            return Json(companies, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MarketShareByCountry(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            var allSales = from o in northwind.Orders
                         join od in northwind.Order_Details on o.OrderID equals od.OrderID
                         where o.OrderDate >= FromDate && o.OrderDate <= ToDate
                         select new
                         {
                             Country = o.ShipCountry,
                             Sales = od.Quantity * od.UnitPrice
                         };


            return Json(new [] {
                new { Country = "All", Sales = allSales.Sum(x => x.Sales) },
                new { Country = Country, Sales = allSales.Where(w=>w.Country == Country).Sum(s => s.Sales) }
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountryRevenue(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            var q1 = (from o in northwind.Orders
                      join od in northwind.Order_Details on o.OrderID equals od.OrderID
                      where o.OrderDate >= FromDate && o.OrderDate <= ToDate && o.ShipCountry == Country
                      select new
                      {
                          OrderID = o.OrderID,
                          EmployeeID = o.EmployeeID,
                          Date = o.OrderDate,
                          Sales = od.Quantity * od.UnitPrice
                      }).AsEnumerable();
            var q2 = (from allSales in q1
                      group allSales by allSales.OrderID into g
                      select new
                      {
                          OrderID = g.Key,
                          EmployeeID = g.FirstOrDefault().EmployeeID,
                          Sales = g.Sum(x => x.Sales),
                          Date = new DateTime(g.FirstOrDefault().Date.Value.Year, g.FirstOrDefault().Date.Value.Month, 1),
                      });
            var q3 = (from groupedSales in q2
                      group groupedSales by new { groupedSales.EmployeeID, groupedSales.Date } into gs
                      select new
                      {
                          EmployeeID = gs.FirstOrDefault().EmployeeID,
                          Date = gs.Key.Date,
                          Sales = gs.Sum(x => x.Sales)
                      });
            var result = (from totalSales in q3
                          group totalSales by totalSales.Date into gs
                          select new
                          {
                              Date = gs.Key,
                              Value = gs.Sum(x => x.Sales)
                          }
            );

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountryOrders(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            IQueryable<Order> data = northwind.Orders.Where(o => o.OrderDate >= FromDate && o.OrderDate <= ToDate && o.ShipCountry == Country);
            var result = from o in data
                         group o by o.OrderDate into g
                         select new { Date = g.Key, Value = g.Count() };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountryOrdersTotal(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            IQueryable<Order> data = northwind.Orders.Where(o => o.OrderDate >= FromDate && o.OrderDate <= ToDate && o.ShipCountry == Country);
            var result = from o in data
                         group o by o.OrderDate into g
                         select new { Date = g.Key, Value = g.Count() };
            int? total = 0;
            if (result.Count() > 0)
            {
                total = result.Sum(x => x.Value);
            }
            return Json(new { Orders = total}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountryCustomers(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            var result = northwind.CountryCustomers(Country, FromDate.ToString("yyyyMMdd"), ToDate.ToString("yyyyMMdd"));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountryCustomersTotal(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            var result = northwind.CountryCustomersTotal(Country, FromDate.ToString("yyyyMMdd"), ToDate.ToString("yyyyMMdd"));
            return Json(new { Customers = result }, JsonRequestBehavior.AllowGet);
        }

    }
}