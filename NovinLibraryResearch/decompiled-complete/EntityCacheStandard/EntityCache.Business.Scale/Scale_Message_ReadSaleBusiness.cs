using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_Message_ReadSaleBusiness : IScale_Message_ReadSale, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid ScaleGuid { get; set; }

	public Guid MemoryGuid { get; set; }

	public int Count { get; set; }

	public DateTime DateM { get; set; }

	public short Priority { get; set; }

	public Guid Guid { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_Message_ReadSaleBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
