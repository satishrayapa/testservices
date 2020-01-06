using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "search");

            migrationBuilder.CreateTable(
                name: "LegalPartySearch",
                schema: "search",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddrId = table.Column<int>(nullable: true),
                    Addr = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    AddrBegEffDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AddrRoleBegEffDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AddrRoleId = table.Column<int>(nullable: true),
                    AddrType = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    AIN = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    DisplayName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    BegEffDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffStatus = table.Column<string>(type: "char(1)", nullable: true, defaultValue: "A"),
                    LegalPartyId = table.Column<int>(nullable: false),
                    LegalPartyRole = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    LegalPartyRoleId = table.Column<int>(nullable: false),
                    PIN = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    RevObjBegEffDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RevObjId = table.Column<int>(nullable: true),
                    SearchDoc = table.Column<string>(type: "varchar(350)", maxLength: 350, nullable: true),
                    SearchPin = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalPartySearch", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegalPartySearch_LegalPartyId",
                schema: "search",
                table: "LegalPartySearch",
                column: "LegalPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalPartySearch_LegalPartyRoleId_BegEffDate",
                schema: "search",
                table: "LegalPartySearch",
                columns: new[] { "LegalPartyRoleId", "BegEffDate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegalPartySearch",
                schema: "search");
        }
    }
}
