--01. Find Names of All Employees by First Name

SELECT [FirstName], [LastName] FROM Employees
WHERE FirstName LIKE 'Sa%'

----------------------------------------------
--02. Find Names of All Employees by Last Name

SELECT [FirstName], [LastName] FROM Employees
WHERE LastName LIKE '%ei%'

---------------------------------------
--03. Find First Names of All Employess

SELECT [FirstName] FROM Employees
WHERE (DepartmentID = 3 OR DepartmentID = 10)
		AND (HireDate >= '1995/01/01' OR HireDate <= '2005/01/01')

-----------------------------------------
--04. Find All Employees Except Engineers

SELECT [FirstName], [LastName] FROM Employees
WHERE JobTitle NOT LIKE '%engineer%'

-----------------------------------
--05. Find Towns with Name Length

SELECT [Name] FROM Towns
WHERE LEN([Name]) = 5 OR LEN([Name]) = 6
ORDER BY [Name] ASC

-------------------------------
--06. Find Towns Starting With

SELECT [TownID], [Name] FROM Towns
WHERE SUBSTRING([Name], 1, 1) = 'M' OR 
	  SUBSTRING([Name], 1, 1) = 'K' OR
	  SUBSTRING([Name], 1, 1) = 'B' OR
	  SUBSTRING([Name], 1, 1) = 'E'
ORDER BY [Name] ASC

----------------------------------
--07. Find Towns Not Starting With

SELECT [TownID], [Name] FROM Towns
WHERE [Name] LIKE '[^R,B,D]%'
ORDER BY [Name] ASC

----------------------------------------
--08. Create View Employees Hired After

CREATE VIEW V_EmployeesHiredAfter2000 AS 
SELECT [FirstName], [LastName] FROM Employees
WHERE YEAR(HireDate) > 2000

----------------------------
--09. Length of Last Name

SELECT [FirstName], [LastName] FROM Employees
WHERE LEN(LastName) = 5


--10. Rank Employees by Salary

SELECT EmployeeID, FirstName, LastName, Salary, DENSE_RANK() OVER (PARTITION BY Salary ORDER BY EmployeeId) AS Rank FROM Employees
WHERE Salary Between 10000 AND 50000
ORDER BY Salary DESC

---------------------------
--12. Countries Holding 'A'

SELECT [CountryName] AS [Country Name], [IsoCode] AS [ISO Code] FROM Countries
WHERE [CountryName] LIKE '%A%A%A%'
ORDER BY [Iso Code]

-----------------------------------
--13. Mix of Peak and River Names

SELECT p.PeakName, r.RiverName, LOWER(p.PeakName) + SUBSTRING(LOWER(r.RiverName),2, LEN(r.RiverName)) AS MIX FROM Rivers AS r, Peaks AS p
WHERE RIGHT(p.PeakName, 1) = LEFT(r.RiverName, 1)
ORDER BY Mix

-------------------------------------
--14. Games From 2011 and 2012 Year

SELECT TOP(50) [Name], FORMAT(Start, 'yyyy-MM-dd') AS [Start] FROM Games
WHERE YEAR([Start]) BETWEEN 2011 AND 2012
ORDER BY [Start], [Name]

---------------------------
--15. User Email Providers

SELECT Username, 
SUBSTRING(Email, CHARINDEX('@', Email, 1) + 1, LEN(Email))
AS [Email Provider] 
  FROM Users
ORDER BY [Email Provider], Username

-------------------------------------------
--16. Get Users with IPAddress Like Pattern

SELECT Username, IpAddress AS [IP Address] FROM Users
WHERE IpAddress LIKE '[0-9][0-9][0-9].1%.%[0-9][0-9][0-9]'
ORDER BY Username 

----------------------------------
--17. Show All Games with Duration

SELECT [Name] AS Game,
		CASE 
			WHEN DATEPART(HOUR, [Start]) BETWEEN 0 AND 11 THEN 'Morning'
			WHEN DATEPART(HOUR, [Start]) BETWEEN 12 AND 17 THEN 'Afternoon' 
			WHEN DATEPART(HOUR, [Start]) BETWEEN 18 AND 23 THEN 'Evening' 
			END AS [Part of the Day],
		CASE
			WHEN Duration <= 3 THEN 'Extra Short'
			WHEN Duration BETWEEN 4 AND 6 THEN 'Short'
			WHEN Duration > 6 THEN 'Long'
			WHEN Duration IS NULL THEN 'Extra Long'
			END AS Duration
FROM Games
ORDER BY [Name], Duration, [Part of the Day]

----------------------
--18. Orders Table

SELECT ProductName, OrderDate,
	DATEADD(DAY, 3, OrderDate) AS [Pay Due],
	DATEADD(MONTH, 1, OrderDate) AS [Deliver Due]
FROM Orders