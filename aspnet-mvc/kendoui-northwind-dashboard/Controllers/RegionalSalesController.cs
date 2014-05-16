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
            var result = northwind.Database.SqlQuery<TopSellingProductsViewModel>("TopSellingProducts @Country, @FromDate, @ToDate",
                new SqlParameter("@Country", Country), new SqlParameter("@FromDate", FromDate.ToString("yyyyMMdd")), new SqlParameter("@ToDate", ToDate.ToString("yyyyMMdd")));
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
            var result = northwind.Database.SqlQuery<MarketShareViewModel>("MarketShareByCountry @Country, @FromDate, @ToDate",
                new SqlParameter("@Country", Country), new SqlParameter("@FromDate", FromDate.ToString("yyyyMMdd")), new SqlParameter("@ToDate", ToDate.ToString("yyyyMMdd")));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        

    }
}
