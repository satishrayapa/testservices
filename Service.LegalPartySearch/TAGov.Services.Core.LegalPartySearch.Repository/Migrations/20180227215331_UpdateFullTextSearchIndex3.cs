using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class UpdateFullTextSearchIndex3 : Migration
    {
		protected override void Up(MigrationBuilder migrationBuilder)
	    {		   
		    migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON [search].[LegalPartySearch];
			CREATE FULLTEXT INDEX ON[search].[LegalPartySearch]
		    (
			    DisplayName,
			    Addr,
			    PIN,
			    AIN,
			    GeoCode,
			    SearchAll LANGUAGE 0,   --This tells SQL Server to use the Neutral word breaker
			    Tag)
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
			    SearchAll,
			    Tag)
		    KEY INDEX PK_LegalPartySearch
		    WITH STOPLIST = OFF;		
			", true);
		}
	}
}
