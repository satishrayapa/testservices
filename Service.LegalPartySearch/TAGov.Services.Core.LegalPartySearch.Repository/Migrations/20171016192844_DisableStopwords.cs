using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
	public partial class DisableStopwords : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("ALTER FULLTEXT INDEX ON [search].[LegalPartySearch] SET STOPLIST = OFF", true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("ALTER FULLTEXT INDEX ON [search].[LegalPartySearch] SET STOPLIST = SYSTEM", true);
		}
	}
}
