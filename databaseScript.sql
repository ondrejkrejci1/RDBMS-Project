USE [krejci3]
GO
/****** Object:  UserDefinedFunction [dbo].[AthleteExists]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
    IF EXISTS (
        SELECT 1 
        FROM Athlete 
        WHERE FirstName = @FirstName 
          AND LastName = @LastName 
          AND BirthDate = @BirthDate
    )
        RETURN 1;
    
    RETURN 0;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[ClubExists]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ClubExists]
(
    @Name NVARCHAR(100)
)
RETURNS BIT
AS
BEGIN
    -- Pokud existuje řádek s tímto názvem, vrať 1 (True)
    IF EXISTS (SELECT 1 FROM Club WHERE Name = @Name)
        RETURN 1;
    
    -- Jinak vrať 0 (False)
    RETURN 0;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[CompetitionExists]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[CompetitionExists]
(
    @Name NVARCHAR(100),
    @Date DATE
)
RETURNS BIT
AS
BEGIN
    IF EXISTS (
        SELECT 1 
        FROM Competition 
        WHERE Name = @Name 
          AND Date = @Date
    )
        RETURN 1;
    
    RETURN 0;
END;
GO
/****** Object:  Table [dbo].[Region]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Region](
	[RegionID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RegionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Club]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Club](
	[ClubID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[RegionID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ClubID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Athlete]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Athlete](
	[AthleteID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[BirthDate] [date] NOT NULL,
	[Gender] [char](1) NOT NULL,
	[ClubID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AthleteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Result]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
PRIMARY KEY CLUSTERED 
(
	[ResultID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ClubStatistics]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   VIEW [dbo].[ClubStatistics] AS
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
/****** Object:  Table [dbo].[Discipline]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discipline](
	[DisciplineID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UnitType] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DisciplineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Competition]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Competition](
	[CompetitionID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Date] [date] NOT NULL,
	[Venue] [nvarchar](100) NULL,
	[Type] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CompetitionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[TopPerformances]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   VIEW [dbo].[TopPerformances] AS
SELECT TOP 100
    R.DisciplineID, -- <--- TOTO JSME PŘIDALI
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
ALTER TABLE [dbo].[Athlete] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Athlete]  WITH CHECK ADD FOREIGN KEY([ClubID])
REFERENCES [dbo].[Club] ([ClubID])
GO
ALTER TABLE [dbo].[Club]  WITH CHECK ADD FOREIGN KEY([RegionID])
REFERENCES [dbo].[Region] ([RegionID])
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD FOREIGN KEY([AthleteID])
REFERENCES [dbo].[Athlete] ([AthleteID])
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD FOREIGN KEY([CompetitionID])
REFERENCES [dbo].[Competition] ([CompetitionID])
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD FOREIGN KEY([DisciplineID])
REFERENCES [dbo].[Discipline] ([DisciplineID])
GO
ALTER TABLE [dbo].[Athlete]  WITH CHECK ADD CHECK  (([Gender]='F' OR [Gender]='M'))
GO
ALTER TABLE [dbo].[Competition]  WITH CHECK ADD CHECK  (([Type]='OUTDOOR' OR [Type]='INDOOR'))
GO
ALTER TABLE [dbo].[Discipline]  WITH CHECK ADD CHECK  (([UnitType]='POINTS' OR [UnitType]='METERS' OR [UnitType]='TIME'))
GO
/****** Object:  StoredProcedure [dbo].[AddRaceResutl]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
    SET XACT_ABORT ON; -- Důležité pro automatický rollback při chybě

    BEGIN TRANSACTION;

    BEGIN TRY
        -- 1. Zjistíme ID Regionu (a vytvoříme ho, pokud neexistuje)
        DECLARE @RegionID INT;
        SELECT @RegionID = RegionID FROM Region WHERE Name = @RegionName;

        IF @RegionID IS NULL
        BEGIN
            INSERT INTO Region (Name) VALUES (@RegionName);
            SET @RegionID = SCOPE_IDENTITY(); -- Vezmeme ID nového regionu
        END

        -- 2. Zjistíme/Vytvoříme Klub (použijeme @RegionID, které už 100% máme)
        DECLARE @ClubID INT;
        
        -- Zkusíme najít klub v daném regionu
        SELECT @ClubID = ClubID FROM Club WHERE Name = @ClubName AND RegionID = @RegionID;

        IF @ClubID IS NULL
        BEGIN
            INSERT INTO Club (Name, RegionID) VALUES (@ClubName, @RegionID);
            SET @ClubID = SCOPE_IDENTITY();
        END

        -- 3. Zjistíme/Vytvoříme Atleta
        DECLARE @AthleteID INT;
        -- Používáme tvou funkci pro kontrolu existence
        IF dbo.AthleteExists(@FirstName, @LastName, @BirthDate) = 1
        BEGIN
             -- Atlet existuje, najdeme jeho ID
             SELECT @AthleteID = AthleteID FROM Athlete WHERE FirstName = @FirstName AND LastName = @LastName AND BirthDate = @BirthDate;
        END
        ELSE
        BEGIN
             -- Atlet neexistuje, vytvoříme ho
             INSERT INTO Athlete (FirstName, LastName, BirthDate, Gender, ClubID) 
             VALUES (@FirstName, @LastName, @BirthDate, @Gender, @ClubID);
             SET @AthleteID = SCOPE_IDENTITY();
        END

        -- 4. Zjistíme/Vytvoříme Soutěž
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

        -- 5. Vložíme Výsledek (použijeme IDčka, která jsme si připravili výše)
        INSERT INTO Result(AthleteID, CompetitionID, DisciplineID, Performance, Wind, Placement, Note) 
        VALUES(
            @AthleteID,
            @CompetitionID,
            (SELECT DisciplineID FROM Discipline WHERE Name = @DisciplineName), -- Předpokládáme, že Disciplíny jsou fixní
            @Performance,
            @Wind,
            @Placement,
            @Note
        );

        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[GetAthleteHistory]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
    ORDER BY CompetitionDate DESC; -- Seřazeno od nejnovějšího závodu
END;

GO
/****** Object:  StoredProcedure [dbo].[ImportAthlete]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[ImportAthlete]
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @BirthDate DATETIME,
    @Gender CHAR(1),
    @ClubName NVARCHAR(100),
    @ClubRegionName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @AthleteExists BIT;
    SET @AthleteExists = dbo.AthleteExists(@FirstName, @LastName, @BirthDate);

    IF @AthleteExists = 1
    BEGIN
        PRINT 'Skipping: Athlete already exists.';
        RETURN;
    END

    BEGIN TRANSACTION;

    BEGIN TRY
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
            SELECT @ClubID = ClubID 
            FROM Club 
            WHERE Name = @ClubName AND RegionID = @RegionID;
        END
        ELSE
        BEGIN
            SET @ClubID = NULL;
        END

        IF @ClubID IS NULL
        BEGIN
            INSERT INTO Club (Name, RegionID) 
            VALUES (@ClubName, @RegionID); 

            SET @ClubID = SCOPE_IDENTITY();
        END

        INSERT INTO Athlete (FirstName, LastName, BirthDate, Gender, ClubID)
        VALUES (@FirstName, @LastName, @BirthDate, @Gender, @ClubID);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[ImportCompetition]    Script Date: 1/8/2026 10:39:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ImportCompetition]
    @Name NVARCHAR(100),
    @Date DATETIME,
    @Venue NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON; -- Pro bezpečné transakce

    -- 1. KONTROLA EXISTENCE (Tvá funkce)
    -- Uložíme výsledek do proměnné pro čistotu kódu
    DECLARE @Exists BIT;
    SET @Exists = dbo.CompetitionExists(@Name, @Date);

    IF @Exists = 1
    BEGIN
        PRINT 'Skipping: Competition already exists (' + @Name + ').';
        RETURN;
    END

    -- 2. VLOŽENÍ ZÁVODU
    BEGIN TRANSACTION;

    BEGIN TRY
        INSERT INTO Competition (Name, Date, Venue)
        VALUES (@Name, @Date, @Venue);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        ;THROW; -- Středník před THROW je nutný
    END CATCH
END
GO
