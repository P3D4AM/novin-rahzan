using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;

namespace PacketParser.EventArgs.Attendance;

public class BioMetricDisconnectEventArgs : BaseEventArgs
{
	public MicroDeviceInfo DeviceInfo { get; private set; }

	public ReturnedSaveFuncInfo Result { get; private set; }

	public BioMetricDisconnectEventArgs(MicroDeviceInfo deviceInfo, ReturnedSaveFuncInfo result)
	{
		DeviceInfo = deviceInfo;
		Result = result;
	}
}
