using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.RevenueObject.Repository.Models.V1
{
  public class SysTypeCat
  {
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "shortDescr" )]
    [StringLength( 16 )]
    [Required]
    public string ShortDescription { get; set; }

    [Column( "effStatus" )]
    [StringLength( 1 )]
    [Required]
    public string EffectiveStatus { get; set; }
  }
}
