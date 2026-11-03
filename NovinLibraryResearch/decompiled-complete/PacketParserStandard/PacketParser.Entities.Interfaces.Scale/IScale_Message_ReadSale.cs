using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_Message_ReadSale : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid ScaleGuid { get; set; }

	Guid MemoryGuid { get; set; }

	int Count { get; set; }

	DateTime DateM { get; set; }

	short Priority { get; set; }
}
