using System;

namespace PacketParser.Entities.Interfaces.Accounting.Selection;

public interface IPrdGroupSelected : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Code { get; set; }

	string Title { get; set; }

	Guid CashierGuid { get; set; }

	string Description { get; set; }
}
