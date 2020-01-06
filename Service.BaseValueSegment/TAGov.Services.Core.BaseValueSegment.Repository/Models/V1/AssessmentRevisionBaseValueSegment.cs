using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "AsmtRevnBVS", Schema = "Service.BaseValueSegment" )]
  public class AssessmentRevisionBaseValueSegment
  {
    [Key]
    [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
    public int Id { get; set; }

    /// <summary>
    /// This is a foreign key back to the existing table dbo.AsmtRevn.
    /// </summary>
    [Column( "AsmtRevnId" )]
    public int AssessmentRevisionId { get; set; }

    [Column( "BVSId" )]
    public int? BaseValueSegmentId { get; set; }

    public int DynCalcStepTrackingId { get; set; }

    [Column( "BVSStatusTypeId" )]
    public int BaseValueSegmentStatusTypeId { get; set; }

    [Required]
    [Column( TypeName = "varchar(1024)" )]
    public string ReviewMessage { get; set; }

    public virtual BaseValueSegment BaseValueSegment { get; set; }

    public virtual BaseValueSegmentStatusType BaseValueSegmentStatusType { get; set; }
  }
}
