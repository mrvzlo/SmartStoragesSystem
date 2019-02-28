CREATE TABLE [dbo].[Storages] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (MAX) NOT NULL,
    [TypeId]   INT            CONSTRAINT [DF_Storages_Background] DEFAULT ((0)) NOT NULL,
    [PersonId] INT            NOT NULL,
    CONSTRAINT [PK_Storages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Storages_StorageTypes] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[StorageTypes] ([Id]),
    CONSTRAINT [FK_Storages_Users] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([Id])
);





