CREATE TABLE [planning].[Leave](
	[Id] [uniqueidentifier] NOT NULL,
	[CategoryId] [tinyint] NOT NULL,
	[TeamMemberId] [uniqueidentifier] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[IsPlanned] [bit] NOT NULL,
 CONSTRAINT [PK_Leave] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [planning].[Leave]  WITH CHECK ADD  CONSTRAINT [FK_AdminTime_AdminTimeCategory] FOREIGN KEY([CategoryId])
REFERENCES [category].[LeaveType] ([Id])
GO

ALTER TABLE [planning].[Leave] CHECK CONSTRAINT [FK_AdminTime_AdminTimeCategory]
GO
ALTER TABLE [planning].[Leave]  WITH CHECK ADD  CONSTRAINT [FK_Leave_Employee] FOREIGN KEY([TeamMemberId])
REFERENCES [company].[Employee] ([Id])
GO

ALTER TABLE [planning].[Leave] CHECK CONSTRAINT [FK_Leave_Employee]
GO
ALTER TABLE [planning].[Leave] ADD  DEFAULT (newsequentialid()) FOR [Id]