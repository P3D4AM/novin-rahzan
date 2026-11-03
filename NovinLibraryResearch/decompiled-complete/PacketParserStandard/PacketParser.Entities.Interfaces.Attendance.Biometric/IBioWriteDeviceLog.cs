using System;

namespace PacketParser.Entities.Interfaces.Attendance.Biometric;

public interface IBioWriteDeviceLog : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DateTime DateM { get; set; }

	string Description { get; set; }

	Guid DeviceGuid { get; set; }

	Guid? BioGuid { get; set; }

	int HardwareCode { get; set; }

	string HardwareName { get; set; }

	ReturnedState Result { get; set; }

	Guid? UserGuid { get; set; }
}
