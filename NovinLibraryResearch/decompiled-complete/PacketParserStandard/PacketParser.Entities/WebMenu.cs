using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace PacketParser.Entities;

public class WebMenu : IMenu, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public int RowCount { get; set; }

	public int ColumnCount { get; set; }

	public string Title { get; set; }

	public string Description { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public EnMenuType Type { get; set; }

	public Guid? MasterGuid { get; set; }
}
