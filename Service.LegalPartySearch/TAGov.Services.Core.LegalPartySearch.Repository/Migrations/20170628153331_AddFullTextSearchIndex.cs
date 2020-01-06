using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AddFullTextSearchIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
				CREATE FULLTEXT CATALOG AumentumFt AS DEFAULT;

				CREATE FULLTEXT INDEX ON [search].[LegalPartySearch] (
					DisplayName, 
					Addr,
					PIN,
					AIN,
					SearchDoc,
					SearchPin
				) KEY INDEX PK_LegalPartySearch WITH STOPLIST = SYSTEM;

				ALTER DATABASE CURRENT SET NEW_BROKER WITH rollback immediate; 
			", true);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON [search].[LegalPartySearch];
			DROP FULLTEXT CATALOG AumentumFt;		

			if( (SELECT is_broker_enabled FROM sys.databases where [name] = DB_NAME()) = 1)
			BEGIN
					ALTER DATABASE CURRENT SET DISABLE_BROKER WITH rollback immediate;
			END 

			", true);
		}
    }
}
