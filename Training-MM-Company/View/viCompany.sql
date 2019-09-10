CREATE VIEW [dbo].[viCompany]
	AS 
		SELECT		[Company].[Id]
					,[Company].[Name]
					,[Company].[FoundedDate]
					,COUNT ([Company2Address].[AddressId]) AS Location

		FROM		[Company]
					Left JOIN [Company2Address] 
					ON [Company].[Id] = [Company2Address].[CompanyId]
		GROUP BY	[Company].[Name], [Company].[Id], [Company].[FoundedDate]