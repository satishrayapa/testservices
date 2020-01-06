using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class UpdateFullTextSearchIndex4 : Migration
    {
		protected override void Up(MigrationBuilder migrationBuilder)
	    {
		    migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON [search].[LegalPartySearch];
			CREATE FULLTEXT INDEX ON[search].[LegalPartySearch]
		    (
			    DisplayName LANGUAGE 0,
			    Addr LANGUAGE 0,
			    PIN LANGUAGE 0,
			    AIN LANGUAGE 0,
			    GeoCode LANGUAGE 0,
			    SearchAll LANGUAGE 0,   
			    Tag LANGUAGE 0)
		    KEY INDEX PK_LegalPartySearch
		    WITH STOPLIST = OFF;		
			", true);

	    }

	    protected override void Down(MigrationBuilder migrationBuilder)
	    {
		    migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON [search].[LegalPartySearch];
			CREATE FULLTEXT INDEX ON[search].[LegalPartySearch]
		    (
			    DisplayName,
			    Addr,
			    PIN,
			    AIN,
			    GeoCode,
			    SearchAll LANGUAGE 0,
			    Tag)
		    KEY INDEX PK_LegalPartySearch
		    WITH STOPLIST = OFF;		
			", true);
	    }
	}
}
