using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using TAGov.Common.Security.Claims;

namespace TAGov.Common.Security.Http.Authorization
{
	// https://github.com/blowdart/AspNetAuthorizationWorkshop

	public class HasResourceVerbClaim : AuthorizationHandler<ResourceAccessRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceAccessRequirement requirement)
		{
			var controllerContext = (AuthorizationFilterContext)context.Resource;
			var actionDescriptor = controllerContext.ActionDescriptor as ControllerActionDescriptor;
			if (actionDescriptor != null)
			{
				var resource = actionDescriptor.ControllerName;
				var assertClaim = context.User.Claims.GetClaim(requirement.ApplicationName, resource);
				if (assertClaim != null)
				{
					if (actionDescriptor.MethodInfo.GetCustomAttribute<HttpGetAttribute>() != null)
					{
						if (assertClaim.CanGet())
						{
							context.Succeed(requirement);
							return Task.CompletedTask;
						}

						context.Fail();
						return Task.CompletedTask;
					}

					if (actionDescriptor.MethodInfo.GetCustomAttribute<HttpPostAttribute>() != null)
					{
						if (assertClaim.CanPost())
						{
							context.Succeed(requirement);
							return Task.CompletedTask;
						}

						context.Fail();
						return Task.CompletedTask;
					}

					if (actionDescriptor.MethodInfo.GetCustomAttribute<HttpPutAttribute>() != null)
					{
						if (assertClaim.CanPut())
						{
							context.Succeed(requirement);
							return Task.CompletedTask;
						}

						context.Fail();
						return Task.CompletedTask;
					}

					if (actionDescriptor.MethodInfo.GetCustomAttribute<HttpDeleteAttribute>() != null)
					{
						if (assertClaim.CanDelete())
						{
							context.Succeed(requirement);
							return Task.CompletedTask;
						}

						context.Fail();
						return Task.CompletedTask;
					}
				}
			}

			context.Fail();

			return Task.CompletedTask;
		}
	}
}
