using System;
using System.Collections.Generic;

namespace PacketParser.Filters;

public class ResidFilter
{
	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public List<int> SelectedStatus { get; set; }

	public Guid? GroupGuid { get; set; }
}
