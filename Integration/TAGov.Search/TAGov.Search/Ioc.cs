using TAGov.Common.ResourceLocatorClient;
using TAGov.Common.ResourceLocatorClient.Enums;
using TAGov.Common.Security.SecurityClient;

namespace TAGov.Search
{
	public static class Ioc
	{
		public static LegalPartySearchProxy GetLegalPartySearchProxy(Features feature)
		{
			(HttpClientProxy httpClientProxy, Configuration configuration, RestClientWithSecurity restClient) = GetHttpClientProxyConfigurationRestClient();

		  if (feature == Features.LegalPartySearch ||
				feature == Features.RevenueObjectSearch)
			{
				var legalPartySearch = new LegalPartySearchProxy(httpClientProxy, new FeatureToggle(restClient), new UrlServices(restClient, configuration));

				if (legalPartySearch.CanHandle(feature))
					return legalPartySearch;
			}

			return null;
		}

	  private static (HttpClientProxy httpClientProxy, Configuration configuration, RestClientWithSecurity restClient) GetHttpClientProxyConfigurationRestClient()
	  {
	    var securityConfiguration = new SecurityConfiguration();
	    var logger = new InternalLogger( securityConfiguration );
	    var webContext = new WebContext();
	    var jwtTokenRequestClient = new JwtTokenRequestClient( securityConfiguration, logger, new UserProfileId( webContext, logger ), new JwtTokenCache( webContext ) );

	    var securityTokenServiceProxy = new SecurityTokenServiceProxy( jwtTokenRequestClient, securityConfiguration );
	    var httpClientProxy = new HttpClientProxy( securityTokenServiceProxy );
	    var configuration = new Configuration();
	    var restClient = new RestClientWithSecurity( configuration, httpClientProxy );
	    return ( httpClientProxy, configuration, restClient );
	  }

	  public static MyWorklistSearchProxy GetMyWorklistSearchProxy()
	  {
      return new MyWorklistSearchProxy();
	  }
	}
}
