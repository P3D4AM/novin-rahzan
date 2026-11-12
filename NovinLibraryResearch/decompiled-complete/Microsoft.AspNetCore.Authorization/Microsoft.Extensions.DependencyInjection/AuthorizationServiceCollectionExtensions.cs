using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthorizationServiceCollectionExtensions
{
	public static IServiceCollection AddAuthorization(this IServiceCollection services)
	{
		if (services == null)
		{
			throw new ArgumentNullException("services");
		}
		services.TryAdd(ServiceDescriptor.Transient<IAuthorizationService, DefaultAuthorizationService>());
		services.TryAdd(ServiceDescriptor.Transient<IAuthorizationPolicyProvider, DefaultAuthorizationPolicyProvider>());
		services.TryAdd(ServiceDescriptor.Transient<IAuthorizationHandlerProvider, DefaultAuthorizationHandlerProvider>());
		services.TryAdd(ServiceDescriptor.Transient<IAuthorizationEvaluator, DefaultAuthorizationEvaluator>());
		services.TryAdd(ServiceDescriptor.Transient<IAuthorizationHandlerContextFactory, DefaultAuthorizationHandlerContextFactory>());
		services.TryAddEnumerable(ServiceDescriptor.Transient<IAuthorizationHandler, PassThroughAuthorizationHandler>());
		return services;
	}

	public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<AuthorizationOptions> configure)
	{
		if (services == null)
		{
			throw new ArgumentNullException("services");
		}
		if (configure == null)
		{
			throw new ArgumentNullException("configure");
		}
		services.Configure(configure);
		return services.AddAuthorization();
	}
}
