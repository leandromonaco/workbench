CREATE TABLE [survey].[QuestionnaireQuestion](
	[Id] [uniqueidentifier] NOT NULL,
	[SectionId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_QuestionnaireQuestion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [survey].[QuestionnaireQuestion]  WITH CHECK ADD  CONSTRAINT [FK_QuestionnaireQuestion_QuestionnaireSection] FOREIGN KEY([SectionId])
REFERENCES [survey].[QuestionnaireSection] ([Id])
GO

ALTER TABLE [survey].[QuestionnaireQuestion] CHECK CONSTRAINT [FK_QuestionnaireQuestion_QuestionnaireSection]
GO
ALTER TABLE [survey].[QuestionnaireQuestion] ADD  CONSTRAINT [DF_QuestionnaireQuestion_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [survey].[QuestionnaireQuestion] ADD  CONSTRAINT [DF_QuestionnaireQuestion_SectionId]  DEFAULT (newsequentialid()) FOR [SectionId]