using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
	public partial class SeedLegalPartySearch17 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// Refer to SeedLegalPartySearch18 for seeding logic.
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"TRUNCATE TABLE [search].[LegalPartySearch]");
		}
	}
}
