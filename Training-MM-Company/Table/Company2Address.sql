CREATE TABLE [dbo].[Company2Address]
(
	[CompanyId] INT NOT NULL REFERENCES [Company](Id),
	[AddressId] INT NOT NULL REFERENCES [Address](Id),
	[CreationTime] DATETIME2,
	Constraint [PK_Company2Address] PRIMARY KEY ([CompanyId], [AddressId]),
)
