using System;

namespace PacketParser.Entities.Interfaces.Accounting.Bill;

public interface IBillDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	Guid PlacementGuid { get; set; }

	Guid OrderGuid { get; set; }

	new Guid MasterGuid { get; set; }
}
