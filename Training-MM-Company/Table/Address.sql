CREATE TABLE [dbo].[Address]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Street] NVARCHAR(256) ,
	[City] NVARCHAR(256),
	[Zip] NVARCHAR(128),
	[Country] NVARCHAR(128),
	[DeleteTime] Datetime2 
)
