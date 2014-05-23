SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE CountryCustomersByMonth
	@Country VARCHAR(50),
	@FromDate VARCHAR(50),
	@ToDate VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT COUNT(Data.Customers) AS Value, Data.Date FROM (
		SELECT 1 AS Customers, DATEFROMPARTS(DATEPART(YEAR, Orders.OrderDate), DATEPART(MONTH, Orders.OrderDate), 1) AS Date
		FROM Orders
		WHERE Orders.ShipCountry = @Country AND Orders.OrderDate >=  CONVERT(DATE, @FromDate,112) AND Orders.OrderDate <=  CONVERT(DATE, @ToDate,112)
		GROUP BY Orders.CustomerID, DATEFROMPARTS(DATEPART(YEAR, Orders.OrderDate), DATEPART(MONTH, Orders.OrderDate), 1)
	) AS Data 
	GROUP BY Data.Date
END
GO
