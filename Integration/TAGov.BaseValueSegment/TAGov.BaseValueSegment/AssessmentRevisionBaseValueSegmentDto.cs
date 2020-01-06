namespace TAGov.BaseValueSegment
{
    public class AssessmentRevisionBaseValueSegmentDto
    {
        public int Id { get; set; }

        public int AssessmentRevisionId { get; set; }

        public int BaseValueSegmentId { get; set; }

        public int DynCalcStepTrackingId { get; set; }

        public int BaseValueSegmentStatusTypeId { get; set; }

        public string ReviewMessage { get; set; }

        public BaseValueSegmentStatusTypeDto BaseValueSegmentStatusType { get; set; }
    }
}
