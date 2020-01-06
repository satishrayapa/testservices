using System.Collections.Generic;

namespace TAGov.BaseValueSegment
{
    public class BaseValueSegmentOwnerDto
    {
        public int? Id { get; set; }

        public int BaseValueSegmentTransactionId { get; set; }

        public int LegalPartyRoleId { get; set; }

        public decimal BeneficialInterestPercent { get; set; }

        public int DynCalcStepTrackingId { get; set; }

        public int? GRMEventId { get; set; }

	    public GrmEventCreateInformation GrmEventInformation { get; set; }

		public virtual IList<BaseValueSegmentOwnerValueDto> BaseValueSegmentOwnerValueValues { get; set; }
    }
}
