namespace TAGov.BaseValueSegment
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
