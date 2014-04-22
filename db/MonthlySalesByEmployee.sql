SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO 
CREATE PROCEDURE MonthlySalesByEmployee
	@EmployeeID INT
AS
BEGIN 
	SET NOCOUNT ON;

		SELECT  Orders.EmployeeID, 
				SUM((Quantity * UnitPrice) - (Quantity * UnitPrice * Discount))  AS Amount,  
				DATEFROMPARTS(DATEPART(YEAR, Orders.OrderDate), DATEPART(MONTH, Orders.OrderDate), 1) AS Date
		FROM [Order Details]
			INNER JOIN Orders ON Orders.OrderID = [Order Details].OrderID
		WHERE Orders.EmployeeID = @EmployeeID
		GROUP BY Orders.EmployeeID, DATEFROMPARTS(DATEPART(YEAR, Orders.OrderDate), DATEPART(MONTH, Orders.OrderDate), 1) 
	END
GO
