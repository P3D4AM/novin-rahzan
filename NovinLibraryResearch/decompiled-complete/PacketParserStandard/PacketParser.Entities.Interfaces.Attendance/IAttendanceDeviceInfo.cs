using System;
using PacketParser.Entities.Attendance;

namespace PacketParser.Entities.Interfaces.Attendance;

public interface IAttendanceDeviceInfo : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	short? DepId { get; set; }

	string Name { get; set; }

	long SerialId { get; set; }

	int TerminalId { get; set; }

	DeviceModelEnumType TerminalType { get; set; }

	int PortNumber { get; set; }

	string Description { get; set; }

	DateTime? LastPingTime { get; set; }

	string SerialIdString { get; set; }

	string DepName { get; set; }
}
