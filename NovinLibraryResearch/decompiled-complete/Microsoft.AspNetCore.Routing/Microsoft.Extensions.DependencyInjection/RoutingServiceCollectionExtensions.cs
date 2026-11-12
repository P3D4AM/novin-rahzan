using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.Extensions.DependencyInjection;

public static class RoutingServiceCollectionExtensions
{
	public static IServiceCollection AddRouting(this IServiceCollection services)
	{
		if (services == null)
		{
			throw new ArgumentNullException("services");
		}
		services.TryAddTransient<IInlineConstraintResolver, DefaultInlineConstraintResolver>();
		services.TryAddSingleton((IServiceProvider s) => s.GetRequiredService<ObjectPoolProvider>().Create(new UriBuilderContextPooledObjectPolicy()));
		services.TryAdd(ServiceDescriptor.Transient(delegate(IServiceProvider s)
		{
			ILoggerFactory requiredService = s.GetRequiredService<ILoggerFactory>();
			ObjectPool<UriBuildingContext> requiredService2 = s.GetRequiredService<ObjectPool<UriBuildingContext>>();
			IInlineConstraintResolver requiredService3 = s.GetRequiredService<IInlineConstraintResolver>();
			return new TreeRouteBuilder(requiredService, requiredService2, requiredService3);
		}));
		services.TryAddSingleton(typeof(RoutingMarkerService));
		return services;
	}

	public static IServiceCollection AddRouting(this IServiceCollection services, Action<RouteOptions> configureOptions)
	{
		if (services == null)
		{
			throw new ArgumentNullException("services");
		}
		if (configureOptions == null)
		{
			throw new ArgumentNullException("configureOptions");
		}
		services.Configure(configureOptions);
		services.AddRouting();
		return services;
	}
}
