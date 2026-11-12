using PacketParser.Entities;

namespace Server.Core.Connection.EventArgs;

public class HandleConnectionEventArgs
{
	public string ServerUniqueId { get; set; }

	public ConnectionData connection { get; set; }
}
