using System;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;

public static class WebSocketMiddlewareExtensions
{
	public static IApplicationBuilder UseWebSockets(this IApplicationBuilder app)
	{
		if (app == null)
		{
			throw new ArgumentNullException("app");
		}
		return app.UseMiddleware<WebSocketMiddleware>(Array.Empty<object>());
	}

	public static IApplicationBuilder UseWebSockets(this IApplicationBuilder app, WebSocketOptions options)
	{
		if (app == null)
		{
			throw new ArgumentNullException("app");
		}
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		return app.UseMiddleware<WebSocketMiddleware>(new object[1] { Options.Create(options) });
	}
}
