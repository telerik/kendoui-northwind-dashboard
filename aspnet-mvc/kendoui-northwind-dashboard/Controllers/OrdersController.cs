using KendoUI.Northwind.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Data;

namespace KendoUI.Northwind.Dashboard.Controllers
{
    public class OrdersController : Controller
    {

        public ActionResult Orders_Read([DataSourceRequest] DataSourceRequest request, string ID)
        {
            var orders = GetOrders().Where(o => o.CustomerID == (ID));
            return Json(orders.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Orders_Create([DataSourceRequest]DataSourceRequest request, OrderViewModel order, string ID)
        {
            if (ModelState.IsValid)
            {
                using (var northwind = new NorthwindEntities())
                {
                    var entity = new Order
                    {
                        CustomerID = ID,
                        EmployeeID = order.EmployeeID,
                        OrderDate = order.OrderDate,
                        ShippedDate = order.ShippedDate 
                    }; 
                    northwind.Orders.Add(entity); 
                    northwind.SaveChanges(); 
                    order.OrderID = entity.OrderID;
                }
            } 
            return Json(new[] { order }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Orders_Update([DataSourceRequest]DataSourceRequest request, OrderViewModel order, string ID)
        {
            if (ModelState.IsValid)
            {
                using (var northwind = new NorthwindEntities())
                {
                    var entity = new Order
                    {
                        
                        CustomerID = ID,
                        OrderID = order.OrderID,
                        EmployeeID = order.EmployeeID,
                        OrderDate = order.OrderDate,
                        ShippedDate = order.ShippedDate,
                    };
                    northwind.Orders.Attach(entity);
                    northwind.Entry(entity).State = EntityState.Modified;
                    northwind.SaveChanges(); 
                }
            }
            return Json(new[] { order }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Orders_Destroy([DataSourceRequest]DataSourceRequest request, OrderViewModel order, string ID)
        {
            if (ModelState.IsValid)
            {
                using (var northwind = new NorthwindEntities())
                {
                    var entity = new Order
                    {
                        CustomerID = ID,
                        EmployeeID = order.EmployeeID,
                        OrderDate = order.OrderDate,
                        ShippedDate = order.ShippedDate,
                    };
                    northwind.Orders.Attach(entity);
                    northwind.Orders.Remove(entity);
                    northwind.SaveChanges();
                }
            }
            return Json(new[] { order }.ToDataSourceResult(request, ModelState));
        } 

        private static IEnumerable<OrderViewModel> GetOrders()
        {
            var northwind = new NorthwindEntities();
            var orders = northwind.Orders.Select(order => new OrderViewModel
            {
                CustomerID = order.CustomerID,
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                ShippedDate = order.ShippedDate,
                EmployeeID = order.EmployeeID,
                ShipName = order.ShipName
            });

            return orders;
        }

    }
}
