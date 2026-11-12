namespace Serilog.Sinks.Async;

public interface IAsyncLogEventSinkInspector
{
	int BufferSize { get; }

	int Count { get; }

	long DroppedMessagesCount { get; }
}
