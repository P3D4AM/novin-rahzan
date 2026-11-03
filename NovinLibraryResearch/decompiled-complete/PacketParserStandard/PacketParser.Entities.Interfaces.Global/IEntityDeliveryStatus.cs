using System;

namespace PacketParser.Entities.Interfaces.Global;

public interface IEntityDeliveryStatus
{
	EntityPart EntityPart { get; set; }

	bool IsTemp { get; set; }

	Guid EntityGuid { get; set; }
}
