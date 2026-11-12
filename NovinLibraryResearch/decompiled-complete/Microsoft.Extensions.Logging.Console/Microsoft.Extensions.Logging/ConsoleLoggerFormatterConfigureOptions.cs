using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging;

[UnsupportedOSPlatform("browser")]
internal sealed class ConsoleLoggerFormatterConfigureOptions<TFormatter, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions> : ConfigureFromConfigurationOptions<TOptions> where TFormatter : ConsoleFormatter where TOptions : ConsoleFormatterOptions
{
	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public ConsoleLoggerFormatterConfigureOptions(ILoggerProviderConfiguration<ConsoleLoggerProvider> providerConfiguration)
		: base((IConfiguration)providerConfiguration.Configuration.GetSection("FormatterOptions"))
	{
	}
}
