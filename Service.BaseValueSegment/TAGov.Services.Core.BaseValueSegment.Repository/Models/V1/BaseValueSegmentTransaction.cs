using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "BVSTran", Schema = "Service.BaseValueSegment" )]
  public class BaseValueSegmentTransaction
  {
    [Key]
    [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
    public int Id { get; set; }

    [Column( "BVSId" )]
    public int BaseValueSegmentId { get; set; }

    [Column( "TranId" )]
    public long TransactionId { get; set; }

    [Required]
    [Column( "EffStatus", TypeName = "char(1)" )]
    public string EffectiveStatus { get; set; }

    [Column( "BVSTranTypeId" )]
    public int BaseValueSegmentTransactionTypeId { get; set; }

    public int? DynCalcStepTrackingId { get; set; }

    public virtual BaseValueSegmentTransactionType BaseValueSegmentTransactionType { get; set; }

    public virtual ICollection<BaseValueSegmentOwner> BaseValueSegmentOwners { get; set; }
      = new List<BaseValueSegmentOwner>();

    public virtual ICollection<BaseValueSegmentValueHeader> BaseValueSegmentValueHeaders { get; set; }
      = new List<BaseValueSegmentValueHeader>();

    public virtual BaseValueSegment BaseValueSegment { get; set; }

    public virtual ICollection<BaseValueSegmentTransactionValue> BaseValueSegmentTransactionValues { get; set; }
      = new List<BaseValueSegmentTransactionValue>();
  }
}
