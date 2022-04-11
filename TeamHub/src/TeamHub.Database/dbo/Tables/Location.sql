CREATE TABLE [dbo].[Location] (
    [Id]       UNIQUEIDENTIFIER CONSTRAINT [DF_Location_Id] DEFAULT (newsequentialid()) NOT NULL,
    [Name]     NVARCHAR (50)    NOT NULL,
    [TimeZone] NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([Id] ASC)
);

