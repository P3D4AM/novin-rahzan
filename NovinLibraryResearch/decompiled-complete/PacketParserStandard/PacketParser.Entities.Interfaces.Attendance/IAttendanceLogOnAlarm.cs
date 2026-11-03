using System;

namespace PacketParser.Entities.Interfaces.Attendance;

public interface IAttendanceLogOnAlarm : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid DeviceGuid { get; set; }

	int PartitionId { get; set; }

	int TransactionId { get; set; }

	DateTime LogTime { get; set; }

	long UserId { get; set; }

	int DoorId { get; set; }

	string AlarmType { get; set; }

	string IpAddress { get; set; }
}
