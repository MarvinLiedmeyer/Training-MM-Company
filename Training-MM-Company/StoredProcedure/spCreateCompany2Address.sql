CREATE PROCEDURE [dbo].[spCreateCompany2Address]
	@Street		NVARCHAR,
	@Zip		NVARCHAR,
	@City		NVARCHAR,
	@Country	NVARCHAR,
	@CompanyId	Int,
	@AddressId	Int

AS
	declare @dbId int = (select Id from [viAddress] where Id = @AddressId) 

      if(@dbId is null) 
      begin 
             INSERT INTO [dbo].[Address] 
           ([Street],
			[City],
			[Zip],
			[Country])

             VALUES 
                     (@Street,@City,@Zip,@Country) 

            set @dbId = @@IDENTITY 
			INSERT INTO [dbo].[Company2Address]
			([CompanyId],
			[AddressId])

			VALUES

			(@CompanyId, @dbId)
      end 
            else 
      begin 
			INSERT INTO [dbo].[Company2Address]
			([CompanyId],
			[AddressId])

			VALUES

			(@CompanyId, @dbId)
      end 



Select @dbId; 
RETURN @dbId;
