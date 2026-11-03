using System;

namespace PacketParser.Entities.Interfaces.Accounting.Product;

public interface IProductPriceRelation : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid ProductGuid { get; set; }

	decimal Price { get; set; }

	decimal Quantity { get; set; }

	decimal Profit { get; set; }

	decimal KasrRial { get; set; }

	Guid PriceLevelGuid { get; set; }

	EnPriceLevelType Type { get; set; }

	Guid? UnitGuid { get; set; }
}
