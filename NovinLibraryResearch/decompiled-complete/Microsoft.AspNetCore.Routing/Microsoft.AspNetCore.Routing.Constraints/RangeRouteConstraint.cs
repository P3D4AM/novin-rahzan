using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing.Constraints;

public class RangeRouteConstraint : IRouteConstraint
{
	public long Min { get; private set; }

	public long Max { get; private set; }

	public RangeRouteConstraint(long min, long max)
	{
		if (min > max)
		{
			string message = Resources.FormatRangeConstraint_MinShouldBeLessThanOrEqualToMax("min", "max");
			throw new ArgumentOutOfRangeException("min", min, message);
		}
		Min = min;
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
			if (result >= Min)
			{
				return result <= Max;
			}
			return false;
		}
		return false;
	}
}
