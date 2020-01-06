using Microsoft.AspNetCore.Authorization;

namespace TAGov.Common.Security.Http.Authorization
{
	public class ResourceAccessRequirement : IAuthorizationRequirement
	{
		public string ApplicationName { get; }

		public ResourceAccessRequirement(string applicationName)
		{
			ApplicationName = applicationName;
		}
	}
}
