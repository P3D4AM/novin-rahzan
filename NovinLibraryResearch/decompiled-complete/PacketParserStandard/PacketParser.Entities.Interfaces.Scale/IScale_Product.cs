using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_Product : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription
{
	string Name { get; set; }

	string Code { get; set; }

	Guid GroupGuid { get; set; }

	Guid VahedGuid { get; set; }

	bool IsWeight { get; set; }

	decimal Price { get; set; }

	string Barcode { get; set; }
}
