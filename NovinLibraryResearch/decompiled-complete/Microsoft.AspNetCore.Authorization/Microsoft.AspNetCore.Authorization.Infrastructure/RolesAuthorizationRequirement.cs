using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization.Infrastructure;

public class RolesAuthorizationRequirement : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationRequirement
{
	public IEnumerable<string> AllowedRoles { get; }

	public RolesAuthorizationRequirement(IEnumerable<string> allowedRoles)
	{
		if (allowedRoles == null)
		{
			throw new ArgumentNullException("allowedRoles");
		}
		if (allowedRoles.Count() == 0)
		{
			throw new InvalidOperationException(Resources.Exception_RoleRequirementEmpty);
		}
		AllowedRoles = allowedRoles;
	}

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
	{
		if (context.User != null)
		{
			bool flag = false;
			if (requirement.AllowedRoles != null && requirement.AllowedRoles.Any())
			{
				flag = requirement.AllowedRoles.Any((string r) => context.User.IsInRole(r));
			}
			if (flag)
			{
				context.Succeed(requirement);
			}
		}
		return Task.CompletedTask;
	}
}
