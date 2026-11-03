using PacketParser.Entities;

namespace PacketParser;

internal class ParseConnectionInfo
{
	public byte[] Bytes { get; set; }

	public string ConnectionUniqueId { get; set; } = "";


	public ENSource Source { get; set; }

	public short DepId { get; set; }

	public int UId { get; set; }

	public long LkSerial { get; set; }

	public string Ip { get; set; } = "";

}
