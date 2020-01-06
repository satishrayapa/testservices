using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Models.V1
{
  [Table("AsmtEventValue")]
  public class AssessmentEventValue
  {
    public long Id { get; set; }
    public int AsmtEventTranId { get; set; }
    public int DynCalcStepTrackingId { get; set; }
    public int ValueTypeId { get; set; }
    public short TaxYear { get; set; }
    public int Attribute1 { get; set; }
    public int Attribute2 { get; set; }
    public decimal ValueAmount { get; set; }

    [ForeignKey("ValueTypeId")]
    public ValueType ValueType { get; set; }

    /// <summary>
    /// This column is set via a join operation.
    /// </summary>
    [NotMapped]
    public string Attribute2Description { get; set; }
  }
}
