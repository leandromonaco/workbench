CREATE TABLE [okrs].[KeyResult](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[ObjectiveId] [uniqueidentifier] NOT NULL,
	[EmployeeId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_KeyResult] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [okrs].[KeyResult]  WITH CHECK ADD  CONSTRAINT [FK_KeyResult_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [company].[Employee] ([Id])
GO

ALTER TABLE [okrs].[KeyResult] CHECK CONSTRAINT [FK_KeyResult_Employee]
GO

