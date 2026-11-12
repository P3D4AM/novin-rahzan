using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace Serilog;

public static class LoggerSinkConfigurationExtensions
{
	public static LoggerConfiguration Providers(this LoggerSinkConfiguration configuration, LoggerProviderCollection providers, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null)
	{
		if (configuration == null)
		{
			throw new ArgumentNullException("configuration");
		}
		if (providers == null)
		{
			throw new ArgumentNullException("providers");
		}
		return configuration.Sink(new LoggerProviderCollectionSink(providers), restrictedToMinimumLevel, levelSwitch);
	}
}
