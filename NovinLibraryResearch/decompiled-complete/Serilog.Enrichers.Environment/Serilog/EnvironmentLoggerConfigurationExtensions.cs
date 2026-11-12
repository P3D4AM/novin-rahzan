using System;
using Serilog.Configuration;
using Serilog.Enrichers;

namespace Serilog;

public static class EnvironmentLoggerConfigurationExtensions
{
	public static LoggerConfiguration WithEnvironmentName(this LoggerEnrichmentConfiguration enrichmentConfiguration)
	{
		if (enrichmentConfiguration == null)
		{
			throw new ArgumentNullException("enrichmentConfiguration");
		}
		return enrichmentConfiguration.With<EnvironmentNameEnricher>();
	}

	public static LoggerConfiguration WithMachineName(this LoggerEnrichmentConfiguration enrichmentConfiguration)
	{
		if (enrichmentConfiguration == null)
		{
			throw new ArgumentNullException("enrichmentConfiguration");
		}
		return enrichmentConfiguration.With<MachineNameEnricher>();
	}

	public static LoggerConfiguration WithEnvironmentUserName(this LoggerEnrichmentConfiguration enrichmentConfiguration)
	{
		if (enrichmentConfiguration == null)
		{
			throw new ArgumentNullException("enrichmentConfiguration");
		}
		return enrichmentConfiguration.With<EnvironmentUserNameEnricher>();
	}

	public static LoggerConfiguration WithEnvironmentVariable(this LoggerEnrichmentConfiguration enrichmentConfiguration, string environmentVariableName, string propertyName = null)
	{
		if (enrichmentConfiguration == null)
		{
			throw new ArgumentNullException("enrichmentConfiguration");
		}
		EnvironmentVariableEnricher environmentVariableEnricher = new EnvironmentVariableEnricher(environmentVariableName, propertyName);
		return enrichmentConfiguration.With(environmentVariableEnricher);
	}
}
