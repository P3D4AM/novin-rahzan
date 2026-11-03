using PacketParser.Entities;

namespace PacketParser.EventArgs.Connection;

public class ConnectionStateEventArgs
{
	public string ConnectionUrl { get; set; }

	public EnConnectionState PreviousState { get; set; }

	public EnConnectionState NewState { get; set; }
}
