using System;

namespace PacketParser.Entities.Interfaces.Accounting.Hotel;

public interface ICustomerReceiptRelation
{
	Guid CustomerGuid { get; set; }

	Guid ReceiptGuid { get; set; }
}
