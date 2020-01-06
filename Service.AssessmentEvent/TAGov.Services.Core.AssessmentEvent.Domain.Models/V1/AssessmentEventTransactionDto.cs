namespace TAGov.Services.Core.AssessmentEvent.Domain.Models.V1
{
    public class AssessmentEventTransactionDto
	{
		public int Id { get; set; }
		public int AsmtEventId { get; set; }
		public int AsmtEventState { get; set; }
        public string AsmtEventStateDescription { get; set; }
        public int AsmtRevnEventId { get; set; }
	}
}
