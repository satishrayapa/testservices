using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.RevenueObject.Repository.Models.V1
{
  public class SysType
  {
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "shortDescr" )]
    [Required]
    [StringLength( 16 )]
    public string ShortDescription { get; set; }

    [Column( "effStatus" )]
    [Required]
    [StringLength( 1 )]
    public string EffectiveStatus { get; set; }

    public int SysTypeCatId { get; set; }

    [Column( "Descr" )]
    [Required]
    [StringLength( 64 )]
    public string Description { get; set; }
  }
}
