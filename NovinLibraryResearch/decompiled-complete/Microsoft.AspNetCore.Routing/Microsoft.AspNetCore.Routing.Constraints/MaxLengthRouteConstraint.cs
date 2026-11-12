using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing.Constraints;

public class MaxLengthRouteConstraint : IRouteConstraint
{
	public int MaxLength { get; }

	public MaxLengthRouteConstraint(int maxLength)
	{
		if (maxLength < 0)
		{
			string message = Resources.FormatArgumentMustBeGreaterThanOrEqualTo(0);
			throw new ArgumentOutOfRangeException("maxLength", maxLength, message);
		}
		MaxLength = maxLength;
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
		if (values.TryGetValue(routeKey, out var value) && value != null)
		{
			return Convert.ToString(value, CultureInfo.InvariantCulture).Length <= MaxLength;
		}
		return false;
	}
}
