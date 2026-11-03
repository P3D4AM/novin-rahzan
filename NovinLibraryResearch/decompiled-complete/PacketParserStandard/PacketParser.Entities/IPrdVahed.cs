using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IPrdVahed : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Description { get; set; }

	string Name { get; set; }

	ServerStatusForClient ServerStatus { get; set; }

	bool DecimalSales { get; set; }

	Guid? TaxUnitGuid { get; set; }
}
