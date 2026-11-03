using System;
using PacketParser.Entities;

namespace PacketParser.LogPacket;

public class WebLogPacket
{
	public long Id { get; set; }

	public DateTime Date { get; set; }

	public WebContextHolder Context { get; set; }

	public string ConnectionUniqueId { get; set; }

	public int DepId { get; set; }

	public long HashId { get; set; }

	public long PacketLengh { get; set; }

	public short PacketType { get; set; }

	public short PacketAction { get; set; }

	public string PacketRefrenced { get; set; }

	public bool IsProcess { get; set; }

	public bool IsSentPacket { get; set; }

	public string ExteraDataForDebug { get; set; }
}
