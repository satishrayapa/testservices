using TAGov.Common.ResourceLocatorClient.Enums;
using TAGov.Common.Security.SecurityClient;

namespace TAGov.Common.ResourceLocatorClient
{
	public static class Ioc
	{
		public static bool IsFeatureToggleEnabled(Features feature)
		{
			var securityConfiguration = new SecurityConfiguration();
			var internalLogger = new InternalLogger(securityConfiguration);
			var webContext = new WebContext();
			IUserProfileId userProfileId = new UserProfileId(webContext, internalLogger);

			var jwt = new JwtTokenRequestClient(securityConfiguration, internalLogger, userProfileId, new JwtTokenCache(webContext));

			var securityTokenServiceProxy = new SecurityTokenServiceProxy(jwt, securityConfiguration);
			var httpClientProxy = new HttpClientProxy(securityTokenServiceProxy);
			var configuration = new Configuration();
			var restClient = new RestClient(configuration, httpClientProxy);
			var featureToggle = new FeatureToggle(restClient);

			return featureToggle.IsEnabled(feature);
		}
	}
}
