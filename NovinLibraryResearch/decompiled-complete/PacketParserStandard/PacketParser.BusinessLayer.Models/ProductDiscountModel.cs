using System;

namespace PacketParser.BusinessLayer.Models;

public class ProductDiscountModel
{
	public Guid Guid { get; set; }

	public string IdProduct { get; set; }

	public string Name { get; set; }

	public string VahedName { get; set; }

	public decimal KardexPrice { get; set; }

	public decimal Sell1 { get; set; }

	public decimal DiscountPercent { get; set; }

	public decimal Prefit { get; set; }

	public decimal PrefitPercent { get; set; }
}
