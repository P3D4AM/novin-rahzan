using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_MemoryHistoryBusiness : IScale_MemoryHistory, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public Guid ScaleGuid { get; set; }

	public DateTime Date { get; set; }

	public string ScaleName { get; set; }

	public string ProductCode { get; set; }

	public string BarCode { get; set; }

	public string ProductName { get; set; }

	public int MemoryNumber { get; set; }

	public bool Vazni { get; set; }

	public decimal UnitPrice { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_MemoryHistoryBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
