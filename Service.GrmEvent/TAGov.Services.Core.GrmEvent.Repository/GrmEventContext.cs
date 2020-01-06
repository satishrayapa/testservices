using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.GrmEvent.Repository.Models.V1;

namespace TAGov.Services.Core.GrmEvent.Repository
{
  public class GrmEventContext : DbContext
  {
    public GrmEventContext( DbContextOptionsBuilder optionsBuilder ) : base( optionsBuilder.Options )
    {
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
      modelBuilder.Entity<SystemType>()
                  .HasKey( st => new { st.Id, st.begEffDate } );
    }

    public DbSet<Models.V1.GrmEvent> GrmEvent { get; set; }

    public DbSet<SystemType> SystemType { get; set; }

    public DbSet<GrmEventInformation> GrmEventInformation { get; set; }

    public DbSet<SubComponentValue> SubComponentValues { get; set; }

    public DbSet<GrmEventGroup> GrmEventGroups { get; set; }

    public DbSet<TransactionHeader> TransactionHeaders { get; set; }

    public DbSet<TransactionDetail> TransactionDetails { get; set; }

  }
}