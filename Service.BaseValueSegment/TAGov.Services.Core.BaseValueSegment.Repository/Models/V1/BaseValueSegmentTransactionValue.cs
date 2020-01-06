using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "BVSTranValue", Schema = "Service.BaseValueSegment" )]
  public class BaseValueSegmentTransactionValue
  {
    public int Id { get; set; }

    [Column( "BVSTranId" )]
    public int BaseValueSegmentTransactionId { get; set; }

    public int TaxYear { get; set; }

    public int ValueTypeId { get; set; }

    public int Attribute1 { get; set; }

    public int Attribute2 { get; set; }

    public int Attribute3 { get; set; }

    public int Attribute4 { get; set; }

    [Column( TypeName = "decimal(28, 10)" )]
    public decimal ValueAmount { get; set; }

    public int DynCalcStepTrackingId { get; set; }

    public virtual BaseValueSegmentTransaction BaseValueSegmentTransaction { get; set; }
  }
}
