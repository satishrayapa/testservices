using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class RemoveGRMEventIdNullability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GRMEventId",
                schema: "Service.BaseValueSegment",
                table: "BVSValueHeader",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GRMEventId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GRMEventId",
                schema: "Service.BaseValueSegment",
                table: "BVSValueHeader",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "GRMEventId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
