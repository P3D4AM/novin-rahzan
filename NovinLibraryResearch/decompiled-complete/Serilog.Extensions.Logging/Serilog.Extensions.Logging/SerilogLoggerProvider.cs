using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Extensions.Logging;

[ProviderAlias("Serilog")]
public class SerilogLoggerProvider : ILoggerProvider, IDisposable, ILogEventEnricher
{
	internal const string OriginalFormatPropertyName = "{OriginalFormat}";

	internal const string ScopePropertyName = "Scope";

	private readonly ILogger? _logger;

	private readonly Action? _dispose;

	private readonly AsyncLocal<SerilogLoggerScope?> _value = new AsyncLocal<SerilogLoggerScope>();

	internal SerilogLoggerScope? CurrentScope
	{
		get
		{
			return _value.Value;
		}
		set
		{
			_value.Value = value;
		}
	}

	public SerilogLoggerProvider(ILogger? logger = null, bool dispose = false)
	{
		ILogger logger2 = logger;
		base._002Ector();
		if (logger2 != null)
		{
			_logger = logger2.ForContext(new SerilogLoggerProvider[1] { this });
		}
		if (!dispose)
		{
			return;
		}
		if (logger2 != null)
		{
			_dispose = delegate
			{
				(logger2 as IDisposable)?.Dispose();
			};
		}
		else
		{
			_dispose = Log.CloseAndFlush;
		}
	}

	public Microsoft.Extensions.Logging.ILogger CreateLogger(string name)
	{
		return new SerilogLogger(this, _logger, name);
	}

	public IDisposable BeginScope<T>(T state)
	{
		if (CurrentScope != null)
		{
			return new SerilogLoggerScope(this, state);
		}
		IDisposable chainedDisposable = LogContext.Push(this);
		return new SerilogLoggerScope(this, state, chainedDisposable);
	}

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		List<LogEventPropertyValue> list = null;
		for (SerilogLoggerScope serilogLoggerScope = CurrentScope; serilogLoggerScope != null; serilogLoggerScope = serilogLoggerScope.Parent)
		{
			serilogLoggerScope.EnrichAndCreateScopeItem(logEvent, propertyFactory, out LogEventPropertyValue scopeItem);
			if (scopeItem != null)
			{
				if (list == null)
				{
					list = new List<LogEventPropertyValue>();
				}
				list.Add(scopeItem);
			}
		}
		if (list != null)
		{
			list.Reverse();
			logEvent.AddPropertyIfAbsent(new LogEventProperty("Scope", new SequenceValue(list)));
		}
	}

	public void Dispose()
	{
		_dispose?.Invoke();
	}
}
