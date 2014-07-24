using KendoUI.Northwind.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KendoUI.Northwind.Dashboard.Controllers
{
    public class SalesController : ApiController
    {
        // GET api/sales
        public List<CountryMarketShare_Result> GetMarketShare(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            var result = northwind.CountryMarketShare(Country, FromDate.ToString("yyyyMMdd"), ToDate.ToString("yyyyMMdd"));
            return result.ToList();
        }

        public List<CountryRevenue_Result> GetCountryRevenue(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            var result = northwind.CountryRevenue(Country, FromDate.ToString("yyyyMMdd"), ToDate.ToString("yyyyMMdd"));
            return result.ToList();
        }

        public List<CountryCustomers_Result> GetCountryCustomers(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            var result = northwind.CountryCustomers(Country, FromDate.ToString("yyyyMMdd"), ToDate.ToString("yyyyMMdd"));
            return result.ToList();
        }

        public int GetCountryCustomersTotal(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            int result = northwind.CountryCustomers(Country, FromDate.ToString("yyyyMMdd"), ToDate.ToString("yyyyMMdd")).ToList().Count();
            return result;
        }

        public List<CountryOrderViewModel> GetCountryOrders(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            IQueryable<Order> data = northwind.Orders.Where(o => o.OrderDate >= FromDate && o.OrderDate <= ToDate && o.ShipCountry == Country);
            var result = from o in data
                         group o by o.OrderDate into g
                         select new CountryOrderViewModel{ Date = g.Key, Value = g.Count() };

            return result.ToList();
        }

        public int GetCountryOrdersTotal(string Country, DateTime FromDate, DateTime ToDate)
        {
            var northwind = new NorthwindEntities();
            IQueryable<Order> data = northwind.Orders.Where(o => o.OrderDate >= FromDate && o.OrderDate <= ToDate && o.ShipCountry == Country);
            var result = from o in data
                         group o by o.OrderDate into g
                         select new { Date = g.Key, Value = g.Count() };
            int total = 0;
            if (result.Count() > 0)
            {
                total = result.Sum(x => x.Value);
            }
            return total;
        }

      public List<CustomerViewModel> GetCountryCompanies(string Country)
        {
            var northwind = new NorthwindEntities();
            var companies = northwind.Customers.Select(customer => new CustomerViewModel
            {
                CompanyName = customer.CompanyName,
                Country = customer.Country
            }).Where(x => x.Country == Country);
          
          return companies.ToList();
        }

      public List<CountryTopProducts_Result> GetTopSellingProducts(string Country, DateTime FromDate, DateTime ToDate)
      {
          var northwind = new NorthwindEntities();
          var result = northwind.CountryTopProducts(Country, FromDate.ToString("yyyyMMdd"), ToDate.ToString("yyyyMMdd"));
          return result.ToList();
      }

       
    }
}
