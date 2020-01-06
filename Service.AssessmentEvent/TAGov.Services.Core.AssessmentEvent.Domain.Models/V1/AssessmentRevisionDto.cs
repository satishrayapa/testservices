using System;

namespace TAGov.Services.Core.AssessmentEvent.Domain.Models.V1
{
    public class AssessmentRevisionDto
	{
		public int Id { get; set; }
        public string ReferenceNumber { get; set; }
		public int ValChangeReason { get; set; }
		public string ChangeReason { get; set; }
		public string Note { get; set; }
	}
}
