using System.Collections.Generic;
using PacketParser.Entities.Attendance;

namespace PacketParser.Filters.Attendance;

public class DeviceInfoFilter
{
	public short Port { get; set; }

	public List<DeviceModelEnumType> ModelList { get; set; }
}
