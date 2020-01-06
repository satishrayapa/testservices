using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.RevenueObject.Repository.Models.V1
{
  public class TAG
  {
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Column( "BegEffYear" )]
    public short BeginEffectiveYear { get; set; }

    [Column( "Descr" )]
    [StringLength( 64 )]
    [Required]
    public string Description { get; set; }

    [Column( "EffStatus" )]
    [StringLength( 1 )]
    public string EffectiveStatus { get; set; }
  }
}
