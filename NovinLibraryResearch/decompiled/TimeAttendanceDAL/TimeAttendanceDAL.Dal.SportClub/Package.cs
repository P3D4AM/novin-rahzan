using System;
using PacketParser.Entities.Attendance;

namespace TimeAttendanceDAL.Dal.SportClub;

public class Package
{
	public short ShiftId { get; set; }

	public string Name { get; set; }

	public decimal Price { get; set; }

	public PackageEnumType PackageType { get; set; }

	public short Periodic { get; set; }

	public Guid PackageGuid { get; set; }
}
