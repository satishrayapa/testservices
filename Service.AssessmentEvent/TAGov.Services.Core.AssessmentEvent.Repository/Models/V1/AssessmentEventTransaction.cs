using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Models.V1
{
  [Table( "AsmtEventTran" )]
  public class AssessmentEventTransaction
  {
    [Key]
    public int Id { get; set; }

    public int AsmtEventId { get; set; }
    public int AsmtEventState { get; set; }

    /// <summary>
    /// This property does not map to a column in the table but is retrieved separately by at least one query.
    /// </summary>
    [NotMapped]
    public string AsmtEventStateDescription { get; set; }

    public int AsmtRevnEventId { get; set; }
  }
}
