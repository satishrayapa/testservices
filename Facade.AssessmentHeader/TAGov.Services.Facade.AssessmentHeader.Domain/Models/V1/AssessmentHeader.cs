namespace TAGov.Services.Facade.AssessmentHeader.Domain.Models.V1
{
  public class AssessmentHeader
  {
    public AssessmentEvent AssessmentEvent { get; set; }
    public LegalParty LegalParty { get; set; }
    public RevenueObject RevenueObject { get; set; }
  }
}
