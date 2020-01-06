using System;
using System.Collections.Generic;

namespace TAGov.BaseValueSegment
{
    public class BaseValueSegmentDto
    {
        public BaseValueSegmentDto()
        {
            BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>();
        }

        public int? Id { get; set; }

        public DateTime AsOf { get; set; }

        public int RevenueObjectId { get; set; }

        public int SequenceNumber { get; set; }

        public long TransactionId { get; set; }

        public int DynCalcInstanceId { get; set; }

        public int AssessmentEventTransactionId { get; set; }

        public IList<BaseValueSegmentTransactionDto> BaseValueSegmentTransactions { get; set; }

        public IList<AssessmentRevisionBaseValueSegmentDto> BaseValueSegmentAssessmentRevisions { get; set; }
    }
}
