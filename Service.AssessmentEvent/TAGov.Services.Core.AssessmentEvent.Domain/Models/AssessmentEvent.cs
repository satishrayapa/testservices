using System;

namespace TAGov.Services.Core.AssessmentEvent.Domain.Models
{
	public class AssessmentEvent
	{
		public int Id { get; set; }
		public long TranId { get; set; }
		public int DynCalcStepTrackingId { get; set; }
		public int RevObjId { get; set; }
		public short TaxYear { get; set; }
		public int AsmtEventType { get; set; }
		public DateTime EventDate { get; set; }
	}
}
