using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Repository
{
  /// <summary>
  /// Don't use this class for EF Migrations--too many tables will be created.
  /// </summary>
  public class BaseValueSegmentQueryContext : BaseValueSegmentContext
  {
    public BaseValueSegmentQueryContext( DbContextOptions<BaseValueSegmentQueryContext> options ) :
      base( options )
    {
    }

    public DbSet<SubComponentDetail> SubComponentDetails { get; set; }

    public DbSet<BeneficialInterestInfo> BeneficialInterests { get; set; }

    public DbSet<BaseValueSegmentConclusion> BaseValueSegmentConclusions { get; set; }

    public DbSet<BaseValueSegmentHistory> BaseValueSegmentHistories { get; set; }
  }
}