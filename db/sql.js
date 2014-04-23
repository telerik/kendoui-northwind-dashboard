SELECT Orders.OrderID, Orders.EmployeeID, Orders.OrderDate, Details.Amount FROM Orders 
INNER JOIN (
	SELECT Orders.OrderID, sum((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Amount FROM [Order Details] AS Details
	INNER JOIN Orders ON Orders.OrderID = Details.OrderID
	GROUP BY Orders.OrderID
) AS Details  ON Orders.OrderID = Details.OrderID



SELECT Orders.EmployeeID, Orders.OrderDate, SUM(Details.Amount) AS Amount FROM Orders 
INNER JOIN (
	SELECT Orders.OrderID, sum((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Amount FROM [Order Details] AS Details
	INNER JOIN Orders ON Orders.OrderID = Details.OrderID
	GROUP BY Orders.OrderID
) AS Details  ON Orders.OrderID = Details.OrderID 
GROUP BY Orders.OrderDate, EmployeeID



ALL SALES

SELECT Orders.OrderDate, SUM(Details.Amount) AS Amount FROM Orders 
INNER JOIN (
	SELECT Orders.OrderID, sum((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Amount FROM [Order Details] AS Details
	INNER JOIN Orders ON Orders.OrderID = Details.OrderID
	GROUP BY Orders.OrderID
) AS Details  ON Orders.OrderID = Details.OrderID 
GROUP BY Orders.OrderDate



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
		WHERE Orders.EmployeeID = 3
		GROUP BY Orders.OrderDate
	) AS EmployeeSales ON AllSales.Date = EmployeeSales.Date  
