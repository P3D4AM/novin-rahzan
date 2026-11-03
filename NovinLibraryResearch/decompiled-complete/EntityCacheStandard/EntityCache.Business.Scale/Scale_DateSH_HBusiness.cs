using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_DateSH_HBusiness : IScale_DateSH_H, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public Guid ScaleGuid { get; set; }

	public int FactorNumber { get; set; }

	public DateTime UpdateDate { get; set; }

	public int BatchGuid { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_DateSH_HBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
