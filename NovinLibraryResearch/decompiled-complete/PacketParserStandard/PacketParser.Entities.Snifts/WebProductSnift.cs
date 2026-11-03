using System;

namespace PacketParser.Entities.Snifts;

public class WebProductSnift
{
	public Guid Guid { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public string GroupName { get; set; }

	public string VahedName { get; set; }

	public decimal Price { get; set; }

	public decimal TaxPercent { get; set; }

	public decimal AvarezPercent { get; set; }

	public WebOrderSnifts LastOrderBuy { get; set; }
}
