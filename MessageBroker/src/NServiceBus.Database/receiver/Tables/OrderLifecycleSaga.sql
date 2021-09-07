CREATE TABLE [receiver].[OrderLifecycleSaga] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [Metadata]            NVARCHAR (MAX)   NOT NULL,
    [Data]                NVARCHAR (MAX)   NOT NULL,
    [PersistenceVersion]  VARCHAR (23)     NOT NULL,
    [SagaTypeVersion]     VARCHAR (23)     NOT NULL,
    [Concurrency]         INT              NOT NULL,
    [Correlation_OrderId] UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Index_Correlation_OrderId]
    ON [receiver].[OrderLifecycleSaga]([Correlation_OrderId] ASC) WHERE ([Correlation_OrderId] IS NOT NULL);

