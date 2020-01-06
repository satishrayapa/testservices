using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AumentumChangeTrackingVersionSavedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
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

				IF NOT EXISTS (SELECT * FROM search.AumentumChangeTrackingVersionSaved)
				BEGIN
					INSERT INTO search.AumentumChangeTrackingVersionSaved
					VALUES ('LegalParty', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersionSaved
					VALUES ('LegalPartyRole', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersionSaved
					VALUES ('Comm', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersionSaved
					VALUES ('CommRole', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersionSaved
					VALUES ('SitusAddr', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersionSaved
					VALUES ('SitusAddrRole', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersionSaved
					VALUES ('ApplSite', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersionSaved
					VALUES ('ApplSiteRole', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersionSaved
					VALUES ('RevObj', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersionSaved
					VALUES ('TAG', @ChangeTracking_version)

					INSERT INTO search.AumentumChangeTrackingVersionSaved
					VALUES ('TAGRole', @ChangeTracking_version)
				END
			");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
				TRUNCATE TABLE search.AumentumChangeTrackingVersion;

				TRUNCATE TABLE search.AumentumChangeTrackingVersionSaved;
			");
        }
    }
}
