using System.Collections.Generic;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Authorization;

public interface IAuthorizationHandlerContextFactory
{
	AuthorizationHandlerContext CreateContext(IEnumerable<IAuthorizationRequirement> requirements, ClaimsPrincipal user, object resource);
}
