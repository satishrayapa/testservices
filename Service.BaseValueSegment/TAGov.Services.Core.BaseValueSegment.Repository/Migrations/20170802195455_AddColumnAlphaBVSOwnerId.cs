using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class AddColumnAlphaBVSOwnerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlphaBVSOwnerId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlphaBVSOwnerId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner");
        }
    }
}
