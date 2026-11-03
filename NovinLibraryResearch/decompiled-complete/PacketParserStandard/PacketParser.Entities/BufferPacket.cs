using System;

namespace PacketParser.Entities;

public class BufferPacket
{
	public int HashId { get; set; }

	public DateTime InsertTime { get; set; }

	public byte[] Content { get; set; }

	public PacketType PacketType { get; set; }

	public PacketAction PacketAction { get; set; }
}
