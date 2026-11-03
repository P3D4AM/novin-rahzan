using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_Message_WriteMemoryBusiness : IScale_Message_WriteMemory, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public Guid ScaleGuid { get; set; }

	public string Code { get; set; }

	public string Barcode { get; set; }

	public string Name { get; set; }

	public decimal UnitPrice { get; set; }

	public DateTime DateM { get; set; }

	public DateTime Expiredate { get; set; }

	public short ST { get; set; }

	public bool Vazni { get; set; }

	public short NextTry { get; set; }

	public short TryCount { get; set; }

	public short MaxTryCount { get; set; }

	public string Parametr1 { get; set; }

	public string Parametr2 { get; set; }

	public string Parametr3 { get; set; }

	public string Parametr4 { get; set; }

	public int MemoryNumber { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_Message_WriteMemoryBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
