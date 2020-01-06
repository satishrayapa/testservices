using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.GrmEvent.Repository.Models.V1
{
  public class GrmEventInformation
  {
    [Key]
    [Column( "GRMEventId" )]
    public int GrmEventId { get; set; }

    [Column( "Descr" )]
    public string Description { get; set; }

    public DateTime EffectiveDate { get; set; }

    public int RevenueObjectId { get; set; }

    public string EventType { get; set; }

    public DateTime EventDate { get; set; }
  }
}