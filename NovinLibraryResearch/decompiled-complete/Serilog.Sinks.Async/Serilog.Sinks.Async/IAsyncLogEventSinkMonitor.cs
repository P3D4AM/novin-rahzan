namespace Serilog.Sinks.Async;

public interface IAsyncLogEventSinkMonitor
{
	void StartMonitoring(IAsyncLogEventSinkInspector inspector);

	void StopMonitoring(IAsyncLogEventSinkInspector inspector);
}
