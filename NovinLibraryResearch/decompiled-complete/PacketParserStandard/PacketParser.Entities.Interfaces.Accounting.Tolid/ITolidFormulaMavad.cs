using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid;

public interface ITolidFormulaMavad : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	Guid ProductGuid { get; set; }

	Guid DepotGuid { get; set; }

	decimal Quantity { get; set; }

	string Serial { get; set; }

	string Description { get; set; }

	Guid? V2Guid { get; set; }
}
