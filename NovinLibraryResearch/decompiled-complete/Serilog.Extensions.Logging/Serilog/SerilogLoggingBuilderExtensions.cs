using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;

namespace Serilog;

public static class SerilogLoggingBuilderExtensions
{
	public static ILoggingBuilder AddSerilog(this ILoggingBuilder builder, ILogger? logger = null, bool dispose = false)
	{
		ILogger logger2 = logger;
		if (builder == null)
		{
			throw new ArgumentNullException("builder");
		}
		if (dispose)
		{
			builder.Services.AddSingleton<ILoggerProvider, SerilogLoggerProvider>((IServiceProvider services) => new SerilogLoggerProvider(logger2, dispose: true));
		}
		else
		{
			builder.AddProvider(new SerilogLoggerProvider(logger2));
		}
		builder.AddFilter<SerilogLoggerProvider>(null, LogLevel.Trace);
		return builder;
	}
}
