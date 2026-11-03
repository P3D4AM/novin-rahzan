using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class BufferStateChangedEventArgs
{
	public EnConnectionState Status { get; set; }
}
