using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.LegalParty.Repository.Models.V1
{
  [Table( "RightHist" )]
  public class RightHistory
  {
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "LPRBegEffDate" )]
    public DateTime LegalPartyRoleBeginEffectiveDate { get; set; }

    [Column( "EffStatus" )]
    [Required]
    [StringLength( 1 )]
    public string EffectiveStatus { get; set; }

    public short GrantorGrantee { get; set; }

    [Column( "LPRId" )]
    public int LegalPartyRoleId { get; set; }

    public int RightTransferId { get; set; }
  }
}