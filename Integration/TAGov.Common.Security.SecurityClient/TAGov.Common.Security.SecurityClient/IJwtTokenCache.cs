namespace TAGov.Common.Security.SecurityClient
{
	public interface IJwtTokenCache
	{
		void Add(string key, JwtTokenRequestResult jwtTokenRequestResult);

		JwtTokenRequestResult Get(string key);
	}
}
