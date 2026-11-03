using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class OrderDiscountCouponModel
{
	public Guid CouponGuid { get; set; }

	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }

	public int MaxUseForAll { get; set; }

	public int UseCountForAll { get; set; }

	public int MaxUseForTafsil { get; set; }

	public int UseCountForTafsil { get; set; }

	public EnDiscountType Type { get; set; }

	public decimal Amount { get; set; }

	public decimal MinOrderAghlam { get; set; }

	public decimal MinQuantityInOrder { get; set; }

	public decimal MinBuy { get; set; }

	public decimal MaxBuy { get; set; }

	public decimal MinDiscount { get; set; }

	public decimal MaxDiscount { get; set; }
}
