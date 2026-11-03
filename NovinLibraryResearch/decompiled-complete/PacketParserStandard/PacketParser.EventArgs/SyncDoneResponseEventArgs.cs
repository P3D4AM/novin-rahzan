using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class SyncDoneResponseEventArgs : BaseEventArgs
{
	public SyncInformation SyncInfo { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
