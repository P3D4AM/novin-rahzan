using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_LogBusiness : IScale_Log, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription, IHasContext
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string Description { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_LogBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
