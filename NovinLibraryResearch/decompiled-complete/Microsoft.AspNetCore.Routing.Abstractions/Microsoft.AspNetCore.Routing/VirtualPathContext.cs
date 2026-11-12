using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing;

public class VirtualPathContext
{
	public RouteValueDictionary AmbientValues { get; }

	public HttpContext HttpContext { get; }

	public string RouteName { get; }

	public RouteValueDictionary Values { get; set; }

	public VirtualPathContext(HttpContext httpContext, RouteValueDictionary ambientValues, RouteValueDictionary values)
		: this(httpContext, ambientValues, values, null)
	{
	}

	public VirtualPathContext(HttpContext httpContext, RouteValueDictionary ambientValues, RouteValueDictionary values, string routeName)
	{
		HttpContext = httpContext;
		AmbientValues = ambientValues;
		Values = values;
		RouteName = routeName;
	}
}
