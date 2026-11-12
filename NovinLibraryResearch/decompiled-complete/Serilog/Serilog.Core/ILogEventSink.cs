using Serilog.Events;

namespace Serilog.Core;

public interface ILogEventSink
{
	void Emit(LogEvent logEvent);
}
