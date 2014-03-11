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
    public class OrderDetailsController : Controller
    {
        public ActionResult OrderDetails_Read([DataSourceRequest] DataSourceRequest request, int OrderID)
        {
            var orders = GetOrderDetails().Where(o => o.OrderID.Equals(OrderID));
            return Json(orders.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private static IEnumerable<OrderDetailViewModel> GetOrderDetails()
        {
            var northwind = new NorthwindEntities();
            var order_details = northwind.Order_Details.Select(od => new OrderDetailViewModel
            {
                OrderID = od.OrderID,
                ProductID = od.ProductID,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity,
                Discount = od.Discount
            });

            return order_details;
        }

    }
}
