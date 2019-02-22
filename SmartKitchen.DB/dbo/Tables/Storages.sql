CREATE TABLE [dbo].[Storages] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (MAX) NOT NULL,
    [Type]  INT            CONSTRAINT [DF_Storages_Background] DEFAULT ((0)) NOT NULL,
    [Owner] INT            NOT NULL,
    CONSTRAINT [PK_Storages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Storages_StorageTypes] FOREIGN KEY ([Type]) REFERENCES [dbo].[StorageTypes] ([Id]),
    CONSTRAINT [FK_Storages_Users] FOREIGN KEY ([Owner]) REFERENCES [dbo].[People] ([Id])
);

