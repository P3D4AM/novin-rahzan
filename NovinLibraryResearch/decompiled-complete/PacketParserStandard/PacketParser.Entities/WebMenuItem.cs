using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace PacketParser.Entities;

public class WebMenuItem : IMenuItem, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	public EnMenuItemType Type { get; set; }

	public Guid TypeGuid { get; set; }

	public int Ordinal { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }
}
