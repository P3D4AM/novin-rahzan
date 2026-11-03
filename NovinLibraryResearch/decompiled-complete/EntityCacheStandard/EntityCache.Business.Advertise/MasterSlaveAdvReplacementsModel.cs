using System;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Advertise;

public class MasterSlaveAdvReplacementsModel : IHasContext
{
	public string AdvReplacement1 { get; set; }

	public string AdvReplacement2 { get; set; }

	public string Phone { get; set; }

	public Guid? MasterGuid { get; set; }

	public Guid? SlaveGuid { get; set; }

	public string ContextGuid { get; set; }

	public MasterSlaveAdvReplacementsModel(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
