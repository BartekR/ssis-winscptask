CREATE TABLE [dbo].[FileStatus] (
    [Id]         TINYINT      NOT NULL,
    [StatusName] VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_FileStatusId] PRIMARY KEY CLUSTERED ([Id] ASC)
);

