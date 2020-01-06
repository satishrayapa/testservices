using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TAGov.Common.Security.Repository;

namespace TAGov.Common.Security.Repository.Migrations.AumentumSecurityDb
{
    [DbContext(typeof(AumentumSecurityContext))]
    [Migration("20180105174317_SeedBaseValueSegmentFlags")]
    partial class SeedBaseValueSegmentFlags
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TAGov.Common.Security.Repository.Models.AppFunction", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("App");

                    b.Property<string>("AppFunctionType")
                        .HasColumnName("AppFunctionType")
                        .HasColumnType("char(32)");

                    b.Property<int>("FieldSysType");

                    b.Property<string>("FieldValue")
                        .HasColumnName("FieldValue")
                        .HasColumnType("varchar(25)");

                    b.Property<short>("IgnoreSec");

                    b.Property<short>("IsMenuItem");

                    b.Property<string>("LongDescription")
                        .HasColumnName("LongDescr")
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasColumnType("char(64)");

                    b.Property<int>("ObjectType");

                    b.Property<int>("ParentId");

                    b.Property<string>("ParentName")
                        .HasColumnName("ParentName")
                        .HasColumnType("char(64)");

                    b.Property<int>("SecAppId");

                    b.Property<int>("SecSyncKey");

                    b.Property<long>("TransactionId")
                        .HasColumnName("TranId");

                    b.HasKey("Id");

                    b.ToTable("AppFunction");
                });
        }
    }
}
