CREATE TABLE [dbo].[ServerFiles] (
	[RemoteFilePath]      VARCHAR (255) NOT NULL,
	[RemoteDirectoryName] VARCHAR (255) NOT NULL,
	[LocalFileName]       VARCHAR (255) NOT NULL, 
	[ReadDTTM] DATETIME2(0)
		CONSTRAINT DF_ServerFiles_ReadDTTM
		DEFAULT SYSUTCDATETIME() NOT NULL
);

