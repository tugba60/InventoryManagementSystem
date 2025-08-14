CREATE TABLE Users (
	UserId INT PRIMARY KEY IDENTITY(1,1),
	UserName VARCHAR(50) NOT NULL UNIQUE,
	eMail VARCHAR(100) UNIQUE,
	PasswordHash VARCHAR(128) NOT NULL,
	Role_ BIT NOT NULL DEFAULT 0, --herkes user olarak kaydolur. 0:user 1:admin
	CreateDate DATETIME NOT NULL DEFAULT GETDATE(),
	AccountStatus VARCHAR(20) NOT NULL DEFAULT 'Active'
);

CREATE TABLE Products (
	ProductId INT PRIMARY KEY IDENTITY(1,1),
	ProductName VARCHAR(50),
	ProductCode VARCHAR(30) NOT NULL UNIQUE,
	Category VARCHAR(30),
	BrandModel VARCHAR(50),
	Quantity INT NOT NULL DEFAULT 0,
	Unit VARCHAR(20) NOT NULL,
	EntryDate DATETIME NOT NULL DEFAULT GETDATE(),
	WarrantyEndDate DATE NOT NULL,
	Location_ VARCHAR(50) NOT NULL,
	Status_ VARCHAR(20) NOT NULL DEFAULT 'New',
	Description_ TEXT,
	AddedByUserId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,	
	CONSTRAINT chk_WarrantyDuration CHECK (DATEDIFF(DAY,EntryDate,WarrantyEndDate)>60)
);

CREATE TABLE Transactions (
	TransactionId INT PRIMARY KEY IDENTITY(1,1),
	Productid INT FOREIGN KEY REFERENCES Products(ProductId) NOT NULL,
	Userid INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
	TransactionType BIT NOT NULL, --1:ekleme, 0:çýkarma
	Amount INT NOT NULL DEFAULT 0,
	TransactionDate DATETIME NOT NULL DEFAULT GETDATE(),
	Note VARCHAR(MAX)
);

CREATE TABLE MainTenance (
	MainTenanceId INT PRIMARY KEY IDENTITY(1,1),
	ProductId INT FOREIGN KEY REFERENCES Products(ProductId) NOT NULL,
	PerformedByUserId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
	ProcessDate DATETIME NOT NULL DEFAULT GETDATE(),
	Description_ VARCHAR(255),
	StatusAfter VARCHAR(50) NOT NULL
);

CREATE TABLE Logs (
	LogId INT PRIMARY KEY IDENTITY(1,1),
	UserId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
	Action_ VARCHAR(50) NOT NULL,
	TableAffected VARCHAR(30),
	RecordId INT,
	logDescription_ VARCHAR(50),
	ActionDate DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TRIGGER trg_trans
ON Transactions
AFTER INSERT
AS
BEGIN
	UPDATE P
	SET P.Quantity =
		CASE 
			WHEN T.TransactionType = 1 THEN P.Quantity + T.Amount
			WHEN T.TransactionType = 0 THEN P.Quantity - T.Amount
		END
	FROM Products P
	INNER JOIN Inserted T ON P.ProductId=T.Productid
END