using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebHesabType : IHesabType, IHasJustGuid, IHasStatus
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public short Code { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;

}
