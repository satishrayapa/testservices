using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.LegalParty.Repository.Models;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;

namespace TAGov.Services.Core.LegalParty.Repository
{
  public class LegalPartyContext : DbContext
  {
    public LegalPartyContext( DbContextOptionsBuilder optionsBuilder ) : base( optionsBuilder.Options )
    {
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
      modelBuilder.Entity<SysTypeCat>()
                  .HasKey( st => new { st.Id, begEffDate = st.BeginEffectiveDate } );

      modelBuilder.Entity<LegalPartyRole>()
                  .HasKey( lpr => new { lpr.Id, lpr.BegEffDate } );
      modelBuilder.Entity<SystemType>()
                  .HasKey( st => new { st.Id, begEffDate = st.BegEffDate } );
      modelBuilder.Entity<LegalPartyDocument>()
                  .HasKey( cr => new { cr.LegalPartyRoleId, cr.RightTransferId } );
      modelBuilder.Entity<RightHistory>()
                  .HasKey( c => new { c.Id, c.BeginEffectiveDate } );
      modelBuilder.Entity<RightTransfer>()
                  .HasKey( c => new { c.Id, c.BeginEffectiveDate } );
      modelBuilder.Entity<OfficialDocument>()
                  .HasKey( c => new { c.Id, c.BeginEffectiveDate } );
    }

    public DbSet<Models.V1.LegalParty> LegalParty { get; set; }

    public DbSet<LegalPartyRole> LegalPartyRole { get; set; }

    public DbSet<LegalPartyDocument> LegalPartyDocument { get; set; }

    public DbSet<SystemType> SystemType { get; set; }

    public DbSet<SysTypeCat> SystemTypeCat { get; set; }

    public DbSet<RightHistory> RightHistories { get; set; }

    public DbSet<RightTransfer> RightTransfers { get; set; }

    public DbSet<OfficialDocument> OfficialDocuments { get; set; }

    public DbSet<GrmEventArtifact> GrmEventArtifacts { get; set; }

    public DbSet<GrmEvent> GrmEvents { get; set; }
  }
}