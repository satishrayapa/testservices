using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class AddColumnOriginalBVSValueHeaderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginalBVSValueHeaderId",
                schema: "Service.BaseValueSegment",
                table: "BVSValueHeader",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalBVSValueHeaderId",
                schema: "Service.BaseValueSegment",
                table: "BVSValueHeader");
        }
    }
}
