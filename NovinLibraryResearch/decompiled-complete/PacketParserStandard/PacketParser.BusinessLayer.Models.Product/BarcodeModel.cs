using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Product;

public class BarcodeModel : IBarcode
{
	public string BarCode { get; set; }

	public Guid PrdGuid { get; set; }

	public string ProductName { get; set; }

	public DateTime CreateDate { get; set; }

	public Guid? DepotGuid { get; set; }

	public string Description { get; set; }

	public string Name { get; set; }

	public decimal Price { get; set; }

	public decimal Quantity { get; set; }

	public BarcodePriceType Type { get; set; }

	public short Ordinal { get; set; }

	public Guid? PriceTypeGuid { get; set; }
}
