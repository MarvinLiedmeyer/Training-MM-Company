CREATE VIEW [dbo].[viEmployee]
	AS 
		SELECT	[Employee].[Id] AS Nummer
				,[Employee].[FirstName]
				,[Employee].[LastName]
				,[Employee].[BeginDate]
				,[Address].[Id] 
				,[Address].[Street]
				,[Address].[City]
				,[Address].[Zip]
				,[Address].[Country]
				,[Department].[Name]

		FROM [Employee]
		INNER JOIN [Address]
		ON [Employee].[AddressId] = [Address].[Id] 
		INNER JOIN [Department]
		ON [Employee].[DepartmentId] = [Department].[Id] 
	
