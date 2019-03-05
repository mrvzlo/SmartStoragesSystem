CREATE TABLE [dbo].[StorageTypes] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (128) NOT NULL,
    [Background] VARCHAR (6)    NOT NULL,
    CONSTRAINT [PK_StorageType] PRIMARY KEY CLUSTERED ([Id] ASC)
);







