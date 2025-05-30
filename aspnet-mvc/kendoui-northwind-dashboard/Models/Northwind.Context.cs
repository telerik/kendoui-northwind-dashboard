﻿namespace KendoUI.Northwind.Dashboard.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Core.Objects.DataClasses;
    using System.Linq;
    using System.Data.SQLite;

    public partial class NorthwindEntities : DbContext
    {
        public NorthwindEntities()
            : base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder()
                {
                    DataSource = System.IO.Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "northwind.db"),
                    ForeignKeys = true
                }.ConnectionString
            }, true)
        {
            Database.SetInitializer<NorthwindEntities>(null);
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
        }
    
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Territory> Territories { get; set; }
        public virtual DbSet<CustomerDemographic> CustomerDemographics { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }

        
    
        public virtual ObjectResult<CustOrderHist_Result> CustOrderHist(string customerID)
        {
            var customerIDParameter = customerID != null ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CustOrderHist_Result>("CustOrderHist", customerIDParameter);
        }
    
        public virtual ObjectResult<CustOrdersDetail_Result> CustOrdersDetail(Nullable<int> orderID)
        {
            var orderIDParameter = orderID.HasValue ?
                new ObjectParameter("OrderID", orderID) :
                new ObjectParameter("OrderID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CustOrdersDetail_Result>("CustOrdersDetail", orderIDParameter);
        }
    
        public virtual ObjectResult<CustOrdersOrders_Result> CustOrdersOrders(string customerID)
        {
            var customerIDParameter = customerID != null ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CustOrdersOrders_Result>("CustOrdersOrders", customerIDParameter);
        }
    
        public virtual ObjectResult<Employee_Sales_by_Country_Result> Employee_Sales_by_Country(Nullable<System.DateTime> beginning_Date, Nullable<System.DateTime> ending_Date)
        {
            var beginning_DateParameter = beginning_Date.HasValue ?
                new ObjectParameter("Beginning_Date", beginning_Date) :
                new ObjectParameter("Beginning_Date", typeof(System.DateTime));
    
            var ending_DateParameter = ending_Date.HasValue ?
                new ObjectParameter("Ending_Date", ending_Date) :
                new ObjectParameter("Ending_Date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Employee_Sales_by_Country_Result>("Employee_Sales_by_Country", beginning_DateParameter, ending_DateParameter);
        }
    
        public virtual ObjectResult<Sales_by_Year_Result> Sales_by_Year(Nullable<System.DateTime> beginning_Date, Nullable<System.DateTime> ending_Date)
        {
            var beginning_DateParameter = beginning_Date.HasValue ?
                new ObjectParameter("Beginning_Date", beginning_Date) :
                new ObjectParameter("Beginning_Date", typeof(System.DateTime));
    
            var ending_DateParameter = ending_Date.HasValue ?
                new ObjectParameter("Ending_Date", ending_Date) :
                new ObjectParameter("Ending_Date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sales_by_Year_Result>("Sales_by_Year", beginning_DateParameter, ending_DateParameter);
        }
    
        public virtual ObjectResult<SalesByCategory_Result> SalesByCategory(string categoryName, string ordYear)
        {
            var categoryNameParameter = categoryName != null ?
                new ObjectParameter("CategoryName", categoryName) :
                new ObjectParameter("CategoryName", typeof(string));
    
            var ordYearParameter = ordYear != null ?
                new ObjectParameter("OrdYear", ordYear) :
                new ObjectParameter("OrdYear", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SalesByCategory_Result>("SalesByCategory", categoryNameParameter, ordYearParameter);
        }
    
        public virtual ObjectResult<Ten_Most_Expensive_Products_Result> Ten_Most_Expensive_Products()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Ten_Most_Expensive_Products_Result>("Ten_Most_Expensive_Products");
        }
    
        public virtual ObjectResult<ProductsSalesByMonth_Result> ProductsSalesByMonth(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ProductsSalesByMonth_Result>("ProductsSalesByMonth", productIDParameter);
        }
    
        public virtual ObjectResult<CountryCustomers_Result> CountryCustomers(string country, string fromDate, string toDate)
        {
            var countryParameter = country != null ?
                new ObjectParameter("Country", country) :
                new ObjectParameter("Country", typeof(string));
    
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CountryCustomers_Result>("CountryCustomers", countryParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> CountryCustomersTotal(string country, string fromDate, string toDate)
        {
            var countryParameter = country != null ?
                new ObjectParameter("Country", country) :
                new ObjectParameter("Country", typeof(string));
    
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("CountryCustomersTotal", countryParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<CountryMarketShare_Result> CountryMarketShare(string country, string fromDate, string toDate)
        {
            var countryParameter = country != null ?
                new ObjectParameter("Country", country) :
                new ObjectParameter("Country", typeof(string));
    
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CountryMarketShare_Result>("CountryMarketShare", countryParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<CountryOrders_Result> CountryOrders(string country, string fromDate, string toDate)
        {
            var countryParameter = country != null ?
                new ObjectParameter("Country", country) :
                new ObjectParameter("Country", typeof(string));
    
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CountryOrders_Result>("CountryOrders", countryParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<CountryRevenue_Result> CountryRevenue(string country, string fromDate, string toDate)
        {
            var countryParameter = country != null ?
                new ObjectParameter("Country", country) :
                new ObjectParameter("Country", typeof(string));
    
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CountryRevenue_Result>("CountryRevenue", countryParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<CountryTopProducts_Result> CountryTopProducts(string country, string fromDate, string toDate)
        {
            var countryParameter = country != null ?
                new ObjectParameter("Country", country) :
                new ObjectParameter("Country", typeof(string));
    
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CountryTopProducts_Result>("CountryTopProducts", countryParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<SalesAmounts_Result> SalesAmounts(Nullable<int> employeeID)
        {
            var employeeIDParameter = employeeID.HasValue ?
                new ObjectParameter("EmployeeID", employeeID) :
                new ObjectParameter("EmployeeID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SalesAmounts_Result>("SalesAmounts", employeeIDParameter);
        }
    
        public virtual ObjectResult<MonthlySalesByEmployee_Result> MonthlySalesByEmployee(Nullable<int> employeeID)
        {
            var employeeIDParameter = employeeID.HasValue ?
                new ObjectParameter("EmployeeID", employeeID) :
                new ObjectParameter("EmployeeID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MonthlySalesByEmployee_Result>("MonthlySalesByEmployee", employeeIDParameter);
        }
    }
}
