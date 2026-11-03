using System;

namespace PacketParser.Entities.Interfaces.Attendance.Biometric;

public interface IBiometric : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	EnDeviceGroup DeviceGroup { get; set; }

	Guid? PersonGuid { get; set; }
}
