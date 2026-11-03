using System;

namespace PacketParser.Entities.Interfaces;

public interface IProductMicro : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	long Code { get; set; }

	decimal Amount { get; set; }

	string IdProduct { get; set; }

	bool Special { get; set; }

	Guid? V2Guid { get; set; }

	decimal V2Qt { get; set; }

	EnProductKind Kind { get; set; }

	bool IsLastPriceForCustomer { get; set; }

	string Description { get; set; }

	decimal DiscountPercent { get; set; }

	Guid? MasterGuid { get; set; }
}
