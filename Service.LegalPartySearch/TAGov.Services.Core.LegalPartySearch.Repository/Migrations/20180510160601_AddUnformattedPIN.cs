using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AddUnformattedPIN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnformattedPIN",
                schema: "search",
                table: "LegalPartySearch",
                type: "varchar(32)",
                maxLength: 32,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnformattedPIN",
                schema: "search",
                table: "LegalPartySearch");
        }
    }
}
