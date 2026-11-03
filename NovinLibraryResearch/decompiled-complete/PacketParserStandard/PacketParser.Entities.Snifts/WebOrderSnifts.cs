using System;

namespace PacketParser.Entities.Snifts;

public class WebOrderSnifts
{
	public DateTime? LastOrderDate { get; set; }

	public decimal LastBuyPrice { get; set; }

	public decimal LastBuyTaxPercent { get; set; }

	public decimal LastBuyAvarezPercent { get; set; }

	public decimal LastBuyDiscountPercent { get; set; }

	public WebCustomerSnifts Seller { get; set; }
}
