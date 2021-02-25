CREATE TABLE [dbo].[Vehicle]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [Name] NVARCHAR(50) NOT NULL, 
    [License ] NVARCHAR(10) NOT NULL, 
    [Make] NVARCHAR(20) NOT NULL, 
    [Model] NVARCHAR(20) NOT NULL, 
    [Year] SMALLINT NOT NULL,
    CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )
)

GO

CREATE UNIQUE NONCLUSTERED INDEX [vName_IDX] ON [dbo].[Vehicle] ([Name])