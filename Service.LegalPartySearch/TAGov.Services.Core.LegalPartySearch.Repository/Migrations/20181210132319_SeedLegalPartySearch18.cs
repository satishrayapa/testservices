using Microsoft.EntityFrameworkCore.Migrations;
using TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class SeedLegalPartySearch18 : Migration
    {
	    protected override void Up(MigrationBuilder migrationBuilder)
	    {
		    migrationBuilder.Sql(@"
IF EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[search].[LegalPartySearch]')) 
DROP FULLTEXT INDEX ON [search].[LegalPartySearch];", true);

		    migrationBuilder.Sql(UpdatedSeedQuery.BuildSeedQuery());

		    migrationBuilder.Sql(@"
			CREATE FULLTEXT INDEX ON[search].[LegalPartySearch]
		    (
			    DisplayName LANGUAGE 0,
			    Addr LANGUAGE 0,
			    PIN LANGUAGE 0,
			    AIN LANGUAGE 0,
			    GeoCode LANGUAGE 0,
			    SearchAll LANGUAGE 0,   
			    Tag LANGUAGE 0,
				UnformattedPIN LANGUAGE 0)
		    KEY INDEX PK_LegalPartySearch
		    WITH STOPLIST = OFF;		
			", true);
	    }

	    protected override void Down(MigrationBuilder migrationBuilder)
	    {
		    migrationBuilder.Sql(@"TRUNCATE TABLE [search].[LegalPartySearch]");
	    }
	}
}
