using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class OptionsConfigurationServiceCollectionExtensions
{
	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public static IServiceCollection Configure<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this IServiceCollection services, IConfiguration config) where TOptions : class
	{
		return services.Configure<TOptions>(Microsoft.Extensions.Options.Options.DefaultName, config);
	}

	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public static IServiceCollection Configure<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this IServiceCollection services, string name, IConfiguration config) where TOptions : class
	{
		return services.Configure<TOptions>(name, config, delegate
		{
		});
	}

	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public static IServiceCollection Configure<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this IServiceCollection services, IConfiguration config, Action<BinderOptions> configureBinder) where TOptions : class
	{
		return services.Configure<TOptions>(Microsoft.Extensions.Options.Options.DefaultName, config, configureBinder);
	}

	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public static IServiceCollection Configure<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this IServiceCollection services, string name, IConfiguration config, Action<BinderOptions> configureBinder) where TOptions : class
	{
		if (services == null)
		{
			throw new ArgumentNullException("services");
		}
		if (config == null)
		{
			throw new ArgumentNullException("config");
		}
		services.AddOptions();
		services.AddSingleton((IOptionsChangeTokenSource<TOptions>)new ConfigurationChangeTokenSource<TOptions>(name, config));
		return services.AddSingleton((IConfigureOptions<TOptions>)new NamedConfigureFromConfigurationOptions<TOptions>(name, config, configureBinder));
	}
}
