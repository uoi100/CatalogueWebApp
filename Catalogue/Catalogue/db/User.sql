CREATE TABLE [dbo].[User]
(
	[UserID] INT NOT NULL PRIMARY KEY, 
    [UserName] NVARCHAR(50) NOT NULL, 
    [Password] VARCHAR(20) NOT NULL, 
    [Email] NVARCHAR(50) NOT NULL
)
