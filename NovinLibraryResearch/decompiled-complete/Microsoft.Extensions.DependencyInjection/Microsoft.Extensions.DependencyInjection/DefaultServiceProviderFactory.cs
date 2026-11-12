using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.")]
public class DefaultServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
{
	private readonly ServiceProviderOptions _options;

	public DefaultServiceProviderFactory()
		: this(ServiceProviderOptions.Default)
	{
	}

	public DefaultServiceProviderFactory(ServiceProviderOptions options)
	{
		_options = options ?? throw new ArgumentNullException("options");
	}

	public IServiceCollection CreateBuilder(IServiceCollection services)
	{
		return services;
	}

	public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
	{
		return containerBuilder.BuildServiceProvider(_options);
	}
}
