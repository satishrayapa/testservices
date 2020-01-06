using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Repository
{
  public class AumentumContext : DbContext
  {
    public AumentumContext( DbContextOptions<AumentumContext> options ) :
      base( options )
    {
    }

    public DbSet<ValueType> ValueTypes { get; set; }

    public DbSet<CaliforniaConsumerPriceIndex> CaliforniaConsumerPriceIndexes { get; set; }

    public DbSet<SysTypeCat> SysTypeCats { get; set; }

    /// <summary>
    /// This represents the actual SysType table (if you need to join against it).
    /// </summary>
    public DbSet<SystemType> SystemTypes { get; set; }

    /// <summary>
    /// This is used in the context of a stored proc, rather than representing the SysType table.
    /// </summary>
    public DbSet<SysType> SysTypes { get; set; }

    public DbSet<FlagHeader> FlagHeaders { get; set; }

    public DbSet<FlagRole> FlagRoles { get; set; }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
      modelBuilder.Entity<CaliforniaConsumerPriceIndex>()
                  .HasKey( table => new { table.Id, table.BeginEffectiveYear } );

      modelBuilder.Entity<SysTypeCat>()
                  .HasKey( table => new { table.Id, table.BegEffDate } );

      modelBuilder.Entity<SystemType>()
                  .HasKey( table => new { table.Id, table.BeginEffectiveDate } );

      modelBuilder.Entity<FlagHeader>()
                  .HasKey( table => new { table.Id, table.BeginEffectiveDate } );

      modelBuilder.Entity<FlagRole>()
                  .HasKey( table => new { table.Id, table.BeginEffectiveDate } );
    }
  }
}
