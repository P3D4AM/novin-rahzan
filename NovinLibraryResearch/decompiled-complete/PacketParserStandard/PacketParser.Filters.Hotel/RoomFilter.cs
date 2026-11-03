using System;

namespace PacketParser.Filters.Hotel;

public class RoomFilter
{
	public bool? Status { get; set; } = true;


	public Guid? GroupGuid { get; set; }

	public Guid? MasterGuid { get; set; }
}
