CREATE TABLE [dbo].[Stats]
(
	[UserID] INT NOT NULL PRIMARY KEY, 
    [SundayHrs] INT NULL, 
    [MondayHrs] INT NULL, 
    [TuesdayHrs] INT NULL, 
    [WednesdayHrs] INT NULL, 
    [ThursdayHrs] INT NULL, 
    [FridayHrs] INT NULL, 
    [SaturdayHrs] INT NULL,
	CONSTRAINT [FK_dbo.Stats_dbo.User_UserID] FOREIGN KEY ([UserID]) 
        REFERENCES [dbo].[User] ([UserID]) ON DELETE CASCADE
)
