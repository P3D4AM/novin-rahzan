using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Capturing;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Core.Sinks;
using Serilog.Events;

namespace Serilog;

public class LoggerConfiguration
{
	private readonly List<ILogEventSink> _logEventSinks = new List<ILogEventSink>();

	private readonly List<ILogEventSink> _auditSinks = new List<ILogEventSink>();

	private readonly List<ILogEventEnricher> _enrichers = new List<ILogEventEnricher>();

	private readonly List<ILogEventFilter> _filters = new List<ILogEventFilter>();

	private readonly List<Type> _additionalScalarTypes = new List<Type>();

	private readonly HashSet<Type> _additionalDictionaryTypes = new HashSet<Type>();

	private readonly List<IDestructuringPolicy> _additionalDestructuringPolicies = new List<IDestructuringPolicy>();

	private readonly Dictionary<string, LoggingLevelSwitch> _overrides = new Dictionary<string, LoggingLevelSwitch>();

	private LogEventLevel _minimumLevel = LogEventLevel.Information;

	private LoggingLevelSwitch? _levelSwitch;

	private int _maximumDestructuringDepth = 10;

	private int _maximumStringLength = int.MaxValue;

	private int _maximumCollectionCount = int.MaxValue;

	private bool _loggerCreated;

	public LoggerSinkConfiguration WriteTo { get; internal set; }

	public LoggerAuditSinkConfiguration AuditTo => new LoggerAuditSinkConfiguration(this, delegate(ILogEventSink s)
	{
		_auditSinks.Add(s);
	});

	public LoggerMinimumLevelConfiguration MinimumLevel => new LoggerMinimumLevelConfiguration(this, delegate(LogEventLevel l)
	{
		_minimumLevel = l;
		_levelSwitch = null;
	}, delegate(LoggingLevelSwitch sw)
	{
		_levelSwitch = sw;
	}, delegate(string s, LoggingLevelSwitch lls)
	{
		_overrides[s] = lls;
	});

	public LoggerEnrichmentConfiguration Enrich { get; internal set; }

	public LoggerFilterConfiguration Filter => new LoggerFilterConfiguration(this, delegate(ILogEventFilter f)
	{
		_filters.Add(f);
	});

	public LoggerDestructuringConfiguration Destructure => new LoggerDestructuringConfiguration(this, _additionalScalarTypes.Add, delegate(Type type)
	{
		_additionalDictionaryTypes.Add(type);
	}, _additionalDestructuringPolicies.Add, delegate(int depth)
	{
		_maximumDestructuringDepth = depth;
	}, delegate(int length)
	{
		_maximumStringLength = length;
	}, delegate(int count)
	{
		_maximumCollectionCount = count;
	});

	public LoggerSettingsConfiguration ReadFrom => new LoggerSettingsConfiguration(this);

	public LoggerConfiguration()
	{
		WriteTo = new LoggerSinkConfiguration(this, delegate(ILogEventSink s)
		{
			_logEventSinks.Add(s);
		});
		Enrich = new LoggerEnrichmentConfiguration(this, delegate(ILogEventEnricher e)
		{
			_enrichers.Add(e);
		});
	}

	public Logger CreateLogger()
	{
		if (_loggerCreated)
		{
			throw new InvalidOperationException("CreateLogger() was previously called and can only be called once.");
		}
		_loggerCreated = true;
		ILogEventSink logEventSink = null;
		if (_logEventSinks.Count > 0)
		{
			logEventSink = new SafeAggregateSink(_logEventSinks);
		}
		bool flag = _auditSinks.Any();
		if (flag)
		{
			IEnumerable<ILogEventSink> sinks;
			if (logEventSink != null)
			{
				sinks = new ILogEventSink[1] { logEventSink }.Concat(_auditSinks);
			}
			else
			{
				IEnumerable<ILogEventSink> auditSinks = _auditSinks;
				sinks = auditSinks;
			}
			logEventSink = new AggregateSink(sinks);
		}
		if (logEventSink == null)
		{
			logEventSink = new SafeAggregateSink(Array.Empty<ILogEventSink>());
		}
		if (_filters.Any())
		{
			logEventSink = new FilteringSink(logEventSink, _filters, flag);
		}
		MessageTemplateProcessor messageTemplateProcessor = new MessageTemplateProcessor(new PropertyValueConverter(_maximumDestructuringDepth, _maximumStringLength, _maximumCollectionCount, _additionalScalarTypes, _additionalDictionaryTypes, _additionalDestructuringPolicies, flag));
		ILogEventEnricher enricher = _enrichers.Count switch
		{
			0 => new EmptyEnricher(), 
			1 => _enrichers[0], 
			_ => new SafeAggregateEnricher(_enrichers), 
		};
		LevelOverrideMap overrideMap = null;
		if (_overrides.Count != 0)
		{
			overrideMap = new LevelOverrideMap(_overrides, _minimumLevel, _levelSwitch);
		}
		ILogEventSink[] disposableSinks = (from s in _logEventSinks.Concat(_auditSinks)
			where s is IDisposable
			select s).ToArray();
		return new Logger(messageTemplateProcessor, (_levelSwitch == null) ? _minimumLevel : LogEventLevel.Verbose, _levelSwitch, logEventSink, enricher, Dispose, overrideMap);
		void Dispose()
		{
			ILogEventSink[] array = disposableSinks;
			for (int i = 0; i < array.Length; i++)
			{
				(array[i] as IDisposable)?.Dispose();
			}
		}
	}
}
