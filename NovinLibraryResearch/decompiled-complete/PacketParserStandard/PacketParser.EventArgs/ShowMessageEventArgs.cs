using Serilog.Events;

namespace PacketParser.EventArgs;

public class ShowMessageEventArgs
{
	public string Title { get; set; }

	public string Message { get; set; }

	public LogEventLevel Level { get; set; }
}
