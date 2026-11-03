using System;

namespace PacketParser.Filters;

public class TaxOrderLogFilter
{
	public Guid? OrderGuid { get; set; }

	public string TaxOrderId { get; set; } = "";


	public string TaxRefrencedNumber { get; set; } = "";

}
