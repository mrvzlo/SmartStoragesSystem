CREATE TABLE [dbo].[BasketProducts] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [BasketId]   INT             NOT NULL,
    [CellId]     INT             NOT NULL,
    [Bought]     BIT             CONSTRAINT [DF_BasketProducts_Status] DEFAULT ((0)) NOT NULL,
    [BestBefore] DATETIME        NULL,
    [Price]      DECIMAL (18, 2) CONSTRAINT [DF_BasketProducts_Price] DEFAULT ((0)) NOT NULL,
    [Amount]     DECIMAL (18, 2) CONSTRAINT [DF_BasketProducts_Amount] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BasketProducts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BasketProducts_Baskets] FOREIGN KEY ([BasketId]) REFERENCES [dbo].[Baskets] ([Id]),
    CONSTRAINT [FK_BasketProducts_Cells] FOREIGN KEY ([CellId]) REFERENCES [dbo].[Cells] ([Id])
);



