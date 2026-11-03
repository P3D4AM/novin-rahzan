using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid.Base;

public interface IBaseMavadBusiness : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	decimal Quantity { get; set; }

	string Serial { get; set; }

	string Description { get; set; }

	Guid? V2Guid { get; set; }

	decimal Price { get; }

	string ProductCode { get; }

	string ProductName { get; }

	string DepotName { get; }

	string PrdVahed { get; }
}
