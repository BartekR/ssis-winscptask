CREATE TABLE [config].[ReimportFiles] (
    [RemoteFilePath]      VARCHAR (255) NOT NULL,
    [RemoteDirectoryName] VARCHAR (255) NOT NULL,
    [IsActive]            BIT           CONSTRAINT [DF_ReimportFiles_IsActive] DEFAULT ((1)) NOT NULL
);

