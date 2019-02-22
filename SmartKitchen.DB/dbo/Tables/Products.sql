CREATE TABLE [dbo].[Products] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (MAX) NOT NULL,
    [Category] INT            NOT NULL,
    CONSTRAINT [PK__Products__3214EC072DC8B59A] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Products_Categories] FOREIGN KEY ([Category]) REFERENCES [dbo].[Categories] ([Id])
);





