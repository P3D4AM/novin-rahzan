using System;
using System.Collections.Generic;

namespace PacketParser.Filters;

public class KardexSumFilter
{
	public string SearchString { get; set; } = "";


	public List<Guid> ProductGroupGuids { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public bool HasGardeshManfi { get; set; }

	public bool HasOrderPoint { get; set; }

	public bool? ProductStatus { get; set; }

	public bool? DepotStatus { get; set; }

	public bool KardexAffective { get; set; } = true;

}
