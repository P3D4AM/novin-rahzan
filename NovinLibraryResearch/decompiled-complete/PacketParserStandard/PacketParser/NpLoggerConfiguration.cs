using Microsoft.Extensions.Logging;

namespace PacketParser;

public class NpLoggerConfiguration
{
	public int EventId { get; set; }

	public LogLevel MinLogLevel { get; set; } = LogLevel.Error;

}
