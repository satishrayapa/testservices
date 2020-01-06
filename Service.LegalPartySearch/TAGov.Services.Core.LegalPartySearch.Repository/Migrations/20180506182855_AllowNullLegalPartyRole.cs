using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
  public partial class AllowNullLegalPartyRole : Migration
  {
	protected override void Up(MigrationBuilder migrationBuilder)
	{
	  migrationBuilder.AlterColumn<int>(
		  name: "LegalPartyRoleId",
		  schema: "search",
		  table: "LegalPartySearch",
		  nullable: true,
		  oldClrType: typeof(int));
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
	  migrationBuilder.AlterColumn<int>(
		  name: "LegalPartyRoleId",
		  schema: "search",
		  table: "LegalPartySearch",
		  nullable: false,
		  oldClrType: typeof(int),
		  oldNullable: true);
	}
  }
}
