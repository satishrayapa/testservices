using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.RevenueObject.Repository.Models.V1
{
  [Table( "SitusAddr" )]
  public class SitusAddress
  {
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "effStatus" )]
    [StringLength( 1 )]
    [Required]
    public string EffectiveStatus { get; set; }

    [Column( "FreeFormAddr" )]
    [StringLength( 64 )]
    [Required]
    public string FreeFormAddress { get; set; }

    [StringLength( 32 )]
    [Required]
    public string City { get; set; }

    [Column( "State" )]
    [StringLength( 4 )]
    [Required]
    public string StateCode { get; set; }

    [Column( "PostalCd" )]
    [StringLength( 16 )]
    [Required]
    public string PostalCode { get; set; }
  }
}
