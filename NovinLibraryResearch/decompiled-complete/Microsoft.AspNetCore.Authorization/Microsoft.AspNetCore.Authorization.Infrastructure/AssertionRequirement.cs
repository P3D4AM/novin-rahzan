using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization.Infrastructure;

public class AssertionRequirement : IAuthorizationHandler, IAuthorizationRequirement
{
	public Func<AuthorizationHandlerContext, Task<bool>> Handler { get; }

	public AssertionRequirement(Func<AuthorizationHandlerContext, bool> handler)
	{
		if (handler == null)
		{
			throw new ArgumentNullException("handler");
		}
		Handler = (AuthorizationHandlerContext context) => Task.FromResult(handler(context));
	}

	public AssertionRequirement(Func<AuthorizationHandlerContext, Task<bool>> handler)
	{
		if (handler == null)
		{
			throw new ArgumentNullException("handler");
		}
		Handler = handler;
	}

	public async Task HandleAsync(AuthorizationHandlerContext context)
	{
		if (await Handler(context))
		{
			context.Succeed(this);
		}
	}
}
