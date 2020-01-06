using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
	public partial class UpdateFullTextSearchIndex2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"CREATE FULLTEXT INDEX ON [search].[LegalPartySearch] (
					DisplayName, 
					Addr,
					PIN,
					AIN,
					GeoCode,
					SearchAll,
					Tag)
					KEY INDEX PK_LegalPartySearch
					WITH STOPLIST = SYSTEM;
			", true);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON [search].[LegalPartySearch];", true);
		}
    }
}
