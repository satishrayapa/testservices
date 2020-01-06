DECLARE @jobId BINARY(16)

SELECT @jobId = CONVERT(uniqueidentifier, job_id) FROM msdb.dbo.sysjobs
WHERE name = 'ESS-UpdatePerfCounters';

if( @jobId is not null )
BEGIN
	EXEC msdb.dbo.sp_delete_job @job_id=@jobId, @delete_unused_schedule=1;
END

IF  EXISTS (SELECT object_id FROM sys.fulltext_indexes where object_id = object_id('search.LegalPartySearch'))
BEGIN
    DROP FULLTEXT INDEX ON search.LegalPartySearch
END
GO
IF EXISTS ( SELECT * FROM sysfulltextcatalogs ftc WHERE ftc.name = N'AumentumFt' )
BEGIN
   DROP FULLTEXT CATALOG [AumentumFt]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LegalPartySearch' AND TABLE_SCHEMA = 'search')
BEGIN
    DROP TABLE search.LegalPartySearch
END

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AumentumChangeTrackingVersion' AND TABLE_SCHEMA = 'search')
BEGIN
    DROP TABLE search.AumentumChangeTrackingVersion
END

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AumentumChangeTrackingVersionSaved' AND TABLE_SCHEMA = 'search')
BEGIN
    DROP TABLE search.AumentumChangeTrackingVersionSaved
END

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SearchTableUpdateRate' AND TABLE_SCHEMA = 'search')
BEGIN
    DROP TABLE search.SearchTableUpdateRate
END

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SimpleSearchSeed_Log' AND TABLE_SCHEMA = 'search')
BEGIN
    DROP TABLE search.SimpleSearchSeed_Log
END

IF EXISTS ( SELECT *  FROM sysobjects  WHERE  id = object_id(N'[search].[UpdatePerfCounters]')  and OBJECTPROPERTY(id, N'IsProcedure') = 1  )
BEGIN
	DROP PROCEDURE search.UpdatePerfCounters;
END

dbcc deleteinstance ('SQLServer:User Settable', 'ESS-LPST_NumberOfPendingChanges');
dbcc deleteinstance ('SQLServer:User Settable', 'ESS-LPST_UpdateRateOfChanges');

DELETE FROM __EFMigrationsHistory WHERE  MigrationId IN (
N'20170628153233_InitialCreate'
,N'20170628153331_AddFullTextSearchIndex'
,N'20170628153819_SeedLegalPartySearch'
,N'20170713131747_AddTagGeoLegalPartyTypeSubType'
,N'20170713134517_SeedLegalPartySearch2'
,N'20170714152322_updateFullTextSearchIndex'
,N'20170727130622_AddRealPropertyAppraisal'
,N'20170727130655_SeedLegalPartySearch3'
,N'20170808225230_AddMineralSearch'
,N'20170809135423_AddNServiceBusPersistence'
,N'20170809172345_AddNServiceBusLegalPartySync'
,N'20170809192638_AddNServiceBusEntityChangeMessageCollectionSaga'
,N'20170814202005_UpdateMineralField'
,N'20170814202431_SeedSearchWithMineralField'
,N'20170822125937_AddSourceInfo'
,N'20170822130031_SeedLegalPartySearch5'
,N'20170822165128_SeedLegalPartySearch6'
,N'20170824155939_SeedLegalPartySearch7'
,N'20170914155932_SeedLegalPartySearch8'
,N'20170927183439_AddStreetType'
,N'20170927183711_SeedLegalPartySearch9'
,N'20171003223045_AddSearchAllAndDropOtherSearchColumns'
,N'20171003223859_UpdateFullTextSearchIndex2'
,N'20171003224426_SeedLegalPartySearch10'
,N'20171004161134_SeedLegalPartySearch11'
,N'20171005185224_AddPrimeAddress'
,N'20171005185605_SeedLegalPartySearch12'
,N'20171006181602_AddPrimeOwner'
,N'20171006181659_SeedLegalPartySearch13'
,N'20171011143250_AddAddressRoleEffectiveStatus'
,N'20171011143337_SeedLegalPartySearch14'
,N'20171016192844_DisableStopwords'
,N'20171017143751_AddCityStatePostalCode'
,N'20171017143937_SeedLegalPartySearch15'
,N'20171023190732_AddRevenueObjectEffectiveStatus'
,N'20171023190829_SeedLegalPartySearch16'
,N'20171030163500_SitusAddressNotReq'
,N'20180227215331_UpdateFullTextSearchIndex3'
,N'20180301225752_UpdateFullTextSearchIndex4'
,N'20180506182855_AllowNullLegalPartyRole'
,N'20180506183011_SeedLegalPartyWithLegalPartyRoleNullable'
,N'20180509211431_SeedAliases'
,N'20180510160601_AddUnformattedPIN'
,N'20180510160704_UpdateFullTextSearchIndex5'
,N'20180511144234_AddLatestPin'
,N'20180511144621_SeedLatestPin'
,N'20180524204827_SeedMailingAddressCommType'
,N'20180718220009_AddAumentumChangeTrackingVersion'
,N'20180727160155_SeedWithCorrectAlias'
,N'20180904180925_AumentumChangeTrackingVersionSaved'
,N'20180904200327_AumentumChangeTrackingVersionSavedData'

,N'20181031145018_UpdateSearchAllSchema'
,N'20181102211652_SeedUpdateSchema'
,N'20181112220045_ChangeTrackingRetention'
,N'20181113202333_AddSearchTableUpdateCountTable'
,N'20181129173308_UpdateStateSchema'
,N'20181129175604_SeedLegalPartySearch17'
)

IF EXISTS (SELECT 1 FROM sys.change_tracking_tables
				               WHERE object_id = OBJECT_ID('dbo.LegalParty'))
BEGIN
		ALTER TABLE dbo.LegalParty DISABLE CHANGE_TRACKING
END
IF EXISTS (SELECT 1 FROM sys.change_tracking_tables
				WHERE object_id = OBJECT_ID('dbo.Comm'))
BEGIN
		ALTER TABLE dbo.Comm DISABLE CHANGE_TRACKING
END
IF EXISTS (SELECT 1 FROM sys.change_tracking_tables
				WHERE object_id = OBJECT_ID('dbo.CommRole'))
BEGIN
		ALTER TABLE dbo.CommRole DISABLE CHANGE_TRACKING
END
IF EXISTS (SELECT 1 FROM sys.change_tracking_tables
				WHERE object_id = OBJECT_ID('dbo.SitusAddr'))
BEGIN
		ALTER TABLE dbo.SitusAddr DISABLE CHANGE_TRACKING
END
IF EXISTS (SELECT 1 FROM sys.change_tracking_tables
				WHERE object_id = OBJECT_ID('dbo.LegalParty'))
BEGIN
		ALTER TABLE dbo.LegalParty DISABLE CHANGE_TRACKING
END
IF EXISTS (SELECT 1 FROM sys.change_tracking_tables
				WHERE object_id = OBJECT_ID('dbo.SitusAddrRole'))
BEGIN
		ALTER TABLE dbo.SitusAddrRole DISABLE CHANGE_TRACKING
END
IF EXISTS (SELECT 1 FROM sys.change_tracking_tables
				WHERE object_id = OBJECT_ID('dbo.ApplSite'))
BEGIN
		ALTER TABLE dbo.ApplSite DISABLE CHANGE_TRACKING
END
IF EXISTS (SELECT 1 FROM sys.change_tracking_tables
				WHERE object_id = OBJECT_ID('dbo.ApplSiteRole'))
BEGIN
		ALTER TABLE dbo.ApplSiteRole DISABLE CHANGE_TRACKING
END
IF EXISTS (SELECT 1 FROM sys.change_tracking_tables
				WHERE object_id = OBJECT_ID('dbo.LegalPartyRole'))
BEGIN
		ALTER TABLE dbo.LegalPartyRole DISABLE CHANGE_TRACKING
END
IF EXISTS (SELECT 1 FROM sys.change_tracking_tables
				WHERE object_id = OBJECT_ID('dbo.RevObj'))
BEGIN
		ALTER TABLE dbo.RevObj DISABLE CHANGE_TRACKING
END
IF EXISTS (SELECT 1 FROM sys.change_tracking_tables
				WHERE object_id = OBJECT_ID('dbo.TAG'))
BEGIN
		ALTER TABLE dbo.TAG DISABLE CHANGE_TRACKING
END
IF EXISTS (SELECT 1 FROM sys.change_tracking_tables
				WHERE object_id = OBJECT_ID('dbo.TAGRole'))
BEGIN
		ALTER TABLE dbo.TAGRole DISABLE CHANGE_TRACKING
END

IF EXISTS (SELECT 1 FROM sys.change_tracking_databases WHERE database_id=DB_ID())
BEGIN
	ALTER DATABASE CURRENT SET CHANGE_TRACKING = OFF
END

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AumentumChangeTrackingVersion' AND TABLE_SCHEMA = 'search')
BEGIN
	DROP TABLE search.AumentumChangeTrackingVersion
END

