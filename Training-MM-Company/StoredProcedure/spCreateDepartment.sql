CREATE PROCEDURE [dbo].[spCreateDepartment]
	@Id				Int,
	@Name			nvaRCHAR(128),
	@Description	nvarChar(MAX),
	@CompanyId		Int
AS
	declare @dbId int = (select Id from [viDepartment] where Id = @Id) 

      if(@dbId is null) 
      begin 
             INSERT INTO [dbo].[Department] 
           ([Name],
		    [Description],
			[CompanyId])

             VALUES 
                     (@Name, @Description, @CompanyId) 

            set @dbId = @@IDENTITY 
      end 
            else 
      begin 
            update  [dbo].[Department] set 
                        [Name] = case when @Name is null then [Name] else @Name end,
						[Description] = case when @Description is null then [Description] else @Description end,
						[CompanyId] = case when @CompanyId is null then [CompanyId] else @CompanyId end
			where Id = @dbId 
      end 



Select @dbId; 
RETURN @dbId;