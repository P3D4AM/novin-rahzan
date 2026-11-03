using System;

namespace PacketParser.Entities.Interfaces.Attendance.Biometric;

public interface IBioDeviceRelation
{
	Guid BioGuid { get; set; }

	Guid DeviceGuid { get; set; }

	int HardwareCode { get; set; }

	EnAttendancePrivilage Privilage { get; set; }

	DateTime? FromDateM { get; set; }

	DateTime? ToDateM { get; set; }

	bool Status { get; set; }

	DateTime Modified { get; set; }
}
