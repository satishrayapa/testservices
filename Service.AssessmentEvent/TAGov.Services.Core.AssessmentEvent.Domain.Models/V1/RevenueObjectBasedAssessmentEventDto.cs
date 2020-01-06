using System;

namespace TAGov.Services.Core.AssessmentEvent.Domain.Models.V1
{
    public class RevenueObjectBasedAssessmentEventDto
    {
	    public int Id { get; set; }

	    public DateTime EventDate { get; set; }

	    public int RevObjId { get; set; }
	}
}
