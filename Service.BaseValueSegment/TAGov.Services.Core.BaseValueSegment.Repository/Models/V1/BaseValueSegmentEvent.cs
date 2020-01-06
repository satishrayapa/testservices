using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{

  [NotMapped]
  public class BaseValueSegmentEvent
  {
    public int BvsId { get; set; }

    public DateTime BvsAsOf { get; set; }

    public int RevenueObjectId { get; set; }

    public int SequenceNumber { get; set; }

    public int? GRMEventId { get; set; }
  }
}
