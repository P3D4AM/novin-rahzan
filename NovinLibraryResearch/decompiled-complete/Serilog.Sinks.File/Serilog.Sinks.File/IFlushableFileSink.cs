namespace Serilog.Sinks.File;

public interface IFlushableFileSink
{
	void FlushToDisk();
}
