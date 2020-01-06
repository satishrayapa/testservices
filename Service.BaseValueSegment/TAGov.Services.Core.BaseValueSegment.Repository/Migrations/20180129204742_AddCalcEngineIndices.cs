using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class AddCalcEngineIndices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.CreateIndex(
		        name: "IX_BVS_DynCalcStepTrackingId",
		        schema: "Service.BaseValueSegment",
		        table: "BVS",
		        column: "DynCalcStepTrackingId");

	        migrationBuilder.CreateIndex(
		        name: "IX_BVSTran_DynCalcStepTrackingId",
		        schema: "Service.BaseValueSegment",
		        table: "BVSTran",
		        column: "DynCalcStepTrackingId");

			migrationBuilder.CreateIndex(
		        name: "IX_BVSOwner_DynCalcStepTrackingId",
		        schema: "Service.BaseValueSegment",
		        table: "BVSOwner",
		        column: "DynCalcStepTrackingId");

	        migrationBuilder.CreateIndex(
		        name: "IX_BVSValueHeader_DynCalcStepTrackingId",
		        schema: "Service.BaseValueSegment",
		        table: "BVSValueHeader",
		        column: "DynCalcStepTrackingId");

	        migrationBuilder.CreateIndex(
		        name: "IX_BVSOwnerValue_DynCalcStepTrackingId",
		        schema: "Service.BaseValueSegment",
		        table: "BVSOwnerValue",
		        column: "DynCalcStepTrackingId");

	        migrationBuilder.CreateIndex(
		        name: "IX_BVSValue_DynCalcStepTrackingId",
		        schema: "Service.BaseValueSegment",
		        table: "BVSValue",
		        column: "DynCalcStepTrackingId");

	        migrationBuilder.CreateIndex(
		        name: "IX_BVS_RevObjId",
		        schema: "Service.BaseValueSegment",
		        table: "BVS",
		        column: "RevObjId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex("IX_BVS_DynCalcStepTrackingId", "BVS", "Service.BaseValueSegment");
			migrationBuilder.DropIndex("IX_BVSTran_DynCalcStepTrackingId", "BVSTran", "Service.BaseValueSegment");
			migrationBuilder.DropIndex("IX_BVSOwner_DynCalcStepTrackingId", "BVSOwner", "Service.BaseValueSegment");
			migrationBuilder.DropIndex("IX_BVSValueHeader_DynCalcStepTrackingId", "BVSValueHeader", "Service.BaseValueSegment");
			migrationBuilder.DropIndex("IX_BVSOwnerValue_DynCalcStepTrackingId", "BVSOwnerValue", "Service.BaseValueSegment");
			migrationBuilder.DropIndex("IX_BVSValue_DynCalcStepTrackingId", "BVSValue", "Service.BaseValueSegment");
			migrationBuilder.DropIndex("IX_BVS_RevObjId", "BVS", "Service.BaseValueSegment");
		}
	}
}
