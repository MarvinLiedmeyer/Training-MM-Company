CREATE PROCEDURE [dbo].[spCreateCompany]
	@Id				INT=0,
	@Name			NVARCHAR (128),
	@FoundedDate	date
AS
	declare @dbId int = (select Id from [viCompany] where Id = @Id) 

      if(@dbId is null) 
      begin 
             INSERT INTO [dbo].[Company] 
           ([Name] 
           ,[FoundedDate])
             VALUES 
                     (@Name,@FoundedDate) 

            set @dbId = @@IDENTITY 
      end 
            else 
      begin 
            update  [dbo].[Company] set 
                        [Name] =		case when @Name is null then [Name] else @Name end,
						[FoundedDate] =	case when @FoundedDate is null then [FoundedDate] else @FoundedDate end
			where Id = @dbId 
      end 



Select @dbId; 
RETURN @dbId;