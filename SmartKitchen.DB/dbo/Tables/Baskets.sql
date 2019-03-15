CREATE TABLE [dbo].[Baskets] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (MAX) NOT NULL,
    [PersonId]     INT            NOT NULL,
    [CreationDate] DATETIME       NOT NULL,
    [Closed]       BIT            CONSTRAINT [DF_Baskets_Status] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Baskets] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Baskets_People] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([Id])
);



