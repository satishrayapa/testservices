using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AddRealPropertyAppraisal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddrStreetName",
                schema: "search",
                table: "LegalPartySearch",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddrStreetNumber",
                schema: "search",
                table: "LegalPartySearch",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddrUnitNumber",
                schema: "search",
                table: "LegalPartySearch",
                type: "varchar(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppraisalBegEffDate",
                schema: "search",
                table: "LegalPartySearch",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppraisalClass",
                schema: "search",
                table: "LegalPartySearch",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppraisalEffStatus",
                schema: "search",
                table: "LegalPartySearch",
                type: "char(1)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppraisalRoleBegEffDate",
                schema: "search",
                table: "LegalPartySearch",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppraisalRoleEffStatus",
                schema: "search",
                table: "LegalPartySearch",
                type: "char(1)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppraisalRoleId",
                schema: "search",
                table: "LegalPartySearch",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppraisalSiteId",
                schema: "search",
                table: "LegalPartySearch",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppraisalSiteName",
                schema: "search",
                table: "LegalPartySearch",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Neighborhood",
                schema: "search",
                table: "LegalPartySearch",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddrStreetName",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "AddrStreetNumber",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "AddrUnitNumber",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "AppraisalBegEffDate",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "AppraisalClass",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "AppraisalEffStatus",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "AppraisalRoleBegEffDate",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "AppraisalRoleEffStatus",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "AppraisalRoleId",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "AppraisalSiteId",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "AppraisalSiteName",
                schema: "search",
                table: "LegalPartySearch");

            migrationBuilder.DropColumn(
                name: "Neighborhood",
                schema: "search",
                table: "LegalPartySearch");
        }
    }
}
