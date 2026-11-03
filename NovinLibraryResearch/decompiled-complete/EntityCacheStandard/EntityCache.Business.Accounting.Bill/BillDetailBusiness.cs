using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Bill;

namespace EntityCache.Business.Accounting.Bill;

public class BillDetailBusiness : IBillDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext
{
	public Guid Guid { get; set; }

	public Guid PlacementGuid { get; set; }

	public Guid OrderGuid { get; set; }

	public Guid MasterGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	public BillDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
