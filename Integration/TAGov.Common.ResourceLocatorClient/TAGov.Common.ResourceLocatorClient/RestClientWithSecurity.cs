namespace TAGov.Common.ResourceLocatorClient
{
	public class RestClientWithSecurity : RestClient
	{
		public RestClientWithSecurity(IConfiguration configuration, IHttpClientProxy httpClientProxy) :
			base(configuration, httpClientProxy)
		{ }

		protected override string GetVersion()
		{
			return "v1.1";
		}
	}
}
