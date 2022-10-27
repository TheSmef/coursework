CREATE PROCEDURE [dbo].[DeleteCategory]
@Id integer
AS 
Delete From [dbo].[Categories] 
Where Id_Category = @Id 
GO
CREATE PROCEDURE [dbo].[UpdateCategory]
@Id int, @Name varchar(30)
AS 
Update [dbo].[Categories]
SET Name = @Name 
Where Id_Category = @Id 
GO
CREATE PROCEDURE [dbo].[InsertCategory] 
@Name varchar(30) 
AS 
Insert Into [dbo].[Categories] 
([Name]) values (@Name) 
GO