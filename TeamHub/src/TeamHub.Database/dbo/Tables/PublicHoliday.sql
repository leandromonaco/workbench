CREATE TABLE [dbo].[PublicHoliday] (
    [Id]          UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Date]        DATE             NOT NULL,
    [Description] NTEXT            NOT NULL,
    [LocationId]  UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_PublicHoliday] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PublicHoliday_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id])
);

