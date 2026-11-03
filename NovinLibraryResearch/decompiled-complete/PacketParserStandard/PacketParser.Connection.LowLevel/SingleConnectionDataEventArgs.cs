using System;
using System.Threading;
using PacketParser.Entities;

namespace PacketParser.Connection.LowLevel;

public class SingleConnectionDataEventArgs : System.EventArgs
{
	public byte[] Data { get; set; }

	public CancellationToken cancellationToken { get; set; }

	public string ConnectionId { get; set; }

	public string Url { get; set; }

	public ENSource Source { get; set; }
}
