using System;

namespace PacketParser.Entities.Interfaces.Accounting.Bill;

public interface IBillMaster : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	new Guid Guid { get; set; }

	Guid CashierGuid { get; set; }

	Guid? PardakhtGuid { get; set; }

	Guid? ReceptionGuid { get; set; }

	SanadSt SanadSt { get; set; }

	int SanadNumber { get; set; }

	DateTime Date { get; set; }

	decimal Sum { get; set; }
}
