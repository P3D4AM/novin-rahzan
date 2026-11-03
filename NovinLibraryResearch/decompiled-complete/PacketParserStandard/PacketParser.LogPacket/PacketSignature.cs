using PacketParser.Entities;

namespace PacketParser.LogPacket;

public struct PacketSignature
{
	public bool IsReceivedPacket { get; set; }

	public PacketType Type { get; set; }

	public short Action { get; set; }
}
