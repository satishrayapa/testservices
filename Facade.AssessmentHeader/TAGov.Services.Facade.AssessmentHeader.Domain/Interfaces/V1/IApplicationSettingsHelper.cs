namespace TAGov.Services.Facade.AssessmentHeader.Domain.Interfaces.V1
{
  public interface IApplicationSettingsHelper
  {
    string AssessmentEventServiceApiUrl { get; }
    string RevenueObjectServiceApiUrl { get; }
    string LegalPartyServiceApiUrl { get; }
    string BaseValueSegmentServiceApiUrl { get; }
  }
}
