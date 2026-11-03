using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_PRD : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	short MemoryNumber { get; set; }

	string MemoryName { get; set; }

	decimal MemoryPrice { get; set; }

	bool Vazni { get; set; }

	decimal TotalQT { get; set; }

	decimal TotalPrice { get; set; }

	Guid ProductGuid { get; set; }
}
