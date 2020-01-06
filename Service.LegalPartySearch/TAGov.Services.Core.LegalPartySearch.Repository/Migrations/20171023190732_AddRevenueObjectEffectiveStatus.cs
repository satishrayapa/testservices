using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AddRevenueObjectEffectiveStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RevObjEffStatus",
                schema: "search",
                table: "LegalPartySearch",
                type: "char(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevObjEffStatus",
                schema: "search",
                table: "LegalPartySearch");
        }
    }
}
