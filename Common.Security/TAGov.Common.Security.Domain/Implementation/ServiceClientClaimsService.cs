using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using TAGov.Common.Security.Domain.Interfaces;

namespace TAGov.Common.Security.Domain.Implementation
{
	public class ServiceClientClaimsService : DefaultClaimsService
	{
		private readonly IClaimsProvider _claimsProvider;

		public ServiceClientClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger, IClaimsProvider claimsProvider) : base(profile, logger)
		{
			_claimsProvider = claimsProvider;
		}

		public override async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, Resources resources, ValidatedRequest request)
		{
			var claims = (await base.GetAccessTokenClaimsAsync(subject, resources, request)).ToList();
			
			if (request.Client.ClientId == "service.tagov.devops")
			{
				claims.AddRange(_claimsProvider.GetFullSecurityClaims());
				return claims;
			}

			if (request.Client.ClientId.StartsWith("service."))
			{
				claims.AddRange(_claimsProvider.GetFullApplicationClaims());
				return claims;
			}

			return subject.Claims;
		}
	}
}