using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
	public partial class AddCascadeDeleteOnAsmtRevnBVS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.DropForeignKey("FK_AsmtRevnBvs_AsmtRevn", "AsmtRevnBVS", "Service.BaseValueSegment");

	        migrationBuilder.AddForeignKey(
		        name: "FK_AsmtRevnBvs_AsmtRevn",
		        table: "AsmtRevnBVS",
		        column: "AsmtRevnId",
		        principalTable: "AsmtRevn",
		        schema: "Service.BaseValueSegment",
		        principalSchema: "dbo",
		        principalColumn: "Id",
		        onDelete: ReferentialAction.Cascade);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.DropForeignKey("FK_AsmtRevnBvs_AsmtRevn", "AsmtRevnBVS", "Service.BaseValueSegment");

	        migrationBuilder.AddForeignKey(
		        name: "FK_AsmtRevnBvs_AsmtRevn",
		        table: "AsmtRevnBVS",
		        column: "AsmtRevnId",
		        principalTable: "AsmtRevn",
		        schema: "Service.BaseValueSegment",
		        principalSchema: "dbo",
		        principalColumn: "Id",
		        onDelete: ReferentialAction.Restrict);
		}
    }
}
