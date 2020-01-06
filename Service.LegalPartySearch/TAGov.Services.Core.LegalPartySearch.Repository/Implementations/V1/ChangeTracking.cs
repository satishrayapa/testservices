using System;
using System.Collections.Generic;
using System.Text;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1
{
public static class ChangeTracking
	{

		public const string EnableChangeTracking = @"
				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_databases WHERE database_id=DB_ID())
				BEGIN
					ALTER DATABASE CURRENT
					SET CHANGE_TRACKING = ON  
					(CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON)
				END

				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables 
				               WHERE object_id = OBJECT_ID('dbo.LegalParty'))
				BEGIN
				     ALTER TABLE dbo.LegalParty
				     ENABLE CHANGE_TRACKING
				     WITH (TRACK_COLUMNS_UPDATED = OFF)
				END
				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables 
				               WHERE object_id = OBJECT_ID('dbo.Comm'))
				BEGIN
				     ALTER TABLE dbo.Comm
				     ENABLE CHANGE_TRACKING
				     WITH (TRACK_COLUMNS_UPDATED = OFF)
				END
				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables 
				               WHERE object_id = OBJECT_ID('dbo.CommRole'))
				BEGIN
				     ALTER TABLE dbo.CommRole
				     ENABLE CHANGE_TRACKING
				     WITH (TRACK_COLUMNS_UPDATED = OFF)
				END
				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables 
				               WHERE object_id = OBJECT_ID('dbo.SitusAddr'))
				BEGIN
				     ALTER TABLE dbo.SitusAddr
				     ENABLE CHANGE_TRACKING
				     WITH (TRACK_COLUMNS_UPDATED = OFF)
				END
				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables 
				               WHERE object_id = OBJECT_ID('dbo.LegalParty'))
				BEGIN
				     ALTER TABLE dbo.LegalParty
				     ENABLE CHANGE_TRACKING
				     WITH (TRACK_COLUMNS_UPDATED = OFF)
				END
				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables 
				               WHERE object_id = OBJECT_ID('dbo.SitusAddrRole'))
				BEGIN
				     ALTER TABLE dbo.SitusAddrRole
				     ENABLE CHANGE_TRACKING
				     WITH (TRACK_COLUMNS_UPDATED = OFF)
				END
				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables 
				               WHERE object_id = OBJECT_ID('dbo.ApplSite'))
				BEGIN
				     ALTER TABLE dbo.ApplSite
				     ENABLE CHANGE_TRACKING
				     WITH (TRACK_COLUMNS_UPDATED = OFF)
				END
				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables 
				               WHERE object_id = OBJECT_ID('dbo.ApplSiteRole'))
				BEGIN
				     ALTER TABLE dbo.ApplSiteRole
				     ENABLE CHANGE_TRACKING
				     WITH (TRACK_COLUMNS_UPDATED = OFF)
				END
				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables 
				               WHERE object_id = OBJECT_ID('dbo.LegalPartyRole'))
				BEGIN
				     ALTER TABLE dbo.LegalPartyRole
				     ENABLE CHANGE_TRACKING
				     WITH (TRACK_COLUMNS_UPDATED = OFF)
				END
				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables 
				               WHERE object_id = OBJECT_ID('dbo.RevObj'))
				BEGIN
				     ALTER TABLE dbo.RevObj
				     ENABLE CHANGE_TRACKING
				     WITH (TRACK_COLUMNS_UPDATED = OFF)
				END
				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables 
				               WHERE object_id = OBJECT_ID('dbo.TAG'))
				BEGIN
				     ALTER TABLE dbo.TAG
				     ENABLE CHANGE_TRACKING
				     WITH (TRACK_COLUMNS_UPDATED = OFF)
				END
				IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables 
				               WHERE object_id = OBJECT_ID('dbo.TAGRole'))
				BEGIN
				     ALTER TABLE dbo.TAGRole
				     ENABLE CHANGE_TRACKING
				     WITH (TRACK_COLUMNS_UPDATED = OFF)
				END

				IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AumentumChangeTrackingVersion' AND TABLE_SCHEMA = 'search')
				BEGIN
					CREATE TABLE  search.AumentumChangeTrackingVersion
					(
					    TableName varchar(255),
					    ChangeVersion BIGINT, 
					);
				END

				DECLARE @ChangeTracking_version BIGINT
				SET @ChangeTracking_version = CHANGE_TRACKING_CURRENT_VERSION();

				IF NOT EXISTS (SELECT * FROM search.AumentumChangeTrackingVersion)
				BEGIN
					INSERT INTO search.AumentumChangeTrackingVersion
					VALUES ('LegalParty', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersion
					VALUES ('LegalPartyRole', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersion
					VALUES ('Comm', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersion
					VALUES ('CommRole', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersion
					VALUES ('SitusAddr', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersion
					VALUES ('SitusAddrRole', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersion
					VALUES ('ApplSite', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersion
					VALUES ('ApplSiteRole', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersion
					VALUES ('RevObj', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersion
					VALUES ('TAG', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersion
					VALUES ('TAGRole', @ChangeTracking_version)
				END
			";

		public const string DisableChangeTracking = @"


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
			";




		public const string SqlErrorNumberForLocked = "51000";

		private const string GetLockSqlBeforeSqlErrorCode = @"
DECLARE @getAppLockResult int;
EXEC @getAppLockResult = sp_getapplock @Resource = 'ChangeTracking',
	@LockMode = 'Exclusive',
	@LockOwner = 'Session',
	@LockTimeout = 0;
IF @getAppLockResult = -1	-- the SQL is already running
	THROW ";

		private const string GetSqlSqlAfterSqlErrorCode = @", 'ResetChageTrackingQuery is already running', 1;";

		private const string GetLockSql =
			GetLockSqlBeforeSqlErrorCode + SqlErrorNumberForLocked + GetSqlSqlAfterSqlErrorCode;

		private const string ReleaseLockSql = @"EXEC sp_releaseapplock @Resource = 'ChangeTracking',
@LockOwner = 'Session';";


		public static string ChangeTrackingTablesEnable()
		{
			string rebuildAllSql = string.Format(EnableChangeTracking);
			return string.Concat(GetLockSql, rebuildAllSql, ReleaseLockSql);
		}

		public static string ChangeTrackingTablesDisable()
		{
			string rebuildAllSql = string.Format(DisableChangeTracking);
			return string.Concat(GetLockSql, rebuildAllSql, ReleaseLockSql);
		}
	}
}
