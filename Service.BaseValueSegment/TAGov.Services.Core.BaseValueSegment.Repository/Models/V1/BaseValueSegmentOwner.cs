using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "BVSOwner", Schema = "Service.BaseValueSegment" )]
  public class BaseValueSegmentOwner
  {
    [Key]
    [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
    public int Id { get; set; }

    [Column( "BVSTranId" )]
    public int BaseValueSegmentTransactionId { get; set; }

    public int LegalPartyRoleId { get; set; }

    [Column( "BIPercent", TypeName = "decimal(28, 10)" )]
    public decimal BeneficialInterestPercent { get; set; }

    public int DynCalcStepTrackingId { get; set; }

    public int GRMEventId { get; set; }

    public int? AlphaBVSOwnerId { get; set; }

    public virtual ICollection<BaseValueSegmentOwnerValue> BaseValueSegmentOwnerValueValues { get; set; }
      = new List<BaseValueSegmentOwnerValue>();

    public virtual BaseValueSegmentTransaction BaseValueSegmentTransaction { get; set; }

    public bool? IsUserOverride { get; set; }
  }
}
