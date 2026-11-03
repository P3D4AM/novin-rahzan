using System;

namespace PacketParser.Entities.Interfaces.Accounting.DepotReceipt;

public interface IDepotReceipt : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int Id2 { get; set; }

	DateTime DateM { get; set; }

	Guid? TafsilGuid { get; set; }

	string Description { get; set; }

	bool IsTemp { get; set; }

	Guid CashierGuid { get; set; }

	string CarNumber { get; set; }

	string DriverName { get; set; }

	string DriverPhone { get; set; }

	string BarnameNo { get; set; }

	string RefrencedNumber { get; set; }

	EnDepotReceiptType Type { get; set; }

	Guid? OrderGuid { get; set; }
}
