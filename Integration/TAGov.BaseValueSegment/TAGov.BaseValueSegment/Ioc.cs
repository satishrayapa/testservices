using TAGov.Common.ResourceLocatorClient;
using TAGov.Common.Security.SecurityClient;

namespace TAGov.BaseValueSegment
{
	public static class Ioc
	{
		public static BaseValueSegmentProxy GetBaseValueSegmentProxy()
		{
			var securityConfiguration = new SecurityConfiguration();
			var logger = new InternalLogger(securityConfiguration);
			var webContext = new WebContext();
			var jwtTokenRequestClient = new JwtTokenRequestClient(securityConfiguration, logger, new UserProfileId(webContext, logger), new JwtTokenCache(webContext));

			var securityTokenServiceProxy = new SecurityTokenServiceProxy(jwtTokenRequestClient, securityConfiguration);
			var httpClientProxy = new HttpClientProxy(securityTokenServiceProxy);
			var configuration = new Configuration();
			var restClient = new RestClient(configuration, httpClientProxy);

			return new BaseValueSegmentProxy(httpClientProxy, new UrlServices(restClient, configuration));
		}
	}
}
