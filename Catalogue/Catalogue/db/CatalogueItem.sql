CREATE TABLE [dbo].[CatalogueItem]
(
	[ItemID] INT NOT NULL PRIMARY KEY, 
    [CataID] INT NOT NULL, 
    [Title] NVARCHAR(50) NOT NULL, 
    [Deadline] DATETIME NOT NULL, 
    [Description] NVARCHAR(4000) NULL, 
    [DateCreated] DATETIME NOT NULL, 
    [DateModified] DATETIME NULL, 
    [Complete] BIT NOT NULL,
	CONSTRAINT [FK_dbo.CatalogueItem_dbo.Catalogue_CataID] FOREIGN KEY ([CataID]) 
        REFERENCES [dbo].[Catalogue] ([CataID]) ON DELETE CASCADE
)
