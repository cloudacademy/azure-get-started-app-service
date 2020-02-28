CREATE TABLE [dbo].[Pickup]
(
	[Id] INT NOT NULL IDENTITY(1,1),
    [FromAddress] NVARCHAR(100) NOT NULL, 
    [ToAddress] NVARCHAR(100) NOT NULL, 
    [SignatureRequired] BIT NOT NULL, 
    [PickedUp] DATETIME, 
    [Delivered] DATETIME,
    [Barcode] NVARCHAR(30)
    CONSTRAINT [PK_Pickup] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )
)
