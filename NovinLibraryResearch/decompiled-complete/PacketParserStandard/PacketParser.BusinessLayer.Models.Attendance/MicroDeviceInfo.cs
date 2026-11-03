using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Attendance;

public record MicroDeviceInfo
{
	public string Ip { get; private set; }

	public int Port { get; private set; }

	public int DeviceNumber { get; private set; }

	public int DevicePassword { get; private set; }

	public EnDeviceGroup Group { get; private set; }

	public Guid Guid { get; set; }

	public MicroDeviceInfo(Guid guid, string ip, int port, int deviceNumber, int password, EnDeviceGroup group)
	{
		Guid = guid;
		Ip = ip;
		Port = port;
		DeviceNumber = deviceNumber;
		Group = group;
		DevicePassword = password;
	}
}
