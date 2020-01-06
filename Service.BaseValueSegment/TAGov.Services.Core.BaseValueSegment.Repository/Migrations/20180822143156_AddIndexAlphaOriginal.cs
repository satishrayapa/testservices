using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class AddIndexAlphaOriginal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BVSValueHeader_OriginalBVSValueHeaderId",
                schema: "Service.BaseValueSegment",
                table: "BVSValueHeader",
                column: "OriginalBVSValueHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_BVSOwner_AlphaBVSOwnerId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner",
                column: "AlphaBVSOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BVSValueHeader_BVSValueHeader",
                table: "BVSValueHeader",
                column: "OriginalBVSValueHeaderId",
                principalTable: "BVSValueHeader",
                schema: "Service.BaseValueSegment",
                principalSchema: "Service.BaseValueSegment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

    protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BVSValueHeader_OriginalBVSValueHeaderId",
                schema: "Service.BaseValueSegment",
                table: "BVSValueHeader");

            migrationBuilder.DropIndex(
                name: "IX_BVSOwner_AlphaBVSOwnerId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner");

            migrationBuilder.DropForeignKey(
                name: "FK_BVSValueHeader_BVSValueHeader",
                schema: "Service.BaseValueSegment",
                table: "BVSValueHeader");
        }
  }
}
