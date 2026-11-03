using System;

namespace PacketParser.Filters.Hotel;

public class ReceiptFilter
{
	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public Guid? CustomerGuid { get; set; }

	public decimal TotalPriceFrom { get; set; }

	public decimal TotalPriceTo { get; set; }
}
