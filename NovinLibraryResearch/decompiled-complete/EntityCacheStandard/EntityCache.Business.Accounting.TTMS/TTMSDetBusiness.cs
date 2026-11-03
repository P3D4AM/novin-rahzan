using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.TTMS;

namespace EntityCache.Business.Accounting.TTMS;

public class TTMSDetBusiness : Serializable<TTMSDetBusiness>, ITTMSDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext
{
	public Guid Guid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public string Description { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid MasterGuid { get; set; }

	public string ContextGuid { get; set; }

	public TTMSDetBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
