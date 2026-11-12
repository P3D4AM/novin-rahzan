using System;
using Serilog.Events;

namespace Serilog.Core.Filters;

internal class DelegateFilter : ILogEventFilter
{
	private readonly Func<LogEvent, bool> _isEnabled;

	public DelegateFilter(Func<LogEvent, bool> isEnabled)
	{
		_isEnabled = Guard.AgainstNull(isEnabled, "isEnabled");
	}

	public bool IsEnabled(LogEvent logEvent)
	{
		Guard.AgainstNull(logEvent, "logEvent");
		return _isEnabled(logEvent);
	}
}
