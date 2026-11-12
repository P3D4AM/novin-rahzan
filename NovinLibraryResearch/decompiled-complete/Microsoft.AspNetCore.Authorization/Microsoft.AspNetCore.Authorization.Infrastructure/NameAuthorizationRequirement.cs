using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization.Infrastructure;

public class NameAuthorizationRequirement : AuthorizationHandler<NameAuthorizationRequirement>, IAuthorizationRequirement
{
	public string RequiredName { get; }

	public NameAuthorizationRequirement(string requiredName)
	{
		if (requiredName == null)
		{
			throw new ArgumentNullException("requiredName");
		}
		RequiredName = requiredName;
	}

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NameAuthorizationRequirement requirement)
	{
		if (context.User != null && context.User.Identities.Any((ClaimsIdentity i) => string.Equals(i.Name, requirement.RequiredName)))
		{
			context.Succeed(requirement);
		}
		return Task.CompletedTask;
	}
}
