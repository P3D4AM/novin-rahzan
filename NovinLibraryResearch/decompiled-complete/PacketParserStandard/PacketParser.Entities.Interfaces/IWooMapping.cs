using System;

namespace PacketParser.Entities.Interfaces;

public interface IWooMapping
{
	int DepId { get; set; }

	WooEntityType WooEntityType { get; set; }

	int Id { get; set; }

	Guid Guid { get; set; }

	long Crc { get; set; }

	string MetaData { get; set; }
}
