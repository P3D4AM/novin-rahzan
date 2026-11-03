using System;

namespace PacketParser.Filters;

public class SumKolFilter
{
	public long CodeFrom { get; set; }

	public long CodeTo { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }
}
