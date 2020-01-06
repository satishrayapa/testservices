namespace TAGov.BaseValueSegment
{
	interface IBaseValueSegmentProxy
	{
		BaseValueSegmentDto Save( int baseValueSegmentId, int assessmentEventId, BaseValueSegmentDto baseValueSegmentDto );
	}
}
