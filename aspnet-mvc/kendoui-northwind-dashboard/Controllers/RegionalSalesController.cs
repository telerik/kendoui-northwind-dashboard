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
            var result = northwind.CountryMarketShare(Country, FromDate.ToString("yyyyMMdd"), ToDate.ToString("yyyyMMdd"));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountryRevenue(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            var result = northwind.CountryRevenue(Country, FromDate.ToString("yyyyMMdd"), ToDate.ToString("yyyyMMdd"));
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
                         select new { Date = g.Key, Value = g.Count()};

            return Json(new { Orders = (int?)result.Sum(x => x.Value) ?? 0}, JsonRequestBehavior.AllowGet);
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