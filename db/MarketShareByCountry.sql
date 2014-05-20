SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE MarketShareByCountry
	@Country VARCHAR(50),
	@FromDate VARCHAR(50),
	@ToDate VARCHAR(50)
AS
BEGIN 
	SET NOCOUNT ON;
	SELECT 'All' AS Country, SUM(AllCountries.Sales) AS Sales FROM 
	( 
		SELECT SUM((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Sales 
		FROM [Order Details]
			INNER JOIN Orders ON Orders.OrderID = [Order Details].OrderID
		WHERE Orders.OrderDate >=  CONVERT(DATE, @FromDate,112) AND Orders.OrderDate <=  CONVERT(DATE, @ToDate,112)
		GROUP BY Orders.ShipCountry
	) AS AllCountries
 
	UNION

	SELECT  Orders.ShipCountry, SUM((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Sales 
	FROM [Order Details]
		INNER JOIN Orders ON Orders.OrderID = [Order Details].OrderID
	WHERE Orders.ShipCountry = @Country AND Orders.OrderDate >=  CONVERT(DATE, @FromDate,112) AND Orders.OrderDate <=  CONVERT(DATE, @ToDate,112)
	GROUP BY Orders.ShipCountry
END
GO
