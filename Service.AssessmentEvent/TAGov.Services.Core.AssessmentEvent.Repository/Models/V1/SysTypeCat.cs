using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Models.V1
{
  public class SysTypeCat
  {
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "ShortDescr" )]
    [Required]
    [StringLength( 16 )]
    public string ShortDescription { get; set; }

    [Column( "EffStatus" )]
    [Required]
    [StringLength( 1 )]
    public string EffectiveStatus { get; set; }
  }
}
