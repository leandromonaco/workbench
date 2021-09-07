CREATE TABLE [sender].[Samples.Sql.Sender.Delayed] (
    [Headers]    NVARCHAR (MAX)  NOT NULL,
    [Body]       VARBINARY (MAX) NULL,
    [Due]        DATETIME        NOT NULL,
    [RowVersion] BIGINT          IDENTITY (1, 1) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [Index_Due]
    ON [sender].[Samples.Sql.Sender.Delayed]([Due] ASC);

