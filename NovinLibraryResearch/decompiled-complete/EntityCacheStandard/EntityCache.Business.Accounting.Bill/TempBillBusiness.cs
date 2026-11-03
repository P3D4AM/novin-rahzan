using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Bill;

namespace EntityCache.Business.Accounting.Bill;

public class TempBillBusiness : ITempBill, IHasContext
{
	public string ContextGuid { get; set; }

	public Guid PlacementGuid { get; set; }

	public Guid OrderGuid { get; set; }

	public TempBillBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
