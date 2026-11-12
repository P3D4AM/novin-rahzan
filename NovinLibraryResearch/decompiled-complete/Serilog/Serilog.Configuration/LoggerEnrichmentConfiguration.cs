using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Configuration;

public class LoggerEnrichmentConfiguration
{
	private readonly LoggerConfiguration _loggerConfiguration;

	private readonly Action<ILogEventEnricher> _addEnricher;

	internal LoggerEnrichmentConfiguration(LoggerConfiguration loggerConfiguration, Action<ILogEventEnricher> addEnricher)
	{
		_loggerConfiguration = Guard.AgainstNull(loggerConfiguration, "loggerConfiguration");
		_addEnricher = Guard.AgainstNull(addEnricher, "addEnricher");
	}

	public LoggerConfiguration With(params ILogEventEnricher[] enrichers)
	{
		Guard.AgainstNull(enrichers, "enrichers");
		foreach (ILogEventEnricher logEventEnricher in enrichers)
		{
			Guard.AgainstNull(logEventEnricher, "logEventEnricher");
			_addEnricher(logEventEnricher);
		}
		return _loggerConfiguration;
	}

	public LoggerConfiguration With<TEnricher>() where TEnricher : ILogEventEnricher, new()
	{
		return With(new TEnricher());
	}

	public LoggerConfiguration WithProperty(string name, object? value, bool destructureObjects = false)
	{
		return With(new PropertyEnricher(name, value, destructureObjects));
	}

	public LoggerConfiguration FromLogContext()
	{
		return With<LogContextEnricher>();
	}

	public LoggerConfiguration When(Func<LogEvent, bool> condition, Action<LoggerEnrichmentConfiguration> configureEnricher)
	{
		Func<LogEvent, bool> condition2 = condition;
		Guard.AgainstNull(condition2, "condition");
		Guard.AgainstNull(configureEnricher, "configureEnricher");
		return Wrap(this, (ILogEventEnricher e) => new ConditionalEnricher(e, condition2), configureEnricher);
	}

	public LoggerConfiguration AtLevel(LogEventLevel enrichFromLevel, Action<LoggerEnrichmentConfiguration> configureEnricher)
	{
		Guard.AgainstNull(configureEnricher, "configureEnricher");
		return Wrap(this, (ILogEventEnricher e) => new ConditionalEnricher(e, (LogEvent le) => le.Level >= enrichFromLevel), configureEnricher);
	}

	public LoggerConfiguration AtLevel(LoggingLevelSwitch levelSwitch, Action<LoggerEnrichmentConfiguration> configureEnricher)
	{
		LoggingLevelSwitch levelSwitch2 = levelSwitch;
		Guard.AgainstNull(configureEnricher, "configureEnricher");
		return Wrap(this, (ILogEventEnricher e) => new ConditionalEnricher(e, (LogEvent le) => le.Level >= levelSwitch2.MinimumLevel), configureEnricher);
	}

	public static LoggerConfiguration Wrap(LoggerEnrichmentConfiguration loggerEnrichmentConfiguration, Func<ILogEventEnricher, ILogEventEnricher> wrapEnricher, Action<LoggerEnrichmentConfiguration> configureWrappedEnricher)
	{
		Guard.AgainstNull(loggerEnrichmentConfiguration, "loggerEnrichmentConfiguration");
		Guard.AgainstNull(wrapEnricher, "wrapEnricher");
		Guard.AgainstNull(configureWrappedEnricher, "configureWrappedEnricher");
		List<ILogEventEnricher> list = new List<ILogEventEnricher>();
		LoggerConfiguration loggerConfiguration = new LoggerConfiguration();
		LoggerEnrichmentConfiguration obj = (loggerConfiguration.Enrich = new LoggerEnrichmentConfiguration(loggerConfiguration, list.Add));
		configureWrappedEnricher(obj);
		if (list.Count == 0)
		{
			return loggerEnrichmentConfiguration._loggerConfiguration;
		}
		ILogEventEnricher logEventEnricher2;
		if (list.Count != 1)
		{
			ILogEventEnricher logEventEnricher = new SafeAggregateEnricher(list);
			logEventEnricher2 = logEventEnricher;
		}
		else
		{
			logEventEnricher2 = list.Single();
		}
		ILogEventEnricher arg = logEventEnricher2;
		ILogEventEnricher logEventEnricher3 = wrapEnricher(arg);
		if (!(logEventEnricher3 is IDisposable))
		{
			SelfLog.WriteLine("Wrapping enricher {0} does not implement IDisposable; to ensure wrapped enrichers are properly disposed, wrappers should dispose their wrapped contents", logEventEnricher3);
		}
		return loggerEnrichmentConfiguration.With(logEventEnricher3);
	}
}
