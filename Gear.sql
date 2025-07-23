use aamcgeardb

GO

DROP TABLE IF EXISTS ReservationGear;
DROP TABLE IF EXISTS GearInventory;
DROP TABLE IF EXISTS Reservations;
DROP TABLE IF EXISTS Categories;
DROP TABLE IF EXISTS Users;

GO

CREATE TABLE Users (
    UserID INT IDENTITY(30,1) NOT NULL PRIMARY KEY,
    FullName VARCHAR(255) NOT NULL,
);

CREATE TABLE Categories (
    CategoryID INT IDENTITY(20,1) NOT NULL PRIMARY KEY,
    CategoryName VARCHAR(255) UNIQUE NOT NULL -- 'Rope', 'Tent', 'Boots', 'Crampons'
);

CREATE TABLE GearInventory (
    GearID INT IDENTITY(10,1) NOT NULL PRIMARY KEY,
    TagNumber VARCHAR(50) UNIQUE NOT NULL,
    Picture VARCHAR(500) NULL, -- Static images first, then file upload later
    CategoryID INT NOT NULL,
    Description TEXT NOT NULL,
    Brand VARCHAR(255) NOT NULL, -- 'Mammut', 'Tedon', 'Beal'
    Size VARCHAR(50),
    Condition VARCHAR(50), -- 'New', 'Good', 'Needs Repair', 'Retired'
	HoursUsed INT NOT NULL,
	HoursLimit INT,
    IsAvailable BIT DEFAULT 1,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

CREATE TABLE Reservations (
    ReservationID INT IDENTITY(40,1) NOT NULL PRIMARY KEY,
    UserID INT NOT NULL,
    Status VARCHAR(50) NOT NULL, -- : 'Pending', 'Approved', 'Checked Out', 'Returned', 'Cancelled'
    RequestDate DATETIME DEFAULT GETDATE(),
    StartDate DATETIME NULL,
    EndDate DATETIME NULL,
    ReservationInstructions VARCHAR(255),
	EstimatedUseHours INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
);

CREATE TABLE ReservationGear (
	ReservationGearID INT IDENTITY (50,1) NOT NULL PRIMARY KEY,
	ReservationID INT NOT NULL,
	GearID INT NOT NULL,
	FOREIGN KEY (ReservationID) REFERENCES Reservations(ReservationID),
	FOREIGN KEY (GearID) REFERENCES GearInventory(GearID)
);

SELECT * FROM Users
SELECT * FROM Categories
SELECT * FROM GearInventory
SELECT * FROM Reservations
SELECT * FROM ReservationGear