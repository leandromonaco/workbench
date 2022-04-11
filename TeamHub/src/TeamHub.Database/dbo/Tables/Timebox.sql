CREATE TABLE [dbo].[Timebox] (
    [Id]            UNIQUEIDENTIFIER CONSTRAINT [DF__Timebox__Id__5AEE82B9] DEFAULT (newsequentialid()) NOT NULL,
    [SprintId]      UNIQUEIDENTIFIER NULL,
    [TeamId]        UNIQUEIDENTIFIER NOT NULL,
    [TimeboxTypeId] TINYINT          NOT NULL,
    [Hours]         INT              NOT NULL,
    CONSTRAINT [PK_Timebox] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Timebox_DevelopmentTeam] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Team] ([Id]),
    CONSTRAINT [FK_Timebox_Sprint] FOREIGN KEY ([SprintId]) REFERENCES [dbo].[Sprint] ([Id]),
    CONSTRAINT [FK_Timebox_TimeboxCategory] FOREIGN KEY ([TimeboxTypeId]) REFERENCES [dbo].[TimeboxType] ([Id])
);

