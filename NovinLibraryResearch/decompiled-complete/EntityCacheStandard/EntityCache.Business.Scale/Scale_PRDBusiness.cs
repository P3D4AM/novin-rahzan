using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_PRDBusiness : IScale_PRD, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid ProductGuid { get; set; }

	public short MemoryNumber { get; set; }

	public string MemoryName { get; set; }

	public decimal MemoryPrice { get; set; }

	public bool Vazni { get; set; }

	public decimal TotalQT { get; set; }

	public decimal TotalPrice { get; set; }

	public Guid Guid { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_PRDBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
