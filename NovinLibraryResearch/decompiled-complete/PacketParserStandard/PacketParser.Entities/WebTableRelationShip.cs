using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebTableRelationShip : ITableRelationShip, IHasJustGuid
{
	public Guid Guid { get; set; }

	public decimal Price { get; set; }

	public Guid SanadGuid { get; set; }

	public Document SanadType { get; set; }
}
