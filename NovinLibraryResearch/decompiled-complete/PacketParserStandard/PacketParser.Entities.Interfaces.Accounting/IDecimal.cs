using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IDecimal : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	string Country { get; set; }

	string Description { get; set; }

	string Abstract { get; set; }

	DateTime LastUpdate { get; set; }

	decimal LastPrice { get; set; }

	short IconIndex { get; set; }

	string Symbol { get; set; }
}
