using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.LegalParty.Repository.Models.V1
{
  [Table( "RightTransfer" )]
  public class RightTransfer
  {
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "EffStatus" )]
    [Required]
    [StringLength( 1 )]
    public string EffectiveStatus { get; set; }

    [Column( "OfficialDocId" )]
    public int OfficialDocumentId { get; set; }
  }
}