﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using TAGov.Common.Security.Repository;

namespace TAGov.Common.Security.Repository.Migrations.PersistedGrantDb
{
    [DbContext(typeof(ProxyPersistedGrantDbContext))]
    partial class ProxyPersistedGrantDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(200);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreationTime");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(50000);

                    b.Property<DateTime?>("Expiration");

                    b.Property<string>("SubjectId")
                        .HasMaxLength(200);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Key");

                    b.HasIndex("SubjectId", "ClientId", "Type");

                    b.ToTable("PersistedGrants");
                });
#pragma warning restore 612, 618
        }
    }
}
