using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing.Constraints;

public class HttpMethodRouteConstraint : IRouteConstraint
{
	public IList<string> AllowedMethods { get; }

	public HttpMethodRouteConstraint(params string[] allowedMethods)
	{
		if (allowedMethods == null)
		{
			throw new ArgumentNullException("allowedMethods");
		}
		AllowedMethods = new List<string>(allowedMethods);
	}

	public virtual bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
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
		switch (routeDirection)
		{
		case RouteDirection.IncomingRequest:
			return AllowedMethods.Contains<string>(httpContext.Request.Method, StringComparer.OrdinalIgnoreCase);
		case RouteDirection.UrlGeneration:
		{
			if (!values.TryGetValue(routeKey, out var value))
			{
				return true;
			}
			return AllowedMethods.Contains<string>(Convert.ToString(value), StringComparer.OrdinalIgnoreCase);
		}
		default:
			throw new ArgumentOutOfRangeException("routeDirection");
		}
	}
}
