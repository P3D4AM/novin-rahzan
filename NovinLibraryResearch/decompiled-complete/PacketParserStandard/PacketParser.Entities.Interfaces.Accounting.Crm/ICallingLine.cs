using System;

namespace PacketParser.Entities.Interfaces.Accounting.Crm;

public interface ICallingLine : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string ComputerName { get; set; }

	short DeviceNumber { get; set; }

	short LineNumber { get; set; }

	string Number { get; set; }

	bool IsCo { get; set; }

	Guid CashierGuid { get; set; }
}
