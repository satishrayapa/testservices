using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
	public partial class AddSearchAllAndDropOtherSearchColumns : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON [search].[LegalPartySearch];", true);

			migrationBuilder.DropColumn(
				name: "SearchDoc",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.DropColumn(
				name: "SearchGeoTag",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.DropColumn(
				name: "SearchPin",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.AddColumn<string>(
				name: "SearchAll",
				schema: "search",
				table: "LegalPartySearch",
				type: "varchar(1000)",
				maxLength: 1000,
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "SearchAll",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.AddColumn<string>(
				name: "SearchDoc",
				schema: "search",
				table: "LegalPartySearch",
				type: "varchar(500)",
				maxLength: 500,
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "SearchGeoTag",
				schema: "search",
				table: "LegalPartySearch",
				type: "varchar(100)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "SearchPin",
				schema: "search",
				table: "LegalPartySearch",
				type: "varchar(100)",
				maxLength: 100,
				nullable: true);

			migrationBuilder.Sql(@"CREATE FULLTEXT INDEX ON [search].[LegalPartySearch] (
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
	}
}
