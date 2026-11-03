using System;
using PacketParser.Entities.Attendance;

namespace PacketParser.Entities.Interfaces.Attendance;

public interface IDeviceLogOnPerson
{
	Guid Guid { get; set; }

	string DeviceIp { get; set; }

	string DeviceSerial { get; set; }

	string DeviceModel { get; set; }

	string DevicePassword { get; set; }

	string InOut { get; set; }

	string VerifyMode { get; set; }

	string LogPhoto { get; set; }

	int DeviceNo { get; set; }

	int DevicePort { get; set; }

	int IoMode { get; set; }

	DeviceModelEnumType TerminalType { get; set; }

	DateTime DateTimeLogM { get; set; }

	long IdTraffic { get; set; }

	int PersonNo { get; set; }

	DateTime FixedDateM { get; }
}
