using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Models.V1
{

  [Table( "AsmtRevnEvent" )]
  public class AssessmentRevisionEvent
  {
    public int Id { get; set; }
    public int AsmtRevnId { get; set; }

    [Column( "AsmtEventId" )]
    public int AssessmentEventId { get; set; }
  }
}
