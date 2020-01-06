using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Models.V1
{
  public class Note
  {
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "EffStatus" )]
    [Required]
    [StringLength( 1 )]
    public string EffectiveStatus { get; set; }

    public int ObjectType { get; set; }
    public int ObjectId { get; set; }
    public int Privacy { get; set; }

    [Column( "noteText" )]
    [Required]
    [StringLength( 4000 )]
    public string NoteText { get; set; }
  }
}
