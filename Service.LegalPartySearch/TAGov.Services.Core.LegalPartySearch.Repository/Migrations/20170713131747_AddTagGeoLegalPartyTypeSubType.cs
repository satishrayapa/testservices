using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
	public partial class AddTagGeoLegalPartyTypeSubType : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "SearchDoc",
				schema: "search",
				table: "LegalPartySearch",
				type: "varchar(500)",
				maxLength: 500,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "varchar(350)",
				oldMaxLength: 350,
				oldNullable: true);

			migrationBuilder.AddColumn<string>(
				name: "GeoCode",
				schema: "search",
				table: "LegalPartySearch",
				type: "varchar(32)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "LegalPartySubType",
				schema: "search",
				table: "LegalPartySearch",
				type: "varchar(64)",
				maxLength: 64,
				nullable: true);

			migrationBuilder.AddColumn<int>(
				name: "LegalPartySubTypeId",
				schema: "search",
				table: "LegalPartySearch",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "LegalPartyType",
				schema: "search",
				table: "LegalPartySearch",
				type: "varchar(64)",
				maxLength: 64,
				nullable: true);

			migrationBuilder.AddColumn<int>(
				name: "LegalPartyTypeId",
				schema: "search",
				table: "LegalPartySearch",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "SearchGeoTag",
				schema: "search",
				table: "LegalPartySearch",
				type: "varchar(100)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "Tag",
				schema: "search",
				table: "LegalPartySearch",
				type: "varchar(64)",
				maxLength: 64,
				nullable: true);

			migrationBuilder.AddColumn<short>(
				name: "TagBegEffYear",
				schema: "search",
				table: "LegalPartySearch",
				type: "smallint",
				nullable: true);

			migrationBuilder.AddColumn<int>(
				name: "TagId",
				schema: "search",
				table: "LegalPartySearch",
				nullable: true);

			migrationBuilder.AddColumn<DateTime>(
				name: "TagRoleBegEffDate",
				schema: "search",
				table: "LegalPartySearch",
				nullable: true);

			migrationBuilder.AddColumn<int>(
				name: "TagRoleId",
				schema: "search",
				table: "LegalPartySearch",
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON [search].[LegalPartySearch];", true);
			migrationBuilder.DropColumn(
				name: "GeoCode",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.DropColumn(
				name: "LegalPartySubType",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.DropColumn(
				name: "LegalPartySubTypeId",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.DropColumn(
				name: "LegalPartyType",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.DropColumn(
				name: "LegalPartyTypeId",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.DropColumn(
				name: "SearchGeoTag",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.DropColumn(
				name: "Tag",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.DropColumn(
				name: "TagBegEffYear",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.DropColumn(
				name: "TagId",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.DropColumn(
				name: "TagRoleBegEffDate",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.DropColumn(
				name: "TagRoleId",
				schema: "search",
				table: "LegalPartySearch");

			migrationBuilder.AlterColumn<string>(
				name: "SearchDoc",
				schema: "search",
				table: "LegalPartySearch",
				type: "varchar(350)",
				maxLength: 350,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "varchar(500)",
				oldMaxLength: 500,
				oldNullable: true);

			migrationBuilder.Sql(@"CREATE FULLTEXT INDEX ON [search].[LegalPartySearch] (
					DisplayName, 
					Addr,
					PIN,
					AIN,
					SearchDoc,
					SearchPin
					)
					KEY INDEX PK_LegalPartySearch
					WITH STOPLIST = SYSTEM;
			", true);
		}
	}
}
