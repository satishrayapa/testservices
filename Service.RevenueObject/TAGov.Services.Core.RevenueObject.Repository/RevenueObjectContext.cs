using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.RevenueObject.Repository.Models.V1;

namespace TAGov.Services.Core.RevenueObject.Repository.Maps.V1
{
  public class RevenueObjectContext : DbContext
  {
    public RevenueObjectContext( DbContextOptionsBuilder optionsBuilder ) : base( optionsBuilder.Options )
    {
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
      modelBuilder.Entity<DescriptionHeader>()
                  .HasKey( lpr => new { lpr.Id, lpr.BeginEffectiveDate } );

      modelBuilder.Entity<Models.V1.RevenueObject>()
                  .HasKey( lpr => new { lpr.Id, lpr.BeginEffectiveDate } );

      modelBuilder.Entity<TAG>()
                  .HasKey( lpr => new { lpr.Id, lpr.BeginEffectiveYear } );

      modelBuilder.Entity<TAGRole>()
                  .HasKey( lpr => new { lpr.Id, lpr.BeginEffectiveDate } );

      modelBuilder.Entity<SysType>()
                  .HasKey( lpr => new { lpr.Id, lpr.BeginEffectiveDate } );

      modelBuilder.Entity<SysTypeCat>()
                  .HasKey( lpr => new { lpr.Id, lpr.BeginEffectiveDate } );

      modelBuilder.Entity<SitusAddress>()
                  .HasKey( lpr => new { lpr.Id, lpr.BeginEffectiveDate } );

      modelBuilder.Entity<SitusAddressRole>()
                  .HasKey( lpr => new { lpr.Id, lpr.BeginEffectiveDate } );

      modelBuilder.Entity<RelatedRevenueObject>()
                  .HasKey( lpr => new { lpr.Id, lpr.BeginEffectiveDate } );

    }

    public DbSet<Models.V1.RevenueObject> RevenueObject { get; set; }

    public DbSet<TAG> TAG { get; set; }

    public DbSet<TAGRole> TAGRole { get; set; }

    public DbSet<SysTypeCat> SysTypeCat { get; set; }

    public DbSet<SysType> SysType { get; set; }

    public DbSet<SitusAddress> SitusAddress { get; set; }

    public DbSet<SitusAddressRole> SitusAddressRole { get; set; }

    public DbSet<MarketAndRestrictedValue> MarketAndRestrcitedValue { get; set; }

    public DbSet<DescriptionHeader> DescriptionHeader { get; set; }

    public DbSet<RelatedRevenueObject> RelatedRevenueObject { get; set; }

    public DbSet<ClassCodeMap> ClassCodeMap { get; set; }
  }
}
