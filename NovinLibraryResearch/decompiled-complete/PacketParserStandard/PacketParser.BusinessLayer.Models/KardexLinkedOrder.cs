using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class KardexLinkedOrder
{
	public string Id2 { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Description { get; set; }

	public decimal SellQuantity { get; set; }

	public decimal SellUnitPrice { get; set; }

	public string SellUnitPriceForDisplay => SellUnitPrice.NpToString("N0", 0);

	public decimal SellTotalPrice { get; set; }

	public Guid DetailGuid { get; set; }

	public Guid OrderGuid { get; set; }

	public Document OrderType { get; set; }

	public Guid ProductGuid { get; set; }

	public decimal BackSellQuantity { get; set; }

	public decimal RemainQuantity { get; set; }

	public Guid? V2Guid { get; set; }

	public decimal V2Quantity { get; set; }

	public decimal Tax { get; set; }

	public decimal Avarez { get; set; }

	public decimal Discount { get; set; }
}
