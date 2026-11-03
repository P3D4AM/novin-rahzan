using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace PacketParser.Entities;

public class WebCard : ICard, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }

	public EnCardTheme Theme { get; set; }

	public string ToolTipMessage { get; set; }

	public byte[]? Image { get; set; }

	public string BackColor { get; set; }

	public EnCardType Type { get; set; }
}
