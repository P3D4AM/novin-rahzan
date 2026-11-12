using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization;

public abstract class AuthorizationHandler<TRequirement> : IAuthorizationHandler where TRequirement : IAuthorizationRequirement
{
	public virtual async Task HandleAsync(AuthorizationHandlerContext context)
	{
		foreach (TRequirement item in context.Requirements.OfType<TRequirement>())
		{
			await HandleRequirementAsync(context, item);
		}
	}

	protected abstract Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement);
}
public abstract class AuthorizationHandler<TRequirement, TResource> : IAuthorizationHandler where TRequirement : IAuthorizationRequirement
{
	public virtual async Task HandleAsync(AuthorizationHandlerContext context)
	{
		if (!(context.Resource is TResource))
		{
			return;
		}
		foreach (TRequirement item in context.Requirements.OfType<TRequirement>())
		{
			await HandleRequirementAsync(context, item, (TResource)context.Resource);
		}
	}

	protected abstract Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, TResource resource);
}
