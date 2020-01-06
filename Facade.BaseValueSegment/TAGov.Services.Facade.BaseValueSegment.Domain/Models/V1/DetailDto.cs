using System;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1
{
  public class DetailDto
  {
    public string BeneficialInterest { get; set; }

    public string OwnershipEventName { get; set; }

    public DateTime? OwnershipEventDate { get; set; }

    public string OwnershipEventType { get; set; }

    public string DocNumber { get; set; }

    public string DocType { get; set; }

    public decimal? PercentageInterestGain { get; set; }

    public decimal BiPercentage { get; set; }

    public string BaseValueSegmentEventName { get; set; }

    public DateTime? BaseValueSegmentEventDate { get; set; }

    public string BaseValueSegmentEventType { get; set; }

    public string ComponentName { get; set; }

    public string SubComponentName { get; set; }

    public int BaseYear { get; set; }

    public bool? OwnerIsOverride { get; set; }

    public bool? ComponentIsOverride { get; set; }
    public int? OwnerId { get; set; }
    public int SubComponentId { get; set; }
    public DateTime AssessmentEventDate { get; set; }

    public int ValueHeaderId { get; set; }
  }
}