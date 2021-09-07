CREATE TABLE [planning].[TeamChange](
	[Id] [uniqueidentifier] NOT NULL,
	[TeamId] [uniqueidentifier] NOT NULL,
	[TeamMemberId] [uniqueidentifier] NOT NULL,
	[FirstDay] [date] NOT NULL,
	[LastDay] [date] NULL,
 CONSTRAINT [PK_TeamChanges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [planning].[TeamChange]  WITH CHECK ADD  CONSTRAINT [FK_TeamChanges_Developer] FOREIGN KEY([TeamMemberId])
REFERENCES [company].[Employee] ([Id])
GO

ALTER TABLE [planning].[TeamChange] CHECK CONSTRAINT [FK_TeamChanges_Developer]
GO
ALTER TABLE [planning].[TeamChange]  WITH CHECK ADD  CONSTRAINT [FK_TeamChanges_Team] FOREIGN KEY([TeamId])
REFERENCES [company].[Team] ([Id])
GO

ALTER TABLE [planning].[TeamChange] CHECK CONSTRAINT [FK_TeamChanges_Team]
GO
ALTER TABLE [planning].[TeamChange] ADD  DEFAULT (newsequentialid()) FOR [Id]