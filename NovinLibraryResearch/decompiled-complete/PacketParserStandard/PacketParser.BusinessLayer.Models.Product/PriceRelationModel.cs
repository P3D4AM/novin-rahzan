using System;

namespace PacketParser.BusinessLayer.Models.Product;

public class PriceRelationModel
{
	public Guid Guid { get; set; }

	public Guid ProductGuid { get; set; }

	public Guid PriceLevelGuid { get; set; }

	public decimal Price { get; set; }

	public decimal Quantity { get; set; }

	public decimal Profit { get; set; }

	public decimal KasrRial { get; set; }

	public decimal KardexPrice { get; set; }

	public int Ordinal { get; set; }
}
