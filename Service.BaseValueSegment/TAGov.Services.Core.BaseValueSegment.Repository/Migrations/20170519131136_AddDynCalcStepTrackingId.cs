using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class AddDynCalcStepTrackingId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSValueHeader",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSValue",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwnerValue",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVS",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSValueHeader");

            migrationBuilder.DropColumn(
                name: "DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVS");

            migrationBuilder.AlterColumn<int>(
                name: "DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSValue",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwnerValue",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);
        }
    }
}
