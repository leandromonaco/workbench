CREATE TABLE [survey].[QuestionnaireSection](
	[Id] [uniqueidentifier] NOT NULL,
	[QuestionnaireId] [uniqueidentifier] NOT NULL,
	[Order] [smallint] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_QuestionnaireSection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [survey].[QuestionnaireSection]  WITH CHECK ADD  CONSTRAINT [FK_QuestionnaireSection_Questionnaire] FOREIGN KEY([QuestionnaireId])
REFERENCES [survey].[Questionnaire] ([Id])
GO

ALTER TABLE [survey].[QuestionnaireSection] CHECK CONSTRAINT [FK_QuestionnaireSection_Questionnaire]
GO
ALTER TABLE [survey].[QuestionnaireSection] ADD  CONSTRAINT [DF_QuestionnaireSection_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [survey].[QuestionnaireSection] ADD  CONSTRAINT [DF_QuestionnaireSection_QuestionnaireId]  DEFAULT (newsequentialid()) FOR [QuestionnaireId]