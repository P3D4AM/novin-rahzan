using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing.Constraints;

public class MaxRouteConstraint : IRouteConstraint
{
	public long Max { get; private set; }

	public MaxRouteConstraint(long max)
	{
		Max = max;
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
		if (values.TryGetValue(routeKey, out var value) && value != null && long.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return result <= Max;
		}
		return false;
	}
}
