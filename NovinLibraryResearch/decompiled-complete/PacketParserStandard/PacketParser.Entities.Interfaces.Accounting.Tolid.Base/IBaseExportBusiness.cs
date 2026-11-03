using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid.Base;

public interface IBaseExportBusiness : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	string PrdName { get; }

	string PrdVahed { get; }

	string PrdCode { get; }

	Guid DepotGuid { get; set; }

	decimal Quantity { get; set; }

	Guid? V2Guid { get; set; }

	string Description { get; set; }

	decimal Price { get; }
}
