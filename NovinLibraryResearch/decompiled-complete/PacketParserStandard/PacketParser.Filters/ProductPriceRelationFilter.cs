using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class ProductPriceRelationFilter
{
	public Guid ProductGuid { get; set; }

	public Guid ProductGroupGuid { get; set; }

	public EnPriceLevelType? Type { get; set; }

	public Guid? PriceLevelGuid { get; set; }
}
