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
     
SELECT AllSales.EmployeeID, AllSales.Date, AllSales.TotalSales, EmployeeSales.EmployeeSales from 
    (SELECT Orders.EmployeeID, Orders.OrderDate AS Date, SUM(Details.Amount) AS TotalSales FROM Orders 
        INNER JOIN (
            SELECT  Orders.OrderID, SUM((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Amount FROM [Order Details]
            INNER JOIN Orders ON Orders.OrderID = [Order Details].OrderID
            GROUP BY Orders.OrderID
        ) AS Details  ON Orders.OrderID = Details.OrderID 
        GROUP BY Orders.OrderDate, Orders.EmployeeID
    ) as AllSales
    LEFT OUTER JOIN
    (SELECT Orders.OrderDate AS Date, SUM(Details.Amount) AS EmployeeSales FROM Orders 
        INNER JOIN (
            SELECT Orders.OrderID, SUM((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Amount FROM [Order Details]
            INNER JOIN Orders ON Orders.OrderID = [Order Details].OrderID
            GROUP BY Orders.OrderID
        ) AS Details  ON Orders.OrderID = Details.OrderID 
        WHERE Orders.EmployeeID = @EmployeeID
        GROUP BY Orders.OrderDate
    ) AS EmployeeSales ON AllSales.Date = EmployeeSales.Date
END
