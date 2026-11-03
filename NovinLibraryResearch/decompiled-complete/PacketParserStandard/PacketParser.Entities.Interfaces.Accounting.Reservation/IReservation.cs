using System;

namespace PacketParser.Entities.Interfaces.Accounting.Reservation;

public interface IReservation : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DateTime DateM { get; set; }

	Guid TafsilGuid { get; set; }

	Guid? OrderGuid { get; set; }

	Guid CashierGuid { get; set; }

	Guid? ReceptionGuid { get; set; }

	string Description { get; set; }

	int Number { get; set; }
}
