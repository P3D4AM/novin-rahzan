using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Routing;

public static class RouteConstraintMatcher
{
	public static bool Match(IDictionary<string, IRouteConstraint> constraints, RouteValueDictionary routeValues, HttpContext httpContext, IRouter route, RouteDirection routeDirection, ILogger logger)
	{
		if (routeValues == null)
		{
			throw new ArgumentNullException("routeValues");
		}
		if (httpContext == null)
		{
			throw new ArgumentNullException("httpContext");
		}
		if (route == null)
		{
			throw new ArgumentNullException("route");
		}
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		if (constraints == null || constraints.Count == 0)
		{
			return true;
		}
		foreach (KeyValuePair<string, IRouteConstraint> constraint in constraints)
		{
			if (!constraint.Value.Match(httpContext, route, constraint.Key, routeValues, routeDirection))
			{
				if (routeDirection.Equals(RouteDirection.IncomingRequest))
				{
					routeValues.TryGetValue(constraint.Key, out var value);
					logger.RouteValueDoesNotMatchConstraint(value, constraint.Key, constraint.Value);
				}
				return false;
			}
		}
		return true;
	}
}
