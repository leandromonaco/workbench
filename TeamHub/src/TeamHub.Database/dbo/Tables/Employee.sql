CREATE TABLE [dbo].[Employee] (
    [Id]               UNIQUEIDENTIFIER CONSTRAINT [DF__Employee__Id__4AB81AF0] DEFAULT (newsequentialid()) NOT NULL,
    [SpecializationId] TINYINT          NOT NULL,
    [Name]             NVARCHAR (50)    NOT NULL,
    [Email]            NVARCHAR (50)    NOT NULL,
    [HoursPerDay]      TINYINT          NOT NULL,
    [LocationId]       UNIQUEIDENTIFIER NULL,
    [ReportsTo]        UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Developer] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Developer_Specialization] FOREIGN KEY ([SpecializationId]) REFERENCES [dbo].[Specialization] ([Id]),
    CONSTRAINT [FK_Employee_Employee] FOREIGN KEY ([ReportsTo]) REFERENCES [dbo].[Employee] ([Id]),
    CONSTRAINT [FK_Employee_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id])
);

