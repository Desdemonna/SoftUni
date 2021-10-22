--01. Employee Address

SELECT TOP(5) e.EmployeeID, e.JobTitle, a.AddressID, a.AddressText FROM Employees AS e
	JOIN Addresses AS a ON e.AddressID = a.AddressID
	ORDER BY AddressID ASC

---------------------------
--02. Addresses with Towns

SELECT TOP(50) e.FirstName, e.LastName, t.Name AS Town, a.AddressText FROM Employees AS e
	JOIN Addresses AS a ON e.AddressID = a.AddressID
	JOIN Towns AS t ON a.TownID = t.TownID
ORDER BY e.FirstName, e.LastName ASC

-----------------------
--03. Sales Employees

SELECT e.EmployeeID, e.FirstName, e.LastName, d.Name AS DepartmentName  FROM Employees AS e
	JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
WHERE e.DepartmentID = 3
ORDER BY e.EmployeeID ASC

---------------------------
--04. Employee Departments

SELECT TOP(5) e.EmployeeID, e.FirstName, e.Salary, d.[Name] AS DepartmentName FROM Employees AS e
	JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
WHERE e.Salary > 15000
ORDER BY d.DepartmentID ASC

---------------------------------
--05. Employees Without Projects

SELECT TOP(3) e.EmployeeID, e.FirstName FROM Employees AS e
	LEFT JOIN EmployeesProjects AS a ON e.EmployeeID = a.EmployeeID
WHERE a.ProjectID IS NULL
ORDER BY e.EmployeeID ASC

----------------------------
--06. Employees Hired After

SELECT e.FirstName, e.LastName, e.HireDate, d.[Name] AS DeptName FROM Employees AS e
	JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
WHERE e.HireDate > '1/1/1999' AND (d.DepartmentID = 3 OR d.DepartmentID = 10)
ORDER BY e.HireDate

-----------------------------
--07. Employees With Project

SELECT TOP(5) a.EmployeeID, e.FirstName, j.[Name] AS ProjectName FROM Employees AS e
	JOIN EmployeesProjects AS a ON e.EmployeeID = a.EmployeeID
	JOIN Projects AS j ON a.ProjectID = j.ProjectID
WHERE (a.ProjectID IS NOT NULL) AND j.StartDate > '2002-08-13' AND j.EndDate IS NULL
ORDER BY e.EmployeeID ASC

--------------------
--08. Employee 24

SELECT TOP(5) a.EmployeeID, e.FirstName,
	CASE
		WHEN YEAR(j.StartDate) >= 2005 THEN NULL 
		ELSE j.[Name]
	END AS ProjectName
	FROM Employees AS e
	LEFT JOIN EmployeesProjects AS a ON e.EmployeeID = a.EmployeeID
	JOIN Projects AS j ON a.ProjectID = j.ProjectID
WHERE e.EmployeeID = 24

-----------------------
--09. Employee Manager

SELECT e.EmployeeID, e.FirstName, e.ManagerID, m.FirstName AS ManagerName FROM Employees AS e
	LEFT JOIN Employees AS m ON e.ManagerID = m.EmployeeID
WHERE e.ManagerID IN (3,7)
ORDER BY e.EmployeeID

--------------------------
--10. Employees Summary

SELECT TOP(50) e.EmployeeID, 
		CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName, 
		CONCAT(m.FirstName, ' ', m.LastName) AS ManagerName, 
		d.[Name] AS DepartmentName FROM Employees AS e
	JOIN Employees AS m ON e.ManagerID = m.EmployeeID
	JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
ORDER BY e.EmployeeID

-------------------------
--11. Min Average Salary

SELECT TOP(1) AVG(Salary) AS MinAverageSalary FROM Employees
GROUP BY DepartmentID
ORDER BY MinAverageSalary

---------------------------------
--12. Highest Peaks in Bulgaria

SELECT * FROM (SELECT mc.CountryCode, m.MountainRange, p.PeakName, p.Elevation FROM MountainsCountries AS mc
	JOIN Mountains AS m ON mc.MountainId = m.Id
	JOIN Countries AS c ON mc.CountryCode = c.CountryCode
	JOIN Peaks AS p ON m.Id = p.MountainId) AS mcp
WHERE mcp.Elevation >= 2835 AND mcp.CountryCode = 'BG'
ORDER BY mcp.Elevation DESC

---------------------------
--13. Count Mountain Ranges

SELECT msc.CountryCode, COUNT(msc.MountainRange) FROM(SELECT mc.CountryCode, m.MountainRange FROM MountainsCountries AS mc
	JOIN Mountains AS m ON mc.MountainId = m.Id
	JOIN Countries AS c ON mc.CountryCode = c.CountryCode) AS msc
WHERE msc.CountryCode IN ('BG', 'RU', 'US')
GROUP BY msc.CountryCode

----------------------------------------
--14. Countries With or Without Rivers

SELECT TOP(5) * FROM (SELECT  c.CountryName, r.RiverName FROM Countries AS c
		LEFT JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
		LEFT JOIN Rivers AS r ON cr.RiverId = r.Id
		WHERE c.ContinentCode = 'AF') AS cr
ORDER BY cr.CountryName

--------------------------------------
--16. Countries Without any Mountains

SELECT COUNT(c.CountryCode) AS CountryCode
FROM Countries AS c
     LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
WHERE mc.CountryCode IS NULL; 

-------------------------------------------------
--17. Highest Peak and Longest River by Country

SELECT TOP (5) peaks.CountryName,
               peaks.Elevation AS HighestPeakElevation,
               rivers.Length AS LongestRiverLength
FROM
(
    SELECT c.CountryName,
           c.CountryCode,
           DENSE_RANK() OVER(PARTITION BY c.CountryName ORDER BY p.Elevation DESC) AS DescendingElevationRank,
           p.Elevation
    FROM Countries AS c
         FULL OUTER JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
         FULL OUTER JOIN Mountains AS m ON mc.MountainId = m.Id
         FULL OUTER JOIN Peaks AS p ON m.Id = p.MountainId
) AS peaks
FULL OUTER JOIN
(
    SELECT c.CountryName,
           c.CountryCode,
           DENSE_RANK() OVER(PARTITION BY c.CountryCode ORDER BY r.Length DESC) AS DescendingRiversLenghRank,
           r.Length
    FROM Countries AS c
         FULL OUTER JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode
         FULL OUTER JOIN Rivers AS r ON cr.RiverId = r.Id
) AS rivers ON peaks.CountryCode = rivers.CountryCode
WHERE peaks.DescendingElevationRank = 1
      AND rivers.DescendingRiversLenghRank = 1
      AND (peaks.Elevation IS NOT NULL
           OR rivers.Length IS NOT NULL)
ORDER BY HighestPeakElevation DESC,
         LongestRiverLength DESC,
         CountryName