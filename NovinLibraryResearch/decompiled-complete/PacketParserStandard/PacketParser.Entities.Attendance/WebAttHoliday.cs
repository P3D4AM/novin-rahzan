using System;
using PacketParser.Attendance.InterFaces;

namespace PacketParser.Entities.Attendance;

internal class WebAttHoliday : IHoliday
{
	public DateTime DateM { get; set; }

	public string Monasebat { get; set; }

	public string Description { get; set; }

	public string Day { get; }

	public string DateCalture { get; }

	public bool StRasmi { get; set; }
}
