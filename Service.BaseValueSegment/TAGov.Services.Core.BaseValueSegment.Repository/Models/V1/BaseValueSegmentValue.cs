using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "BVSValue", Schema = "Service.BaseValueSegment" )]
  public class BaseValueSegmentValue
  {
    [Key]
    [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
    public int Id { get; set; }

    [Column( "BVSValueHeaderId" )]
    public int BaseValueSegmentValueHeaderId { get; set; }

    public int SubComponent { get; set; }

    [Column( TypeName = "decimal(28, 10)" )]
    public decimal ValueAmount { get; set; }

    [Column( "PctComplete", TypeName = "decimal(14, 10)" )]
    public decimal PercentComplete { get; set; }

    [Column( TypeName = "decimal(28, 10)" )]
    public decimal FullValueAmount { get; set; }

    public int DynCalcStepTrackingId { get; set; }

    public virtual BaseValueSegmentValueHeader BaseValueSegmentValueHeader { get; set; }

    public bool? IsUserOverride { get; set; }
  }
}
