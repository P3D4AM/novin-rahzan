using System;

namespace PacketParser.SiteManagementApiModel;

public class WebLogPacketSumModel
{
	public short DepId { get; set; }

	public long HashId { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }
}
