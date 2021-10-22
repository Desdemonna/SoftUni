--01. DDL

CREATE TABLE [Addresses](
	[Id] INT PRIMARY KEY IDENTITY,
	[Town] VARCHAR(30) NOT NULL,
	[Country] NVARCHAR(30) NOT NULL,
	[Streat] NVARCHAR(100) NOT NULL,
	[ZIP] VARCHAR(20) NOT NULL
)

CREATE TABLE [Sizes](
	[Id] INT PRIMARY KEY IDENTITY,
	[Length] INT NOT NULL,
	[RingRange] DECIMAL(8,2) NOT NULL,
	CHECK([Length] BETWEEN 10 AND 25),
	CHECK([RingRange] BETWEEN 1.5 AND 7.5)
)

CREATE TABLE [Brands](
	[Id] INT PRIMARY KEY IDENTITY,
	[BrandName] VARCHAR(30) UNIQUE NOT NULL,
	[BrandDescription] VARCHAR(max) NOT NULL
)

CREATE TABLE[Tastes](
	[Id] INT PRIMARY KEY IDENTITY,
	[TasteType] VARCHAR(20) NOT NULL,
	[TasteStrength] VARCHAR(15) NOT NULL,
	[ImageURL] NVARCHAR(100) NOT NULL
)

CREATE TABLE [Clients](
	[Id] INT PRIMARY KEY IDENTITY,
	[FirstName] NVARCHAR(30) NOT NULL,
	[LastName] NVARCHAR(30) NOT NULL,
	[Email] NVARCHAR(50) NOT NULL,
	[AddressId] INT FOREIGN KEY REFERENCES [Addresses]([Id]) NOT NULL
)

CREATE TABLE [Cigars](
	[Id] INT PRIMARY KEY IDENTITY,
	[CigarName] VARCHAR(80) NOT NULL,
	[BrandId] INT FOREIGN KEY REFERENCES [Brands]([Id]) NOT NULL,
	[TastId] INT FOREIGN KEY REFERENCES [Tastes]([Id]) NOT NULL,
	[SizeId] INT FOREIGN KEY REFERENCES [Sizes]([Id]) NOT NULL,
	[PriceForSingleCigar] DECIMAL(8,2) NOT NULL,
	[ImageURL] NVARCHAR(100) NOT NULL
)

CREATE TABLE [ClientsCigars](
	[ClientId] INT FOREIGN KEY REFERENCES [Clients]([Id]) NOT NULL,
	[CigarId] INT FOREIGN KEY REFERENCES [Cigars]([Id]) NOT NULL,
	PRIMARY KEY([ClientId], [CigarId])
)

--------------
--02. Insert

INSERT INTO [Cigars]([CigarName], [BrandId], [TastId], [SizeId], [PriceForSingleCigar], [ImageURL])
VALUES
('COHIBA ROBUSTO', 9, 1, 5, 15.50, 'cohiba-robusto-stick_18.jpg'),
('COHIBA SIGLO I', 9, 1, 10, 410.00, 'cohiba-siglo-i-stick_12.jpg'),
('HOYO DE MONTERREY LE HOYO DU MAIRE', 14, 5, 11, 7.50, 'hoyo-du-maire-stick_17.jpg'),
('HOYO DE MONTERREY LE HOYO DE SAN JUAN', 14, 4, 15, 32.00, 'hoyo-de-san-juan-stick_20.jpg'),
('TRINIDAD COLONIALES', 2, 3, 8, 85.21, 'trinidad-coloniales-stick_30.jpg')

INSERT INTO [Addresses]([Town], [Country], [Streat], [ZIP])
VALUES
('Sofia', 'Bulgaria', '18 Bul. Vasil levski', '1000'),
('Athens', 'Greece', '4342 McDonald Avenue', '10435'),
('Zagreb', 'Croatia', '4333 Lauren Drive', '10000')

-------------
--03. Update

UPDATE c
SET c.[PriceForSingleCigar] = c.[PriceForSingleCigar] * 1.2
FROM [Cigars] AS c
	JOIN Tastes AS t ON c.TastId = t.Id
	--JOIN Brands AS b ON c.BrandId = b.Id
	WHERE t.TasteType = 'Spicy'

UPDATE Brands
SET BrandDescription = 'New description'
WHERE BrandDescription IS NULL

--------------
--04. Delete

DELETE FROM [Clients]
WHERE [AddressId] IN (7, 8, 10) 

DELETE FROM [Addresses]
WHERE LEFT(Country, 1) = 'c'

----------------------
--05. Cigars by Price

SELECT [CigarName], PriceForSingleCigar, ImageURL FROM Cigars
ORDER BY PriceForSingleCigar ASC, CigarName DESC

------------------------
--06. Cigars by Taste

SELECT c.Id, c.CigarName, c.PriceForSingleCigar, t.TasteType, t.TasteStrength FROM Cigars AS c
LEFT JOIN Tastes AS t ON c.TastId = t.Id
WHERE t.TasteType IN ('Earthy', 'Woody')
ORDER BY c.PriceForSingleCigar DESC

-----------------------------
--07. Clients without Cigars

SELECT Id, CONCAT(c.FirstName, ' ', c.LastName) AS ClientName, Email FROM Clients AS c
LEFT JOIN ClientsCigars AS cc ON c.Id = cc.ClientId
WHERE ClientId IS NULL
ORDER BY ClientName

--------------------
--08. First 5 Cigars

SELECT TOP(5) c.CigarName, c.PriceForSingleCigar, c.ImageURL FROM Cigars AS c
LEFT JOIN Sizes AS s ON c.SizeId = s.Id
WHERE s.[Length] >= 12 AND (c.CigarName LIKE '%ci%' OR c.PriceForSingleCigar > 50) AND s.RingRange > 2.55
ORDER BY c.CigarName, c.PriceForSingleCigar DESC

----------------------------
--09. Clients with ZIP Codes

SELECT CONCAT(c.FirstName, ' ', c.LastName) AS [FullName], a.Country, a.ZIP, CONCAT('$', MAX(ci.PriceForSingleCigar)) AS [CigarPrice] FROM Clients c
LEFT JOIN Addresses a ON c.AddressId = a.Id
LEFT JOIN ClientsCigars cc ON c.Id = cc.ClientId
LEFT JOIN Cigars ci ON cc.CigarId = ci.Id
WHERE a.ZIP NOT LIKE '%[^0-9]%'
GROUP BY c.FirstName, c.LastName, a.Country, a.ZIP
ORDER BY FullName

--------------------
--10. Cigars by Size

SELECT LastName, AVG(s.[Length]) AS CiagrLength, CEILING(AVG(s.RingRange)) AS CiagrRingRange FROM Clients AS c
JOIN ClientsCigars AS cc on cc.ClientId = c.Id
JOIN Cigars AS ci ON ci.Id = cc.CigarId
JOIN Sizes AS s ON s.Id = ci.SizeId
GROUP BY LastName
ORDER BY AVG(s.[Length]) DESC

--------------------------
--11. Client with Cigars

CREATE FUNCTION udf_ClientWithCigars (@name VARCHAR(30))
RETURNS INT
AS
BEGIN
	DECLARE @countCigars INT 
	SET @countCigars = (SELECT COUNT(*) FROM ClientsCigars AS cc 
	JOIN Clients AS c ON cc.ClientId = c.Id 
	WHERE c.FirstName = @name);
	RETURN @countCigar
END

--------------------------------------------
--12. Search for Cigar with Specific Taste

CREATE PROC usp_SearchByTaste(@taste NVARCHAR(MAX))
AS
BEGIN
	SELECT c.CigarName, CONCAT('$', c.PriceForSingleCigar) AS [Price], t.TasteType, b.BrandName, CONCAT(s.[Length],' ', 'cm') AS [CigarLength], CONCAT(s.RingRange,' ', 'cm') AS [CigarRingRange] FROM Cigars AS c
	JOIN Brands AS b ON b.Id = c.BrandId
	JOIN Sizes as s ON s.Id = c.SizeId
	JOIN Tastes AS t ON t.Id = c.TastId
	WHERE t.TasteType = @taste
	ORDER BY s.[Length], s.RingRange DESC
END