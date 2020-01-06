using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class updateFullTextSearchIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON [search].[LegalPartySearch];
			  CREATE FULLTEXT INDEX ON [search].[LegalPartySearch] (
					DisplayName, 
					Addr,
					PIN,
					AIN,
					SearchDoc,
					SearchPin,
					GeoCode,
					SearchGeoTag,
					Tag)
					KEY INDEX PK_LegalPartySearch
					WITH STOPLIST = SYSTEM;
			", true);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON [search].[LegalPartySearch];
			  CREATE FULLTEXT INDEX ON [search].[LegalPartySearch] (
					DisplayName, 
					Addr,
					PIN,
					AIN,
					SearchDoc,
					SearchPin
					)
					KEY INDEX PK_LegalPartySearch
					WITH STOPLIST = SYSTEM;
			", true);
		}
    }
}
