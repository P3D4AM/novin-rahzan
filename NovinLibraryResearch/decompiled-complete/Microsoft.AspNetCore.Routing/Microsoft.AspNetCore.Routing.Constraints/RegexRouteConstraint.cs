using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing.Constraints;

public class RegexRouteConstraint : IRouteConstraint
{
	private static readonly TimeSpan RegexMatchTimeout = TimeSpan.FromSeconds(10.0);

	public Regex Constraint { get; private set; }

	public RegexRouteConstraint(Regex regex)
	{
		if (regex == null)
		{
			throw new ArgumentNullException("regex");
		}
		Constraint = regex;
	}

	public RegexRouteConstraint(string regexPattern)
	{
		if (regexPattern == null)
		{
			throw new ArgumentNullException("regexPattern");
		}
		Constraint = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, RegexMatchTimeout);
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
			string input = Convert.ToString(value, CultureInfo.InvariantCulture);
			return Constraint.IsMatch(input);
		}
		return false;
	}
}
