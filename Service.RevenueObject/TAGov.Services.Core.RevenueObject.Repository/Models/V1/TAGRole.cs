using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.RevenueObject.Repository.Models.V1
{
  public class TAGRole
  {
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "EffStatus" )]
    [StringLength( 1 )]
    public string EffectiveStatus { get; set; }

    public int TAGId { get; set; }
    public int ObjectId { get; set; }
    public int ObjectType { get; set; }
  }
}
