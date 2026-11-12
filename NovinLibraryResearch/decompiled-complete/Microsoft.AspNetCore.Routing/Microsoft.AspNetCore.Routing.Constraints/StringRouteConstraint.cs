using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing.Constraints;

public class StringRouteConstraint : IRouteConstraint
{
	private string _value;

	public StringRouteConstraint(string value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		_value = value;
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
			return Convert.ToString(value, CultureInfo.InvariantCulture).Equals(_value, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}
}
