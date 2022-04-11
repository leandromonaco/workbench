CREATE TABLE [dbo].[Specialization] (
    [Id]               TINYINT       NOT NULL,
    [Description]      NVARCHAR (50) NOT NULL,
    [ShortDescription] NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK_Specialization] PRIMARY KEY CLUSTERED ([Id] ASC)
);

