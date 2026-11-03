using System;
using PacketParser.Entities;

namespace PacketParser.Filters.Khazane;

public class AghsatShFilter
{
	public Guid? TafsilGuid { get; set; }

	public Guid? GroupGuid { get; set; }

	public EnAghsatShStatus? Status { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public string SearchString { get; set; } = "";


	public Guid? PardakhtGuid { get; set; }
}
