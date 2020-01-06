using System;

namespace TAGov.Services.Facade.AssessmentHeader.Domain.Models.V1
{
  public class AssessmentEvent
  {
    public int AssessmentEventId { get; set; }
    public int RevenueObjectId { get; set; }
    public short TaxYear { get; set; }
    public int AssessmentEventType { get; set; }
    public string AssessmentEventTypeDescription { get; set; }
    public DateTime EventDate { get; set; }
    public string ChangeReason { get; set; }
    public string EventState { get; set; }
    public int RevisionId { get; set; }
    public int? TransactionId { get; set; }
    public string ReferenceNumber { get; set; }
    public string Note { get; set; }
    public string RevenueAndTaxCode { get; set; }
    public string BVSTranType { get; set; }
    public int? PrimaryBaseYear { get; set; }
    public string PrimaryBaseYearMultipleOrSingleDescription { get; set; }
  }
}
