using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class SyncResetDoneEventArgs : BaseEventArgs
{
	public EntityPart SyncPart { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
