using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_CommandBusiness : IScale_Command, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public Guid ScaleGuid { get; set; }

	public DateTime DateM { get; set; }

	public string DateSH { get; set; }

	public string Command { get; set; }

	public string Sender { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_CommandBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
