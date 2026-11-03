using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Hotel;

namespace EntityCache.Business.Accounting.Hotel;

public class CustomerReceiptRelationBusiness : ICustomerReceiptRelation, IHasContext
{
	public Guid ReceiptGuid { get; set; }

	public Guid CustomerGuid { get; set; }

	public string CustomerName { get; set; }

	public string CustomerFamily { get; set; }

	public string ContextGuid { get; set; }

	public long Code { get; set; }

	public bool IsChecked { get; set; }

	public CustomerReceiptRelationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
