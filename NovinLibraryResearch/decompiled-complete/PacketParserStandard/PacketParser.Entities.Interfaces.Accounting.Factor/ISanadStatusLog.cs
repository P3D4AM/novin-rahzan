using System;

namespace PacketParser.Entities.Interfaces.Accounting.Factor;

public interface ISanadStatusLog : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid AsGuid { get; set; }

	Document AsType { get; set; }

	Guid? CashierGuid { get; set; }

	int OldStatus { get; set; }

	int NewStatus { get; set; }

	DateTime DateM { get; set; }

	int Ordinal { get; set; }
}
