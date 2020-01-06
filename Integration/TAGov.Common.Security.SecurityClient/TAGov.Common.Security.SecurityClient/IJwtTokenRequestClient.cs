namespace TAGov.Common.Security.SecurityClient
{
	public interface IJwtTokenRequestClient
	{
		JwtTokenRequestResult ProcessByUserProfileId();
		JwtTokenRequestResult ProcessByServiceCredentials();
	}
}
