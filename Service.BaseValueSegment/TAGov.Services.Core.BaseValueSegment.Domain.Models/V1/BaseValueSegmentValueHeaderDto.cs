using System.Collections.Generic;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Models.V1
{
  public class BaseValueSegmentValueHeaderDto
  {
    public int Id { get; set; }

    public int BaseValueSegmentTransactionId { get; set; }

    public int? GRMEventId { get; set; }

    public GrmEventCreateInformation GrmEventInformation { get; set; }

    public int BaseYear { get; set; }

    public virtual IList<BaseValueSegmentValueDto> BaseValueSegmentValues { get; set; }

    public virtual IList<BaseValueSegmentOwnerValueDto> BaseValueSegmentOwnerValues { get; set; }
  }
}
