CREATE TABLE [planning].[Timebox](
	[Id] [uniqueidentifier] NOT NULL,
	[TeamId] [uniqueidentifier] NOT NULL,
	[WorkItemId] NVARCHAR(50) NOT NULL,
	[TimeboxCategoryId] [tinyint] NOT NULL,
	[Hours] [int] NOT NULL,
 CONSTRAINT [PK_Timebox] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [planning].[Timebox]  WITH CHECK ADD  CONSTRAINT [FK_Timebox_DevelopmentTeam] FOREIGN KEY([TeamId])
REFERENCES [company].[Team] ([Id])
GO

ALTER TABLE [planning].[Timebox] CHECK CONSTRAINT [FK_Timebox_DevelopmentTeam]
GO

ALTER TABLE [planning].[Timebox]  WITH CHECK ADD  CONSTRAINT [FK_Timebox_TimeboxCategory] FOREIGN KEY([TimeboxCategoryId])
REFERENCES [category].[TimeboxType] ([Id])
GO

ALTER TABLE [planning].[Timebox] CHECK CONSTRAINT [FK_Timebox_TimeboxCategory]
GO
ALTER TABLE [planning].[Timebox] ADD  DEFAULT (newsequentialid()) FOR [Id]