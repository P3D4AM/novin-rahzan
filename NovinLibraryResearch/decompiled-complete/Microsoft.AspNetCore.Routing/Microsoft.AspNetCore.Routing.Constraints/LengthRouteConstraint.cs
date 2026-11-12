using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing.Constraints;

public class LengthRouteConstraint : IRouteConstraint
{
	public int MinLength { get; }

	public int MaxLength { get; }

	public LengthRouteConstraint(int length)
	{
		if (length < 0)
		{
			string message = Resources.FormatArgumentMustBeGreaterThanOrEqualTo(0);
			throw new ArgumentOutOfRangeException("length", length, message);
		}
		MinLength = (MaxLength = length);
	}

	public LengthRouteConstraint(int minLength, int maxLength)
	{
		if (minLength < 0)
		{
			string message = Resources.FormatArgumentMustBeGreaterThanOrEqualTo(0);
			throw new ArgumentOutOfRangeException("minLength", minLength, message);
		}
		if (maxLength < 0)
		{
			string message2 = Resources.FormatArgumentMustBeGreaterThanOrEqualTo(0);
			throw new ArgumentOutOfRangeException("maxLength", maxLength, message2);
		}
		if (minLength > maxLength)
		{
			string message3 = Resources.FormatRangeConstraint_MinShouldBeLessThanOrEqualToMax("minLength", "maxLength");
			throw new ArgumentOutOfRangeException("minLength", minLength, message3);
		}
		MinLength = minLength;
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
			int length = Convert.ToString(value, CultureInfo.InvariantCulture).Length;
			if (length >= MinLength)
			{
				return length <= MaxLength;
			}
			return false;
		}
		return false;
	}
}
