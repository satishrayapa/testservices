using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "BVS", Schema = "Service.BaseValueSegment" )]
  public class BaseValueSegment
  {
    [Key]
    [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
    public int Id { get; set; }

    [Column( TypeName = "date" )]
    public DateTime AsOf { get; set; }

    [Column( "RevObjId" )]
    public int RevenueObjectId { get; set; }

    public int SequenceNumber { get; set; }

    [Column( "TranId" )]
    public long TransactionId { get; set; }

    public int DynCalcInstanceId { get; set; }

    public int DynCalcStepTrackingId { get; set; }

    public virtual ICollection<BaseValueSegmentTransaction> BaseValueSegmentTransactions { get; set; }
      = new List<BaseValueSegmentTransaction>();

    public virtual ICollection<AssessmentRevisionBaseValueSegment> BaseValueSegmentAssessmentRevisions { get; set; }
      = new List<AssessmentRevisionBaseValueSegment>();
  }
}
