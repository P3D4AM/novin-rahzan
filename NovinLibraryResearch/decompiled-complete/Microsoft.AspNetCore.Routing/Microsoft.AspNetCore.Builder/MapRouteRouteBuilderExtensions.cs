using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder;

public static class MapRouteRouteBuilderExtensions
{
	public static IRouteBuilder MapRoute(this IRouteBuilder routeBuilder, string name, string template)
	{
		routeBuilder.MapRoute(name, template, null);
		return routeBuilder;
	}

	public static IRouteBuilder MapRoute(this IRouteBuilder routeBuilder, string name, string template, object defaults)
	{
		return routeBuilder.MapRoute(name, template, defaults, null);
	}

	public static IRouteBuilder MapRoute(this IRouteBuilder routeBuilder, string name, string template, object defaults, object constraints)
	{
		return routeBuilder.MapRoute(name, template, defaults, constraints, null);
	}

	public static IRouteBuilder MapRoute(this IRouteBuilder routeBuilder, string name, string template, object defaults, object constraints, object dataTokens)
	{
		if (routeBuilder.DefaultHandler == null)
		{
			throw new RouteCreationException(Resources.FormatDefaultHandler_MustBeSet("IRouteBuilder"));
		}
		IInlineConstraintResolver requiredService = routeBuilder.ServiceProvider.GetRequiredService<IInlineConstraintResolver>();
		routeBuilder.Routes.Add(new Route(routeBuilder.DefaultHandler, name, template, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new RouteValueDictionary(dataTokens), requiredService));
		return routeBuilder;
	}
}
