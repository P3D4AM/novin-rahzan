using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class ProductPriceRelationModel
{
	public Guid Guid { get; set; }

	public bool Status { get; set; }

	public Guid ProductGuid { get; set; }

	public decimal Price { get; set; }

	public Guid PriceLevelGuid { get; set; }

	public string PriceLevelName { get; set; }

	public EnPriceLevelType Type { get; set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();

	public short Ordinal { get; set; }

	public decimal Profit { get; set; }

	public decimal KasrRial { get; set; }

	public decimal Quantity { get; set; }

	public string TitleForDisplay => PriceLevelName + " (" + Price.NpToString("N0", 0) + ")";

	public string PriceString => Price.NpToString("N0", 0) ?? "";
}
