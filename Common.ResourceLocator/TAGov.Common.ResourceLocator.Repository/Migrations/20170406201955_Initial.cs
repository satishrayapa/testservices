using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.ResourceLocator.Repository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Common.Resource");

			migrationBuilder.CreateTable(
                name: "Resource",
                schema: "Common.Resource",
                columns: table => new
                {
                    Key = table.Column<string>(maxLength: 200, nullable: false),
                    Partition = table.Column<string>(maxLength: 200, nullable: false),
                    Value = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => new { x.Key, x.Partition });
                });		
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resource",
                schema: "Common.Resource");
        }
    }
}
