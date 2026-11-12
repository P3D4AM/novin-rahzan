using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Options;

public class ConfigureFromConfigurationOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions> : ConfigureOptions<TOptions> where TOptions : class
{
	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public ConfigureFromConfigurationOptions(IConfiguration config)
		: base((Action<TOptions>?)delegate(TOptions options)
		{
			BindFromOptions(options, config);
		})
	{
		if (config == null)
		{
			throw new ArgumentNullException("config");
		}
	}

	[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "The only call to this method is the constructor which is already annotated as RequiresUnreferencedCode.")]
	private static void BindFromOptions(TOptions options, IConfiguration config)
	{
		config.Bind(options);
	}
}
