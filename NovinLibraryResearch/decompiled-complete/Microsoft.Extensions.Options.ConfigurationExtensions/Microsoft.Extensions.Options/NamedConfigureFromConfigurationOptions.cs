using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Options;

public class NamedConfigureFromConfigurationOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions> : ConfigureNamedOptions<TOptions> where TOptions : class
{
	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public NamedConfigureFromConfigurationOptions(string name, IConfiguration config)
		: this(name, config, (Action<BinderOptions>)delegate
		{
		})
	{
	}

	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public NamedConfigureFromConfigurationOptions(string name, IConfiguration config, Action<BinderOptions> configureBinder)
		: base(name, (Action<TOptions>?)delegate(TOptions options)
		{
			BindFromOptions(options, config, configureBinder);
		})
	{
		if (config == null)
		{
			throw new ArgumentNullException("config");
		}
	}

	[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "The only call to this method is the constructor which is already annotated as RequiresUnreferencedCode.")]
	private static void BindFromOptions(TOptions options, IConfiguration config, Action<BinderOptions> configureBinder)
	{
		config.Bind(options, configureBinder);
	}
}
