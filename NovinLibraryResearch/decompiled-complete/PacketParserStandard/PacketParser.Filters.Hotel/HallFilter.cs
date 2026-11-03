using System;

namespace PacketParser.Filters.Hotel;

public class HallFilter
{
	public bool? Status { get; set; } = true;


	public Guid? MasterGuid { get; set; }

	public bool IsLoadGuidEmpty { get; set; }
}
