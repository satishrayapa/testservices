using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
	public partial class SeedLegalPartySearch14 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			//Seeding only needs to happen once during migration so comment out deprecated seedings.
			//migrationBuilder.Sql(RebuildSearchLegalPartyIndex.GetMigrationVersion());
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"TRUNCATE TABLE [search].[LegalPartySearch]");
		}
	}
}
