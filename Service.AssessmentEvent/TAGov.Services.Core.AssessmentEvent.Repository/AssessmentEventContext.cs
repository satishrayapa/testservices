using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.AssessmentEvent.Repository.Models.V1;

namespace TAGov.Services.Core.AssessmentEvent.Repository
{
  public class AssessmentEventContext : DbContext
  {
    public AssessmentEventContext( DbContextOptionsBuilder<AssessmentEventContext> optionsBuilder ) : base( optionsBuilder.Options )
    {
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
      modelBuilder.Entity<SysTypeCat>()
                  .HasKey( st => new { st.Id, begEffDate = st.BeginEffectiveDate } );
      modelBuilder.Entity<Note>()
                  .HasKey( st => new { st.Id, begEffDate = st.BeginEffectiveDate } );
    }

    public DbSet<Models.V1.AssessmentEvent> AssessmentEvent { get; set; }

    public DbSet<AssessmentEventTransaction> AssessmentEventTransaction { get; set; }

    public DbSet<ValueType> ValueType { get; set; }

    public DbSet<AssessmentEventValue> AssessmentEventValue { get; set; }

    public DbSet<SystemType> SystemType { get; set; }

    public DbSet<AssessmentRevision> AssessmentRevision { get; set; }
    public DbSet<AssessmentRevisionEvent> AssessmentRevisionEvent { get; set; }

    public DbSet<SysTypeCat> SysTypeCat { get; set; }
    public DbSet<Note> Note { get; set; }

    public DbSet<StatutoryReference> StatutoryReference { get; set; }
  }
}
