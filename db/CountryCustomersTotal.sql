SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE CountryCustomersTotal
	@Country VARCHAR(50),
	@FromDate VARCHAR(50),
	@ToDate VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT COUNT(C.CustomerID) AS Value FROM 
	(
		SELECT Orders.CustomerID
		FROM Orders
		WHERE Orders.ShipCountry = @Country AND Orders.OrderDate >=  CONVERT(DATE, @FromDate,112) AND Orders.OrderDate <=  CONVERT(DATE, @ToDate,112)
		GROUP By Orders.CustomerID
	) AS C

END
GO
