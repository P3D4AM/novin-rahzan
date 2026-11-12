using System.Collections.Generic;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Authorization;

public class DefaultAuthorizationHandlerContextFactory : IAuthorizationHandlerContextFactory
{
	public virtual AuthorizationHandlerContext CreateContext(IEnumerable<IAuthorizationRequirement> requirements, ClaimsPrincipal user, object resource)
	{
		return new AuthorizationHandlerContext(requirements, user, resource);
	}
}
