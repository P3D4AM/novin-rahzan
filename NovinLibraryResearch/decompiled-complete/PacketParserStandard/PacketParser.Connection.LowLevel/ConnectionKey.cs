namespace PacketParser.Connection.LowLevel;

public class ConnectionKey
{
	public byte[] SymmetricKey { get; set; }

	public byte[] SymmetricIv { get; set; }
}
