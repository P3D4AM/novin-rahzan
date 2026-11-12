using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Exceptions.Core;

namespace Serilog.Exceptions;

public static class LoggerEnrichmentConfigurationExtensions
{
	public static LoggerConfiguration WithExceptionDetails(this LoggerEnrichmentConfiguration loggerEnrichmentConfiguration)
	{
		if (loggerEnrichmentConfiguration == null)
		{
			throw new ArgumentNullException("loggerEnrichmentConfiguration");
		}
		ExceptionEnricher exceptionEnricher = new ExceptionEnricher(new DestructuringOptionsBuilder().WithDefaultDestructurers().WithIgnoreStackTraceAndTargetSiteExceptionFilter());
		return loggerEnrichmentConfiguration.With(exceptionEnricher);
	}

	public static LoggerConfiguration WithExceptionDetails(this LoggerEnrichmentConfiguration loggerEnrichmentConfiguration, IDestructuringOptions destructuringOptions)
	{
		if (loggerEnrichmentConfiguration == null)
		{
			throw new ArgumentNullException("loggerEnrichmentConfiguration");
		}
		ILogEventEnricher logEventEnricher = new ExceptionEnricher(destructuringOptions);
		return loggerEnrichmentConfiguration.With(logEventEnricher);
	}
}
