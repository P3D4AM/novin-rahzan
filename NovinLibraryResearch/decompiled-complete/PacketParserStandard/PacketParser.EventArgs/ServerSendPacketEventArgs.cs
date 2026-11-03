using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class ServerSendPacketEventArgs
{
	public Packet Packet { get; set; }

	public Priority SendPriority { get; set; }

	public string ConnectionUniqueId { get; set; }

	public ServerSendPacketEventArgs(Packet packet, Priority sendPriority, string connectionUniqueId)
	{
		ConnectionUniqueId = connectionUniqueId;
		SendPriority = sendPriority;
		Packet = packet;
	}

	public ServerSendPacketEventArgs()
	{
	}
}
