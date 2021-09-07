CREATE TABLE [company].[Employee](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[SpecializationId] [tinyint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[LoginUser] [nvarchar](50) NOT NULL,
	[HoursPerDay] [tinyint] NOT NULL,
	[ReportsTo] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Developer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [company].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Developer_Specialization] FOREIGN KEY([SpecializationId])
REFERENCES [category].[Specialization] ([Id])
GO

ALTER TABLE [company].[Employee] CHECK CONSTRAINT [FK_Developer_Specialization]
GO

ALTER TABLE [company].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Employee] FOREIGN KEY([ReportsTo])
REFERENCES [company].[Employee] ([Id])
GO

ALTER TABLE [company].[Employee] CHECK CONSTRAINT [FK_Employee_Employee]
GO