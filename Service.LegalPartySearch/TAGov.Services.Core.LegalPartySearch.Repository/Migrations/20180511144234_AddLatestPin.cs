using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AddLatestPin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PINLatest",
                schema: "search",
                table: "LegalPartySearch",
                type: "varchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RevObjBegEffDateLatest",
                schema: "search",
                table: "LegalPartySearch",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PINLatest",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "RevObjBegEffDateLatest",
                schema: "search",
                table: "LegalPartySearch");
        }
    }
}
