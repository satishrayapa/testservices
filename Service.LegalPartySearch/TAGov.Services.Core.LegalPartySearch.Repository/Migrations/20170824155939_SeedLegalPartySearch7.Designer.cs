using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TAGov.Services.Core.LegalPartySearch.Repository;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    [DbContext(typeof(SearchLegalPartyContext))]
    [Migration("20170824155939_SeedLegalPartySearch7")]
    partial class SeedLegalPartySearch7
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TAGov.Services.Core.LegalPartySearch.Repository.Models.V1.SearchLegalParty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasColumnName("Addr")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("AddressEffectiveDate")
                        .HasColumnName("AddrBegEffDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("AddressId")
                        .HasColumnName("AddrId");

                    b.Property<DateTime?>("AddressRoleEffectiveDate")
                        .HasColumnName("AddrRoleBegEffDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("AddressRoleId")
                        .HasColumnName("AddrRoleId");

                    b.Property<string>("AddressStreetName")
                        .HasColumnName("AddrStreetName")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<int?>("AddressStreetNumber")
                        .HasColumnName("AddrStreetNumber");

                    b.Property<string>("AddressType")
                        .HasColumnName("AddrType")
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("AddressUnitNumber")
                        .HasColumnName("AddrUnitNumber")
                        .HasColumnType("varchar(8)")
                        .HasMaxLength(8);

                    b.Property<string>("Ain")
                        .HasColumnName("AIN")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<DateTime?>("AppraisalBeginEffectiveDate")
                        .HasColumnName("AppraisalBegEffDate");

                    b.Property<string>("AppraisalClass")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("AppraisalEffectiveStatus")
                        .HasColumnName("AppraisalEffStatus")
                        .HasColumnType("char(1)");

                    b.Property<DateTime?>("AppraisalRoleBeginEffectiveDate")
                        .HasColumnName("AppraisalRoleBegEffDate");

                    b.Property<string>("AppraisalRoleEffectiveStatus")
                        .HasColumnName("AppraisalRoleEffStatus")
                        .HasColumnType("char(1)");

                    b.Property<int?>("AppraisalRoleId");

                    b.Property<int?>("AppraisalSiteId");

                    b.Property<string>("AppraisalSiteName")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("DisplayName")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<DateTime>("EffectiveDate")
                        .HasColumnName("BegEffDate")
                        .HasColumnType("datetime");

                    b.Property<string>("EffectiveStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EffStatus")
                        .HasColumnType("char(1)")
                        .HasDefaultValue("A");

                    b.Property<string>("GeoCode")
                        .HasColumnName("GeoCode")
                        .HasColumnType("varchar(32)");

                    b.Property<DateTime?>("LastUpdated");

                    b.Property<int>("LegalPartyId");

                    b.Property<string>("LegalPartyRole")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<int>("LegalPartyRoleId");

                    b.Property<string>("LegalPartySubType")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<int?>("LegalPartySubTypeId");

                    b.Property<string>("LegalPartyType")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<int?>("LegalPartyTypeId");

                    b.Property<bool?>("MineralIsNotNullWithValue")
                        .HasColumnName("Mineral");

                    b.Property<string>("Neighborhood")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Pin")
                        .HasColumnName("PIN")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<DateTime?>("RevenueObjectEffectiveDate")
                        .HasColumnName("RevObjBegEffDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("RevenueObjectId")
                        .HasColumnName("RevObjId");

                    b.Property<string>("SearchDoc")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("SearchGeoTag")
                        .HasColumnName("SearchGeoTag")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("SearchPin")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Source")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Tag")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<short?>("TagBegEffYear")
                        .HasColumnType("smallint");

                    b.Property<int?>("TagId");

                    b.Property<DateTime?>("TagRoleBegEffDate");

                    b.Property<int?>("TagRoleId");

                    b.HasKey("Id");

                    b.HasIndex("LegalPartyId");

                    b.HasIndex("LegalPartyRoleId", "EffectiveDate");

                    b.ToTable("LegalPartySearch","search");
                });
        }
    }
}
