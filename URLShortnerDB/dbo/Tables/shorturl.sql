CREATE TABLE [dbo].[shorturl]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [originalUrl] VARCHAR(5000) NOT NULL, 
    [shortUrl] VARCHAR(50) NOT NULL, 
    [clicks] INT NOT NULL, 
    [created] DATETIME NOT NULL
)
