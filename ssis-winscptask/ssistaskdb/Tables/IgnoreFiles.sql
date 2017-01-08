CREATE TABLE [config].[IgnoreFiles] (
    [RemoteFilePath]      VARCHAR (255) NOT NULL,
    [RemoteDirectoryName] VARCHAR (255) NOT NULL,
    [Reason]              VARCHAR (150) NOT NULL,
    [InsertedByLogin]     VARCHAR (128) NOT NULL,
    [AuditKey]            INT           NULL
);

