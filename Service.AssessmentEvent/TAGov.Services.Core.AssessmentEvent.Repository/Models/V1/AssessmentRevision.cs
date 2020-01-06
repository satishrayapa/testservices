using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Models.V1
{
  [Table( "AsmtRevn" )]
  public class AssessmentRevision
  {
    public int Id { get; set; }
    public string ReferenceNumber { get; set; }
    public int ValChangeReason { get; set; }

    /// <summary>
    /// This property will be set by a separate query.
    /// </summary>
    [NotMapped]
    public string ChangeReason { get; set; }

    [NotMapped]
    public string Note { get; set; }

    [Column( "RevnDate" )]
    public DateTime RevisionDate { get; set; }

    public short Active { get; set; }
  }
}
