using System;

namespace PacketParser.Entities.Interfaces.Accounting.Order.OpenInvoices;

public interface IOpenInvoice : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int Number { get; set; }

	DateTime DateM { get; set; }

	Guid TafsilGuid { get; set; }

	Guid CashierGuid { get; set; }

	string Description { get; set; }

	Guid? ReceptionGuid { get; set; }

	Guid? PardakhtGuid { get; set; }

	int PeopleCount { get; set; }

	int SanadNo { get; set; }

	decimal Sum { get; set; }

	DateTime? SettlementDate { get; set; }
}
