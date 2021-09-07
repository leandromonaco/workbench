CREATE TABLE [survey].[QuestionnairePeriod](
	[Id] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[From] [datetime] NOT NULL,
	[To] [datetime] NOT NULL,
 CONSTRAINT [PK_QuestionnairePeriod] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [survey].[QuestionnairePeriod] ADD  CONSTRAINT [DF_QuestionnairePeriod_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO