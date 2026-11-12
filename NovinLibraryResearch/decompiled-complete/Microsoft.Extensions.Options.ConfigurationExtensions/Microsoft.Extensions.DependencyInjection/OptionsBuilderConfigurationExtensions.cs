using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class OptionsBuilderConfigurationExtensions
{
	internal const string TrimmingRequiredUnreferencedCodeMessage = "TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.";

	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public static OptionsBuilder<TOptions> Bind<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this OptionsBuilder<TOptions> optionsBuilder, IConfiguration config) where TOptions : class
	{
		return optionsBuilder.Bind(config, delegate
		{
		});
	}

	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public static OptionsBuilder<TOptions> Bind<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this OptionsBuilder<TOptions> optionsBuilder, IConfiguration config, Action<BinderOptions> configureBinder) where TOptions : class
	{
		if (optionsBuilder == null)
		{
			throw new ArgumentNullException("optionsBuilder");
		}
		optionsBuilder.Services.Configure<TOptions>(optionsBuilder.Name, config, configureBinder);
		return optionsBuilder;
	}

	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public static OptionsBuilder<TOptions> BindConfiguration<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this OptionsBuilder<TOptions> optionsBuilder, string configSectionPath, Action<BinderOptions> configureBinder = null) where TOptions : class
	{
		if (optionsBuilder == null)
		{
			throw new ArgumentNullException("optionsBuilder");
		}
		if (configSectionPath == null)
		{
			throw new ArgumentNullException("configSectionPath");
		}
		optionsBuilder.Configure(delegate(TOptions opts, IConfiguration config)
		{
			BindFromOptions(opts, config, configSectionPath, configureBinder);
		});
		optionsBuilder.Services.AddSingleton<IOptionsChangeTokenSource<TOptions>, ConfigurationChangeTokenSource<TOptions>>();
		return optionsBuilder;
	}

	[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "The only call to this method is in BindConfiguration method which is already annotated as RequiresUnreferencedCode.")]
	private static void BindFromOptions<TOptions>(TOptions opts, IConfiguration config, string configSectionPath, Action<BinderOptions> configureBinder) where TOptions : class
	{
		IConfiguration configuration;
		if (!string.Equals("", configSectionPath, StringComparison.OrdinalIgnoreCase))
		{
			IConfiguration section = config.GetSection(configSectionPath);
			configuration = section;
		}
		else
		{
			configuration = config;
		}
		IConfiguration configuration2 = configuration;
		configuration2.Bind(opts, configureBinder);
	}
}
