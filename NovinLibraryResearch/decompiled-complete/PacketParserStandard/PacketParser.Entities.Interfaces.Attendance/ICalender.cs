using System;

namespace PacketParser.Entities.Interfaces.Attendance;

public interface ICalender : IHasJustGuid, IHasStatus
{
	DateTime DateM { get; set; }

	string Monasebat { get; set; }

	string Description { get; set; }

	bool STRasmi { get; set; }

	DateTime Modified { get; set; }
}
