CREATE TABLE [company].[Team](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[LocationId] [uniqueidentifier] NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SprintPlanningCutOff] DATETIME NOT NULL, 
    [ReleasePlanningCutOff] DATETIME NOT NULL, 
	[IsDisplayed] [bit] NOT NULL CONSTRAINT [DF_Team_IsDisplayed]  DEFAULT ((0))
    CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [company].[Team]  WITH CHECK ADD  CONSTRAINT [FK_DevelopmentTeam_Locations] FOREIGN KEY([LocationId])
REFERENCES [company].[Location] ([Id])
GO

ALTER TABLE [company].[Team] CHECK CONSTRAINT [FK_DevelopmentTeam_Locations]
GO
ALTER TABLE [company].[Team]  WITH CHECK ADD  CONSTRAINT [FK_DevelopmentTeam_Product] FOREIGN KEY([ProductId])
REFERENCES [company].[Product] ([Id])
GO

ALTER TABLE [company].[Team] CHECK CONSTRAINT [FK_DevelopmentTeam_Product]