﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TAGov.Services.Core.BaseValueSegment.Repository;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    [DbContext(typeof(BaseValueSegmentContext))]
    [Migration("20180914150429_AddIndexAsmtRevnBVS")]
    partial class AddIndexAsmtRevnBVS
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.AssessmentRevisionBaseValueSegment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AssessmentRevisionId")
                        .HasColumnName("AsmtRevnId");

                    b.Property<int?>("BaseValueSegmentId")
                        .HasColumnName("BVSId");

                    b.Property<int>("BaseValueSegmentStatusTypeId")
                        .HasColumnName("BVSStatusTypeId");

                    b.Property<int>("DynCalcStepTrackingId");

                    b.Property<string>("ReviewMessage")
                        .IsRequired()
                        .HasColumnType("varchar(1024)");

                    b.HasKey("Id");

                    b.HasIndex("BaseValueSegmentId");

                    b.HasIndex("BaseValueSegmentStatusTypeId");

                    b.HasIndex("AssessmentRevisionId", "BaseValueSegmentId");

                    b.ToTable("AsmtRevnBVS","Service.BaseValueSegment");
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AsOf")
                        .HasColumnType("date");

                    b.Property<int>("DynCalcInstanceId");

                    b.Property<int>("DynCalcStepTrackingId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("RevenueObjectId")
                        .HasColumnName("RevObjId");

                    b.Property<int>("SequenceNumber");

                    b.Property<long>("TransactionId")
                        .HasColumnName("TranId");

                    b.HasKey("Id");

                    b.HasIndex("DynCalcStepTrackingId");

                    b.HasIndex("RevenueObjectId");

                    b.HasIndex("AsOf", "RevenueObjectId", "SequenceNumber")
                        .IsUnique();

                    b.ToTable("BVS","Service.BaseValueSegment");
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentOwner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AlphaBVSOwnerId");

                    b.Property<int>("BaseValueSegmentTransactionId")
                        .HasColumnName("BVSTranId");

                    b.Property<decimal>("BeneficialInterestPercent")
                        .HasColumnName("BIPercent")
                        .HasColumnType("decimal(28, 10)");

                    b.Property<int>("DynCalcStepTrackingId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("GRMEventId");

                    b.Property<bool?>("IsUserOverride");

                    b.Property<int>("LegalPartyRoleId");

                    b.HasKey("Id");

                    b.HasIndex("AlphaBVSOwnerId");

                    b.HasIndex("BaseValueSegmentTransactionId");

                    b.HasIndex("DynCalcStepTrackingId");

                    b.ToTable("BVSOwner","Service.BaseValueSegment");
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentOwnerValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("BaseValue")
                        .HasColumnType("decimal(28, 10)");

                    b.Property<int>("BaseValueSegmentOwnerId")
                        .HasColumnName("BVSOwnerId");

                    b.Property<int>("BaseValueSegmentValueHeaderId")
                        .HasColumnName("BVSValueHeaderId");

                    b.Property<int>("DynCalcStepTrackingId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("BaseValueSegmentValueHeaderId");

                    b.HasIndex("DynCalcStepTrackingId");

                    b.HasIndex("BaseValueSegmentOwnerId", "DynCalcStepTrackingId");

                    b.ToTable("BVSOwnerValue","Service.BaseValueSegment");
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentStatusType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("BVSStatusType","Service.BaseValueSegment");
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BaseValueSegmentId")
                        .HasColumnName("BVSId");

                    b.Property<int>("BaseValueSegmentTransactionTypeId")
                        .HasColumnName("BVSTranTypeId");

                    b.Property<int?>("DynCalcStepTrackingId");

                    b.Property<string>("EffectiveStatus")
                        .IsRequired()
                        .HasColumnName("EffStatus")
                        .HasColumnType("char(1)");

                    b.Property<long>("TransactionId")
                        .HasColumnName("TranId");

                    b.HasKey("Id");

                    b.HasIndex("BaseValueSegmentId");

                    b.HasIndex("BaseValueSegmentTransactionTypeId");

                    b.HasIndex("DynCalcStepTrackingId");

                    b.ToTable("BVSTran","Service.BaseValueSegment");
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentTransactionType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("BVSTranType","Service.BaseValueSegment");
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentTransactionValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Attribute1")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("Attribute2")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("Attribute3")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("Attribute4")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("BaseValueSegmentTransactionId")
                        .HasColumnName("BVSTranId");

                    b.Property<int>("DynCalcStepTrackingId");

                    b.Property<int>("TaxYear");

                    b.Property<decimal>("ValueAmount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(28, 10)")
                        .HasDefaultValue(1m);

                    b.Property<int>("ValueTypeId");

                    b.HasKey("Id");

                    b.HasIndex("DynCalcStepTrackingId");

                    b.HasIndex("BaseValueSegmentTransactionId", "ValueTypeId");

                    b.HasIndex("BaseValueSegmentTransactionId", "ValueTypeId", "Attribute1", "Attribute2", "Attribute3", "Attribute4");

                    b.ToTable("BVSTranValue","Service.BaseValueSegment");
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BaseValueSegmentValueHeaderId")
                        .HasColumnName("BVSValueHeaderId");

                    b.Property<int>("DynCalcStepTrackingId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<decimal>("FullValueAmount")
                        .HasColumnType("decimal(28, 10)");

                    b.Property<bool?>("IsUserOverride");

                    b.Property<decimal>("PercentComplete")
                        .HasColumnName("PctComplete")
                        .HasColumnType("decimal(14, 10)");

                    b.Property<int>("SubComponent");

                    b.Property<decimal>("ValueAmount")
                        .HasColumnType("decimal(28, 10)");

                    b.HasKey("Id");

                    b.HasIndex("BaseValueSegmentValueHeaderId");

                    b.HasIndex("DynCalcStepTrackingId");

                    b.ToTable("BVSValue","Service.BaseValueSegment");
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentValueHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BaseValueSegmentTransactionId")
                        .HasColumnName("BVSTranId");

                    b.Property<int>("BaseYear");

                    b.Property<int>("DynCalcStepTrackingId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("GRMEventId");

                    b.Property<int?>("OriginalBVSValueHeaderId");

                    b.HasKey("Id");

                    b.HasIndex("BaseValueSegmentTransactionId");

                    b.HasIndex("DynCalcStepTrackingId");

                    b.HasIndex("OriginalBVSValueHeaderId");

                    b.ToTable("BVSValueHeader","Service.BaseValueSegment");
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.AssessmentRevisionBaseValueSegment", b =>
                {
                    b.HasOne("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegment", "BaseValueSegment")
                        .WithMany("BaseValueSegmentAssessmentRevisions")
                        .HasForeignKey("BaseValueSegmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentStatusType", "BaseValueSegmentStatusType")
                        .WithMany("AssessmentRevisionBaseValueSegments")
                        .HasForeignKey("BaseValueSegmentStatusTypeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentOwner", b =>
                {
                    b.HasOne("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentTransaction", "BaseValueSegmentTransaction")
                        .WithMany("BaseValueSegmentOwners")
                        .HasForeignKey("BaseValueSegmentTransactionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentOwnerValue", b =>
                {
                    b.HasOne("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentOwner", "Owner")
                        .WithMany("BaseValueSegmentOwnerValueValues")
                        .HasForeignKey("BaseValueSegmentOwnerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentValueHeader", "Header")
                        .WithMany("BaseValueSegmentOwnerValues")
                        .HasForeignKey("BaseValueSegmentValueHeaderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentTransaction", b =>
                {
                    b.HasOne("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegment", "BaseValueSegment")
                        .WithMany("BaseValueSegmentTransactions")
                        .HasForeignKey("BaseValueSegmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentTransactionType", "BaseValueSegmentTransactionType")
                        .WithMany("BaseValueSegmentTransactions")
                        .HasForeignKey("BaseValueSegmentTransactionTypeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentTransactionValue", b =>
                {
                    b.HasOne("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentTransaction", "BaseValueSegmentTransaction")
                        .WithMany("BaseValueSegmentTransactionValues")
                        .HasForeignKey("BaseValueSegmentTransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentValue", b =>
                {
                    b.HasOne("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentValueHeader", "BaseValueSegmentValueHeader")
                        .WithMany("BaseValueSegmentValues")
                        .HasForeignKey("BaseValueSegmentValueHeaderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentValueHeader", b =>
                {
                    b.HasOne("TAGov.Services.Core.BaseValueSegment.Repository.Models.V1.BaseValueSegmentTransaction", "BaseValueSegmentTransaction")
                        .WithMany("BaseValueSegmentValueHeaders")
                        .HasForeignKey("BaseValueSegmentTransactionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
