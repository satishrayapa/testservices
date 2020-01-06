namespace TAGov.Common.Security.SecurityClient
{
	public class JwtTokenRequestResult
	{
		public string AccessToken { get; set; }
		public long ExpiresIn { get; set; }
		public string TokenType { get; set; }
	}
}
