using System;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Routing.Logging;

internal static class TreeRouterLoggerExtensions
{
	private static readonly Action<ILogger, string, string, Exception> _matchedRoute;

	static TreeRouterLoggerExtensions()
	{
		_matchedRoute = LoggerMessage.Define<string, string>(LogLevel.Debug, 1, "Request successfully matched the route with name '{RouteName}' and template '{RouteTemplate}'.");
	}

	public static void MatchedRoute(this ILogger logger, string routeName, string routeTemplate)
	{
		_matchedRoute(logger, routeName, routeTemplate, null);
	}
}
