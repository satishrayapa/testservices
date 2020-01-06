using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "BVSValueHeader", Schema = "Service.BaseValueSegment" )]
  public class BaseValueSegmentValueHeader
  {
    [Key]
    [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
    public int Id { get; set; }

    [Column( "BVSTranId" )]
    public int BaseValueSegmentTransactionId { get; set; }

    public int DynCalcStepTrackingId { get; set; }

    public int GRMEventId { get; set; }

    public int BaseYear { get; set; }

    public int? OriginalBVSValueHeaderId { get; set; }

    public virtual ICollection<BaseValueSegmentValue> BaseValueSegmentValues { get; set; }
      = new List<BaseValueSegmentValue>();

    public virtual ICollection<BaseValueSegmentOwnerValue> BaseValueSegmentOwnerValues { get; set; }
      = new List<BaseValueSegmentOwnerValue>();

    public virtual BaseValueSegmentTransaction BaseValueSegmentTransaction { get; set; }
  }
}
