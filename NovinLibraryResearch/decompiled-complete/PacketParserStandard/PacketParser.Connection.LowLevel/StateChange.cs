using PacketParser.Entities;

namespace PacketParser.Connection.LowLevel;

public class StateChange
{
	public EnConnectionState OldState { get; set; }

	public EnConnectionState NewState { get; set; }
}
