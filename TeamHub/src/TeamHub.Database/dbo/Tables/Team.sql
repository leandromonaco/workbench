CREATE TABLE [dbo].[Team] (
    [Id]   UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Name] NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED ([Id] ASC)
);

