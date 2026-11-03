using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class SyncResetEventArgs : BaseEventArgs
{
	public EntityPart EntityPart { get; set; }

	public List<SyncResetItem> SyncResetItems { get; set; }
}
