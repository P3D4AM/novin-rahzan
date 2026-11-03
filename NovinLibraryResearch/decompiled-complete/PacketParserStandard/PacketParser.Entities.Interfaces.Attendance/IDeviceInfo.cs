using System;
using PacketParser.Attendance.Entities;
using PacketParser.Entities.Attendance;

namespace PacketParser.Entities.Interfaces.Attendance;

public interface IDeviceInfo : IHasContext, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	bool AutoRecieve { get; set; }

	bool AutoSetDateTime { get; set; }

	string CommunicationPass { get; set; }

	string DefultGetWay { get; set; }

	string Desc { get; set; }

	string DeviceName { get; set; }

	short DeviceNo { get; set; }

	string DeviceType { get; set; }

	string EngineVersion { get; set; }

	string FwVersion { get; set; }

	bool GetNewTaradodOnly { get; set; }

	string HostIp { get; set; }

	short Id { get; set; }

	string Ip { get; set; }

	string MacAddress { get; set; }

	int MinCheck { get; set; }

	DeviceModelEnumType Model { get; set; }

	int PortRealtime { get; set; }

	DateTime RdEnrollDataTime { get; set; }

	string SerialNumberStr { get; set; }

	object SourceObj { get; set; }

	string SubnetMask { get; set; }

	DateTime SyncTime { get; set; }

	int TcpPort { get; set; }

	TypeConnection TransferMode { get; set; }

	DeviceManufacture TypeDeviceManufacture { get; set; }

	bool UseDhcp { get; set; }

	bool UseRealTime { get; set; }

	MicroDeviceInfo MappToMicro();
}
