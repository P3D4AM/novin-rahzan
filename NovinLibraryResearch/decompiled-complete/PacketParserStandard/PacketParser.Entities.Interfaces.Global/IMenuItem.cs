using System;

namespace PacketParser.Entities.Interfaces.Global;

public interface IMenuItem : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	EnMenuItemType Type { get; set; }

	Guid TypeGuid { get; set; }

	int Ordinal { get; set; }
}
