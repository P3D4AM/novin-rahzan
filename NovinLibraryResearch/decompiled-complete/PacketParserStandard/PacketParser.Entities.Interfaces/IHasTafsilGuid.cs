using System;

namespace PacketParser.Entities.Interfaces;

public interface IHasTafsilGuid
{
	Guid TafsilGuid { get; }

	Guid? TafsilGuid2 { get; set; }

	Guid? TafsilGuid3 { get; set; }
}
