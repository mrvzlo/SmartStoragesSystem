CREATE TABLE [dbo].[Cells] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [ProductId]  INT      NOT NULL,
    [StorageId]  INT      NOT NULL,
    [BestBefore] DATETIME NULL,
    CONSTRAINT [PK_ProductStatus] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductStatus_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_ProductStatus_Storages] FOREIGN KEY ([StorageId]) REFERENCES [dbo].[Storages] ([Id])
);







