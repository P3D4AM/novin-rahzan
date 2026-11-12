namespace Serilog.Sinks.Http.Private.Durable;

public interface IBufferFiles
{
	string BookmarkFileName { get; }

	string[] Get();
}
