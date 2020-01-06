using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "BVSOwnerValue", Schema = "Service.BaseValueSegment" )]
  public class BaseValueSegmentOwnerValue
  {
    [Key]
    [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
    public int Id { get; set; }

    [Column( "BVSOwnerId" )]
    public int BaseValueSegmentOwnerId { get; set; }

    [Column( "BVSValueHeaderId" )]
    public int BaseValueSegmentValueHeaderId { get; set; }

    [Column( TypeName = "decimal(28, 10)" )]
    public decimal BaseValue { get; set; }

    public int DynCalcStepTrackingId { get; set; }

    public virtual BaseValueSegmentOwner Owner { get; set; }

    public virtual BaseValueSegmentValueHeader Header { get; set; }
  }
}
