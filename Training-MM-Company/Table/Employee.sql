CREATE TABLE [dbo].[Employee]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[FirstName] NVARCHAR(128) NOT NULL,
	[LastName] NVARCHAR(128) NOT NULL,
	[BeginDate] DATE,
	[CreationTime] DATETIME2 DEFAULT getDate(),
	[DeleteTime] DATETIME2,
	[DepartmentId] INT Foreign KEY REFERENCES [Department](Id),
	[AddressId] INT Foreign KEY REFERENCES [Address](Id)
)
