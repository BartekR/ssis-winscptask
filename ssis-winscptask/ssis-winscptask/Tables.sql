USE [master]
GO

IF DB_ID(N'ssistaskdb') IS NULL
	CREATE DATABASE [ssistaskdb]
	ON  PRIMARY (
		NAME = N'ssistaskdb',
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQL2016CTP3\MSSQL\DATA\ssistaskdb.mdf',
		SIZE = 51200KB,
		MAXSIZE = UNLIMITED,
		FILEGROWTH = 10240KB
	)
	LOG ON (
		NAME = N'ssistaskdb_log',
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQL2016CTP3\MSSQL\DATA\ssistaskdb_log.ldf',
		SIZE = 20480KB,
		MAXSIZE = 2048GB,
		FILEGROWTH = 10240KB
	)
GO

ALTER DATABASE [ssistaskdb] SET RECOVERY SIMPLE 
GO

USE ssistaskdb;
GO

IF OBJECT_ID(N'dbo.ServerFiles', N'U') IS NOT NULL
	DROP TABLE dbo.ServerFiles;
GO

IF OBJECT_ID(N'dbo.DownloadedFiles', N'U') IS NOT NULL
	DROP TABLE dbo.DownloadedFiles;
GO

IF OBJECT_ID(N'dbo.FileStatus', N'U') IS NOT NULL
	DROP TABLE dbo.FileStatus;
GO

CREATE TABLE dbo.FileStatus (
	Id			TINYINT				NOT NULL,
	StatusName	VARCHAR(10)			NOT NULL,

	CONSTRAINT PK_FileStatusId
		PRIMARY KEY CLUSTERED (Id)
);
GO

INSERT INTO dbo.FileStatus (Id, StatusName)
VALUES
	(0, 'Unknown'),
	(1, 'New'),
	(2, 'Processed'),
	(3, 'Error')
;
GO

CREATE TABLE dbo.DownloadedFiles (
	Id					INT				IDENTITY(1, 1)		NOT NULL,
	RemoteFilePath		VARCHAR(255)						NOT NULL,
	RemoteDirectoryName	VARCHAR(255)						NOT NULL,
	LocalFileName		VARCHAR(255)						NOT NULL,

	LoadDTTM			DATETIME2(0)
		CONSTRAINT DF_DownloadedFiles_LoadDTTM
			DEFAULT SYSUTCDATETIME()						NOT NULL,

	FileStatusId		TINYINT								NOT NULL
		CONSTRAINT FK_DownloadedFiles_FileStatus
			FOREIGN KEY
			REFERENCES dbo.FileStatus(Id),

	AuditKey			INT									NOT NULL
);
GO

CREATE TABLE dbo.ServerFiles (
	RemoteFilePath		VARCHAR(255)						NOT NULL,
	RemoteDirectoryName	VARCHAR(255)						NOT NULL,
	LocalFileName		VARCHAR(255)						NOT NULL
);
GO