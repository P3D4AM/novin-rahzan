using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebWooMapping : WooMapping, IWooMapping
{
	public int DepId { get; set; }

	public WooEntityType WooEntityType { get; set; }

	public Guid Guid { get; set; }

	public string MetaData { get; set; }
}
