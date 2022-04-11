CREATE TABLE [dbo].[Leave] (
    [Id]          UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [LeaveTypeId] TINYINT          NOT NULL,
    [EmployeeId]  UNIQUEIDENTIFIER NOT NULL,
    [StartDate]   DATE             NOT NULL,
    [EndDate]     DATE             NOT NULL,
    [IsPlanned]   BIT              NOT NULL,
    CONSTRAINT [PK_Leave] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AdminTime_AdminTimeCategory] FOREIGN KEY ([LeaveTypeId]) REFERENCES [dbo].[LeaveType] ([Id]),
    CONSTRAINT [FK_Leave_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([Id])
);

