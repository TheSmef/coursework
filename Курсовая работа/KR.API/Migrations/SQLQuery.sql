

CREATE OR ALTER PROCEDURE DeleteCategory (@Id uniqueidentifier)
AS
Delete From Categories 
Where Id_Category = @Id
GO

CREATE OR ALTER PROCEDURE UpdateCategory (@Id uniqueidentifier, @Name varchar(30)) 
AS 
Update Categories 
SET Name = @Name 
Where Id_Category = @Id
GO

CREATE OR ALTER PROCEDURE InsertCategory (@Name varchar(30)) 
AS 
Insert 
Into Categories (Id_Category, Name) 
Values (NEWID(), @Name)
Go

CREATE OR ALTER TRIGGER ProductStorage_INSERT
ON ProductStorages
AFTER INSERT
AS
INSERT INTO StorageHistory
(Id_StorageHistory ,Summary, ProductStorageId_Product_Storage, Date)
VALUES
(NEWID(), CONCAT('был создан новый товар "', (SELECT Name FROM INSERTED), '", с начальным количеством - ', (SELECT Amount FROM INSERTED)),
(SELECT Id_Product_Storage FROM INSERTED),
GETDATE())
GO

CREATE OR ALTER TRIGGER ProductStorage_UPDATE
ON ProductStorages
AFTER UPDATE
AS
if ((SELECT Amount FROM INSERTED) != (SELECT Amount FROM DELETED))
	INSERT INTO StorageHistory
	(Id_StorageHistory, ProductStorageId_Product_Storage, Summary, Date) VALUES
	(NEWID(), (SELECT Id_Product_Storage FROM INSERTED),
	CONCAT('Товар "', (SELECT Name FROM INSERTED), '", изменил своё количество на складе, теперь данного товара на складе - ',  (SELECT Amount FROM INSERTED)),
	GETDATE())
GO

CREATE OR ALTER TRIGGER OrderProduct_INSERTED
ON OrderProducts
AFTER INSERT
AS
UPDATE ProductStorages
SET Amount = (SELECT Amount FROM ProductStorages Where Id_Product_Storage = (SELECT ProductId_Product_Storage FROM INSERTED)) - (SELECT Amount FROM INSERTED)
WHERE Id_Product_Storage = (SELECT ProductId_Product_Storage FROM INSERTED)
GO

CREATE OR ALTER TRIGGER OrderProduct_UPDATE
ON OrderProducts
AFTER UPDATE
AS
UPDATE ProductStorages
SET Amount = (SELECT Amount FROM ProductStorages Where Id_Product_Storage = (SELECT ProductId_Product_Storage FROM INSERTED)) - ((SELECT Amount FROM INSERTED) - (SELECT Amount FROM DELETED))
WHERE Id_Product_Storage = (SELECT ProductId_Product_Storage FROM INSERTED)
GO

CREATE OR ALTER TRIGGER OrderProduct_DELETE
ON OrderProducts
AFTER DELETE
AS
UPDATE ProductStorages
SET Amount = (SELECT Amount FROM ProductStorages Where Id_Product_Storage = (SELECT ProductId_Product_Storage FROM DELETED)) + (SELECT Amount FROM DELETED)
WHERE Id_Product_Storage = (SELECT ProductId_Product_Storage FROM DELETED)
GO

CREATE OR ALTER TRIGGER Purchase_INSERT
ON Purchases
AFTER INSERT
AS
UPDATE ProductStorages
SET Amount = (SELECT Amount FROM ProductStorages Where Id_Product_Storage = (SELECT ProductStorageId_Product_Storage FROM INSERTED)) + (SELECT Amount FROM INSERTED)
WHERE Id_Product_Storage = (SELECT ProductStorageId_Product_Storage FROM INSERTED)
GO

CREATE OR ALTER TRIGGER Purchase_UPDATE
ON Purchases
AFTER UPDATE
AS
UPDATE ProductStorages
SET Amount = (SELECT Amount FROM ProductStorages Where Id_Product_Storage = (SELECT ProductStorageId_Product_Storage FROM INSERTED)) + ((SELECT Amount FROM INSERTED) - (SELECT Amount FROM DELETED))
WHERE Id_Product_Storage = (SELECT ProductStorageId_Product_Storage FROM INSERTED)
GO

CREATE OR ALTER TRIGGER Purchase_DELETE
ON Purchases
AFTER DELETE
AS
UPDATE ProductStorages
SET Amount = (SELECT Amount FROM ProductStorages Where Id_Product_Storage = (SELECT ProductStorageId_Product_Storage FROM DELETED)) - (SELECT Amount FROM DELETED)
WHERE Id_Product_Storage = (SELECT ProductStorageId_Product_Storage FROM DELETED)
GO

insert into users (Id_User, BirthDate, First_name, Last_name, Otch)
Values ('1d9b1fc1-6273-4102-822b-7d4b92541216', GETDATE(), 'admin', 'admin', 'admin')
insert into Accounts (UserId, Email, Login, Password)
values ('1d9b1fc1-6273-4102-822b-7d4b92541216', 'admin@admin.admin', 'admin', 'c7ad44cbad762a5da0a452f9e854fdc1e0e7a52a38015f23f3eab1d80b931dd472634dfac71cd34ebc35d16ab7fb8a90c81f975113d6c7538dc69dd8de9077ec')
insert into Roles (Id_Role, AccountUserUserId, Name)
Values (NEWID(), '1d9b1fc1-6273-4102-822b-7d4b92541216', 'Администратор')

SET ANSI_WARNINGS  OFF;

DBCC TRACEON(460, -1);

GO