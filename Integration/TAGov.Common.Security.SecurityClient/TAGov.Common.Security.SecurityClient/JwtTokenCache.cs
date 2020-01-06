namespace TAGov.Common.Security.SecurityClient
{
	public class JwtTokenCache : IJwtTokenCache
	{
		private const int FiftyFiveMinutes = 3300;
		private readonly IWebContext _webContext;

		public JwtTokenCache(IWebContext webContext)
		{
			_webContext = webContext;
		}

		public void Add(string key, JwtTokenRequestResult jwtTokenRequestResult)
		{
			_webContext.AddToCache(key, jwtTokenRequestResult, FiftyFiveMinutes);
		}

		public JwtTokenRequestResult Get(string key)
		{
			return _webContext.GetFromCache<JwtTokenRequestResult>(key);
		}
	}
}
