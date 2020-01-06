using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [NotMapped]
  public class BaseValueSegmentConclusion
  {
    [Key]
    public int GrmEventId { get; set; }

    public DateTime ConclusionDate { get; set; }

    public string Description { get; set; }
  }
}
