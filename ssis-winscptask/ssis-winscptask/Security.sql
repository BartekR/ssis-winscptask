USE [master]
GO

CREATE LOGIN [ssis-winscptask] 
WITH PASSWORD=N'123qwe!@#', 
DEFAULT_DATABASE=[ssistaskdb], 
CHECK_EXPIRATION=OFF, 
CHECK_POLICY=OFF
GO

USE [ssistaskdb]
GO
CREATE USER [ssis-winscptask] FOR LOGIN [ssis-winscptask]
GO

USE [ssistaskdb]
GO
ALTER ROLE [db_owner] ADD MEMBER [ssis-winscptask]
GO
