using System;

namespace PacketParser.Filters.GoldAndCurrency;

public class GoldAndCurrencyDetailFilter
{
	public string Namad { get; set; }

	public int? MaxCount { get; set; }

	public DateTime? FromDate { get; set; }

	public DateTime? ToDate { get; set; }
}
