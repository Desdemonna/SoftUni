 --04. Insert Records in Both Tables

 INSERT INTO [Towns]([Id], [Name]) VALUES
(1, 'Sofia'),
(2, 'Plovdiv'),
(3, 'Varna')

INSERT INTO [Minions]([Id], [Name], [Age], [TownId]) VALUES
(1, 'Kevin', 22, 1),
(2, 'Bob', 15, 3),
(3, 'Steward', NULL, 2)

-------------------------
--07. Create Table People

CREATE TABLE [People](
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(200) NOT NULL,
	Picture VARBINARY(MAX) CHECK(DATALENGTH([Picture]) <= 2 * 1024 * 1024),
	Height DECIMAL(3, 2),
	[Weight] DECIMAL(5, 2),
	Gender CHAR CHECK(Gender = 'm' OR Gender = 'f') NOT NULL,
	Birthdate DATETIME2 NOT NULL,
	Biography NVARCHAR(MAX)
)

INSERT INTO People([Name], Height, [Weight], Gender, Birthdate) VALUES
('Pesho', 1.76, 65.5, 'm', '06.06.1995'),
('Gosho', NULL, NULL, 'm', '06.05.1995'),
('Vancho', 1.56, 45.5, 'm', '06.07.1995'),
('Mariq', 1.56, 40.5, 'f', '06.09.1995'),
('Sasho', 1.96, 42.5, 'm', '06.10.1995')

------------------------
--08. Create Table Users

CREATE TABLE [Users](
	[Id] BIGINT PRIMARY KEY IDENTITY,
	[Username] VARCHAR(30) UNIQUE NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	[ProfilePicture] VARBINARY(MAX) CHECK (DATALENGTH ([ProfilePicture]) <= 900000),
	[LastLoginTime] DATETIME2,
	[IsDeleted] BIT NOT NULL
)

INSERT INTO Users (Username, Password, ProfilePicture, LastLoginTime, IsDeleted) VALUES
('Kiro Kirkov', '112234', NULL, '1989-05-20', 0),
('Ivan Ivanov', '1125', NULL, '1969-02-21', 1),
('Pesho Peshov', '1234345567', NULL, '1999-05-16', 1),
('Stamat Stamatov', '1231264567', NULL, '1976-02-09', 0),
('Vanko Vankov', '15534', NULL, '1999-05-05', 0)

----------------------
--13. Movies Database

CREATE TABLE[Directors](
	[Id] INT IDENTITY,
	[DirectorName] NVARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(max)
	CONSTRAINT PK_Id_Directors PRIMARY KEY (Id)
)

CREATE TABLE[Genres](
	[Id] INT IDENTITY,
	[GenreName] NVARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(max)
	CONSTRAINT PK_Id_Genres PRIMARY KEY (Id)
)

CREATE TABLE [Categories](
	[Id] INT IDENTITY,
	[CategoryName] NVARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(max)
	CONSTRAINT PK_Id_Categories PRIMARY KEY (Id)
)

CREATE TABLE [Movies] (
	[Id] INT IDENTITY,
	[Title] NVARCHAR(50) NOT NULL,
	[DirectorId] INT NOT NULL,
	[CopyrightYear] DATETIME2,
	[Length] REAL,
	[GenreId] INT NOT NULL,
	[CategoryId] INT NOT NULL,
	[Rating] REAL,
	[Notes] NVARCHAR(max)

	CONSTRAINT PK_Id_Movies PRIMARY KEY (Id),
	CONSTRAINT FK_DirectorId FOREIGN KEY (DirectorId) REFERENCES Directors(Id),
	CONSTRAINT FK_GenreId FOREIGN KEY (GenreId) REFERENCES Genres(Id),
	CONSTRAINT FK_CategoryId FOREIGN KEY (CategoryId) REFERENCES Categories(Id),

)

INSERT INTO [Directors] (DirectorName, Notes) VALUES
('Ivo', 'pich'),
('Misho', NULL),
('Jore', 'worst'),
('Mircho', 'best'),
('Pesho', NULL)

INSERT INTO [Genres] (GenreName, Notes) VALUES
('Action', NULL),
('Horror', 'creepy'),
('Comedy', 'funny'),
('Romance', NULL),
('Fantasy', 'interesting')

INSERT INTO [Categories] (CategoryName, Notes) VALUES
('Sports', NULL),
('Mystery', 'creepy'),
('Animated', 'interesting'),
('SciFi', NULL),
('Neshto si tam', NULL)


INSERT INTO Movies (Title, DirectorId, CopyrightYear, [Length], GenreId, CategoryId, Rating, Notes) VALUES			   ('Batman', 1, '1999-11-11', 211, 1, 3, 8, 'Full of action'),
('Titanic', 4, '1997-10-10', 268, 3, 3, 9, NULL),
('Fast and Furious 7', 2, '2016-08-08', 212, 1, 4, 10, 'It is a must see!'),
('Ford vs Ferrari', 5, '1995-12-12', 198, 1, 4, 7, NULL),
('Toy Story 4', 3, '2019-01-01', 168, 4, 1, 9, NULL)

--------------------------
-- 14. Car Rental Database

CREATE TABLE Categories(
	Id INT IDENTITY,
	CategoryName NVARCHAR(20) NOT NULL,
	DailyRate DECIMAL(18,2) NOT NULL,
	WeeklyRate DECIMAL(18,2) NOT NULL,
	MonthlyRate DECIMAL(18,2) NOT NULL,
	WeekendRate DECIMAL(18,2) NOT NULL,

	CONSTRAINT PK_IdCategories PRIMARY KEY (Id)
)

CREATE TABLE Cars(
	Id INT IDENTITY,
	PlateNumber NVARCHAR(10) NOT NULL,
	Manufacturer NVARCHAR(30) NOT NULL,
	Model NVARCHAR(30) NOT NULL,
	CarYear INT NOT NULL,
	CategoryId INT NOT NULL,
	Doors INT,
	Picture VARBINARY(max),
	Condition NVARCHAR(200),
	Available NVARCHAR(12),

	CONSTRAINT PK_IdCars PRIMARY KEY (Id),
	CONSTRAINT FK_Category FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
)

CREATE TABLE Employees(
	Id INT IDENTITY,
	FirstName NVARCHAR(10) NOT NULL,
	LastName NVARCHAR(30) NOT NULL,
	Title NVARCHAR(30) NOT NULL,
	Notes NVARCHAR(max),

	CONSTRAINT PK_IdEmployees PRIMARY KEY (Id)
)

CREATE TABLE Customers(
	Id INT IDENTITY,
	DriverLicenceNumber NVARCHAR(20) NOT NULL,
	FullName NVARCHAR(40) NOT NULL,
	Address NVARCHAR(80) NOT NULL,
	City NVARCHAR(20) NOT NULL,
	ZIPCode INT NOT NULL,
	Notes NVARCHAR(max),


	CONSTRAINT PK_IdCustomers PRIMARY KEY (Id),
)

CREATE TABLE RentalOrders(
	Id INT IDENTITY,
	EmployeeId INT NOT NULL,
	CustomerId INT NOT NULL,
	CarId INT NOT NULL,
	TankLevel INT,
	KilometrageStart DECIMAL(18,2),
	KilometrageEnd DECIMAL(18,2),
	TotalKilometrage AS (KilometrageEnd - KilometrageStart),
	StartDate DATETIME2 NOT NULL,
	EndDate DATETIME2 NOT NULL,
	TotalDays AS DATEDIFF(DAY, StartDate, EndDate),
	RateApplied INT NOT NULL,
	TaxRate INT NOT NULL,
	OrderStatus NVARCHAR(20) NOT NULL,
	Notes NVARCHAR(max),

	CONSTRAINT PK_IdRentalOrders PRIMARY KEY (Id),
	CONSTRAINT FK_EmployeeIdRentalOrders FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
	CONSTRAINT FK_CustomerIdRentalOrders FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
	CONSTRAINT FK_CarIdRentalOrders FOREIGN KEY (CarId) REFERENCES Cars(Id),
)

INSERT INTO Categories
VALUES ('Sport', '100', '600', '1500', '150'),
		('Family', '30', '170', '350', '50'),
		('Daily', '20', '120', '300', '300')

INSERT INTO Cars
VALUES ('CT2010RR', 'Opel', 'Astra', '1999', 3,4,NULL,'Perfect', 'Unavailable'),
		('CT1948BR', 'Mercedes', 'E63 AMG', '2018', 1,4,NULL,'PERFECT ICE WHITE ', 'Available'),
		('CA7777AC', 'Audi', 'RS6', '2020', 2,4,NULL,'Scratched', 'Unavailable')

INSERT INTO Employees
VALUES ('Stamat', 'Bonev', 'Seller', 'Smart boy'),
		('Ivan', 'Ivanov', 'Boss', 'Bad guy'),
		('Peter', 'The Fish', 'A Fish', 'Cute black molly fish')

INSERT INTO Customers
VALUES ('9123993', 'Lewis Linkoln', '24 Petkanob', 'Sofia', 1242, 'Rich gut'),
		('3235214', 'Peter McKinon', '25 Petkanob', 'Sofia', 1242, 'Common gut'),
		('9123993', 'Michael Musk', '27 Petkanob', 'Sofia', 1242, 'Poor gut')

INSERT INTO RentalOrders(EmployeeId, CustomerId, CarId, TankLevel, KilometrageStart, KilometrageEnd, StartDate, EndDate, RateApplied, TaxRate, OrderStatus)
VALUES (1, 1, 1, 20, 150, 350, '2021-09-09', '2021-09-27', 10, 20, 'Completed'),
		(2, 2, 2, 50, 261, 401, '2021-08-08', '2021-09-26', 10, 20, 'Completed'),
		(3, 3, 3, 100, 410, 500, '2021-07-07', '2021-09-25', 10, 20, 'Completed')

-------------------
--15. Hotel Database

CREATE TABLE Employees(
Id INT PRIMARY KEY IDENTITY NOT NULL,
FirstName VARCHAR(50),
LastName VARCHAR(50),
Title VARCHAR(50),
Notes VARCHAR(MAX)
)

INSERT INTO Employees
VALUES
('Velizar', 'Velikov', 'Receptionist', 'Nice customer'),
('Ivan', 'Ivanov', 'Concierge', 'Nice one'),
('Elisaveta', 'Bagriana', 'Cleaner', 'Poetesa')

CREATE TABLE Customers(
Id INT PRIMARY KEY IDENTITY NOT NULL,
AccountNumber BIGINt,
FirstName VARCHAR(50),
LastName VARCHAR(50),
PhoneNumber VARCHAR(15),
EmergencyName VARCHAR(150),
EmergencyNumber VARCHAR(15),
Notes VARCHAR(100)
)

INSERT INTO Customers
VALUES
(123456789, 'Ginka', 'Shikerova', '359888777888', 'Sistry mi', '7708315342', 'Kinky'),
(123480933, 'Chaika', 'Stavreva', '359888777888', 'Sistry mi', '7708315342', 'Lawer'),
(123454432, 'Mladen', 'Isaev', '359888777888', 'Sistry mi', '7708315342', 'Wants a call girl')

CREATE TABLE RoomStatus(
Id INT PRIMARY KEY IDENTITY NOT NULL,
RoomStatus BIT,
Notes VARCHAR(MAX)
)

INSERT INTO RoomStatus(RoomStatus, Notes)
VALUES
(1,'Refill the minibar'),
(2,'Check the towels'),
(3,'Move the bed for couple')

CREATE TABLE RoomTypes(
RoomType VARCHAR(50) PRIMARY KEY,
Notes VARCHAR(MAX)
)

INSERT INTO RoomTypes (RoomType, Notes)
VALUES
('Suite', 'Two beds'),
('Wedding suite', 'One king size bed'),
('Apartment', 'Up to 3 adults and 2 children')

CREATE TABLE BedTypes(
BedType VARCHAR(50) PRIMARY KEY,
Notes VARCHAR(MAX)
)

INSERT INTO BedTypes
VALUES
('Double', 'One adult and one child'),
('King size', 'Two adults'),
('Couch', 'One child')

CREATE TABLE Rooms (
RoomNumber INT PRIMARY KEY IDENTITY NOT NULL,
RoomType VARCHAR(50) FOREIGN KEY REFERENCES RoomTypes(RoomType),
BedType VARCHAR(50) FOREIGN KEY REFERENCES BedTypes(BedType),
Rate DECIMAL(6,2),
RoomStatus NVARCHAR(50),
Notes NVARCHAR(MAX)
)

INSERT INTO Rooms (Rate, Notes)
VALUES
(12,'Free'),
(15, 'Free'),
(23, 'Clean it')

CREATE TABLE Payments(
Id INT PRIMARY KEY IDENTITY NOT NULL,
EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
PaymentDate DATE,
AccountNumber BIGINT,
FirstDateOccupied DATE,
LastDateOccupied DATE,
TotalDays AS DATEDIFF(DAY, FirstDateOccupied, LastDateOccupied),
AmountCharged DECIMAL(14,2),
TaxRate DECIMAL(8, 2),
TaxAmount DECIMAL(8, 2),
PaymentTotal DECIMAL(15, 2),
Notes VARCHAR(MAX)
)

INSERT INTO Payments (EmployeeId, PaymentDate, AmountCharged)
VALUES
(1, '12/12/2018', 2000.40),
(2, '12/12/2018', 1500.40),
(3, '12/12/2018', 1000.40)

CREATE TABLE Occupancies(
Id  INT PRIMARY KEY IDENTITY NOT NULL,
EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
DateOccupied DATE,
AccountNumber BIGINT,
RoomNumber INT FOREIGN KEY REFERENCES Rooms(RoomNumber),
RateApplied DECIMAL(6,2),
PhoneCharge DECIMAL(6,2),
Notes VARCHAR(MAX)
)

INSERT INTO Occupancies (EmployeeId, RateApplied, Notes) VALUES
(1, 55.55, 'too'),
(2, 15.55, 'much'),
(3, 35.55, 'typing')

-----------------------------
--19. Basic Select All Fields

SELECT * FROM Towns
SELECT * FROM Departments
SELECT * FROM Employees

--------------------------------------------
--20. Basic Select All Fields and Order Them

SELECT * FROM Towns ORDER BY Name ASC
SELECT * FROM Departments ORDER BY Name ASC
SELECT * FROM Employees ORDER BY Salary DESC

--------------------------------
--21. Basic Select Some Fields

SELECT Name FROM Towns ORDER BY Name ASC
SELECT Name FROM Departments ORDER BY Name ASC
SELECT FirstName, LastName, JobTitle, Salary FROM Employees ORDER BY Salary DESC

---------------------------------
--22. Increase Employees Salary

UPDATE Employees
SET Salary = Salary * 1.1

SELECT Salary FROM Employees

------------------------
--23. Decrease Tax Rate

UPDATE Payments
  SET
      TaxRate = TaxRate - (TaxRate * 0.03);

SELECT TaxRate
FROM Payments

-------------------------
--24. Delete All Records

TRUNCATE TABLE Occupancies