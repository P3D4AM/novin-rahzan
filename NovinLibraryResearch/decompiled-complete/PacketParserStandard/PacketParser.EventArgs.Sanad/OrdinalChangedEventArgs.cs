using System;
using PacketParser.Entities;

namespace PacketParser.EventArgs.Sanad;

public class OrdinalChangedEventArgs : DisposedEventArgs
{
	public Guid DetailGuid { get; private set; }

	public int OldOrdinal { get; private set; }

	public int NewOrdinal { get; private set; }

	public OrdinalChangedEventArgs(int oldOrdinal, int newOrdinal, Guid detailGuid, EnSystem system)
		: base(system)
	{
		OldOrdinal = oldOrdinal;
		NewOrdinal = newOrdinal;
		DetailGuid = detailGuid;
	}
}
