using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Service.BaseValueSegment");

            migrationBuilder.CreateTable(
                name: "BVS",
                schema: "Service.BaseValueSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AsOf = table.Column<DateTime>(type: "date", nullable: false),
                    DynCalcInstanceId = table.Column<int>(nullable: false),
                    RevObjId = table.Column<int>(nullable: false),
                    SequenceNumber = table.Column<int>(nullable: false),
                    TranId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BVS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BVSStatusType",
                schema: "Service.BaseValueSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BVSStatusType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BVSTranType",
                schema: "Service.BaseValueSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BVSTranType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AsmtRevnBVS",
                schema: "Service.BaseValueSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AsmtRevnId = table.Column<int>(nullable: false),
                    BVSId = table.Column<int>(nullable: true),
                    BVSStatusTypeId = table.Column<int>(nullable: false),
                    DynCalcStepTrackingId = table.Column<int>(nullable: false),
                    ReviewMessage = table.Column<string>(type: "varchar(1024)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsmtRevnBVS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsmtRevnBVS_BVS_BVSId",
                        column: x => x.BVSId,
                        principalSchema: "Service.BaseValueSegment",
                        principalTable: "BVS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AsmtRevnBVS_BVSStatusType_BVSStatusTypeId",
                        column: x => x.BVSStatusTypeId,
                        principalSchema: "Service.BaseValueSegment",
                        principalTable: "BVSStatusType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BVSTran",
                schema: "Service.BaseValueSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BVSId = table.Column<int>(nullable: false),
                    BVSTranTypeId = table.Column<int>(nullable: false),
                    DynCalcStepTrackingId = table.Column<int>(nullable: true),
                    EffStatus = table.Column<string>(type: "char(1)", nullable: false),
                    TranId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BVSTran", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BVSTran_BVS_BVSId",
                        column: x => x.BVSId,
                        principalSchema: "Service.BaseValueSegment",
                        principalTable: "BVS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BVSTran_BVSTranType_BVSTranTypeId",
                        column: x => x.BVSTranTypeId,
                        principalSchema: "Service.BaseValueSegment",
                        principalTable: "BVSTranType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BVSOwner",
                schema: "Service.BaseValueSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BVSTranId = table.Column<int>(nullable: false),
                    BIPercent = table.Column<decimal>(type: "decimal(28, 10)", nullable: false),
                    DynCalcStepTrackingId = table.Column<int>(nullable: false),
                    GRMEventId = table.Column<int>(nullable: true),
                    LegalPartyRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BVSOwner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BVSOwner_BVSTran_BVSTranId",
                        column: x => x.BVSTranId,
                        principalSchema: "Service.BaseValueSegment",
                        principalTable: "BVSTran",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BVSValueHeader",
                schema: "Service.BaseValueSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BVSTranId = table.Column<int>(nullable: false),
                    BaseYear = table.Column<int>(nullable: false),
                    GRMEventId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BVSValueHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BVSValueHeader_BVSTran_BVSTranId",
                        column: x => x.BVSTranId,
                        principalSchema: "Service.BaseValueSegment",
                        principalTable: "BVSTran",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BVSOwnerValue",
                schema: "Service.BaseValueSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BaseValue = table.Column<decimal>(type: "decimal(28, 10)", nullable: false),
                    BVSOwnerId = table.Column<int>(nullable: false),
                    BVSValueHeaderId = table.Column<int>(nullable: false),
                    DynCalcStepTrackingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BVSOwnerValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BVSOwnerValue_BVSOwner_BVSOwnerId",
                        column: x => x.BVSOwnerId,
                        principalSchema: "Service.BaseValueSegment",
                        principalTable: "BVSOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BVSOwnerValue_BVSValueHeader_BVSValueHeaderId",
                        column: x => x.BVSValueHeaderId,
                        principalSchema: "Service.BaseValueSegment",
                        principalTable: "BVSValueHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BVSValue",
                schema: "Service.BaseValueSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BVSValueHeaderId = table.Column<int>(nullable: false),
                    DynCalcStepTrackingId = table.Column<int>(nullable: false),
                    FullValueAmount = table.Column<decimal>(type: "decimal(28, 10)", nullable: false),
                    PctComplete = table.Column<decimal>(type: "decimal(14, 10)", nullable: false),
                    SubComponent = table.Column<int>(nullable: false),
                    ValueAmount = table.Column<decimal>(type: "decimal(28, 10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BVSValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BVSValue_BVSValueHeader_BVSValueHeaderId",
                        column: x => x.BVSValueHeaderId,
                        principalSchema: "Service.BaseValueSegment",
                        principalTable: "BVSValueHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsmtRevnBVS_BVSId",
                schema: "Service.BaseValueSegment",
                table: "AsmtRevnBVS",
                column: "BVSId");

            migrationBuilder.CreateIndex(
                name: "IX_AsmtRevnBVS_BVSStatusTypeId",
                schema: "Service.BaseValueSegment",
                table: "AsmtRevnBVS",
                column: "BVSStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BVS_AsOf_RevObjId_SequenceNumber",
                schema: "Service.BaseValueSegment",
                table: "BVS",
                columns: new[] { "AsOf", "RevObjId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BVSOwner_BVSTranId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwner",
                column: "BVSTranId");

            migrationBuilder.CreateIndex(
                name: "IX_BVSOwnerValue_BVSValueHeaderId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwnerValue",
                column: "BVSValueHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_BVSOwnerValue_BVSOwnerId_DynCalcStepTrackingId",
                schema: "Service.BaseValueSegment",
                table: "BVSOwnerValue",
                columns: new[] { "BVSOwnerId", "DynCalcStepTrackingId" });

            migrationBuilder.CreateIndex(
                name: "IX_BVSTran_BVSId",
                schema: "Service.BaseValueSegment",
                table: "BVSTran",
                column: "BVSId");

            migrationBuilder.CreateIndex(
                name: "IX_BVSTran_BVSTranTypeId",
                schema: "Service.BaseValueSegment",
                table: "BVSTran",
                column: "BVSTranTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BVSValue_BVSValueHeaderId",
                schema: "Service.BaseValueSegment",
                table: "BVSValue",
                column: "BVSValueHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_BVSValueHeader_BVSTranId",
                schema: "Service.BaseValueSegment",
                table: "BVSValueHeader",
                column: "BVSTranId");

	        //custom code to add a foreign key that references an existing table
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

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsmtRevnBVS",
                schema: "Service.BaseValueSegment");

            migrationBuilder.DropTable(
                name: "BVSOwnerValue",
                schema: "Service.BaseValueSegment");

            migrationBuilder.DropTable(
                name: "BVSValue",
                schema: "Service.BaseValueSegment");

            migrationBuilder.DropTable(
                name: "BVSStatusType",
                schema: "Service.BaseValueSegment");

            migrationBuilder.DropTable(
                name: "BVSOwner",
                schema: "Service.BaseValueSegment");

            migrationBuilder.DropTable(
                name: "BVSValueHeader",
                schema: "Service.BaseValueSegment");

            migrationBuilder.DropTable(
                name: "BVSTran",
                schema: "Service.BaseValueSegment");

            migrationBuilder.DropTable(
                name: "BVS",
                schema: "Service.BaseValueSegment");

            migrationBuilder.DropTable(
                name: "BVSTranType",
                schema: "Service.BaseValueSegment");
        }
    }
}
