using System;
using Serilog.Events;

namespace Serilog.Core;

public class LoggingLevelSwitchChangedEventArgs : EventArgs
{
	public LogEventLevel OldLevel { get; }

	public LogEventLevel NewLevel { get; }

	public LoggingLevelSwitchChangedEventArgs(LogEventLevel oldLevel, LogEventLevel newLevel)
	{
		OldLevel = oldLevel;
		NewLevel = newLevel;
	}
}
