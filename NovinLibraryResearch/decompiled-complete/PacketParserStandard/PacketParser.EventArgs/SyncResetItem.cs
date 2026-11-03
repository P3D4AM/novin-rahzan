using System;

namespace PacketParser.EventArgs;

public class SyncResetItem
{
	public int Id { get; set; }

	public Guid Guid { get; set; }
}
