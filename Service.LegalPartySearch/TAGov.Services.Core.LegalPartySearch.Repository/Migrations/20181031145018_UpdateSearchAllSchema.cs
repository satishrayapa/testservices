using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class UpdateSearchAllSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                schema: "search",
                table: "AumentumChangeTrackingVersionSaved",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                schema: "search",
                table: "AumentumChangeTrackingVersion",
                newName: "Id");

	        migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON [search].[LegalPartySearch];", true);

	        migrationBuilder.AlterColumn<string>(
		        name: "SearchAll",
		        schema: "search",
		        table: "LegalPartySearch",
		        type: "varchar(max)",
		        nullable: true,
		        oldClrType: typeof(string),
		        oldType: "varchar(1000)",
		        oldMaxLength: 1000,
		        oldNullable: true);

			migrationBuilder.Sql(@"
			CREATE FULLTEXT INDEX ON[search].[LegalPartySearch]
		    (
			    DisplayName LANGUAGE 0,
			    Addr LANGUAGE 0,
			    PIN LANGUAGE 0,
			    AIN LANGUAGE 0,
			    GeoCode LANGUAGE 0,
			    SearchAll LANGUAGE 0,   
			    Tag LANGUAGE 0,
				UnformattedPIN LANGUAGE 0)
		    KEY INDEX PK_LegalPartySearch
		    WITH STOPLIST = OFF;		
			", true);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON [search].[LegalPartySearch];", true);

			migrationBuilder.RenameColumn(
                name: "Id",
                schema: "search",
                table: "AumentumChangeTrackingVersionSaved",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "search",
                table: "AumentumChangeTrackingVersion",
                newName: "ID");

	        migrationBuilder.AlterColumn<string>(
		        name: "SearchAll",
		        schema: "search",
		        table: "LegalPartySearch",
		        type: "varchar(1000)",
		        maxLength: 1000,
		        nullable: true,
		        oldClrType: typeof(string),
		        oldType: "varchar(max)",
		        oldNullable: true);

			migrationBuilder.Sql(@"
			CREATE FULLTEXT INDEX ON[search].[LegalPartySearch]
		    (
			    DisplayName LANGUAGE 0,
			    Addr LANGUAGE 0,
			    PIN LANGUAGE 0,
			    AIN LANGUAGE 0,
			    GeoCode LANGUAGE 0,
			    SearchAll LANGUAGE 0,   
			    Tag LANGUAGE 0,
				UnformattedPIN LANGUAGE 0)
		    KEY INDEX PK_LegalPartySearch
		    WITH STOPLIST = OFF;		
			", true);
		}
    }
}
