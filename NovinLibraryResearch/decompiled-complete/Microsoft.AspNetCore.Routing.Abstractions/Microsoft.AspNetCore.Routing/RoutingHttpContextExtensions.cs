using System;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing;

public static class RoutingHttpContextExtensions
{
	public static RouteData GetRouteData(this HttpContext httpContext)
	{
		if (httpContext == null)
		{
			throw new ArgumentNullException("httpContext");
		}
		return (httpContext.Features[typeof(IRoutingFeature)] as IRoutingFeature)?.RouteData;
	}

	public static object GetRouteValue(this HttpContext httpContext, string key)
	{
		if (httpContext == null)
		{
			throw new ArgumentNullException("httpContext");
		}
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		return (httpContext.Features[typeof(IRoutingFeature)] as IRoutingFeature)?.RouteData.Values[key];
	}
}
