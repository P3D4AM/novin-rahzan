using Serilog.Events;

namespace Serilog.Core;

public interface ILogEventFilter
{
	bool IsEnabled(LogEvent logEvent);
}
