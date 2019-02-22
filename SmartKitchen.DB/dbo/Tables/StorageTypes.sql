CREATE TABLE [dbo].[StorageTypes] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [Icon]       NVARCHAR (50) NOT NULL,
    [Background] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_StorageType] PRIMARY KEY CLUSTERED ([Id] ASC)
);



