using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing;

public interface IRouteHandler
{
	RequestDelegate GetRequestHandler(HttpContext httpContext, RouteData routeData);
}
