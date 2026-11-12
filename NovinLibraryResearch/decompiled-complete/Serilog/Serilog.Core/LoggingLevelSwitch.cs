using System;
using Serilog.Events;

namespace Serilog.Core;

public class LoggingLevelSwitch
{
	private volatile LogEventLevel _minimumLevel;

	private readonly object _levelUpdateLock = new object();

	public LogEventLevel MinimumLevel
	{
		get
		{
			return _minimumLevel;
		}
		set
		{
			lock (_levelUpdateLock)
			{
				LogEventLevel minimumLevel = _minimumLevel;
				if (minimumLevel != value)
				{
					_minimumLevel = value;
					this.MinimumLevelChanged?.Invoke(this, new LoggingLevelSwitchChangedEventArgs(minimumLevel, value));
				}
			}
		}
	}

	public event EventHandler<LoggingLevelSwitchChangedEventArgs>? MinimumLevelChanged;

	public LoggingLevelSwitch(LogEventLevel initialMinimumLevel = LogEventLevel.Information)
	{
		_minimumLevel = initialMinimumLevel;
	}
}
