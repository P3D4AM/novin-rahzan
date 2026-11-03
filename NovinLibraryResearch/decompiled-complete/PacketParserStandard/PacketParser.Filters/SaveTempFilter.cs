using System;

namespace PacketParser.Filters;

public class SaveTempFilter
{
	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public int Id2From { get; set; }

	public int Id2To { get; set; }

	public bool? Status { get; set; }
}
