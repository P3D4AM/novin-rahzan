using System;

namespace PacketParser.Entities.CRM.CallCenter;

public class WebCallDetail
{
	public DateTime EventDateM { get; set; }

	public DateTime RecieveDateM { get; set; }

	public string MetaData { get; set; }

	public EnCallCenterEventType Type { get; set; }
}
