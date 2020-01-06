using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.AssessmentEvent.Repository.Models.V1;

namespace TAGov.Services.Core.AssessmentEvent.Repository
{
  public class QueryOnlyAssessmentEventContext : DbContext
  {
    public QueryOnlyAssessmentEventContext( DbContextOptionsBuilder<QueryOnlyAssessmentEventContext> optionsBuilder ) : base( optionsBuilder.Options )
    {
    }

    public DbSet<RevenueObjectBasedAssessmentEvent> RevenueObjectBasedAssessmentEvents { get; set; }
  }
}
