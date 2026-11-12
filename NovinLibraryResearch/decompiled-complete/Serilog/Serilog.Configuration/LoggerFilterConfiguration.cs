using System;
using Serilog.Core;
using Serilog.Core.Filters;
using Serilog.Events;

namespace Serilog.Configuration;

public class LoggerFilterConfiguration
{
	private readonly LoggerConfiguration _loggerConfiguration;

	private readonly Action<ILogEventFilter> _addFilter;

	internal LoggerFilterConfiguration(LoggerConfiguration loggerConfiguration, Action<ILogEventFilter> addFilter)
	{
		_loggerConfiguration = Guard.AgainstNull(loggerConfiguration, "loggerConfiguration");
		_addFilter = Guard.AgainstNull(addFilter, "addFilter");
	}

	public LoggerConfiguration With(params ILogEventFilter[] filters)
	{
		Guard.AgainstNull(filters, "filters");
		foreach (ILogEventFilter logEventFilter in filters)
		{
			if (logEventFilter == null)
			{
				throw new ArgumentException("Null filter is not allowed.");
			}
			_addFilter(logEventFilter);
		}
		return _loggerConfiguration;
	}

	public LoggerConfiguration With<TFilter>() where TFilter : ILogEventFilter, new()
	{
		return With(new TFilter());
	}

	public LoggerConfiguration ByExcluding(Func<LogEvent, bool> exclusionPredicate)
	{
		Func<LogEvent, bool> exclusionPredicate2 = exclusionPredicate;
		return With(new DelegateFilter((LogEvent logEvent) => !exclusionPredicate2(logEvent)));
	}

	public LoggerConfiguration ByIncludingOnly(Func<LogEvent, bool> inclusionPredicate)
	{
		return With(new DelegateFilter(inclusionPredicate));
	}
}
