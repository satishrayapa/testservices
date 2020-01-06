using System.Collections.Generic;

namespace TAGov.BaseValueSegment
{
    public class BaseValueSegmentTransactionDto
    {
        public BaseValueSegmentTransactionDto()
        {
            BaseValueSegmentOwners = new List<BaseValueSegmentOwnerDto>();
            BaseValueSegmentValueHeaders = new List<BaseValueSegmentValueHeaderDto>();
        }

        public int? Id { get; set; }

        public int BaseValueSegmentId { get; set; }

        public long TransactionId { get; set; }

        public string EffectiveStatus { get; set; }

        public int BaseValueSegmentTransactionTypeId { get; set; }

        public int? DynCalcStepTrackingId { get; set; }

        public BaseValueSegmentTransactionTypeDto BaseValueSegmentTransactionType { get; set; }

        public IList<BaseValueSegmentOwnerDto> BaseValueSegmentOwners { get; set; }

        public IList<BaseValueSegmentValueHeaderDto> BaseValueSegmentValueHeaders { get; set; }
    }
}
