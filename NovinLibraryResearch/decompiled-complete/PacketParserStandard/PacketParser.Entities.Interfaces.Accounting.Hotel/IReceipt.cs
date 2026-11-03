using System;

namespace PacketParser.Entities.Interfaces.Accounting.Hotel;

public interface IReceipt : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Description { get; set; }

	int Number { get; set; }

	decimal Discount { get; set; }

	Guid CustomerGuid { get; set; }

	decimal Penalty { get; }

	int PepolCount { get; set; }

	Guid OrderGuid { get; set; }

	DateTime DateM { get; set; }

	Guid? PresenterGuid { get; set; }
}
