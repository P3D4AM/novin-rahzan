using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing;

public interface IRouteConstraint
{
	bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection);
}
