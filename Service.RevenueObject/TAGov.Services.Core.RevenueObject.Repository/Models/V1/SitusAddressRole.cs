using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.RevenueObject.Repository.Models.V1
{
  [Table( "SitusAddrRole" )]
  public class SitusAddressRole
  {
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "EffStatus" )]
    [StringLength( 1 )]
    [Required]
    public string EffectiveStatus { get; set; }

    [Column( "SitusAddrId" )]
    public int SitusAddressId { get; set; }

    public int ObjectType { get; set; }
    public int ObjectId { get; set; }
    public short PrimeAddr { get; set; }
  }
}
