using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_MemBusiness : IScale_Mem, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid ScaleGuid { get; set; }

	public Guid ProductGuid { get; set; }

	public Guid Guid { get; set; }

	public int MemoryNumber { get; set; }

	public DateTime Date { get; set; }

	public bool ISSaved { get; set; }

	public decimal PriceOnScale { get; set; }

	public string CodeOnScale { get; set; }

	public string NameScale { get; set; }

	public string BarCode { get; set; }

	public string Name { get; set; }

	public decimal Price { get; set; }

	public bool Vazni { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_MemBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
