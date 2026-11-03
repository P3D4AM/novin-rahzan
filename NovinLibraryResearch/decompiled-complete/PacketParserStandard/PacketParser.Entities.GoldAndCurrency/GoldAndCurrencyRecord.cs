using System;

namespace PacketParser.Entities.GoldAndCurrency;

public record GoldAndCurrencyRecord
{
	public decimal Price { get; set; }

	public string Namad { get; set; }

	public DateTime DateM { get; set; }
}
