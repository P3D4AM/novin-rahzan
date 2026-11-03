using System;

namespace PacketParser.Entities.Interfaces.Accounting.TozieList;

public interface ITozieList : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int Number { get; set; }

	DateTime DateM { get; set; }

	string DriverName { get; set; }

	string DriverMobile1 { get; set; }

	string DriverMobile2 { get; set; }

	string CarNumber { get; set; }

	string Description { get; set; }

	Guid CashierGuid { get; set; }
}
