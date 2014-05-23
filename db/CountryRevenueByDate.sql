SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE CountryRevenueByDate
	@Country VARCHAR(50),
	@FromDate VARCHAR(50),
	@ToDate VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Orders.OrderDate As Date, COUNT(Orders.OrderID) AS Value
	FROM Orders
	WHERE Orders.ShipCountry = @Country AND Orders.OrderDate >=  CONVERT(DATE, @FromDate,112) AND Orders.OrderDate <=  CONVERT(DATE, @ToDate,112)
	GROUP BY Orders.OrderDate
END
GO
