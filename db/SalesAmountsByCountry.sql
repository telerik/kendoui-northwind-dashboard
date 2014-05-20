SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SalesAmountsByCountry  
AS
BEGIN 
	SET NOCOUNT ON; 
		SELECT  Orders.ShipCountry, 
				SUM((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Sales,  
				DATEFROMPARTS(DATEPART(YEAR, Orders.OrderDate), DATEPART(MONTH, Orders.OrderDate), 1) AS Date
		FROM [Order Details]
			INNER JOIN Orders ON Orders.OrderID = [Order Details].OrderID
		GROUP BY Orders.ShipCountry, DATEFROMPARTS(DATEPART(YEAR, Orders.OrderDate), DATEPART(MONTH, Orders.OrderDate), 1) 
		
END
GO
