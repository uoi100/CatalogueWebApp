CREATE TABLE [dbo].[Catalogue]
(
	[CataID] INT NOT NULL PRIMARY KEY,  
    [UserID] INT NOT NULL,
    [Title] NVARCHAR(50) NOT NULL, 
    [Priority] INT NOT NULL, 
    [Description] NVARCHAR(4000) NULL, 
    [DateCreated] DATETIME NOT NULL, 
    [DateModified] DATETIME NULL,
	CONSTRAINT [FK_dbo.Catalogue_dbo.User_UserID] FOREIGN KEY ([UserID]) 
        REFERENCES [dbo].[User] ([UserID]) ON DELETE CASCADE
)
