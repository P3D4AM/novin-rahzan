using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing.Constraints;

public class CompositeRouteConstraint : IRouteConstraint
{
	public IEnumerable<IRouteConstraint> Constraints { get; private set; }

	public CompositeRouteConstraint(IEnumerable<IRouteConstraint> constraints)
	{
		if (constraints == null)
		{
			throw new ArgumentNullException("constraints");
		}
		Constraints = constraints;
	}

	public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
	{
		if (httpContext == null)
		{
			throw new ArgumentNullException("httpContext");
		}
		if (route == null)
		{
			throw new ArgumentNullException("route");
		}
		if (routeKey == null)
		{
			throw new ArgumentNullException("routeKey");
		}
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		foreach (IRouteConstraint constraint in Constraints)
		{
			if (!constraint.Match(httpContext, route, routeKey, values, routeDirection))
			{
				return false;
			}
		}
		return true;
	}
}
