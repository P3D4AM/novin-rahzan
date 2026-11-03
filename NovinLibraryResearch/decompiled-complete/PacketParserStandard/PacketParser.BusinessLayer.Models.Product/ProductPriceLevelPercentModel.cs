using System;

namespace PacketParser.BusinessLayer.Models.Product;

public class ProductPriceLevelPercentModel
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public short Ordinal { get; set; }

	public decimal Percent { get; set; }
}
