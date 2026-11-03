using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class DeleteResponseEventArgs : BaseEventArgs
{
	public short PartitionId;

	public EntityPart SyncPart { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
