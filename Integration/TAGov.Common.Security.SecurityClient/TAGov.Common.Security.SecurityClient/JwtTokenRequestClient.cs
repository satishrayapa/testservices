using IdentityModel.Client;

namespace TAGov.Common.Security.SecurityClient
{
	public class JwtTokenRequestClient : IJwtTokenRequestClient
	{
		private readonly ISecurityConfiguration _securityConfiguration;
		private readonly IInternalLogger _internalLogger;
		private readonly IUserProfileId _userProfileId;
		private readonly IJwtTokenCache _jwtTokenCache;

		public JwtTokenRequestClient(
			ISecurityConfiguration securityConfiguration,
			IInternalLogger internalLogger,
			IUserProfileId userProfileId,
			IJwtTokenCache jwtTokenCache)
		{
			_securityConfiguration = securityConfiguration;
			_internalLogger = internalLogger;
			_userProfileId = userProfileId;
			_jwtTokenCache = jwtTokenCache;
		}

		public JwtTokenRequestResult ProcessByUserProfileId()
		{
			int profileLoginId;
			if (_userProfileId.IsAuthenticated(out profileLoginId))
			{
				var jwt = _jwtTokenCache.Get(GetKeyByUserProfileId(profileLoginId));
				if (jwt != null) return jwt;

				var authority = _securityConfiguration.Authority;
				if (!string.IsNullOrEmpty(authority))
				{
					var clientId = _securityConfiguration.ClientId;
					var clientPassword = _securityConfiguration.ClientPassword;
					var clientScope = _securityConfiguration.ClientScope;

					if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientPassword))
					{
						var disco = new DiscoveryClient(authority)
						{
							Policy = { RequireHttps = DetermineRequireHttps(_securityConfiguration) }
						};

						var result = disco.GetAsync().Result;
						if (result.IsError)
						{
							_internalLogger.AppendLog(result.Error);
							return null;
						}

						var tokenClient = new TokenClient(result.TokenEndpoint, clientId, clientPassword);

						var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync(profileLoginId.ToString(),
								profileLoginId.ToString(), clientScope).Result;

						if (!tokenResponse.IsError)
						{
							_internalLogger.AppendLog("Access token generated: " + tokenResponse.AccessToken);

							var jwtResult = new JwtTokenRequestResult
							{
								AccessToken = tokenResponse.AccessToken,
								ExpiresIn = tokenResponse.ExpiresIn,
								TokenType = tokenResponse.TokenType
							};

							_jwtTokenCache.Add(GetKeyByUserProfileId(profileLoginId), jwtResult);

							return jwtResult;
						}

						_internalLogger.AppendLog("Token response error: " + tokenResponse.ErrorDescription);
					}
					else
					{
						_internalLogger.AppendLog("TAGov.Common.Security configuration(s) are missing!");
					}
				}
				else
				{
					_internalLogger.AppendLog("TAGov.Common.Security.Authority missing!");
				}
			}

			return null;
		}

		private string GetKeyByUserProfileId(int userProfileId)
		{
			return $"userProfileId:{userProfileId}";
		}

		private string GetKeyByServiceCredential(string clientId)
		{
			return $"serviceCredential:{clientId}";
		}

		private bool DetermineRequireHttps(ISecurityConfiguration securityConfiguration)
		{
			return !securityConfiguration.DisableRequireHttps;
		}

		public JwtTokenRequestResult ProcessByServiceCredentials()
		{

			var authority = _securityConfiguration.Authority;
			if (!string.IsNullOrEmpty(authority))
			{
				var clientId = _securityConfiguration.ServiceClientId;
				var clientPassword = _securityConfiguration.ServiceClientPassword;
				var clientScope = _securityConfiguration.ServiceClientScope;

				if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientPassword))
				{
					var jwt = _jwtTokenCache.Get(GetKeyByServiceCredential(clientId));
					if (jwt != null) return jwt;

					var disco = new DiscoveryClient(authority)
					{
						Policy = { RequireHttps = DetermineRequireHttps(_securityConfiguration) }
					};

					var result = disco.GetAsync().Result;
					if (result.IsError)
					{
						_internalLogger.AppendLog(result.Error);
						return null;
					}

					var tokenClient = new TokenClient(result.TokenEndpoint, clientId, clientPassword);

					var tokenResponse = tokenClient.RequestClientCredentialsAsync(clientScope).Result;

					if (!tokenResponse.IsError)
					{
						_internalLogger.AppendLog("Access token generated: " + tokenResponse.AccessToken);

						var jwtResult = new JwtTokenRequestResult
						{
							AccessToken = tokenResponse.AccessToken,
							ExpiresIn = tokenResponse.ExpiresIn,
							TokenType = tokenResponse.TokenType
						};

						_jwtTokenCache.Add(GetKeyByServiceCredential(clientId), jwtResult);

						return jwtResult;
					}

					_internalLogger.AppendLog("Token response error: " + tokenResponse.ErrorDescription);
				}
				else
				{
					_internalLogger.AppendLog("TAGov.Common.Security configuration(s) are missing!");
				}
			}
			else
			{
				_internalLogger.AppendLog("TAGov.Common.Security.Authority missing!");
			}

			return null;
		}
	}
}
