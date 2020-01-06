using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
  public partial class SeedLegalPartyWithLegalPartyRoleNullable : Migration
  {
	protected override void Up(MigrationBuilder migrationBuilder)
	{
	  //migrationBuilder.Sql(RebuildSearchLegalPartyIndex.GetMigrationVersion());
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
	  migrationBuilder.Sql(@"TRUNCATE TABLE [search].[LegalPartySearch]");
	}
  }
}