using System;

namespace PacketParser.Attendance.InterFaces;

public interface IHoliday
{
	DateTime DateM { get; set; }

	string Monasebat { get; set; }

	string Description { get; set; }

	string Day { get; }

	string DateCalture { get; }

	bool StRasmi { get; set; }
}
