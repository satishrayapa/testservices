using System;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1
{
  public class OwnerValueDto
  {
    public int OwnerId { get; set; }

    public int OwnerValueId { get; set; }

    public int ValueHeaderId { get; set; }

    public string EventName { get; set; }

    public DateTime? EventDate { get; set; }

    public string EventType { get; set; }

    public int BaseYear { get; set; }

    public decimal BaseValue { get; set; }

    public decimal Fbyv { get; set; }

    public bool IsOverride { get; set; }
  }
}