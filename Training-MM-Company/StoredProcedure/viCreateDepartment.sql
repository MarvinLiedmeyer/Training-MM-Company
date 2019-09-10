CREATE PROCEDURE [dbo].[viCreateDepartment]
	@Id				Int,
	@Name			nvaRCHAR(128),
	@CreationTime	Datetime2,
	@DeleteTime		Datetime2,
	@CompanyId		Int
AS
	declare @dbId int = (select Id from [viDepartment] where Id = @Id) 

      if(@dbId is null) 
      begin 
             INSERT INTO [dbo].[Department] 
           ([Name],
			[CreationTime],
			[DeleteTime],
			[CompanyId])

             VALUES 
                     (@Name,@CreationTime,@DeleteTime,@CompanyId) 

            set @dbId = @@IDENTITY 
      end 
            else 
      begin 
            update  [dbo].[Department] set 
                        [Name] = case when @Name is null then [Name] else @Name end,
						[CompanyId] = case when @CompanyId is null then [CompanyId] else @CompanyId end
			where Id = @dbId 
      end 



Select @dbId; 
RETURN @dbId;