CREATE PROCEDURE [dbo].[spCreateAddress]
	@Id			INT = 0,
	@Street		nVARCHAR(128),
	@City		nVarCHar(128),
	@Zip		nVARCHAR(128),
	@Country	nVARCHAR(128)
AS
	declare @dbId int = (select Id from [viAddress] where Id = @Id) 

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
      end 
            else 
      begin 
            update  [dbo].[Address] set 
                        [Street] = case when @Street is null then [Street] else @Street end,
						[City] = case when @City is null then [City] else @City end,
						[Zip] = case when @Zip is null then [Zip] else @Zip end,
						[Country] = case when @Country is null then [Country] else @Country end
			where Id = @dbId 
      end 



Select @dbId; 
RETURN @dbId;

