using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.File;

internal interface IFileSink : ILogEventSink, IFlushableFileSink
{
	bool EmitOrOverflow(LogEvent logEvent);
}
