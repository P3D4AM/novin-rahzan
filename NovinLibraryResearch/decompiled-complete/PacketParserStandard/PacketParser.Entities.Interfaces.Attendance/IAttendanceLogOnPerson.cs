using System;

namespace PacketParser.Entities.Interfaces.Attendance;

public interface IAttendanceLogOnPerson : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid DeviceGuid { get; set; }

	int PartitionId { get; set; }

	long TransactionId { get; set; }

	DateTime LogTime { get; set; }

	long UserId { get; set; }

	int DoorId { get; set; }

	string AttendanceStatus { get; set; }

	string VerifyMode { get; set; }

	int JobCode { get; set; }

	string AntiPassv { get; set; }

	byte[] Photo { get; set; }

	string IpAddress { get; set; }
}
