using System;

namespace PacketParser.Entities;

public interface IBarcode
{
	string BarCode { get; set; }

	DateTime CreateDate { get; set; }

	string Description { get; set; }

	string Name { get; set; }

	decimal Price { get; set; }

	decimal Quantity { get; set; }

	BarcodePriceType Type { get; set; }

	Guid? DepotGuid { get; set; }

	short Ordinal { get; set; }

	Guid? PriceTypeGuid { get; set; }
}
