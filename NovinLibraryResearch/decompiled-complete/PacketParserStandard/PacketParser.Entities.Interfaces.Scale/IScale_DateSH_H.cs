using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_DateSH_H : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid ScaleGuid { get; set; }

	int FactorNumber { get; set; }

	DateTime UpdateDate { get; set; }

	int BatchGuid { get; set; }
}
