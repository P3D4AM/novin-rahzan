using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder;

public class RouterMiddleware
{
	private readonly ILogger _logger;

	private readonly RequestDelegate _next;

	private readonly IRouter _router;

	public RouterMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IRouter router)
	{
		_next = next;
		_router = router;
		_logger = loggerFactory.CreateLogger<RouterMiddleware>();
	}

	public async Task Invoke(HttpContext httpContext)
	{
		RouteContext context = new RouteContext(httpContext)
		{
			RouteData = 
			{
				Routers = { _router }
			}
		};
		await _router.RouteAsync(context);
		if (context.Handler == null)
		{
			_logger.RequestDidNotMatchRoutes();
			await _next(httpContext);
		}
		else
		{
			httpContext.Features[typeof(IRoutingFeature)] = new RoutingFeature
			{
				RouteData = context.RouteData
			};
			await context.Handler(context.HttpContext);
		}
	}
}
