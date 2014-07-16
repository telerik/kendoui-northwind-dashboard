using KendoUI.Northwind.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KendoUI.Northwind.Dashboard.Controllers
{
    public class ProductsAndOrdersController : ApiController
    {
        // GET api/productsandorders/GetOrders
        public IQueryable<OrderViewModel> GetOrders()
        {
            var northwind = new NorthwindEntities();
            var orders = northwind.Orders.Select(order => new OrderViewModel
            {
                CustomerID = order.CustomerID,
                OrderID = order.OrderID,
                EmployeeID = order.EmployeeID,
                OrderDate = order.OrderDate,
                ShipCountry = order.ShipCountry,
                ShipVia = order.ShipVia,
                ShippedDate = order.ShippedDate,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipPostalCode = order.ShipPostalCode

            });
            return orders;
        }

        public List<OrderDetailViewModel> GetOrderDetails(int OrderID)
        {
            var orders = GetOrderDetails().Where(o => o.OrderID.Equals(OrderID));
            return orders.ToList();
        }

        public ProductViewModel GetProductDetails(int ID)
        {
            ProductViewModel productDetail = ProductDetails().Where(product => product.ProductID == ID).SingleOrDefault();
            return productDetail;
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

        private static IQueryable<ProductViewModel> ProductDetails()
        {
            var northwind = new NorthwindEntities();
            var products = northwind.Products.Select(product => new ProductViewModel
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Category = new CategoryViewModel()
                {
                    CategoryID = product.Category.CategoryID,
                    CategoryName = product.Category.CategoryName
                },
                UnitsInStock = (short)product.UnitsInStock,
                UnitsOnOrder = (short)product.UnitsOnOrder,
                ReorderLevel = (short)product.ReorderLevel,
                QuantityPerUnit = product.QuantityPerUnit
            });

            return products;
        }
       
    }
}
