using System;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing.Constraints;

public class OptionalRouteConstraint : IRouteConstraint
{
	public IRouteConstraint InnerConstraint { get; }

	public OptionalRouteConstraint(IRouteConstraint innerConstraint)
	{
		if (innerConstraint == null)
		{
			throw new ArgumentNullException("innerConstraint");
		}
		InnerConstraint = innerConstraint;
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
		if (values.TryGetValue(routeKey, out var _))
		{
			return InnerConstraint.Match(httpContext, route, routeKey, values, routeDirection);
		}
		return true;
	}
}
