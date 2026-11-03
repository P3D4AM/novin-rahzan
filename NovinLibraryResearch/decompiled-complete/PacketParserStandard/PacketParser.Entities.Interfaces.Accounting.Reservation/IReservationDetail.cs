using System;

namespace PacketParser.Entities.Interfaces.Accounting.Reservation;

public interface IReservationDetail : IHasJustGuid, IHasMasterGuid
{
	Guid PlacementGuid { get; }

	int PeopleCount { get; set; }

	DateTime StartDate { get; set; }

	DateTime EndDate { get; set; }

	Guid? OrderGuid { get; set; }

	string Description { get; set; }
}
