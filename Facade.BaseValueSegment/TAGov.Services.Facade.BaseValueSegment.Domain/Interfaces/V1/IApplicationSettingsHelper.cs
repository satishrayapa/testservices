namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface IApplicationSettingsHelper
  {
    string AssessmentEventServiceApiUrl { get; }
    string RevenueObjectServiceApiUrl { get; }
    string LegalPartyServiceApiUrl { get; }
    string BaseValueSegmentServiceApiUrl { get; }
    string GrmEventServiceApiUrl { get; }
  }
}