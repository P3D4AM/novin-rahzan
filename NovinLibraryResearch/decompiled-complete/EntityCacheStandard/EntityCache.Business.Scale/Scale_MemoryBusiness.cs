using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_MemoryBusiness : IScale_Memory, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription, IHasContext
{
	public Guid ScaleGuid { get; set; }

	public int MyProperty { get; set; }

	public Guid ProductGuid { get; set; }

	public short MemoryNumber { get; set; }

	public string MemoryName { get; set; }

	public decimal MemoryPrice { get; set; }

	public DateTime Date { get; set; }

	public bool Taeed { get; set; }

	public bool Vazni { get; set; }

	public decimal TotalQT { get; set; }

	public decimal TotalPrice { get; set; }

	public Guid Guid { get; set; }

	public string Description { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_MemoryBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
