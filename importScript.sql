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

GO

Omlouvám se, máš pravdu. Nevšiml jsem si, že ve tvé databázi je i sloupec UnitType a že názvy disciplín neobsahují prefix "Run" (např. máš jen 50 m místo Run 50m).

Tady je opravený SQL skript, který přesně odpovídá struktuře a hodnotám z tvého obrázku. Doplnil jsem logicky UnitType i pro ostatní disciplíny (Skoky/Hody = METERS, Víceboje = POINTS).

SQL

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