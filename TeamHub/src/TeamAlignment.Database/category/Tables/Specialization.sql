CREATE TABLE [category].[Specialization](
	[Id] [tinyint] NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[ShortDescription] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Specialization] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]