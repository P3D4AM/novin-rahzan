using System;

namespace PacketParser.Entities.Interfaces;

public interface ITableRelationShip : IHasJustGuid
{
	decimal Price { get; set; }

	Guid SanadGuid { get; set; }

	Document SanadType { get; set; }
}
