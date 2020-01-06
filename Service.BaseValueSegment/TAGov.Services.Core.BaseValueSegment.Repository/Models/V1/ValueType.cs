using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "ValueType" )]
  public class ValueType
  {
    [Key]
    public int Id { get; set; }

    public string ShortDescr { get; set; }
    public string Descr { get; set; }
  }
}
