using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebDecimal : IDecimal, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string Country { get; set; }

	public string Description { get; set; }

	public bool Status { get; set; }

	public string Abstract { get; set; }

	public DateTime LastUpdate { get; set; }

	public decimal LastPrice { get; set; }

	public short IconIndex { get; set; }

	public DateTime Modified { get; set; }

	public string Symbol { get; set; }
}
