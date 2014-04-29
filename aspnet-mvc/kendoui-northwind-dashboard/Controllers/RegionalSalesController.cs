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
        public ActionResult TopSellingProducts()
        {
            var northwind = new NorthwindEntities();
            var result = northwind.Database.SqlQuery<TopSellingProductsViewModel>("TopSellingProducts").ToList();//.Where(d => d.Date >= startDate && d.Date <= endDate);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
