
IISRESET /STOP

echo *********** getting restore database from master...... **********

FOR /F "delims=" %%I IN ('DIR \\C906ZZWLUMDB2.ecomqc.tlrg.com\AumentumDbBackup\. /B /O:-D') DO (ECHO F|XCOPY \\C906ZZWLUMDB2.ecomqc.tlrg.com\AumentumDbBackup\%%I C:\local_aa\db\db.bak /R /Y /Z /F && goto break)

:break

echo *********** delete local snapshot if exists...... **********

SQLCmd -E -S localhost -Q "IF EXISTS (SELECT database_id FROM sys.databases WHERE NAME='LumenGoldSnapshot')DROP DATABASE LumenGoldSnapshot"

echo *********** restore database from master...... **********

SQLCmd -E -S localhost -Q "USE [master] RESTORE DATABASE [LumenGold] FROM  DISK = N'C:\local_aa\db\db.bak' WITH  FILE = 1, MOVE N'DATA' TO N'C:\local_aa\db\LumenGold_DATA.mdf', MOVE N'INDEX' TO N'C:\local_aa\db\LumenGold_INDEX.mdf', MOVE N'New_Common_Data' TO N'C:\local_aa\db\LumenGold_New_Common_Data.mdf', MOVE N'Common_Data' TO N'C:\local_aa\db\LumenGold_Common_Data.mdf', MOVE N'Common_Log' TO N'C:\local_aa\db\LumenGold_Common_Log.ldf', NOUNLOAD, STATS = 5"

echo update resource locator table...

SQLCmd -E -S localhost -Q "USE [LumenGold] DELETE FROM [Common.Resource].[Resource] INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'BaseValueSegmentFeature', N'LOCALTEST', N'true') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'facade.assessmentheader', N'urlservices:LOCALTEST', N'http://localhost/facade.assessmentheader') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'facade.basevaluesegment', N'urlservices:LOCALTEST', N'http://localhost/facade.basevaluesegment') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'service.assessmentevent', N'urlservices:LOCALTEST', N'http://localhost/service.assessmentevent') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'service.basevaluesegment', N'urlservices:LOCALTEST', N'http://localhost/service.basevaluesegment') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'service.grmevent', N'urlservices:LOCALTEST', N'http://localhost/service.grmevent') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'service.legalparty', N'urlservices:LOCALTEST', N'http://localhost/service.legalparty') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'service.revenueobject', N'urlservices:LOCALTEST', N'http://localhost/service.revenueobject')"

echo *********** creating local snapshot...... **********

SQLCmd -E -S localhost -Q "USE LumenGold CREATE DATABASE LumenGoldSnapshot ON 	( NAME = Common_Data, FILENAME = 'C:\local_aa\db\LumenGold_Common_Data.ss'),	( NAME = New_Common_Data, FILENAME = 'C:\local_aa\db\LumenGold_New_Common_Data.ss'),	( NAME = DATA, FILENAME = 'C:\local_aa\db\LumenGold_DATA.ss'),	( NAME = [INDEX], FILENAME = 'C:\local_aa\db\LumenGold_INDEX.ss')	 AS SNAPSHOT OF LumenGold"

echo *********** recreating user to database from login...... **********

SQLCmd -E -S localhost -Q "CREATE USER [IIS APPPOOL\DefaultAppPool] FROM LOGIN [IIS APPPOOL\DefaultAppPool] exec sp_addrolemember 'db_owner', 'IIS APPPOOL\DefaultAppPool' CREATE USER [IIS APPPOOL\LumenPool] FROM LOGIN [IIS APPPOOL\LumenPool] exec sp_addrolemember 'db_owner', 'IIS APPPOOL\LumenPool';"

IISRESET /START

echo ALL DONE !!!! press any key to close window...

pause