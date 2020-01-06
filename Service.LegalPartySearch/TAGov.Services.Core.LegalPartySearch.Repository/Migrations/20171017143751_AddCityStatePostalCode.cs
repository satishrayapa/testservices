using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AddCityStatePostalCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "search",
                table: "LegalPartySearch",
                type: "varchar(32)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                schema: "search",
                table: "LegalPartySearch",
                type: "varchar(16)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                schema: "search",
                table: "LegalPartySearch",
                type: "varchar(4)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "search",
                table: "LegalPartySearch");
        }
    }
}
