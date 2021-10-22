--01. One-To-One Relationship

CREATE TABLE Passports(
	[PassportID] INT PRIMARY KEY NOT NULL,
	[PassportNumber] CHAR(8) NOT NULL
)

CREATE TABLE [Persons](
	[PersonID] INT PRIMARY KEY IDENTITY NOT NULL,
	[FirstName] VARCHAR(50) NOT NULL,
	[Salary] DECIMAL(9,2) NOT NULL,
	[PassportID] INT FOREIGN KEY REFERENCES [Passports]([PassportID]) UNIQUE NOT NULL
)

------------------------------
--02. One-To-Many Relationship

CREATE TABLE [Manufacturers](
	ManufacturerID INT IDENTITY PRIMARY KEY NOT NULL,
	[Name] NVARCHAR(20) NOT NULL,
	EstablishedOn DATETIME2
)

CREATE TABLE [Models](
	ModelID INT IDENTITY PRIMARY KEY NOT NULL,
	[Name] NVARCHAR(20) NOT NULL,
	ManufacturerID INT FOREIGN KEY REFERENCES [Manufacturers]([ManufacturerID]) NOT NULL
)

--------------------------------
--03. Many-To-Many Relationship

CREATE TABLE Students(
	StudentID INT IDENTITY PRIMARY KEY NOT NULL,
	[Name] NVARCHAR(20) NOT NULL,
)

CREATE TABLE Exams(
	ExamID INT IDENTITY PRIMARY KEY NOT NULL,
	[Name] NVARCHAR(20) NOT NULL,
)

CREATE TABLE StudentsExams(
	StudentID INT FOREIGN KEY REFERENCES [Students]([StudentID]) NOT NULL,
	ExamID INT FOREIGN KEY REFERENCES [Exams]([ExamID]) NOT NULL,
	PRIMARY KEY([StudentID], [ExamID])
)

-----------------------
--04. Self-Referencing

CREATE TABLE Teachers(
	TeacherID INT IDENTITY(101,1) PRIMARY KEY NOT NULL,
	[Name] NVARCHAR(30) NOT NULL,
	ManagerID INT FOREIGN KEY REFERENCES [Teachers]([TeacherID])
)

---------------------------
--05. Online Store Database

CREATE TABLE ItemTypes
(
             ItemTypeID INT NOT NULL,
             Name       VARCHAR(50) NOT NULL,
             CONSTRAINT PK_ItemTypes PRIMARY KEY(ItemTypeId)
)

CREATE TABLE Items
(
             ItemID     INT NOT NULL,
             Name       VARCHAR(50) NOT NULL,
             ItemTypeID INT NOT NULL,
             CONSTRAINT PK_Items PRIMARY KEY(ItemID),
             CONSTRAINT FK_Items_ItemTypes FOREIGN KEY(ItemTypeID) REFERENCES ItemTypes(ItemTypeID)
)

CREATE TABLE Cities
(
             CityID INT NOT NULL,
             Name   VARCHAR(50) NOT NULL,
             CONSTRAINT PK_Cities PRIMARY KEY(CityID)
)

CREATE TABLE Customers
(
             CustomerID INT NOT NULL,
             Name       VARCHAR(50) NOT NULL,
             Birthday   DATE,
             CityID     INT,
             CONSTRAINT PK_Customers PRIMARY KEY(CustomerID),
             CONSTRAINT FK_Customers_Cities FOREIGN KEY(CityID) REFERENCES Cities(CityID)
)

CREATE TABLE Orders
(
             OrderID    INT NOT NULL,
             CustomerID INT NOT NULL,
             CONSTRAINT PK_Orders PRIMARY KEY(OrderID),
             CONSTRAINT FK_Orders_Customers FOREIGN KEY(CustomerID) REFERENCES Customers(CustomerID)
)

CREATE TABLE OrderItems
(
             OrderID INT NOT NULL,
             ItemID  INT NOT NULL,
             CONSTRAINT PK_OrderItems PRIMARY KEY(OrderID, ItemID),
             CONSTRAINT FK_OrderItems_Orders FOREIGN KEY(OrderID) REFERENCES Orders(OrderID),
             CONSTRAINT FK_OrderItems_Items FOREIGN KEY(ItemID) REFERENCES Items(ItemID)
)

--------------------------
--06. University Database

CREATE TABLE Subjects(
	SubjectID INT IDENTITY NOT NULL,
	SubjectName NVARCHAR(50) NOT NULL,

	CONSTRAINT PK_Subjects
	PRIMARY KEY(SubjectID)
)

CREATE TABLE Majors(
	MajorID INT IDENTITY NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,

	CONSTRAINT PK_Majors
	PRIMARY KEY(MajorID)
)

CREATE TABLE Students(
	StudentID INT IDENTITY NOT NULL,
	StudentNumber NVARCHAR(50) NOT NULL,
	StudentName NVARCHAR(50) NOT NULL,
	MajorID INT NOT NULL,

	CONSTRAINT PK_Students
	PRIMARY KEY(StudentID),

	CONSTRAINT FK_Students_Majors
	FOREIGN KEY(MajorID)
	REFERENCES Majors(MajorID)
)

CREATE TABLE Payments(
	PaymentID INT IDENTITY NOT NULL,
	PaymentDate DATE NOT NULL,
	PaymentAmount MONEY NOT NULL,
	StudentID INT NOT NULL,

	CONSTRAINT PK_Payments
	PRIMARY KEY (PaymentID),

	CONSTRAINT FK_Payments_Students
	FOREIGN KEY (StudentID)
	REFERENCES Students(StudentID)
)

CREATE TABLE Agenda(
	StudentID INT NOT NULL,
	SubjectID INT NOT NULL,

	CONSTRAINT PK_Agenda
	PRIMARY KEY(StudentID, SubjectID),

	CONSTRAINT FK_Agenda_Students
	FOREIGN KEY(StudentID)
	REFERENCES Students(StudentID),

	CONSTRAINT FK_Agenda_Subjects
	FOREIGN KEY(SubjectID)
	REFERENCES Subjects(SubjectID)
)

---------------------
--09. *Peaks in Rila

SELECT MountainRange,
       PeakName,
       Elevation
FROM Peaks
     JOIN Mountains ON Peaks.MountainId = Mountains.Id
WHERE Peaks.MountainId =
(
    SELECT Id
    FROM Mountains
    WHERE MountainRange = 'Rila'
)
ORDER BY Peaks.Elevation DESC