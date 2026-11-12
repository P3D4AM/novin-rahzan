using System.Runtime.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging;

[UnsupportedOSPlatform("browser")]
internal sealed class ConsoleLoggerFormatterOptionsChangeTokenSource<TFormatter, TOptions> : ConfigurationChangeTokenSource<TOptions> where TFormatter : ConsoleFormatter where TOptions : ConsoleFormatterOptions
{
	public ConsoleLoggerFormatterOptionsChangeTokenSource(ILoggerProviderConfiguration<ConsoleLoggerProvider> providerConfiguration)
		: base((IConfiguration)providerConfiguration.Configuration.GetSection("FormatterOptions"))
	{
	}
}
