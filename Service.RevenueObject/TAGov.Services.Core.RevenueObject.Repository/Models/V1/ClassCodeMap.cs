using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.RevenueObject.Repository.Models.V1
{
  [Table( "ClassCdMap" )]
  public class ClassCodeMap
  {
    [Key]
    public int Id { get; set; }

    [Column( "ClassCd" )]
    public int ClassCode { get; set; }

    public int RollType { get; set; }
  }
}
