
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE ProductsSalesByMonth
	@ProductID INT
AS
BEGIN
	SELECT DATEFROMPARTS(DATEPART(YEAR, Orders.OrderDate), DATEPART(MONTH, Orders.OrderDate), 1) AS Date,  [Order Details].Quantity FROM Orders
	INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID
	WHERE [Order Details].ProductID = @ProductID
	GROUP BY DATEFROMPARTS(DATEPART(YEAR, Orders.OrderDate), DATEPART(MONTH, Orders.OrderDate), 1), [Order Details].Quantity
END
GO
