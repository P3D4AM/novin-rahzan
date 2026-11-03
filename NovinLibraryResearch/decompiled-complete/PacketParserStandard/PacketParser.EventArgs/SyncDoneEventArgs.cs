using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class SyncDoneEventArgs : BaseEventArgs
{
	public int PartitionId;

	public EntityPart SyncPart { get; set; }

	[DefaultValue(true)]
	public bool Result { get; set; } = true;


	public string Message { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
