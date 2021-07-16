CREATE TABLE [dbo].[WebServiceRequests]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Host] NVARCHAR(50) NOT NULL, 
    [Type] NVARCHAR(150) NOT NULL, 
    [Message] VARCHAR(MAX) NULL, 
    [StatusCode] INT NULL, 
    [Time] DATETIME NOT NULL
)
