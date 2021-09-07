CREATE TABLE [calendar].[Milestone](
	[Id] [uniqueidentifier] NOT NULL,
	[TeamId] [uniqueidentifier] NOT NULL,
	[Date] [date] NOT NULL,
	[Description] [ntext] NOT NULL,
 CONSTRAINT [PK_Milestone] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [calendar].[Milestone]  WITH CHECK ADD  CONSTRAINT [FK_Milestone_Teams] FOREIGN KEY([TeamId])
REFERENCES [company].[Team] ([Id])
GO

ALTER TABLE [calendar].[Milestone] CHECK CONSTRAINT [FK_Milestone_Teams]
GO
ALTER TABLE [calendar].[Milestone] ADD  DEFAULT (newsequentialid()) FOR [Id]