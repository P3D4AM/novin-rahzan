using Serilog.Events;

namespace Serilog.Sinks.EventLog;

public interface IEventIdProvider
{
	ushort ComputeEventId(LogEvent logEvent);
}
