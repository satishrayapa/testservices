using System;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using TAGov.Common.Security.Domain.Interfaces;

namespace TAGov.Common.Security.Domain.Implementation
{
	public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
	{
		private readonly IClaimsProvider _claimsProvider;


		public ResourceOwnerPasswordValidator(IClaimsProvider claimsProvider)
		{
			_claimsProvider = claimsProvider;
		}

		public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
		{
			// Validation is done by ensuring the profile user id is valid.

			var userProfileLoginId = Convert.ToInt32(context.UserName);

			context.Result = new GrantValidationResult(context.UserName, "custom", _claimsProvider.GetClaims(userProfileLoginId));
			return Task.CompletedTask;
		}
	}
}
