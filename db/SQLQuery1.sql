SELECT EmployeeSales.Date, EmployeeSales.Amount AS E, AllSales.Amount AS A FROM (
	SELECT Orders.EmployeeID, Orders.OrderDate AS Date, SUM(Details.Amount) AS Amount FROM Orders 
	INNER JOIN (
		SELECT Orders.OrderID, sum((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Amount FROM [Order Details] AS Details
		INNER JOIN Orders ON Orders.OrderID = Details.OrderID
		GROUP BY Orders.OrderID
	) AS Details  ON Orders.OrderID = Details.OrderID 
	WHERE Orders.EmployeeID = 1
	GROUP BY Orders.OrderDate, EmployeeID
) AS EmployeeSales
LEFT OUTER JOIN 
(
	SELECT Orders.OrderDate AS Date, SUM(Details.Amount) AS Amount FROM Orders 
	INNER JOIN (
		SELECT Orders.OrderID, sum((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Amount FROM [Order Details] AS Details
		INNER JOIN Orders ON Orders.OrderID = Details.OrderID
		GROUP BY Orders.OrderID
	) AS Details  ON Orders.OrderID = Details.OrderID 
	GROUP BY Orders.OrderDate
) AS AllSales ON AllSales.Date = EmployeeSales.Date
GROUP BY EmployeeSales.Date, EmployeeSales.Amount, AllSales.Amount