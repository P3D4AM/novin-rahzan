using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionContainerBuilderExtensions
{
	[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.")]
	public static ServiceProvider BuildServiceProvider(this IServiceCollection services)
	{
		return services.BuildServiceProvider(ServiceProviderOptions.Default);
	}

	[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.")]
	public static ServiceProvider BuildServiceProvider(this IServiceCollection services, bool validateScopes)
	{
		return services.BuildServiceProvider(new ServiceProviderOptions
		{
			ValidateScopes = validateScopes
		});
	}

	[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.")]
	public static ServiceProvider BuildServiceProvider(this IServiceCollection services, ServiceProviderOptions options)
	{
		if (services == null)
		{
			throw new ArgumentNullException("services");
		}
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		return new ServiceProvider(services, options);
	}
}
