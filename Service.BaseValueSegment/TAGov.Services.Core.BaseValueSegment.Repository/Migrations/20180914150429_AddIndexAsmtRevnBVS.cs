using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class AddIndexAsmtRevnBVS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AsmtRevnBVS_AsmtRevnId_BVSId",
                schema: "Service.BaseValueSegment",
                table: "AsmtRevnBVS",
                columns: new[] { "AsmtRevnId", "BVSId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AsmtRevnBVS_AsmtRevnId_BVSId",
                schema: "Service.BaseValueSegment",
                table: "AsmtRevnBVS");
        }
    }
}
