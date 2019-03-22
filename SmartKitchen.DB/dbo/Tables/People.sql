CREATE TABLE [dbo].[People] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (MAX) NOT NULL,
    [Email]      NVARCHAR (MAX) NOT NULL,
    [Password]   NVARCHAR (MAX) NOT NULL,
    [Role]       INT            CONSTRAINT [DF_Users_Role] DEFAULT ((0)) NOT NULL,
    [PublicKey]  NVARCHAR (MAX) CONSTRAINT [DF_People_Token] DEFAULT (newid()) NOT NULL,
    [PrivateKey] NVARCHAR (MAX) CONSTRAINT [DF_People_PrivateToken] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);







