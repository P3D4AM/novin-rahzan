using System;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Routing.Logging;

internal static class RouteConstraintMatcherExtensions
{
	private static readonly Action<ILogger, object, string, IRouteConstraint, Exception> _routeValueDoesNotMatchConstraint;

	static RouteConstraintMatcherExtensions()
	{
		_routeValueDoesNotMatchConstraint = LoggerMessage.Define<object, string, IRouteConstraint>(LogLevel.Debug, 1, "Route value '{RouteValue}' with key '{RouteKey}' did not match the constraint '{RouteConstraint}'.");
	}

	public static void RouteValueDoesNotMatchConstraint(this ILogger logger, object routeValue, string routeKey, IRouteConstraint routeConstraint)
	{
		_routeValueDoesNotMatchConstraint(logger, routeValue, routeKey, routeConstraint, null);
	}
}
