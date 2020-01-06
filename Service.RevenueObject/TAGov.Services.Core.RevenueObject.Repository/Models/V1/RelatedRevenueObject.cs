using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.RevenueObject.Repository.Models.V1
{
  [Table( "RelRevObj" )]
  public class RelatedRevenueObject
  {
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "EffStatus" )]
    [StringLength( 1 )]
    [Required]
    public string EffectiveStatus { get; set; }

    [Column( "RevObj1Id" )]
    public int RevenueObject1Id { get; set; }

    [Column( "RevObj2Id" )]
    public int RevenueObject2Id { get; set; }
  }
}
