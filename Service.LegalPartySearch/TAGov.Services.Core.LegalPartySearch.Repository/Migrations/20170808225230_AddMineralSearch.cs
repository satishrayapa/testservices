using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AddMineralSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("TRUNCATE TABLE search.LegalPartySearch");

			migrationBuilder.AddColumn<bool>(
                name: "Mineral",
                schema: "search",
                table: "LegalPartySearch",
                nullable: false,
                defaultValue: false);


            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mineral",
                schema: "search",
                table: "LegalPartySearch");
        }
    }
}
