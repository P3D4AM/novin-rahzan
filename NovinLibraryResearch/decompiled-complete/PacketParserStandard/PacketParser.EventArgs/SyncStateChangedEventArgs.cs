using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class SyncStateChangedEventArgs : BaseEventArgs
{
	public EnConnectionState PreviousState { get; set; }

	public EnConnectionState NextState { get; set; }

	public Sync Payload { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
