namespace TAGov.Services.Core.BaseValueSegment.Domain.Models.V1
{
  public class BaseValueSegmentOwnerValueDto
  {
    public int? Id { get; set; }

    public int BaseValueSegmentOwnerId { get; set; }

    public int BaseValueSegmentValueHeaderId { get; set; }

    public decimal BaseValue { get; set; }

    public int DynCalcStepTrackingId { get; set; }
  }
}
