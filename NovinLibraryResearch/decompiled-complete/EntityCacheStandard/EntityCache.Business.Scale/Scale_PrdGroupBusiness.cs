using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_PrdGroupBusiness : IScale_PrdGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription, IHasContext
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string Description { get; set; }

	public string ContextGuid { get; set; }

	public Scale_PrdGroupBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
