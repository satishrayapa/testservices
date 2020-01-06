using System;

namespace TAGov.Common.Security.SecurityClient
{
	public class SecurityTokenServiceProxy : ISecurityTokenServiceProxy
	{
		private readonly IJwtTokenRequestClient _jwtTokenRequestClient;
		private readonly ISecurityConfiguration _securityConfiguration;

		public SecurityTokenServiceProxy(IJwtTokenRequestClient jwtTokenRequestClient, ISecurityConfiguration securityConfiguration)
		{
			_jwtTokenRequestClient = jwtTokenRequestClient;
			_securityConfiguration = securityConfiguration;
		}

		public string GetAccessToken()
		{
			var result = IsServiceBasedConfiguration() ?
				_jwtTokenRequestClient.ProcessByServiceCredentials() :
				_jwtTokenRequestClient.ProcessByUserProfileId();

			return result?.AccessToken;
		}

		private bool IsServiceBasedConfiguration()
		{
			if (!string.IsNullOrEmpty(_securityConfiguration.ServiceClientId) &&
				!string.IsNullOrEmpty(_securityConfiguration.ClientId))
			{
				throw new ApplicationException("You cannot have both service-based and user-based security configurations turned on.");
			}

			if (!string.IsNullOrEmpty(_securityConfiguration.ServiceClientId) &&
				!string.IsNullOrEmpty(_securityConfiguration.ServiceClientPassword) &&
				!string.IsNullOrEmpty(_securityConfiguration.ServiceClientScope))
			{
				return true;
			}

			if (!string.IsNullOrEmpty(_securityConfiguration.ClientId) &&
				!string.IsNullOrEmpty(_securityConfiguration.ClientPassword) &&
				!string.IsNullOrEmpty(_securityConfiguration.ClientScope))
			{
				return false;
			}

			throw new ApplicationException("Invalid security configuration.");
		}
	}
}
