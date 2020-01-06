using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Models.V1
{
  [Table( "AsmtEvent" )]
  public class AssessmentEvent
  {
    [Key]
    public int Id { get; set; }

    public long TranId { get; set; }
    public int DynCalcStepTrackingId { get; set; }
    public int RevObjId { get; set; }
    public short TaxYear { get; set; }
    public int AsmtEventType { get; set; }

    /// <summary>
    /// This property does not map to a column in the table but is retrieved separately by at least one query.
    /// </summary>
    [NotMapped]
    public string AsmtEventTypeDescription { get; set; }

    public DateTime EventDate { get; set; }

    [ForeignKey( "AsmtEventId" )]
    public List<AssessmentEventTransaction> AssessmentEventTransactions { get; set; }

  }
}
