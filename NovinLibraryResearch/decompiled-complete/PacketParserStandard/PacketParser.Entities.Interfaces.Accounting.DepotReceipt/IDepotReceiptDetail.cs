using System;

namespace PacketParser.Entities.Interfaces.Accounting.DepotReceipt;

public interface IDepotReceiptDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	string Description { get; set; }

	int Ordinal { get; set; }

	Guid? ReferenceGuid { get; set; }
}
