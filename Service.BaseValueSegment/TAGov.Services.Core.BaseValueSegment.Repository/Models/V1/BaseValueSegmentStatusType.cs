using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "BVSStatusType", Schema = "Service.BaseValueSegment" )]
  public class BaseValueSegmentStatusType
  {
    [Key]
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Required]
    [Column( TypeName = "varchar(50)" )]
    public string Name { get; set; }

    [Column( TypeName = "varchar(100)" )]
    public string Description { get; set; }

    public ICollection<AssessmentRevisionBaseValueSegment> AssessmentRevisionBaseValueSegments { get; set; }
      = new List<AssessmentRevisionBaseValueSegment>();
  }
}
