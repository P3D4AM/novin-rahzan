using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_InfoBusiness : IScale_Info, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription, IHasContext
{
	public Guid ScaleGuid { get; set; }

	public int PortNumber { get; set; }

	public int TimerInterval { get; set; }

	public string Name { get; set; }

	public bool Deleted { get; set; }

	public bool SellJamandeh { get; set; }

	public string IP { get; set; }

	public string EndMessage { get; set; }

	public string ShopName { get; set; }

	public bool AccountingMemory { get; set; }

	public bool AccountingDivision { get; set; }

	public bool AccountingIP { get; set; }

	public bool MNGLock { get; set; }

	public short FactorNumber { get; set; }

	public short ConnectionType { get; set; }

	public string Model { get; set; }

	public string Serial { get; set; }

	public Guid Guid { get; set; }

	public string Description { get; set; }

	public bool AccountingDate { get; set; }

	public bool AccountingID { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_InfoBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
