using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Internal;

namespace Microsoft.AspNetCore.Builder;

public static class RoutingBuilderExtensions
{
	public static IApplicationBuilder UseRouter(this IApplicationBuilder builder, IRouter router)
	{
		if (builder == null)
		{
			throw new ArgumentNullException("builder");
		}
		if (router == null)
		{
			throw new ArgumentNullException("router");
		}
		if (builder.ApplicationServices.GetService(typeof(RoutingMarkerService)) == null)
		{
			throw new InvalidOperationException(Resources.FormatUnableToFindServices("IServiceCollection", "AddRouting", "ConfigureServices(...)"));
		}
		return builder.UseMiddleware<RouterMiddleware>(new object[1] { router });
	}

	public static IApplicationBuilder UseRouter(this IApplicationBuilder builder, Action<IRouteBuilder> action)
	{
		if (builder == null)
		{
			throw new ArgumentNullException("builder");
		}
		if (action == null)
		{
			throw new ArgumentNullException("action");
		}
		if (builder.ApplicationServices.GetService(typeof(RoutingMarkerService)) == null)
		{
			throw new InvalidOperationException(Resources.FormatUnableToFindServices("IServiceCollection", "AddRouting", "ConfigureServices(...)"));
		}
		RouteBuilder routeBuilder = new RouteBuilder(builder);
		action(routeBuilder);
		return builder.UseRouter(routeBuilder.Build());
	}
}
