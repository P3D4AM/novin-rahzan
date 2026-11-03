using System;

namespace PacketParser.Entities.Interfaces.Attendance;

public interface IAttendanceLogOnAdmin : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int PartitionId { get; set; }

	short? DepId { get; set; }

	int TransactionId { get; set; }

	DateTime LogTime { get; set; }

	long AdminId { get; set; }

	long UserId { get; set; }

	string Action { get; set; }

	int Result { get; set; }

	string IpAddress { get; set; }

	Guid DeviceGuid { get; set; }
}
