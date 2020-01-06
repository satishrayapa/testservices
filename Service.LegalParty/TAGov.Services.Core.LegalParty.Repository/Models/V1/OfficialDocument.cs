using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.LegalParty.Repository.Models.V1
{
  [Table( "OfficialDoc" )]
  public class OfficialDocument
  {
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "EffStatus" )]
    [Required]
    [StringLength( 1 )]
    public string EffectiveStatus { get; set; }

    [Column( "DocDate" )]
    public DateTime DocumentDate { get; set; }

    [Column( "DocNumber" )]
    [Required]
    [StringLength( 32 )]
    public string DocumentNumber { get; set; }

    [Column( "DocType" )]
    public int DocumentType { get; set; }
  }
}