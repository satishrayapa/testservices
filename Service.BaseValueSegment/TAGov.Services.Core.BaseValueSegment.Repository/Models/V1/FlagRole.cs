using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "FlagRole" )]
  public class FlagRole
  {
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    public int FlagHeaderId { get; set; }

    [Column( "EffStatus" )]
    [StringLength( 1 )]
    [Required]
    public string EffectiveStatus { get; set; }

    public int ObjectId { get; set; }

    public int ObjectType { get; set; }

    [Column( "TerminationDate" )]
    public DateTime TerminationDate { get; set; }

    [Column( "StartDate" )]
    public DateTime StartDate { get; set; }

    [Column( "FlagStatus" )]
    [StringLength( 1 )]
    [Required]
    public string Status { get; set; }
  }
}
