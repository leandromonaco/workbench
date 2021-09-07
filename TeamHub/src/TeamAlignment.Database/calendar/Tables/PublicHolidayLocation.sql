CREATE TABLE [calendar].[PublicHolidayLocation](
	[LocationId] [uniqueidentifier] NOT NULL,
	[PublicHolidayId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PublicHolidayLocationCity] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC,
	[PublicHolidayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [calendar].[PublicHolidayLocation]  WITH CHECK ADD  CONSTRAINT [FK_PublicHolidayLocationCity_PublicHolidayLocation] FOREIGN KEY([PublicHolidayId])
REFERENCES [calendar].[PublicHoliday] ([Id])
GO

ALTER TABLE [calendar].[PublicHolidayLocation] CHECK CONSTRAINT [FK_PublicHolidayLocationCity_PublicHolidayLocation]
GO
ALTER TABLE [calendar].[PublicHolidayLocation]  WITH CHECK ADD  CONSTRAINT [FK_PublicHolidayLocationLocation_Locations] FOREIGN KEY([LocationId])
REFERENCES [company].[Location] ([Id])
GO

ALTER TABLE [calendar].[PublicHolidayLocation] CHECK CONSTRAINT [FK_PublicHolidayLocationLocation_Locations]