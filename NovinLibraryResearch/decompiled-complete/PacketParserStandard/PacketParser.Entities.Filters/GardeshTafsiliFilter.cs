using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PacketParser.Entities.Filters;

public class GardeshTafsiliFilter
{
	public short DepId { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

	public Guid? TafsilGuid4 { get; set; }

	public DateTime? Date1 { get; set; }

	public DateTime? Date2 { get; set; }

	public List<Guid> MoeinGuids { get; set; }

	public Guid? KolGuid { get; set; }

	[DefaultValue(null)]
	public int? MaxCount { get; set; }

	public bool IsLoadTafsilName { get; set; }

	public bool IsLoadSood { get; set; }

	public HesabType HesabType { get; set; }

	public Guid? CashierGuid { get; set; }
}
