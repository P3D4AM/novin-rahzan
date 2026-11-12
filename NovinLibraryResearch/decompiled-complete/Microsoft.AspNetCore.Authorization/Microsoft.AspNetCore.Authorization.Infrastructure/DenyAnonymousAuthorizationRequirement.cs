using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization.Infrastructure;

public class DenyAnonymousAuthorizationRequirement : AuthorizationHandler<DenyAnonymousAuthorizationRequirement>, IAuthorizationRequirement
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DenyAnonymousAuthorizationRequirement requirement)
	{
		ClaimsPrincipal user = context.User;
		if (user?.Identity != null && user.Identities.Any((ClaimsIdentity i) => i.IsAuthenticated))
		{
			context.Succeed(requirement);
		}
		return Task.CompletedTask;
	}
}
