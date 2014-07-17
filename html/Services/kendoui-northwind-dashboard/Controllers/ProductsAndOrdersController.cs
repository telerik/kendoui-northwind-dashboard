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

        public List<ProductsSalesByMonth_Result> GetProductSalesByMonth(int ProductID)
        {
            var northwind = new NorthwindEntities();
            List<ProductsSalesByMonth_Result> result = northwind.ProductsSalesByMonth(ProductID).ToList();
            return result;
        }

        public List<EmployeeDetailViewModel> GetEmployeeDetails()
        {
            var northwind = new NorthwindEntities();
            List<Employee> employees = northwind.Employees.ToList();
            List<EmployeeDetailViewModel> result = new List<EmployeeDetailViewModel>();

            foreach (Employee emp in employees)
            {
                EmployeeDetailViewModel employeeDetails = new EmployeeDetailViewModel
                {
                    value = emp.EmployeeID,
                    text = emp.FirstName + " " + emp.LastName
                };
                result.Add(employeeDetails);
            }
            return result;
        }

        public List<ShipperDetailViewModel> GetShipperDetails()
        {
            var northwind = new NorthwindEntities();
            List<Shipper> shippers = northwind.Shippers.ToList();
            List<ShipperDetailViewModel> result = new List<ShipperDetailViewModel>();

            foreach (Shipper shipper in shippers)
            {
                ShipperDetailViewModel shipperDetails = new ShipperDetailViewModel
                {
                    value = shipper.ShipperID,
                    text = shipper.CompanyName
                };
                result.Add(shipperDetails);
            }
            return result;
        }

        public List<ProductDetailsViewModel> GetProductDetails()
        {
            var northwind = new NorthwindEntities();
            List<Product> products = northwind.Products.ToList();
            List<ProductDetailsViewModel> result = new List<ProductDetailsViewModel>();

            foreach (Product product in products)
            {
                ProductDetailsViewModel productDetails = new ProductDetailsViewModel
                {
                    value = product.ProductID,
                    text = product.ProductName
                };
                result.Add(productDetails);
            }
            return result;
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
