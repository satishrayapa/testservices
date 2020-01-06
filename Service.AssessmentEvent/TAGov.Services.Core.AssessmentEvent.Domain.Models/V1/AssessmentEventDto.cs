using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.AssessmentEvent.Domain.Models.V1
{
    public class AssessmentEventDto
    {
	    public AssessmentEventDto()
	    {
		    this.AssessmentEventTransactions =  new List<AssessmentEventTransactionDto>();
		}
		public int Id { get; set; }
        public long TranId { get; set; }
        public int DynCalcStepTrackingId { get; set; }
        public int RevObjId { get; set; }
        public short TaxYear { get; set; }
        public int AsmtEventType { get; set; }
        public string AsmtEventTypeDescription { get; set; }
        public DateTime EventDate { get; set; }
		public List<AssessmentEventTransactionDto> AssessmentEventTransactions { get; private set; } 
		public int? PrimaryBaseYear { get; set; }
		public string PrimaryBaseYearMultipleOrSingleDescription { get; set; }
	}
}
