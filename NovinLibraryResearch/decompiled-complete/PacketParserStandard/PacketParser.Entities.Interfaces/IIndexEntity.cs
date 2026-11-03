using System;

namespace PacketParser.Entities.Interfaces;

public interface IIndexEntity
{
	int WordId { get; set; }

	Guid EntityGuid { get; set; }
}
