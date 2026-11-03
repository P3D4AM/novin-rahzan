using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebHesabGroup : IHesabGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public int Code { get; set; }

	public Guid TypeGuid { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }
}
