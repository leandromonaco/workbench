CREATE TABLE [dbo].[Sprint] (
    [Id]   UNIQUEIDENTIFIER NOT NULL,
    [From] DATE             NOT NULL,
    [To]   DATE             NOT NULL,
    [Name] NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_Sprint] PRIMARY KEY CLUSTERED ([Id] ASC)
);

