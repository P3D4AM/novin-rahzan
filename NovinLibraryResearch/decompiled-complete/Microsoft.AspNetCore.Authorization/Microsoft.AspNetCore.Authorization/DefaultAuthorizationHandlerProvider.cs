using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization;

public class DefaultAuthorizationHandlerProvider : IAuthorizationHandlerProvider
{
	private readonly IEnumerable<IAuthorizationHandler> _handlers;

	public DefaultAuthorizationHandlerProvider(IEnumerable<IAuthorizationHandler> handlers)
	{
		if (handlers == null)
		{
			throw new ArgumentNullException("handlers");
		}
		_handlers = handlers;
	}

	public Task<IEnumerable<IAuthorizationHandler>> GetHandlersAsync(AuthorizationHandlerContext context)
	{
		return Task.FromResult(_handlers);
	}
}
