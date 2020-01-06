using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class AddIsUserOverride : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUserOverride",
                schema: "Service.BaseValueSegment",
                table: "BVSValue",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUserOverride",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUserOverride",
                schema: "Service.BaseValueSegment",
                table: "BVSValue");

            migrationBuilder.DropColumn(
                name: "IsUserOverride",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner");
        }
    }
}
