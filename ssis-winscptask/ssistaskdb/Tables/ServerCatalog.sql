CREATE TABLE [config].[ServerCatalog] (
    [Name]     VARCHAR (255) NOT NULL,
    [IsActive] BIT           CONSTRAINT [DF_ServerCatalog_IsActive] DEFAULT ((1)) NOT NULL
);

