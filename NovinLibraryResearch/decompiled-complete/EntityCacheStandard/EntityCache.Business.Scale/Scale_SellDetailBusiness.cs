using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_SellDetailBusiness : IScale_SellDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid SC_Guid { get; set; }

	public Guid Guid { get; set; }

	public int PortNumber { get; set; }

	public Guid ScaleGuid { get; set; }

	public int FactorNumber { get; set; }

	public short MemoryNumber { get; set; }

	public decimal Quantity { get; set; }

	public decimal UnitPrice { get; set; }

	public bool Type_Weight { get; set; }

	public DateTime DateM { get; set; }

	public string DateSH_H { get; set; }

	public DateTime DateUpdate { get; set; }

	public string ComputerName { get; set; }

	public bool IsRead { get; set; }

	public short Ghorfe { get; set; }

	public decimal RowSum { get; set; }

	public int RInt1 { get; set; }

	public int RInt2 { get; set; }

	public string RText1 { get; set; }

	public int RText2 { get; set; }

	public decimal RDbl1 { get; set; }

	public decimal RDbl2 { get; set; }

	public string IP { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_SellDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
