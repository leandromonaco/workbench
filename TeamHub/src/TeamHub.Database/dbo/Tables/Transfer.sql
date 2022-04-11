CREATE TABLE [dbo].[Transfer] (
    [Id]         UNIQUEIDENTIFIER CONSTRAINT [DF__TeamChange__Id__59FA5E80] DEFAULT (newsequentialid()) NOT NULL,
    [FromTeamId] UNIQUEIDENTIFIER NOT NULL,
    [ToTeamId]   UNIQUEIDENTIFIER NULL,
    [EmployeeId] UNIQUEIDENTIFIER NOT NULL,
    [Date]       DATE             NOT NULL,
    CONSTRAINT [PK_TeamChanges] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TeamChanges_Developer] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([Id]),
    CONSTRAINT [FK_TeamChanges_Team] FOREIGN KEY ([FromTeamId]) REFERENCES [dbo].[Team] ([Id]),
    CONSTRAINT [FK_Transfer_Team] FOREIGN KEY ([ToTeamId]) REFERENCES [dbo].[Team] ([Id])
);

