using System;
using PacketParser.BusinessLayer.Models.Attendance;

namespace PacketParser.EventArgs.Attendance;

public class BioMetricReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public MicroDeviceInfo DeviceInfo { get; set; }

	public BioMetricReadEventArgs(DateTime modified, MicroDeviceInfo deviceInfo)
	{
		Modified = modified;
		DeviceInfo = deviceInfo;
	}
}
