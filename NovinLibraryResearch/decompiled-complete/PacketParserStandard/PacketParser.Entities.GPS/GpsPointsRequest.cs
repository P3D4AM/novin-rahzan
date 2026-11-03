using System;

namespace PacketParser.Entities.GPS;

public class GpsPointsRequest
{
	public int UserID { get; set; }

	public DateTime Start { get; set; }

	public DateTime Finish { get; set; }
}
