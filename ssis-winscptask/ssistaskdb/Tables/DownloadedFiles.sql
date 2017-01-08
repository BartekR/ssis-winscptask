CREATE TABLE [dbo].[DownloadedFiles] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [RemoteFilePath]      VARCHAR (255) NOT NULL,
    [RemoteDirectoryName] VARCHAR (255) NOT NULL,
    [LocalFileName]       VARCHAR (255) NOT NULL,
    [LoadDTTM]            DATETIME2 (0) CONSTRAINT [DF_DownloadedFiles_LoadDTTM] DEFAULT (sysutcdatetime()) NOT NULL,
    [FileStatusId]        TINYINT       NOT NULL,
    [AuditKey]            INT           NOT NULL,
    CONSTRAINT [FK_DownloadedFiles_FileStatus] FOREIGN KEY ([FileStatusId]) REFERENCES [dbo].[FileStatus] ([Id])
);

