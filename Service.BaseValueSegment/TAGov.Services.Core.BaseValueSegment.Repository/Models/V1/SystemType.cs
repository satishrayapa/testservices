using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  public class SysType
  {
    public int Id { get; set; }
  }

  [Table( "SysType" )]
  public class SystemType
  {
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "Descr" )]
    [StringLength( 64 )]
    [Required]
    public string Description { get; set; }

    [Column( "EffStatus" )]
    [StringLength( 1 )]
    [Required]
    public string EffectiveStatus { get; set; }

    public string ShortDescr { get; set; }

    public int SysTypeCatId { get; set; }
  }
}
