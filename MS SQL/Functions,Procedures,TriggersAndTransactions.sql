--01. Employees with Salary Above 35000

CREATE PROC usp_GetEmployeesSalaryAbove35000
AS
BEGIN
	SELECT FirstName, LastName FROM Employees
	WHERE Salary > 35000
END

-----------------------------------------
--02. Employees with Salary Above Number

CREATE PROC usp_GetEmployeesSalaryAboveNumber (@Number DECIMAL(18,4))
AS
BEGIN
	SELECT FirstName, LastName FROM Employees
	WHERE Salary >= @Number
END

------------------------------
--03. Town Names Starting With

CREATE PROC usp_GetTownsStartingWith (@startingLetter NVARCHAR(max))
AS
BEGIN
	SELECT [Name] FROM Towns WHERE [Name] LIKE @startingLetter + '%'
END

--------------------------
--04. Employees from Town

CREATE PROC usp_GetEmployeesFromTown (@townName NVARCHAR(max))
AS
BEGIN
	SELECT e.FirstName, e.LastName FROM Employees AS e
	JOIN Addresses AS a ON e.AddressID = a.AddressID
	JOIN Towns AS t ON a.TownID = t.TownID

	WHERE t.[Name] = @townName
END

----------------------------
--05. Salary Level Function

CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4))
RETURNS NVARCHAR(20)
AS
BEGIN
	DECLARE @salaryLevel NVARCHAR(20);

	IF(@salary < 30000)
	BEGIN
		SET @salaryLevel = 'Low'
	END 
	ELSE IF(@salary BETWEEN 30000 AND 50000)
	BEGIN
		SET @salaryLevel = 'Average'
	END
	ELSE IF(@salary > 50000)
	BEGIN
		SET @salaryLevel = 'High'
	END

	RETURN @salaryLevel;
END

-------------------------------
--06. Employees by Salary Level

CREATE PROC usp_EmployeesBySalaryLevel (@levelOfSalary NVARCHAR(20))
AS
BEGIN
	SELECT FirstName, LastName FROM Employees
	WHERE dbo.ufn_GetSalaryLevel(Salary) = @levelOfSalary
END

----------------------
--07. Define Function

CREATE FUNCTION ufn_IsWordComprised(@setOfLetters NVARCHAR(max), @word NVARCHAR(50))
RETURNS INT
AS
BEGIN
	DECLARE @count INT = 1;
	DECLARE @letter NVARCHAR

	WHILE @count <= LEN(@word)
	BEGIN
		SET @letter = SUBSTRING(@word, @count, 1)

		IF @setOfLetters NOT LIKE '%' + @letter + '%'
			RETURN 0

		SET @count += 1;
	END;

	RETURN 1
END

----------------------------------------
--08. Delete Employees and Departments

CREATE PROC usp_DeleteEmployeesFromDepartment (@departmentId INT)
AS
BEGIN
	DELETE FROM EmployeesProjects
	WHERE EmployeeID IN (
		SELECT EmployeeID
		  FROM Employees
		 WHERE DepartmentID = @departmentId
	)

	UPDATE Employees
	   SET ManagerID = NULL
	 WHERE ManagerID IN ( 
		SELECT EmployeeID
		  FROM Employees
		 WHERE DepartmentID = @departmentId)

	ALTER TABLE Departments
	ALTER COLUMN ManagerId INT

	UPDATE Departments
	   SET ManagerID = NULL
	 WHERE DepartmentID = @departmentId

	 DELETE FROM Employees
	 WHERE DepartmentID = @departmentId

	 DELETE FROM Departments
	 WHERE DepartmentID = @departmentId

	 SELECT COUNT(*)
	   FROM Employees
       WHERE DepartmentID = @departmentId
END

---------------------
--09. Find Full Name

CREATE PROC usp_GetHoldersFullName 
AS
BEGIN
	SELECT CONCAT(FirstName, ' ', LastName) AS [FullName] FROM dbo.AccountHolders
END

--------------------------------------
--10. People with Balance Higher Than

CREATE PROC usp_GetHoldersWithBalanceHigherThan (@amount MONEY)
AS
BEGIN
	SELECT FirstName AS [First Name], LastName AS [Last Name] FROM dbo.AccountHolders AS ah
	JOIN dbo.Accounts AS a ON a.AccountHolderId = ah.Id
	GROUP BY ah.FirstName, ah.LastName
	HAVING SUM(a.Balance) > @amount
	ORDER BY FirstName, LastName
END

----------------------------
--11. Future Value Function

CREATE FUNCTION ufn_CalculateFutureValue
(
                @sum                MONEY,
                @yearlyInterestRate FLOAT,
                @numberOfYears      INT
)
RETURNS MONEY
AS
     BEGIN
         RETURN @sum * (POWER(1 + @yearlyInterestRate, @numberOfYears));
     END

----------------------------
--12. Calculating Interest

CREATE PROC usp_CalculateFutureValueForAccount(@accountId INT, @interestRate FLOAT) 
AS
BEGIN
	SELECT a.Id, ah.FirstName, ah.LastName, a.Balance AS [Current Balance], dbo.ufn_CalculateFutureValue(a.Balance, @interestRate, 5) AS [Balance in 5 years] FROM AccountHolders AS ah
	JOIN Accounts AS a ON ah.Id = a.AccountHolderId
	WHERE a.Id = @accountId
END

------------------------------------
--13. *Cash in User Games Odd Rows

CREATE FUNCTION ufn_CashInUsersGames(@gameName varchar(max))
RETURNS @returnedTable TABLE
(
SumCash money
)
AS
BEGIN
	DECLARE @result money

	SET @result = 
	(SELECT SUM(ug.Cash) AS Cash
	FROM
		(SELECT Cash, GameId, ROW_NUMBER() OVER (ORDER BY Cash DESC) AS RowNumber
		FROM UsersGames
		WHERE GameId = (SELECT Id FROM Games WHERE Name = @gameName)
		) AS ug
	WHERE ug.RowNumber % 2 != 0
	)

	INSERT INTO @returnedTable SELECT @result
	RETURN
END

-------------------------
--14. Create Table Logs

CREATE TRIGGER tr_AccountChanges ON Accounts FOR UPDATE
AS
BEGIN
	DECLARE @accountId INT;
	DECLARE @oldSum DECIMAL(15, 2);
	DECLARE @newSum DECIMAL(15, 2);

	SET @accountId = (SELECT i.Id
		            FROM inserted AS i)

	SET @oldSum = (SELECT d.Balance
		         FROM deleted AS d)

	SET @newSum = (SELECT i.Balance
		         FROM inserted AS i)

	INSERT INTO Logs(AccountId, OldSum, NewSum)
	VALUES		(@accountId, @oldSum, @newSum)
END

--------------------------
--15. Create Table Emails

CREATE TRIGGER tr_CreateEmail ON Logs FOR INSERT
AS
BEGIN
	DECLARE @recipient INT;
	DECLARE @subject VARCHAR(200);
	DECLARE @oldBalance DECIMAL(15, 2);
	DECLARE @newBalance DECIMAL(15, 2);
	DECLARE @body VARCHAR(200);

	SET @recipient = (SELECT i.AccountId FROM inserted AS i)
	SET @subject = 'Balance change for account: ' + CAST(@recipient AS VARCHAR(MAX))
	SET @oldBalance = (SELECT i.OldSum FROM inserted AS i)
	SET @newBalance = (SELECT i.NewSum FROM inserted AS i)
	SET @body = 'On ' + CAST(GETDATE() AS VARCHAR(MAX)) 
	            + ' your balance was changed from ' + CAST(@oldBalance AS VARCHAR(MAX))
		    + ' to ' + CAST(@newBalance AS VARCHAR(MAX))

	INSERT INTO NotificationEmails(Recipient, [Subject], Body)
	VALUES	    (@recipient, @subject, @body)
END

------------------------
--16. Deposit Money

CREATE PROCEDURE usp_DepositMoney(@AccountId INT, @MoneyAmount MONEY)
AS
BEGIN TRANSACTION

UPDATE Accounts
SET Balance += @MoneyAmount
WHERE Id = @AccountId

COMMIT

------------------------------
--17. Withdraw Money Procedure

CREATE PROCEDURE usp_WithdrawMoney
(
                 @accountId   INT,
                 @moneyAmount MONEY
)
AS
     BEGIN
         IF(@moneyAmount < 0)
             BEGIN
                 RAISERROR('Cannot withdraw negative value', 16, 1)
         END
             ELSE
             BEGIN
                 IF(@accountId IS NULL
                    OR @moneyAmount IS NULL)
                     BEGIN
                         RAISERROR('Missing value', 16, 1)
                 END
         END
         BEGIN TRANSACTION
         UPDATE Accounts
           SET
               Balance-=@moneyAmount
         WHERE Id = @accountId;
         IF(@@ROWCOUNT < 1)
             BEGIN
                 ROLLBACK
                 RAISERROR('Account doesn''t exists', 16, 1)
         END;
             ELSE
             BEGIN
                 IF(0 >
                   (
                       SELECT Balance
                       FROM Accounts
                       WHERE Id = @accountId
                   ))
                     BEGIN
                         ROLLBACK
                         RAISERROR('Balance not enough', 16, 1)
                 END;
         END;
         COMMIT
     END

--------------------
--18. Money Transfer

CREATE PROCEDURE usp_TransferMoney
(
                 @senderId   INT,
                 @receiverId INT,
                 @amount     MONEY
)
AS
     BEGIN
         IF(@amount < 0)
             BEGIN
                 RAISERROR('Cannot transfer negative amount', 16, 1)
         END
             ELSE
             BEGIN
                 IF(@senderId IS NULL
                    OR @receiverId IS NULL
                    OR @amount IS NULL)
                     BEGIN
                         RAISERROR('Missing value', 16, 1)
                 END
         END

-- Withdraw from the sender
         BEGIN TRANSACTION
         UPDATE Accounts
           SET
               Balance-=@amount
         WHERE Id = @senderId
         IF(@@ROWCOUNT < 1)
             BEGIN
                 ROLLBACK
                 RAISERROR('Sender''s account doesn''t exists', 16, 1)
         END;

-- Check sender's current balance
         IF(0 >
           (
               SELECT Balance
               FROM Accounts
               WHERE ID = @senderId
           ))
             BEGIN
                 ROLLBACK
                 RAISERROR('Not enough funds', 16, 1)
         END;

-- Add money to the receiver
         UPDATE Accounts
           SET
               Balance+=@amount
         WHERE ID = @receiverId
         IF(@@ROWCOUNT < 1)
             BEGIN
                 ROLLBACK
                 RAISERROR('Receiver''s account doesn''t exists', 16, 1)
         END
         COMMIT
     END

------------------------------------
--21. Employees with Three Projects

CREATE PROC usp_AssignProject(@EmloyeeId INT , @ProjectID INT)
AS
BEGIN TRANSACTION
DECLARE @ProjectsCount INT;
SET @ProjectsCount = (SELECT COUNT(ProjectID) FROM EmployeesProjects WHERE EmployeeID = @emloyeeId)
IF(@ProjectsCount >= 3)
BEGIN 
 ROLLBACK
 RAISERROR('The employee has too many projects!', 16, 1)
 RETURN
END
INSERT INTO EmployeesProjects
     VALUES
(@EmloyeeId, @ProjectID)
 
 COMMIT

 -------------------------
 --22. Delete Employees

 CREATE TRIGGER tr_DeleteEmployees
  ON Employees
  AFTER DELETE
AS
  BEGIN
    INSERT INTO Deleted_Employees
      SELECT FirstName,LastName,MiddleName,JobTitle,DepartmentID,Salary
      FROM deleted
  END