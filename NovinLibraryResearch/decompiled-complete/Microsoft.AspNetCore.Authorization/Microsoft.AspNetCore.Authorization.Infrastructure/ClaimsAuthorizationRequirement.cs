using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization.Infrastructure;

public class ClaimsAuthorizationRequirement : AuthorizationHandler<ClaimsAuthorizationRequirement>, IAuthorizationRequirement
{
	public string ClaimType { get; }

	public IEnumerable<string> AllowedValues { get; }

	public ClaimsAuthorizationRequirement(string claimType, IEnumerable<string> allowedValues)
	{
		if (claimType == null)
		{
			throw new ArgumentNullException("claimType");
		}
		ClaimType = claimType;
		AllowedValues = allowedValues;
	}

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimsAuthorizationRequirement requirement)
	{
		if (context.User != null)
		{
			bool flag = false;
			if ((requirement.AllowedValues != null && requirement.AllowedValues.Any()) ? context.User.Claims.Any((Claim c) => string.Equals(c.Type, requirement.ClaimType, StringComparison.OrdinalIgnoreCase) && requirement.AllowedValues.Contains<string>(c.Value, StringComparer.Ordinal)) : context.User.Claims.Any((Claim c) => string.Equals(c.Type, requirement.ClaimType, StringComparison.OrdinalIgnoreCase)))
			{
				context.Succeed(requirement);
			}
		}
		return Task.CompletedTask;
	}
}
