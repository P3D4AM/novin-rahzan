using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IDecimalUpdates : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid DecimalGuid { get; set; }

	DateTime DateM { get; set; }

	decimal Price { get; set; }
}
