using System;

namespace PacketParser.Entities.Interfaces;

public interface IEntitiesDeliveryStatus : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	EntityPart EntityPart { get; set; }

	bool SendToAcc { get; set; }

	short DepId { get; set; }

	long SenderHashId { get; set; }

	DateTime InsertTime { get; set; }
}
