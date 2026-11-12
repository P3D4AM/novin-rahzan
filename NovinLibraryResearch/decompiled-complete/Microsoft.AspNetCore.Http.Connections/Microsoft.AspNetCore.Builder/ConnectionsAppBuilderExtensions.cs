using System;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Connections.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder;

public static class ConnectionsAppBuilderExtensions
{
	public static IApplicationBuilder UseConnections(this IApplicationBuilder app, Action<ConnectionsRouteBuilder> configure)
	{
		if (configure == null)
		{
			throw new ArgumentNullException("configure");
		}
		HttpConnectionDispatcher requiredService = app.ApplicationServices.GetRequiredService<HttpConnectionDispatcher>();
		RouteBuilder routeBuilder = new RouteBuilder(app);
		configure(new ConnectionsRouteBuilder(routeBuilder, requiredService));
		app.UseWebSockets();
		app.UseRouter(routeBuilder.Build());
		return app;
	}
}
