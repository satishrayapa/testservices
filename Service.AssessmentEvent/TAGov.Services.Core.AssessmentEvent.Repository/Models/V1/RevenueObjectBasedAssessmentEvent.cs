using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Models.V1
{
  [Table( "AsmtEvent" )]
  public class RevenueObjectBasedAssessmentEvent
  {
    [Key]
    public int Id { get; set; }

    public DateTime EventDate { get; set; }

    public int RevObjId { get; set; }
  }
}
