using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "SysTypeCat" )]
  public class SysTypeCat
  {
    [Key]
    public int Id { get; set; }

    public string ShortDescr { get; set; }
    public string Descr { get; set; }
    public DateTime BegEffDate { get; set; }
    public string EffStatus { get; set; }
  }
}
