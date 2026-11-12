using System;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing;

public class RouteContext
{
	private RouteData _routeData;

	public RequestDelegate Handler { get; set; }

	public HttpContext HttpContext { get; }

	public RouteData RouteData
	{
		get
		{
			return _routeData;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("RouteData");
			}
			_routeData = value;
		}
	}

	public RouteContext(HttpContext httpContext)
	{
		HttpContext = httpContext;
		RouteData = new RouteData();
	}
}
