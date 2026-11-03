using System;

namespace PacketParser.Entities.Interfaces.Attendance.Biometric;

public interface IBioWriteDevice
{
	Guid BioGuid { get; set; }

	Guid DeviceGuid { get; set; }

	int HardwareCode { get; set; }

	string HarwareName { get; set; }

	DateTime InsertedDateM { get; set; }

	short TryCount { get; set; }

	DateTime NextTry { get; set; }

	Guid UserGuid { get; set; }

	string Message { get; set; }

	EnDeviceMessageType MessageType { get; set; }
}
