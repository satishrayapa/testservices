using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class FixUpPreviousIndexMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			Down(migrationBuilder);

            migrationBuilder.CreateIndex(
                name: "IX_BVSValueHeader_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSValueHeader",
                column: "DynCalcStepTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_BVSValue_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSValue",
                column: "DynCalcStepTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_BVSTran_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSTran",
                column: "DynCalcStepTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_BVSOwnerValue_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwnerValue",
                column: "DynCalcStepTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_BVSOwner_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner",
                column: "DynCalcStepTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_BVS_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVS",
                column: "DynCalcStepTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_BVS_RevObjId",
                schema: "Service.BaseValueSegment",
                table: "BVS",
                column: "RevObjId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BVSValueHeader_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSValueHeader");

            migrationBuilder.DropIndex(
                name: "IX_BVSValue_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSValue");

            migrationBuilder.DropIndex(
                name: "IX_BVSTran_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSTran");

            migrationBuilder.DropIndex(
                name: "IX_BVSOwnerValue_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwnerValue");

            migrationBuilder.DropIndex(
                name: "IX_BVSOwner_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner");

            migrationBuilder.DropIndex(
                name: "IX_BVS_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVS");

            migrationBuilder.DropIndex(
                name: "IX_BVS_RevObjId",
                schema: "Service.BaseValueSegment",
                table: "BVS");
        }
    }
}
