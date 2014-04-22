USE [C:\REPOS\KENDO-DASHBOARD\ASPNET-MVC\KENDOUI-NORTHWIND-DASHBOARD\APP_DATA\NORTHWIND.MDF]
GO
/****** Object:  StoredProcedure [dbo].[SalesAmounts2]    Script Date: 4/15/2014 17:46:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON 
GO
ALTER PROCEDURE [dbo].[SalesAmounts2]
    @EmployeeID INT
AS
BEGIN 
    SET NOCOUNT ON;
     
SELECT AllSales.TotalSales AS TotalSales, EmployeeSales.EmployeeSales AS EmployeeSales, AllSales.Date from   
        (SELECT Sales.Date, SUM(Sales.EmployeeSales) AS TotalSales  FROM
            (
                SELECT  Orders.EmployeeID, 
                        SUM((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS EmployeeSales,  
                        DATEFROMPARTS(DATEPART(YEAR, Orders.OrderDate), DATEPART(MONTH, Orders.OrderDate), 1) AS Date
                FROM [Order Details]
                    INNER JOIN Orders ON Orders.OrderID = [Order Details].OrderID
                GROUP BY Orders.EmployeeID, DATEFROMPARTS(DATEPART(YEAR, Orders.OrderDate), DATEPART(MONTH, Orders.OrderDate), 1) 
            ) AS Sales 
            GROUP BY Sales.Date
        ) AS AllSales 
    LEFT OUTER JOIN
        (SELECT  Orders.EmployeeID, 
                SUM((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS EmployeeSales,  
                DATEFROMPARTS(DATEPART(YEAR, Orders.OrderDate), DATEPART(MONTH, Orders.OrderDate), 1) AS Date
        FROM [Order Details]
            INNER JOIN Orders ON Orders.OrderID = [Order Details].OrderID
        WHERE Orders.EmployeeID = @EmployeeID
        GROUP BY Orders.EmployeeID, DATEFROMPARTS(DATEPART(YEAR, Orders.OrderDate), DATEPART(MONTH, Orders.OrderDate), 1) 
        ) AS EmployeeSales  
    ON AllSales.Date = EmployeeSales.Date
END
