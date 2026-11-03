using System;

namespace PacketParser.Connection.LowLevel;

public class RawDataEventArgs : System.EventArgs
{
	public string ConnectionUniqueId { get; private set; }

	public string Payload { get; set; }

	public RawDataEventArgs(string connectionUniqueId)
	{
		ConnectionUniqueId = connectionUniqueId;
	}
}
