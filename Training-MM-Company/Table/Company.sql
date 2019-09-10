CREATE TABLE [dbo].[Company]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] nvarChar(256) NOT NULL,
	[FoundedDate] date ,
	[Creationtime] Datetime2 DEFAULT getDate(),
	[DeleteTime] Datetime2 
)
