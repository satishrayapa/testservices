using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TAGov.Services.Core.LegalPartySearch.Repository;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    [DbContext(typeof(SearchLegalPartyContext))]
    [Migration("20170628153819_SeedLegalPartySearch")]
    partial class SeedLegalPartySearch
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

                    b.Property<int?>("AddrId")
                        .HasColumnName("AddrId");

                    b.Property<string>("Address")
                        .HasColumnName("Addr")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("AddressEffectiveDate")
                        .HasColumnName("AddrBegEffDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("AddressRoleEffectiveDate")
                        .HasColumnName("AddrRoleBegEffDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("AddressRoleId")
                        .HasColumnName("AddrRoleId");

                    b.Property<string>("AddressType")
                        .HasColumnName("AddrType")
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Ain")
                        .HasColumnName("AIN")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

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

                    b.Property<int>("LegalPartyId");

                    b.Property<string>("LegalPartyRole")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<int>("LegalPartyRoleId");

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
                        .HasColumnType("varchar(350)")
                        .HasMaxLength(350);

                    b.Property<string>("SearchPin")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("LegalPartyId");

                    b.HasIndex("LegalPartyRoleId", "EffectiveDate");

                    b.ToTable("LegalPartySearch","search");
                });
        }
    }
}
