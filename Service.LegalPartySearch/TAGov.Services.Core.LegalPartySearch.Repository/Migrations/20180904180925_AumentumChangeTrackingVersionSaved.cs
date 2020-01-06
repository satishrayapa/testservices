using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AumentumChangeTrackingVersionSaved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
				
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AumentumChangeTrackingVersion' AND TABLE_SCHEMA = 'search')
				BEGIN
					DROP TABLE search.AumentumChangeTrackingVersion
				END

			");

            migrationBuilder.CreateTable(
                name: "AumentumChangeTrackingVersion",
				schema: "search",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TableName = table.Column<string>(nullable: true),
	                ChangeVersion = table.Column<long>(nullable: false)
				},
                constraints: table =>
                {
                    table.PrimaryKey("PK_AumentumChangeTrackingVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AumentumChangeTrackingVersionSaved",
                schema: "search",
				columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
	                TableName = table.Column<string>(nullable: true),
					ChangeVersion = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AumentumChangeTrackingVersionSaved", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AumentumChangeTrackingVersion",
	            schema: "search");

            migrationBuilder.DropTable(
                name: "AumentumChangeTrackingVersionSaved",
	            schema: "search");
        }
    }
}
