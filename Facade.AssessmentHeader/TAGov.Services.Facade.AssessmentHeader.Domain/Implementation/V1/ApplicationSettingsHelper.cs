using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using TAGov.Services.Facade.AssessmentHeader.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.AssessmentHeader.Domain.Implementation.V1
{
  public class ApplicationSettingsHelper : IApplicationSettingsHelper
  {
    private readonly Lazy<string> _assessmentEventServiceApiUrl;
    private readonly Lazy<string> _legalPartyServiceApiUrl;
    private readonly Lazy<string> _revenueObjectServiceApiUrl;
    private readonly Lazy<string> _baseValueSegmentServiceApiUrl;
    private readonly IConfiguration _configuration;

    public ApplicationSettingsHelper( IConfiguration configuration )
    {
      _configuration = configuration;

      _assessmentEventServiceApiUrl = new Lazy<string>( () => GetConfigurationSetting( "ServiceApiUrls:assessmentEventServiceApiUrl" ), LazyThreadSafetyMode.ExecutionAndPublication );
      _legalPartyServiceApiUrl = new Lazy<string>( () => GetConfigurationSetting( "ServiceApiUrls:legalPartyServiceApiUrl" ), LazyThreadSafetyMode.ExecutionAndPublication );
      _revenueObjectServiceApiUrl = new Lazy<string>( () => GetConfigurationSetting( "ServiceApiUrls:revenueObjectServiceApiUrl" ), LazyThreadSafetyMode.ExecutionAndPublication );
      _baseValueSegmentServiceApiUrl = new Lazy<string>( () => GetConfigurationSetting( "ServiceApiUrls:baseValueSegmentServiceApiUrl" ), LazyThreadSafetyMode.ExecutionAndPublication );
    }

    public string AssessmentEventServiceApiUrl => _assessmentEventServiceApiUrl.Value;

    public string RevenueObjectServiceApiUrl => _revenueObjectServiceApiUrl.Value;
    public string LegalPartyServiceApiUrl => _legalPartyServiceApiUrl.Value;
    public string BaseValueSegmentServiceApiUrl => _baseValueSegmentServiceApiUrl.Value;

    private string GetConfigurationSetting( string settingName )
    {
      var setting = _configuration.GetSection( settingName ).Value;
      if ( string.IsNullOrWhiteSpace( setting ) )
      {
        throw new ArgumentException( string.Format( "Could not find configuration setting '{0}'.", settingName ) );
      }

      return setting;
    }
  }
}
