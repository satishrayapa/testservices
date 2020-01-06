using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AddSourceInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Mineral",
                schema: "search",
                table: "LegalPartySearch",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                schema: "search",
                table: "LegalPartySearch",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                schema: "search",
                table: "LegalPartySearch",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdated",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "Source",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.AlterColumn<bool>(
                name: "Mineral",
                schema: "search",
                table: "LegalPartySearch",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
