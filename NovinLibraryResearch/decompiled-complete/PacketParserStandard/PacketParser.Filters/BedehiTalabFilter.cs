using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class BedehiTalabFilter
{
	public string Search { get; set; } = "";


	public DateTime? Date1 { get; set; }

	public DateTime? Date2 { get; set; }

	public decimal? FromPrice { get; set; }

	public decimal? ToPrice { get; set; }

	public List<EnBedehiTalabType> TypeList { get; set; }
}
