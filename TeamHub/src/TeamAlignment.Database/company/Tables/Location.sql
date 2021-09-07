CREATE TABLE [company].[Location](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Location_Id]  DEFAULT (newsequentialid()),
	[Name] [nvarchar](50) NOT NULL,
	[TimeZoneId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [company].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Timezone] FOREIGN KEY([TimeZoneId])
REFERENCES [calendar].[Timezone] ([Id])
GO

ALTER TABLE [company].[Location] CHECK CONSTRAINT [FK_Location_Timezone]
GO