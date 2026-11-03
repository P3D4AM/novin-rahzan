using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class TarazHesabFilter
{
	public TarazType Type { get; set; }

	public DateTime Date1 { get; set; }

	public DateTime Date2 { get; set; }

	public int KolCode1 { get; set; }

	public int KolCode2 { get; set; }

	public long MoeinCode1 { get; set; }

	public long MoeinCode2 { get; set; }

	public long TafsilCode1 { get; set; }

	public long TafsilCode2 { get; set; }

	public int SanadNumberRefrencedId { get; set; }

	public int SanadTypesRefrencedId { get; set; }

	public short ColumnCount { get; set; }
}
