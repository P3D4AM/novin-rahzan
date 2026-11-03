using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class OperationTransfer
{
	public Guid ObjectGuid { get; set; }

	public Document ObjectType { get; set; }

	public DateTime? TransferDate { get; set; }
}
