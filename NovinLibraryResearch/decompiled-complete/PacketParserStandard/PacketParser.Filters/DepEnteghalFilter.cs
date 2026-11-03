using System;

namespace PacketParser.Filters;

public class DepEnteghalFilter
{
	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public bool? Status { get; set; }
}
