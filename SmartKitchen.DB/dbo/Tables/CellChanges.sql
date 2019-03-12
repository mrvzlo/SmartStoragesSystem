CREATE TABLE [dbo].[CellChanges] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [CellId]     INT      NOT NULL,
    [Amount]     INT      NOT NULL,
    [UpdateDate] DATETIME NOT NULL,
    CONSTRAINT [PK_CellAmountChanges] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CellChanges_Cells] FOREIGN KEY ([CellId]) REFERENCES [dbo].[Cells] ([Id])
);

