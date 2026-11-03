using System.Collections.Generic;

namespace PacketParser.EventArgs.Connection;

public class LoggerSendEventArgs
{
	public List<string> Connections { get; set; }

	public short DepId { get; set; }

	public short Source { get; set; }

	public int UserId { get; set; }

	public Packet pack { get; set; }
}
