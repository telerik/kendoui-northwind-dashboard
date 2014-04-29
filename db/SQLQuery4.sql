SELECT Orders.OrderDate, SUM(Details.Amount) AS Amount FROM Orders 
INNER JOIN (
	SELECT Orders.OrderID, sum((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Amount FROM [Order Details] AS Details
	INNER JOIN Orders ON Orders.OrderID = Details.OrderID
	GROUP BY Orders.OrderID
) AS Details  ON Orders.OrderID = Details.OrderID 
GROUP BY Orders.OrderDate