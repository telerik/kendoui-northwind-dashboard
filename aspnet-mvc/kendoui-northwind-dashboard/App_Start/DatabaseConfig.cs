using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;

namespace KendoUI.Northwind.Dashboard.App_Start
{
    public static class DatabaseConfig
    {
        public static void Initialize()
        {
            try
            {
                // Ensure the App_Data directory exists
                string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                string sqliteDbPath = Path.Combine(dataDirectory, "northwind.db");

                if (!Directory.Exists(dataDirectory))
                {
                    Directory.CreateDirectory(dataDirectory);
                }

                if (!File.Exists(sqliteDbPath))
                {
                    SQLiteConnection.CreateFile(sqliteDbPath);
                    System.Diagnostics.Debug.WriteLine("SQLite database created successfully.");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Database file already exists.");
                }

                CreateSQLiteSchema(sqliteDbPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Initialization Error: {ex.Message}");
                throw;
            }
        }

        private static void CreateSQLiteSchema(string sqliteDbPath)
        {
            using (SQLiteConnection sqliteConn = new SQLiteConnection($"Data Source={sqliteDbPath}"))
            {
                sqliteConn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = sqliteConn;

                    try
                    {
                        // Create Products table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS Products (
                                ProductID INTEGER PRIMARY KEY, 
                                ProductName TEXT, 
                                SupplierID INTEGER, 
                                CategoryID INTEGER, 
                                QuantityPerUnit TEXT, 
                                UnitPrice REAL, 
                                UnitsInStock INTEGER, 
                                UnitsOnOrder INTEGER, 
                                ReorderLevel INTEGER, 
                                Discontinued INTEGER)";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("Products table created.");

                        // Create Customers table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS Customers (
                                CustomerID TEXT PRIMARY KEY, 
                                CompanyName TEXT, 
                                ContactName TEXT, 
                                ContactTitle TEXT, 
                                Address TEXT, 
                                City TEXT, 
                                Region TEXT, 
                                PostalCode TEXT, 
                                Country TEXT, 
                                Phone TEXT, 
                                Fax TEXT)";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("Customers table created.");

                        // Create Orders table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS Orders (
                                OrderID INTEGER PRIMARY KEY, 
                                CustomerID TEXT, 
                                EmployeeID INTEGER, 
                                OrderDate TEXT, 
                                RequiredDate TEXT, 
                                ShippedDate TEXT, 
                                ShipVia INTEGER, 
                                Freight REAL, 
                                ShipName TEXT, 
                                ShipAddress TEXT, 
                                ShipCity TEXT, 
                                ShipRegion TEXT, 
                                ShipPostalCode TEXT, 
                                ShipCountry TEXT)";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("Orders table created.");

                        // Create OrderDetails table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS OrderDetails (
                                OrderID INTEGER, 
                                ProductID INTEGER, 
                                UnitPrice REAL, 
                                Quantity INTEGER, 
                                Discount REAL,
                                PRIMARY KEY (OrderID, ProductID))";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("OrderDetails table created.");

                        // Create Employees table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS Employees (
                                EmployeeID INTEGER PRIMARY KEY, 
                                LastName TEXT, 
                                FirstName TEXT, 
                                Title TEXT, 
                                TitleOfCourtesy TEXT, 
                                BirthDate TEXT, 
                                HireDate TEXT, 
                                Address TEXT, 
                                City TEXT, 
                                Region TEXT, 
                                PostalCode TEXT, 
                                Country TEXT, 
                                HomePhone TEXT, 
                                Extension TEXT, 
                                Photo BLOB, 
                                Notes TEXT, 
                                ReportsTo INTEGER, 
                                PhotoPath TEXT)";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("Employees table created.");

                        // Create Categories table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS Categories (
                                CategoryID INTEGER PRIMARY KEY, 
                                CategoryName TEXT, 
                                Description TEXT, 
                                Picture BLOB)";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("Categories table created.");

                        // Create Suppliers table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS Suppliers (
                                SupplierID INTEGER PRIMARY KEY, 
                                CompanyName TEXT, 
                                ContactName TEXT, 
                                ContactTitle TEXT, 
                                Address TEXT, 
                                City TEXT, 
                                Region TEXT, 
                                PostalCode TEXT, 
                                Country TEXT, 
                                Phone TEXT, 
                                Fax TEXT, 
                                HomePage TEXT)";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("Suppliers table created.");

                        // Create Shippers table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS Shippers (
                                ShipperID INTEGER PRIMARY KEY, 
                                CompanyName TEXT, 
                                Phone TEXT)";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("Shippers table created.");

                        // Create Region table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS Region (
                                RegionID INTEGER PRIMARY KEY, 
                                RegionDescription TEXT)";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("Region table created.");

                        // Create Territories table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS Territories (
                                TerritoryID TEXT PRIMARY KEY, 
                                TerritoryDescription TEXT, 
                                RegionID INTEGER)";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("Territories table created.");

                        // Create CustomerDemographics table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS CustomerDemographics (
                                CustomerTypeID TEXT PRIMARY KEY, 
                                CustomerDesc TEXT)";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("CustomerDemographics table created.");

                        // Create CustomerCustomerDemo table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS CustomerCustomerDemo (
                                CustomerID TEXT, 
                                CustomerTypeID TEXT, 
                                PRIMARY KEY (CustomerID, CustomerTypeID))";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("CustomerCustomerDemo table created.");

                        // Create EmployeeTerritories table
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS EmployeeTerritories (
                                EmployeeID INTEGER, 
                                TerritoryID TEXT, 
                                PRIMARY KEY (EmployeeID, TerritoryID))";
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("EmployeeTerritories table created.");
                    }
                    catch (SQLiteException ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"SQLite Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public static void MigrateData()
        {
            try
            {
                string sqlServerConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "NORTHWND.MDF")};Integrated Security=True;";
                string sqliteConnectionString = $"Data Source={Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "northwind.db")}";

                using (SqlConnection sqlConn = new SqlConnection(sqlServerConnectionString))
                {
                    sqlConn.Open();
                    System.Diagnostics.Debug.WriteLine("SQL Server connection opened successfully.");

                    using (SQLiteConnection sqliteConn = new SQLiteConnection(sqliteConnectionString))
                    {
                        sqliteConn.Open();
                        System.Diagnostics.Debug.WriteLine("SQLite connection opened successfully.");

                        using (SQLiteTransaction transaction = sqliteConn.BeginTransaction())
                        {
                            MigrateProducts(sqlConn, sqliteConn);
                            MigrateCustomers(sqlConn, sqliteConn);
                            MigrateOrders(sqlConn, sqliteConn);
                            MigrateOrderDetails(sqlConn, sqliteConn);
                            MigrateEmployees(sqlConn, sqliteConn);
                            MigrateCategories(sqlConn, sqliteConn);
                            MigrateSuppliers(sqlConn, sqliteConn);
                            MigrateShippers(sqlConn, sqliteConn);
                            MigrateRegions(sqlConn, sqliteConn);
                            MigrateTerritories(sqlConn, sqliteConn);
                            MigrateCustomerDemographics(sqlConn, sqliteConn);
                            MigrateCustomerCustomerDemo(sqlConn, sqliteConn);
                            MigrateEmployeeTerritories(sqlConn, sqliteConn);

                            transaction.Commit();
                            System.Diagnostics.Debug.WriteLine("Data migration completed successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Migration Error: {ex.Message}");
                throw;
            }
        }

        private static void MigrateProducts(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued FROM Products", sqlConn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Products WHERE ProductID = @ProductID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@ProductID", reader["ProductID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Only insert if the record does not already exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO Products (ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued) " +
                                "VALUES (@ProductID, @ProductName, @SupplierID, @CategoryID, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@ProductID", reader["ProductID"]);
                                insertCmd.Parameters.AddWithValue("@ProductName", reader["ProductName"]);
                                insertCmd.Parameters.AddWithValue("@SupplierID", reader["SupplierID"]);
                                insertCmd.Parameters.AddWithValue("@CategoryID", reader["CategoryID"]);
                                insertCmd.Parameters.AddWithValue("@QuantityPerUnit", reader["QuantityPerUnit"]);
                                insertCmd.Parameters.AddWithValue("@UnitPrice", reader["UnitPrice"]);
                                insertCmd.Parameters.AddWithValue("@UnitsInStock", reader["UnitsInStock"]);
                                insertCmd.Parameters.AddWithValue("@UnitsOnOrder", reader["UnitsOnOrder"]);
                                insertCmd.Parameters.AddWithValue("@ReorderLevel", reader["ReorderLevel"]);
                                insertCmd.Parameters.AddWithValue("@Discontinued", reader["Discontinued"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Duplicate ProductID found: {reader["ProductID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }



        private static void MigrateCustomers(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax FROM Customers", sqlConn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Customers WHERE CustomerID = @CustomerID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@CustomerID", reader["CustomerID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Only insert if the record does not already exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO Customers (CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax) " +
                                "VALUES (@CustomerID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@CustomerID", reader["CustomerID"]);
                                insertCmd.Parameters.AddWithValue("@CompanyName", reader["CompanyName"]);
                                insertCmd.Parameters.AddWithValue("@ContactName", reader["ContactName"]);
                                insertCmd.Parameters.AddWithValue("@ContactTitle", reader["ContactTitle"]);
                                insertCmd.Parameters.AddWithValue("@Address", reader["Address"]);
                                insertCmd.Parameters.AddWithValue("@City", reader["City"]);
                                insertCmd.Parameters.AddWithValue("@Region", reader["Region"]);
                                insertCmd.Parameters.AddWithValue("@PostalCode", reader["PostalCode"]);
                                insertCmd.Parameters.AddWithValue("@Country", reader["Country"]);
                                insertCmd.Parameters.AddWithValue("@Phone", reader["Phone"]);
                                insertCmd.Parameters.AddWithValue("@Fax", reader["Fax"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Duplicate CustomerID found: {reader["CustomerID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }


        private static void MigrateOrders(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT OrderID, CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry FROM Orders", sqlConn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Orders WHERE OrderID = @OrderID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@OrderID", reader["OrderID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Only insert if the record does not already exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO Orders (OrderID, CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry) " +
                                "VALUES (@OrderID, @CustomerID, @EmployeeID, @OrderDate, @RequiredDate, @ShippedDate, @ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@OrderID", reader["OrderID"]);
                                insertCmd.Parameters.AddWithValue("@CustomerID", reader["CustomerID"]);
                                insertCmd.Parameters.AddWithValue("@EmployeeID", reader["EmployeeID"]);
                                insertCmd.Parameters.AddWithValue("@OrderDate", reader["OrderDate"]);
                                insertCmd.Parameters.AddWithValue("@RequiredDate", reader["RequiredDate"]);
                                insertCmd.Parameters.AddWithValue("@ShippedDate", reader["ShippedDate"]);
                                insertCmd.Parameters.AddWithValue("@ShipVia", reader["ShipVia"]);
                                insertCmd.Parameters.AddWithValue("@Freight", reader["Freight"]);
                                insertCmd.Parameters.AddWithValue("@ShipName", reader["ShipName"]);
                                insertCmd.Parameters.AddWithValue("@ShipAddress", reader["ShipAddress"]);
                                insertCmd.Parameters.AddWithValue("@ShipCity", reader["ShipCity"]);
                                insertCmd.Parameters.AddWithValue("@ShipRegion", reader["ShipRegion"]);
                                insertCmd.Parameters.AddWithValue("@ShipPostalCode", reader["ShipPostalCode"]);
                                insertCmd.Parameters.AddWithValue("@ShipCountry", reader["ShipCountry"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Duplicate OrderID found: {reader["OrderID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }


        private static void MigrateOrderDetails(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT OrderID, ProductID, UnitPrice, Quantity, Discount FROM [Order Details]", sqlConn)) // Use correct table name
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM OrderDetails WHERE OrderID = @OrderID AND ProductID = @ProductID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@OrderID", reader["OrderID"]);
                        checkCmd.Parameters.AddWithValue("@ProductID", reader["ProductID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Only insert if the record does not already exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO OrderDetails (OrderID, ProductID, UnitPrice, Quantity, Discount) " +
                                "VALUES (@OrderID, @ProductID, @UnitPrice, @Quantity, @Discount)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@OrderID", reader["OrderID"]);
                                insertCmd.Parameters.AddWithValue("@ProductID", reader["ProductID"]);
                                insertCmd.Parameters.AddWithValue("@UnitPrice", reader["UnitPrice"]);
                                insertCmd.Parameters.AddWithValue("@Quantity", reader["Quantity"]);
                                insertCmd.Parameters.AddWithValue("@Discount", reader["Discount"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Duplicate OrderDetail found for OrderID: {reader["OrderID"]} and ProductID: {reader["ProductID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }



        private static void MigrateEmployees(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT EmployeeID, LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Photo, Notes, ReportsTo, PhotoPath FROM Employees", sqlConn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Employees WHERE EmployeeID = @EmployeeID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@EmployeeID", reader["EmployeeID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Only insert if the record does not already exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO Employees (EmployeeID, LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Photo, Notes, ReportsTo, PhotoPath) " +
                                "VALUES (@EmployeeID, @LastName, @FirstName, @Title, @TitleOfCourtesy, @BirthDate, @HireDate, @Address, @City, @Region, @PostalCode, @Country, @HomePhone, @Extension, @Photo, @Notes, @ReportsTo, @PhotoPath)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@EmployeeID", reader["EmployeeID"]);
                                insertCmd.Parameters.AddWithValue("@LastName", reader["LastName"]);
                                insertCmd.Parameters.AddWithValue("@FirstName", reader["FirstName"]);
                                insertCmd.Parameters.AddWithValue("@Title", reader["Title"]);
                                insertCmd.Parameters.AddWithValue("@TitleOfCourtesy", reader["TitleOfCourtesy"]);
                                insertCmd.Parameters.AddWithValue("@BirthDate", reader["BirthDate"]);
                                insertCmd.Parameters.AddWithValue("@HireDate", reader["HireDate"]);
                                insertCmd.Parameters.AddWithValue("@Address", reader["Address"]);
                                insertCmd.Parameters.AddWithValue("@City", reader["City"]);
                                insertCmd.Parameters.AddWithValue("@Region", reader["Region"]);
                                insertCmd.Parameters.AddWithValue("@PostalCode", reader["PostalCode"]);
                                insertCmd.Parameters.AddWithValue("@Country", reader["Country"]);
                                insertCmd.Parameters.AddWithValue("@HomePhone", reader["HomePhone"]);
                                insertCmd.Parameters.AddWithValue("@Extension", reader["Extension"]);
                                insertCmd.Parameters.AddWithValue("@Photo", reader["Photo"]);
                                insertCmd.Parameters.AddWithValue("@Notes", reader["Notes"]);
                                insertCmd.Parameters.AddWithValue("@ReportsTo", reader["ReportsTo"]);
                                insertCmd.Parameters.AddWithValue("@PhotoPath", reader["PhotoPath"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Duplicate EmployeeID found: {reader["EmployeeID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }


        private static void MigrateCategories(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT CategoryID, CategoryName, Description, Picture FROM Categories", sqlConn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Prepare the SQL command to check for existing records
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Categories WHERE CategoryID = @CategoryID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@CategoryID", reader["CategoryID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Only insert if the record does not already exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO Categories (CategoryID, CategoryName, Description, Picture) " +
                                "VALUES (@CategoryID, @CategoryName, @Description, @Picture)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@CategoryID", reader["CategoryID"]);
                                insertCmd.Parameters.AddWithValue("@CategoryName", reader["CategoryName"]);
                                insertCmd.Parameters.AddWithValue("@Description", reader["Description"]);
                                insertCmd.Parameters.AddWithValue("@Picture", reader["Picture"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Duplicate CategoryID found: {reader["CategoryID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }


        private static void MigrateSuppliers(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT SupplierID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, HomePage FROM Suppliers", sqlConn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Check if the SupplierID already exists in the SQLite database
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Suppliers WHERE SupplierID = @SupplierID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@SupplierID", reader["SupplierID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Only insert if the record does not already exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO Suppliers (SupplierID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, HomePage) " +
                                "VALUES (@SupplierID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax, @HomePage)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@SupplierID", reader["SupplierID"]);
                                insertCmd.Parameters.AddWithValue("@CompanyName", reader["CompanyName"]);
                                insertCmd.Parameters.AddWithValue("@ContactName", reader["ContactName"]);
                                insertCmd.Parameters.AddWithValue("@ContactTitle", reader["ContactTitle"]);
                                insertCmd.Parameters.AddWithValue("@Address", reader["Address"]);
                                insertCmd.Parameters.AddWithValue("@City", reader["City"]);
                                insertCmd.Parameters.AddWithValue("@Region", reader["Region"]);
                                insertCmd.Parameters.AddWithValue("@PostalCode", reader["PostalCode"]);
                                insertCmd.Parameters.AddWithValue("@Country", reader["Country"]);
                                insertCmd.Parameters.AddWithValue("@Phone", reader["Phone"]);
                                insertCmd.Parameters.AddWithValue("@Fax", reader["Fax"]);
                                insertCmd.Parameters.AddWithValue("@HomePage", reader["HomePage"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Log the existing record
                            System.Diagnostics.Debug.WriteLine($"Duplicate SupplierID found: {reader["SupplierID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }


        private static void MigrateShippers(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT ShipperID, CompanyName, Phone FROM Shippers", sqlConn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Check if the ShipperID already exists in the SQLite database
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Shippers WHERE ShipperID = @ShipperID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@ShipperID", reader["ShipperID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Only insert if the record does not already exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO Shippers (ShipperID, CompanyName, Phone) " +
                                "VALUES (@ShipperID, @CompanyName, @Phone)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@ShipperID", reader["ShipperID"]);
                                insertCmd.Parameters.AddWithValue("@CompanyName", reader["CompanyName"]);
                                insertCmd.Parameters.AddWithValue("@Phone", reader["Phone"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Log the existing record
                            System.Diagnostics.Debug.WriteLine($"Duplicate ShipperID found: {reader["ShipperID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }


        private static void MigrateRegions(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT RegionID, RegionDescription FROM Region", sqlConn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Regions WHERE RegionID = @RegionID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@RegionID", reader["RegionID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Insert only if the record does not exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO Regions (RegionID, RegionDescription) " +
                                "VALUES (@RegionID, @RegionDescription)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@RegionID", reader["RegionID"]);
                                insertCmd.Parameters.AddWithValue("@RegionDescription", reader["RegionDescription"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Duplicate RegionID found: {reader["RegionID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }


        private static void MigrateTerritories(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT TerritoryID, TerritoryDescription, RegionID FROM Territories", sqlConn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Territories WHERE TerritoryID = @TerritoryID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@TerritoryID", reader["TerritoryID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Insert only if the record does not exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO Territories (TerritoryID, TerritoryDescription, RegionID) " +
                                "VALUES (@TerritoryID, @TerritoryDescription, @RegionID)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@TerritoryID", reader["TerritoryID"]);
                                insertCmd.Parameters.AddWithValue("@TerritoryDescription", reader["TerritoryDescription"]);
                                insertCmd.Parameters.AddWithValue("@RegionID", reader["RegionID"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Duplicate TerritoryID found: {reader["TerritoryID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }


        private static void MigrateCustomerDemographics(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT CustomerTypeID, CustomerDesc FROM CustomerDemographics", sqlConn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM CustomerDemographics WHERE CustomerTypeID = @CustomerTypeID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@CustomerTypeID", reader["CustomerTypeID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Insert only if the record does not exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO CustomerDemographics (CustomerTypeID, CustomerDesc) " +
                                "VALUES (@CustomerTypeID, @CustomerDesc)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@CustomerTypeID", reader["CustomerTypeID"]);
                                insertCmd.Parameters.AddWithValue("@CustomerDesc", reader["CustomerDesc"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Duplicate CustomerTypeID found: {reader["CustomerTypeID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }


        private static void MigrateCustomerCustomerDemo(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT CustomerID, CustomerTypeID FROM CustomerCustomerDemo", sqlConn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM CustomerCustomerDemo WHERE CustomerID = @CustomerID AND CustomerTypeID = @CustomerTypeID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@CustomerID", reader["CustomerID"]);
                        checkCmd.Parameters.AddWithValue("@CustomerTypeID", reader["CustomerTypeID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Insert only if the record does not exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO CustomerCustomerDemo (CustomerID, CustomerTypeID) " +
                                "VALUES (@CustomerID, @CustomerTypeID)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@CustomerID", reader["CustomerID"]);
                                insertCmd.Parameters.AddWithValue("@CustomerTypeID", reader["CustomerTypeID"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Duplicate CustomerID and CustomerTypeID pair found: {reader["CustomerID"]}, {reader["CustomerTypeID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }


        private static void MigrateEmployeeTerritories(SqlConnection sqlConn, SQLiteConnection sqliteConn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT EmployeeID, TerritoryID FROM EmployeeTerritories", sqlConn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM EmployeeTerritories WHERE EmployeeID = @EmployeeID AND TerritoryID = @TerritoryID", sqliteConn))
                    {
                        checkCmd.Parameters.AddWithValue("@EmployeeID", reader["EmployeeID"]);
                        checkCmd.Parameters.AddWithValue("@TerritoryID", reader["TerritoryID"]);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0) // Insert only if the record does not exist
                        {
                            using (SQLiteCommand insertCmd = new SQLiteCommand(
                                "INSERT INTO EmployeeTerritories (EmployeeID, TerritoryID) " +
                                "VALUES (@EmployeeID, @TerritoryID)", sqliteConn))
                            {
                                insertCmd.Parameters.AddWithValue("@EmployeeID", reader["EmployeeID"]);
                                insertCmd.Parameters.AddWithValue("@TerritoryID", reader["TerritoryID"]);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Duplicate EmployeeID and TerritoryID pair found: {reader["EmployeeID"]}, {reader["TerritoryID"]}, skipping insertion.");
                        }
                    }
                }
            }
        }

    }
}