using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IProductCompact : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	bool Special { get; set; }

	long Code { get; set; }

	string Description { get; set; }

	decimal DiscountPercent { get; set; }

	string IdProduct { get; set; }

	string Name { get; set; }

	Guid? V2Guid { get; set; }

	decimal V2Qt { get; set; }

	Guid GuidVahed { get; set; }

	decimal OrderPoint { get; set; }

	bool IsTax { get; set; }

	EnProductType Type { get; set; }

	ServerStatusForClient ServerStatus { get; set; }
}
