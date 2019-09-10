CREATE PROCEDURE [dbo].[spCreateEmployee]
	@Id				INT = 0,
	@Firstname		nVARCHAR(128),
	@LastName		nVARCHAR(128),
	@BeginDate		date,
	@DepartmentId	INT,
	@AddressId		INT
AS
	declare @dbId int = (select Id from viEmployee where Id = @Id) 

      if(@dbId is null) 
      begin 
             INSERT INTO [dbo].[Employee] 
           ([FirstName] 
           ,[LastName] 
           ,[BeginDate] 
           ,[DepartmentId]
		   ,[AddressId]) 
             VALUES 
                     (@FirstName,@LastName,@BeginDate,@DepartmentId,@AddressId) 

            set @dbId = @@IDENTITY 
      end 
            else 
      begin 
            update  [dbo].[Employee] set 
                        FirstName = case when @FirstName is null then FirstName else @FirstName end,
						LastName = case when @LastName is null then LastName else @LastName end,
						BeginDate = case when @BeginDate is null then BeginDate else @BeginDate end,
						DepartmentId = case when @DepartmentId is null then DepartmentId else @DepartmentId end,
						AddressId = case when @AddressId is null then AddressId else @AddressId end
			where id = @dbId 
      end 



Select @dbId; 
RETURN @dbId;
