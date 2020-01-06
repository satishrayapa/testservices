using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class AddBaseValueSegmentTransactionValueTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BVSTranValue",
                schema: "Service.BaseValueSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Attribute1 = table.Column<int>(nullable: false, defaultValue: 0),
                    Attribute2 = table.Column<int>(nullable: false, defaultValue: 0),
                    Attribute3 = table.Column<int>(nullable: false, defaultValue: 0),
                    Attribute4 = table.Column<int>(nullable: false, defaultValue: 0),
                    BVSTranId = table.Column<int>(nullable: false),
                    DynCalcStepTrackingId = table.Column<int>(nullable: false),
                    TaxYear = table.Column<int>(nullable: false),
                    ValueAmount = table.Column<decimal>(type: "decimal(28, 10)", nullable: false, defaultValue: 1m),
                    ValueTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BVSTranValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BVSTranValue_BVSTran_BVSTranId",
                        column: x => x.BVSTranId,
                        principalSchema: "Service.BaseValueSegment",
                        principalTable: "BVSTran",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BVSTranValue_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSTranValue",
                column: "DynCalcStepTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_BVSTranValue_BVSTranId_ValueTypeId",
                schema: "Service.BaseValueSegment",
                table: "BVSTranValue",
                columns: new[] { "BVSTranId", "ValueTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_BVSTranValue_BVSTranId_ValueTypeId_Attribute1_Attribute2_Attribute3_Attribute4",
                schema: "Service.BaseValueSegment",
                table: "BVSTranValue",
                columns: new[] { "BVSTranId", "ValueTypeId", "Attribute1", "Attribute2", "Attribute3", "Attribute4" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BVSTranValue",
                schema: "Service.BaseValueSegment");
        }
    }
}
