﻿namespace TAGov.BaseValueSegment
{
    public class BaseValueSegmentValueDto
    {
        public int? Id { get; set; }

        public int BaseValueSegmentValueHeaderId { get; set; }

        public int SubComponent { get; set; }

        public decimal ValueAmount { get; set; }

        public decimal PercentComplete { get; set; }

        public decimal FullValueAmount { get; set; }

        public int DynCalcStepTrackingId { get; set; }
    }
}
