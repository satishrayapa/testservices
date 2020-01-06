using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TAGov.Common.ResourceLocator.Repository;

namespace TAGov.Common.ResourceLocator.Repository.Migrations
{
    [DbContext(typeof(ResourceContext))]
    [Migration("20170825133625_SeedResources")]
    partial class SeedResources
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TAGov.Common.ResourceLocator.Repository.Models.V1.Resource", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(200);

                    b.Property<string>("Partition")
                        .HasMaxLength(200);

                    b.Property<string>("Value")
                        .HasMaxLength(1000);

                    b.HasKey("Key", "Partition");

                    b.ToTable("Resource","Common.Resource");
                });
        }
    }
}
