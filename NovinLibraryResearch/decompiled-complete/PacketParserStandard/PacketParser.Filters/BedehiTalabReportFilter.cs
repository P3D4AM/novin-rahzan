using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class BedehiTalabReportFilter
{
	public EnBedehiTalabType Type { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public short Status { get; set; }

	public short Count { get; set; } = 100;

}
