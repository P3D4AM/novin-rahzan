using System;

namespace PacketParser.Entities.GoldAndCurrency;

public class WebGoldAndCurrencyDetail
{
	public long ID { get; set; }

	public string Namad { get; set; }

	public DateTime InsertDate { get; set; }

	public decimal Price { get; set; }
}
