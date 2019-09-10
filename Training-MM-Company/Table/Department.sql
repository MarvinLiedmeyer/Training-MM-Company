CREATE TABLE [dbo].[Department]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] NVARCHAR(256),
	[Description] NVARCHAR (MAX),
	[CreationTime] DATETIME2 DEFAULT getDate(),
	[DeleteTime] DATETIME2,
	[CompanyId] INT FOREIGN KEY REFERENCES [Company](Id)
)
