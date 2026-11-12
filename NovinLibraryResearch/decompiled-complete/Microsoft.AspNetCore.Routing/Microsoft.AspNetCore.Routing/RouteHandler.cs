using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing;

public class RouteHandler : IRouteHandler, IRouter
{
	private readonly RequestDelegate _requestDelegate;

	public RouteHandler(RequestDelegate requestDelegate)
	{
		_requestDelegate = requestDelegate;
	}

	public RequestDelegate GetRequestHandler(HttpContext httpContext, RouteData routeData)
	{
		return _requestDelegate;
	}

	public VirtualPathData GetVirtualPath(VirtualPathContext context)
	{
		return null;
	}

	public Task RouteAsync(RouteContext context)
	{
		context.Handler = _requestDelegate;
		return Task.CompletedTask;
	}
}
