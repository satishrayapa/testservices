using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class ApplicationSettingsHelper : IApplicationSettingsHelper
  {
    private readonly Lazy<string> _assessmentEventServiceApiUrl;
    private readonly Lazy<string> _legalPartyServiceApiUrl;
    private readonly Lazy<string> _revenueObjectServiceApiUrl;
    private readonly Lazy<string> _baseValueSegmentServiceApiUrl;
    private readonly Lazy<string> _grmEventServiceApiUrl;
    private readonly IConfiguration _configuration;

    public ApplicationSettingsHelper( IConfiguration configuration )
    {
      _configuration = configuration;

      _assessmentEventServiceApiUrl = new Lazy<string>( () => GetConfigurationSetting( "ServiceApiUrls:assessmentEventServiceApiUrl" ), LazyThreadSafetyMode.ExecutionAndPublication );
      _legalPartyServiceApiUrl = new Lazy<string>( () => GetConfigurationSetting( "ServiceApiUrls:legalPartyServiceApiUrl" ), LazyThreadSafetyMode.ExecutionAndPublication );
      _revenueObjectServiceApiUrl = new Lazy<string>( () => GetConfigurationSetting( "ServiceApiUrls:revenueObjectServiceApiUrl" ), LazyThreadSafetyMode.ExecutionAndPublication );
      _baseValueSegmentServiceApiUrl = new Lazy<string>( () => GetConfigurationSetting( "ServiceApiUrls:baseValueSegmentServiceApiUrl" ), LazyThreadSafetyMode.ExecutionAndPublication );
      _grmEventServiceApiUrl = new Lazy<string>( () => GetConfigurationSetting( "ServiceApiUrls:grmEventServiceApiUrl" ), LazyThreadSafetyMode.ExecutionAndPublication );
    }

    public string AssessmentEventServiceApiUrl => _assessmentEventServiceApiUrl.Value;

    public string RevenueObjectServiceApiUrl => _revenueObjectServiceApiUrl.Value;
    public string LegalPartyServiceApiUrl => _legalPartyServiceApiUrl.Value;
    public string BaseValueSegmentServiceApiUrl => _baseValueSegmentServiceApiUrl.Value;
    public string GrmEventServiceApiUrl => _grmEventServiceApiUrl.Value;

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