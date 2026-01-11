/*
===================================================================
PROJECT: Athletics Manager
AUTHOR: Ondřej Krejčí
CONTACT: krejci3@spsejecna.cz
DATE: January 11, 2026

DESCRIPTION:
This script performs a full database installation wrapped in a SINGLE TRANSACTION.
It includes:
1. DDL: Database structure (Tables, Views, Procedures, Functions).
2. DML: Static data population (Regions, Disciplines).

IMPORTANT:
- The script uses 'SET XACT_ABORT ON'. If any error occurs, 
  the entire transaction is automatically rolled back.
===================================================================
*/

-- Enable automatic rollback on error
SET XACT_ABORT ON;

-- Start the Global Transaction
BEGIN TRANSACTION;
GO

-- =============================================
-- SECTION 1: DDL - STRUCTURE CREATION
-- =============================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 1.1 TABLES
PRINT 'Creating Tables...'
GO

CREATE TABLE [dbo].[Region](
	[RegionID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED ([RegionID] ASC)
)
GO

CREATE TABLE [dbo].[Club](
	[ClubID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[RegionID] [int] NOT NULL,
PRIMARY KEY CLUSTERED ([ClubID] ASC)
)
GO

CREATE TABLE [dbo].[Athlete](
	[AthleteID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[BirthDate] [date] NOT NULL,
	[Gender] [char](1) NOT NULL,
	[ClubID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT 1,
PRIMARY KEY CLUSTERED ([AthleteID] ASC)
)
GO

CREATE TABLE [dbo].[Discipline](
	[DisciplineID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UnitType] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED ([DisciplineID] ASC)
)
GO

CREATE TABLE [dbo].[Competition](
	[CompetitionID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Date] [date] NOT NULL,
	[Venue] [nvarchar](100) NULL,
	[Type] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED ([CompetitionID] ASC)
)
GO

CREATE TABLE [dbo].[Result](
	[ResultID] [int] IDENTITY(1,1) NOT NULL,
	[AthleteID] [int] NOT NULL,
	[CompetitionID] [int] NOT NULL,
	[DisciplineID] [int] NOT NULL,
	[Performance] [decimal](10, 2) NULL,
	[Wind] [float] NULL,
	[Placement] [int] NULL,
	[Note] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED ([ResultID] ASC)
)
GO

-- 1.2 CONSTRAINTS & FOREIGN KEYS
PRINT 'Adding Constraints...'
GO

ALTER TABLE [dbo].[Athlete] WITH CHECK ADD FOREIGN KEY([ClubID]) REFERENCES [dbo].[Club] ([ClubID])
GO
ALTER TABLE [dbo].[Club] WITH CHECK ADD FOREIGN KEY([RegionID]) REFERENCES [dbo].[Region] ([RegionID])
GO
ALTER TABLE [dbo].[Result] WITH CHECK ADD FOREIGN KEY([AthleteID]) REFERENCES [dbo].[Athlete] ([AthleteID])
GO
ALTER TABLE [dbo].[Result] WITH CHECK ADD FOREIGN KEY([CompetitionID]) REFERENCES [dbo].[Competition] ([CompetitionID])
GO
ALTER TABLE [dbo].[Result] WITH CHECK ADD FOREIGN KEY([DisciplineID]) REFERENCES [dbo].[Discipline] ([DisciplineID])
GO
ALTER TABLE [dbo].[Athlete] WITH CHECK ADD CHECK (([Gender]='F' OR [Gender]='M'))
GO
ALTER TABLE [dbo].[Competition] WITH CHECK ADD CHECK (([Type]='OUTDOOR' OR [Type]='INDOOR'))
GO
ALTER TABLE [dbo].[Discipline] WITH CHECK ADD CHECK (([UnitType]='POINTS' OR [UnitType]='METERS' OR [UnitType]='TIME'))
GO

-- 1.3 FUNCTIONS
PRINT 'Creating Functions...'
GO

CREATE FUNCTION [dbo].[AthleteExists]
(
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@BirthDate DATE
)
RETURNS BIT
AS
BEGIN
	IF EXISTS (SELECT 1 FROM Athlete WHERE FirstName = @FirstName AND LastName = @LastName AND BirthDate = @BirthDate)
		RETURN 1;
	RETURN 0;
END
GO

CREATE FUNCTION [dbo].[ClubExists]
(
	@Name NVARCHAR(100)
)
RETURNS BIT
AS
BEGIN
	IF EXISTS (SELECT 1 FROM Club WHERE Name = @Name)
		RETURN 1;
	RETURN 0;
END
GO

CREATE FUNCTION [dbo].[CompetitionExists]
(
	@Name NVARCHAR(100),
	@Date DATE
)
RETURNS BIT
AS
BEGIN
	IF EXISTS (SELECT 1 FROM Competition WHERE Name = @Name AND Date = @Date)
		RETURN 1;
	RETURN 0;
END
GO

-- 1.4 VIEWS
PRINT 'Creating Views...'
GO

CREATE VIEW [dbo].[ClubStatistics] AS
SELECT 
	C.Name AS ClubName,
	R.Name AS RegionName,
	COUNT(DISTINCT A.AthleteID) AS AthleteCount,
	COUNT(Res.ResultID) AS TotalEntries,
	SUM(CASE WHEN Res.Placement = 1 THEN 1 ELSE 0 END) AS GoldMedals,
	MIN(A.BirthDate) AS OldestAthleteBorn,
	MAX(A.BirthDate) AS YoungestAthleteBorn
FROM Club C
JOIN Region R ON C.RegionID = R.RegionID
LEFT JOIN Athlete A ON C.ClubID = A.ClubID
LEFT JOIN Result Res ON A.AthleteID = Res.AthleteID
GROUP BY C.Name, R.Name;
GO

CREATE VIEW [dbo].[TopPerformances] AS
SELECT TOP 100
	R.DisciplineID,
	D.Name AS Discipline,
	R.Performance,
	A.FirstName + ' ' + A.LastName AS AthleteName,
	C.Name AS CompetitionName,
	C.Date AS CompetitionDate
FROM Result R
JOIN Athlete A ON R.AthleteID = A.AthleteID
JOIN Competition C ON R.CompetitionID = C.CompetitionID
JOIN Discipline D ON R.DisciplineID = D.DisciplineID
ORDER BY R.Performance DESC;
GO

-- FIX: Adding missing View 'AllResultsDetail' required for GetAthleteHistory
CREATE VIEW [dbo].[AllResultsDetail] AS
SELECT 
    R.ResultID,
    R.AthleteID,
    C.Date AS CompetitionDate,
    C.Name AS CompetitionName,
    D.Name AS Discipline,
    R.Performance,
    D.UnitType,
    R.Wind,
    R.Placement,
    R.Note,
    C.Type AS CompetitionType
FROM Result R
JOIN Competition C ON R.CompetitionID = C.CompetitionID
JOIN Discipline D ON R.DisciplineID = D.DisciplineID;
GO

-- 1.5 STORED PROCEDURES
PRINT 'Creating Stored Procedures...'
GO

CREATE PROCEDURE [dbo].[AddRaceResutl] 
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@BirthDate DATE,
	@Gender CHAR(1),
	@ClubName NVARCHAR(100),
	@RegionName NVARCHAR(50),
	@CompetitionName NVARCHAR(100),
	@CompetitionDate DATE,
	@CompetitionVenue NVARCHAR(100),
	@CompetitionType NVARCHAR(20),
	@DisciplineName NVARCHAR(50),
	@Performance DECIMAL(10,2),
	@Wind FLOAT = NULL,
	@Placement INT = NULL,
	@Note NVARCHAR(10) = NULL
AS
BEGIN
	SET NOCOUNT ON;
    -- Note: Nested transactions are handled automatically by XACT_ABORT in the outer scope
	DECLARE @RegionID INT;
	SELECT @RegionID = RegionID FROM Region WHERE Name = @RegionName;
	IF @RegionID IS NULL
	BEGIN
		INSERT INTO Region (Name) VALUES (@RegionName);
		SET @RegionID = SCOPE_IDENTITY();
	END

	DECLARE @ClubID INT;
	SELECT @ClubID = ClubID FROM Club WHERE Name = @ClubName AND RegionID = @RegionID;
	IF @ClubID IS NULL
	BEGIN
		INSERT INTO Club (Name, RegionID) VALUES (@ClubName, @RegionID);
		SET @ClubID = SCOPE_IDENTITY();
	END

	DECLARE @AthleteID INT;
	IF dbo.AthleteExists(@FirstName, @LastName, @BirthDate) = 1
	BEGIN
			SELECT @AthleteID = AthleteID FROM Athlete WHERE FirstName = @FirstName AND LastName = @LastName AND BirthDate = @BirthDate;
	END
	ELSE
	BEGIN
			INSERT INTO Athlete (FirstName, LastName, BirthDate, Gender, ClubID) 
			VALUES (@FirstName, @LastName, @BirthDate, @Gender, @ClubID);
			SET @AthleteID = SCOPE_IDENTITY();
	END

	DECLARE @CompetitionID INT;
	IF dbo.CompetitionExists(@CompetitionName, @CompetitionDate) = 1
	BEGIN
			SELECT @CompetitionID = CompetitionID FROM Competition WHERE Name = @CompetitionName AND Date = @CompetitionDate;
	END
	ELSE
	BEGIN
			INSERT INTO Competition(Name, Date, Venue, Type) 
			VALUES (@CompetitionName, @CompetitionDate, @CompetitionVenue, @CompetitionType);
			SET @CompetitionID = SCOPE_IDENTITY();
	END

	INSERT INTO Result(AthleteID, CompetitionID, DisciplineID, Performance, Wind, Placement, Note) 
	VALUES(
		@AthleteID,
		@CompetitionID,
		(SELECT DisciplineID FROM Discipline WHERE Name = @DisciplineName),
		@Performance,
		@Wind,
		@Placement,
		@Note
	);
END;
GO

CREATE PROCEDURE [dbo].[GetAthleteHistory]
	@AthleteID INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		CompetitionDate,
		CompetitionName,
		Discipline,
		Performance,
		UnitType,
		Wind,
		Placement,
		Note,
		CompetitionType
	FROM AllResultsDetail
	WHERE AthleteID = @AthleteID
	ORDER BY CompetitionDate DESC;
END;
GO

-- UPDATED PROCEDURE: Now includes @IsActive
CREATE PROCEDURE [dbo].[ImportAthlete]
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@BirthDate DATETIME,
	@Gender CHAR(1),
	@IsActive BIT,
	@ClubName NVARCHAR(100),
	@ClubRegionName NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @AthleteExists BIT;
	SET @AthleteExists = dbo.AthleteExists(@FirstName, @LastName, @BirthDate);
	IF @AthleteExists = 1
	BEGIN
		PRINT 'Skipping: Athlete already exists.';
		RETURN;
	END
	
    DECLARE @ClubID INT;
	DECLARE @RegionID INT;
	DECLARE @DoesClubExist BIT;

	SELECT @RegionID = RegionID FROM Region WHERE Name = @ClubRegionName;
	IF @RegionID IS NULL
	BEGIN
		;THROW 50001, 'Error: Region not found.', 1;
	END
	SET @DoesClubExist = dbo.ClubExists(@ClubName);
	IF @DoesClubExist = 1
	BEGIN
		SELECT @ClubID = ClubID FROM Club WHERE Name = @ClubName AND RegionID = @RegionID;
	END
	ELSE
	BEGIN
		SET @ClubID = NULL;
	END
	IF @ClubID IS NULL
	BEGIN
		INSERT INTO Club (Name, RegionID) VALUES (@ClubName, @RegionID); 
		SET @ClubID = SCOPE_IDENTITY();
	END
	INSERT INTO Athlete (FirstName, LastName, BirthDate, Gender, IsActive, ClubID)
	VALUES (@FirstName, @LastName, @BirthDate, @Gender, @IsActive, @ClubID);
END
GO

-- UPDATED PROCEDURE: Now includes @Type
CREATE PROCEDURE [dbo].[ImportCompetition]
	@Name NVARCHAR(100),
	@Date DATETIME,
	@Venue NVARCHAR(100),
	@Type NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Exists BIT;
	SET @Exists = dbo.CompetitionExists(@Name, @Date);
	IF @Exists = 1
	BEGIN
		PRINT 'Skipping: Competition already exists (' + @Name + ').';
		RETURN;
	END
	
    INSERT INTO Competition (Name, Date, Venue, Type)
	VALUES (@Name, @Date, @Venue, @Type);
END
GO


-- =============================================
-- SECTION 2: DML - DATA IMPORT (Static Data)
-- =============================================

PRINT 'Starting Data Import (Regions and Disciplines)...';

-- 2.1 Insert Regions
SET IDENTITY_INSERT Region ON;

INSERT INTO Region (RegionID, Name) VALUES 
(1, 'Hlavní město Praha'),
(2, 'Středočeský kraj'),
(3, 'Jihočeský kraj'),
(4, 'Plzeňský kraj'),
(5, 'Karlovarský kraj'),
(6, 'Ústecký kraj'),
(7, 'Liberecký kraj'),
(8, 'Královéhradecký kraj'),
(9, 'Pardubický kraj'),
(10, 'Kraj Vysočina'),
(11, 'Jihomoravský kraj'),
(12, 'Olomoucký kraj'),
(13, 'Zlínský kraj'),
(14, 'Moravskoslezský kraj'),
(15, 'Other');

SET IDENTITY_INSERT Region OFF;
PRINT 'Regions inserted.';
GO

-- 2.2 Insert Disciplines
SET IDENTITY_INSERT Discipline ON;

INSERT INTO Discipline (DisciplineID, Name, UnitType) VALUES 
(1, '50 m', 'TIME'),
(2, '60 m', 'TIME'),
(3, '100 m', 'TIME'),
(4, '150 m', 'TIME'),
(5, '200 m', 'TIME'),
(6, '300 m', 'TIME'),
(7, '400 m', 'TIME'),
(8, '500 m', 'TIME'),
(9, '600 m', 'TIME'),
(10, '800 m', 'TIME'),
(11, '1000 m', 'TIME'),
(12, '1500 m', 'TIME'),
(13, '1 Mile', 'TIME'),
(14, '2000 m', 'TIME'),
(15, '3000 m', 'TIME'),
(16, '5000 m', 'TIME'),
(17, '10000 m', 'TIME'),
(18, '50 m Hurdles', 'TIME'),
(19, '60 m Hurdles', 'TIME'),
(20, '80 m Hurdles', 'TIME'),
(21, '100 m Hurdles', 'TIME'),
(22, '110 m Hurdles', 'TIME'),
(23, '200 m Hurdles', 'TIME'),
(24, '300 m Hurdles', 'TIME'),
(25, '400 m Hurdles', 'TIME'),
(26, '1500 m Steeplechase', 'TIME'),
(27, '2000 m Steeplechase', 'TIME'),
(28, '3000 m Steeplechase', 'TIME'),
(29, 'Long Jump', 'METERS'),
(30, 'Triple Jump', 'METERS'),
(31, 'High Jump', 'METERS'),
(32, 'Pole Vault', 'METERS'),
(33, 'Standing Long Jump', 'METERS'),
(34, 'Shot Put', 'METERS'),
(35, 'Discus Throw', 'METERS'),
(36, 'Javelin Throw', 'METERS'),
(37, 'Hammer Throw', 'METERS'),
(38, 'Cricket Ball Throw', 'METERS'),
(39, 'Relay 4x60m', 'TIME'),
(40, 'Relay 4x100m', 'TIME'),
(41, 'Relay 4x200m', 'TIME'),
(42, 'Relay 4x300m', 'TIME'),
(43, 'Relay 4x400m', 'TIME'),
(44, 'Walk 3000m', 'TIME'),
(45, 'Walk 5000m', 'TIME'),
(46, 'Walk 10km', 'TIME'),
(47, 'Walk 20km', 'TIME'),
(48, 'Triathlon', 'POINTS'),
(49, 'Tetrathlon', 'POINTS'),
(50, 'Pentathlon', 'POINTS'),
(51, 'Heptathlon', 'POINTS'),
(52, 'Decathlon', 'POINTS');

SET IDENTITY_INSERT Discipline OFF;
PRINT 'Disciplines inserted.';
GO

-- =============================================
-- COMMIT GLOBAL TRANSACTION
-- =============================================
COMMIT TRANSACTION;
PRINT 'Database installation completed successfully.';
GO
