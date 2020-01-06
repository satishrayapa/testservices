using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Repository
{
  public class BaseValueSegmentContext : DbContext
  {
    public const string BaseValueSegmentConnectionStringEnvironmentVariable = "service.basevaluesegment.connectionString";

    private const string DefaultConnectionString = "Data Source=localhost;Database=Aumentum;Trusted_Connection=True;";
    private readonly bool _useInternal;

    public BaseValueSegmentContext()
    {
      // Parameterless constructor for EF migrations to work.
      _useInternal = true;
    }

    public BaseValueSegmentContext( DbContextOptionsBuilder<BaseValueSegmentContext> optionsBuilder ) : base( optionsBuilder.Options )
    {
    }

    public BaseValueSegmentContext( DbContextOptions options ) :
      base( options )
    {
    }

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
      if ( _useInternal )
      {
        var connectionString = Environment.GetEnvironmentVariable( BaseValueSegmentConnectionStringEnvironmentVariable ) ?? DefaultConnectionString;
        optionsBuilder.UseSqlServer( connectionString.Replace( "\"", "" ) );
      }

      base.OnConfiguring( optionsBuilder );
    }

    /// <summary>
    /// Called from query contexts so SQL generation from LINQ will have the full benefit of the model.
    /// </summary>
    /// <param name="modelBuilder">model</param>
    public static void EntityFrameworkMigrationsOnModelCreating( ModelBuilder modelBuilder )
    {
      modelBuilder.Entity<AssessmentRevisionBaseValueSegment>(
        entity =>
        {
          entity.HasOne( d => d.BaseValueSegment )
                .WithMany( p => p.BaseValueSegmentAssessmentRevisions )
                .HasForeignKey( d => d.BaseValueSegmentId )
                .OnDelete( DeleteBehavior.Restrict );

          entity.HasOne( d => d.BaseValueSegmentStatusType )
                .WithMany( p => p.AssessmentRevisionBaseValueSegments )
                .HasForeignKey( d => d.BaseValueSegmentStatusTypeId )
                .OnDelete( DeleteBehavior.Restrict );

          entity.HasIndex( e => e.BaseValueSegmentId );

          entity.HasIndex( e => e.BaseValueSegmentStatusTypeId );

          entity.HasIndex( e => new { e.AssessmentRevisionId, e.BaseValueSegmentId } );
        } );

      modelBuilder.Entity<Models.V1.BaseValueSegment>(
        entity =>
        {
          entity.HasIndex( e => new { e.AsOf, e.RevenueObjectId, e.SequenceNumber } )
                .IsUnique();

          entity.HasIndex( e => e.DynCalcStepTrackingId );

          entity.Property( x => x.DynCalcStepTrackingId ).HasDefaultValue( 0 );

          entity.HasIndex( e => e.RevenueObjectId );
        } );

      modelBuilder.Entity<BaseValueSegmentOwner>(
        entity =>
        {
          entity.HasOne( d => d.BaseValueSegmentTransaction )
                .WithMany( p => p.BaseValueSegmentOwners )
                .HasForeignKey( d => d.BaseValueSegmentTransactionId )
                .OnDelete( DeleteBehavior.Restrict );

          // Note, self referencing foreign key AlphaBVSOwnerId created manually.
          // Did not want to expand EF to fully include here as not used in code,
          // just in custom calcs.

          entity.HasIndex( e => e.BaseValueSegmentTransactionId );

          entity.Property( x => x.DynCalcStepTrackingId ).HasDefaultValue( 0 );

          entity.HasIndex( e => e.DynCalcStepTrackingId );
          entity.HasIndex( e => e.AlphaBVSOwnerId );
        } );

      modelBuilder.Entity<BaseValueSegmentOwnerValue>(
        entity =>
        {
          entity.HasOne( d => d.Owner )
                .WithMany( p => p.BaseValueSegmentOwnerValueValues )
                .HasForeignKey( d => d.BaseValueSegmentOwnerId )
                .OnDelete( DeleteBehavior.Restrict );

          entity.HasOne( d => d.Header )
                .WithMany( p => p.BaseValueSegmentOwnerValues )
                .HasForeignKey( d => d.BaseValueSegmentValueHeaderId )
                .OnDelete( DeleteBehavior.Restrict );

          entity.HasIndex( e => e.BaseValueSegmentValueHeaderId );


          entity.HasIndex( e => new { e.BaseValueSegmentOwnerId, e.DynCalcStepTrackingId } );

          entity.Property( x => x.DynCalcStepTrackingId ).HasDefaultValue( 0 );

          entity.HasIndex( e => e.DynCalcStepTrackingId );

        } );

      modelBuilder.Entity<BaseValueSegmentTransaction>(
        entity =>
        {
          entity.HasOne( d => d.BaseValueSegment )
                .WithMany( p => p.BaseValueSegmentTransactions )
                .HasForeignKey( d => d.BaseValueSegmentId )
                .OnDelete( DeleteBehavior.Restrict );

          entity.HasOne( d => d.BaseValueSegmentTransactionType )
                .WithMany( p => p.BaseValueSegmentTransactions )
                .HasForeignKey( d => d.BaseValueSegmentTransactionTypeId )
                .OnDelete( DeleteBehavior.Restrict );

          entity.HasIndex( e => e.BaseValueSegmentId );

          entity.HasIndex( e => e.BaseValueSegmentTransactionTypeId );

          entity.HasIndex( e => e.DynCalcStepTrackingId );
        } );

      modelBuilder.Entity<BaseValueSegmentValue>(
        entity =>
        {
          entity.HasOne( d => d.BaseValueSegmentValueHeader )
                .WithMany( p => p.BaseValueSegmentValues )
                .HasForeignKey( d => d.BaseValueSegmentValueHeaderId )
                .OnDelete( DeleteBehavior.Restrict );

          entity.HasIndex( e => e.BaseValueSegmentValueHeaderId );

          entity.Property( x => x.DynCalcStepTrackingId ).HasDefaultValue( 0 );

          entity.HasIndex( e => e.DynCalcStepTrackingId );
        } );

      modelBuilder.Entity<BaseValueSegmentValueHeader>(
        entity =>
        {
          entity.HasOne( d => d.BaseValueSegmentTransaction )
                .WithMany( p => p.BaseValueSegmentValueHeaders )
                .HasForeignKey( d => d.BaseValueSegmentTransactionId )
                .OnDelete( DeleteBehavior.Restrict );

          // Note, self referencing foreign key OriginalBVSValueHeaderId created manually.
          // Did not want to expand EF to fully include here as not used in code,
          // just in custom calcs.

          entity.HasIndex( e => e.BaseValueSegmentTransactionId );

          entity.Property( x => x.DynCalcStepTrackingId ).HasDefaultValue( 0 );

          entity.HasIndex( e => e.DynCalcStepTrackingId );
          entity.HasIndex( e => e.OriginalBVSValueHeaderId );
        } );

      modelBuilder.Entity<BaseValueSegmentTransactionValue>(
        entity =>
        {
          entity.HasOne( d => d.BaseValueSegmentTransaction )
                .WithMany( p => p.BaseValueSegmentTransactionValues )
                .HasForeignKey( d => d.BaseValueSegmentTransactionId )
                .OnDelete( DeleteBehavior.Cascade );

          entity.HasIndex( e => new { e.BaseValueSegmentTransactionId, e.ValueTypeId } );
          entity.HasIndex( e => new { e.BaseValueSegmentTransactionId, e.ValueTypeId, e.Attribute1, e.Attribute2, e.Attribute3, e.Attribute4 } );
          entity.HasIndex( e => e.DynCalcStepTrackingId );

          entity.Property( x => x.Attribute1 ).HasDefaultValue( 0 );
          entity.Property( x => x.Attribute2 ).HasDefaultValue( 0 );
          entity.Property( x => x.Attribute3 ).HasDefaultValue( 0 );
          entity.Property( x => x.Attribute4 ).HasDefaultValue( 0 );
          entity.Property( x => x.ValueAmount ).HasDefaultValue( 1 );
        } );

    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
      EntityFrameworkMigrationsOnModelCreating( modelBuilder );
    }

    public DbSet<Models.V1.BaseValueSegment> BaseValueSegments { get; set; }
    public DbSet<BaseValueSegmentTransaction> BaseValueSegmentTransactions { get; set; }
    public DbSet<BaseValueSegmentTransactionType> BaseValueSegmentTransactionTypes { get; set; }
    public DbSet<BaseValueSegmentOwner> BaseValueSegmentOwners { get; set; }
    public DbSet<BaseValueSegmentValueHeader> BaseValueSegmentValueHeaders { get; set; }
    public DbSet<BaseValueSegmentValue> BaseValueSegmentValues { get; set; }
    public DbSet<BaseValueSegmentOwnerValue> BaseValueSegmentOwnerValues { get; set; }
    public DbSet<AssessmentRevisionBaseValueSegment> BaseValueSegmentAssessmentRevisions { get; set; }

    public DbSet<BaseValueSegmentStatusType> BaseValueSegmentStatusTypes { get; set; }
  }
}
