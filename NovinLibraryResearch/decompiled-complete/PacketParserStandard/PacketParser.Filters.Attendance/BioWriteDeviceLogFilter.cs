using System;

namespace PacketParser.Filters.Attendance;

public class BioWriteDeviceLogFilter
{
	public Guid? DeviceGuid { get; set; }

	public Guid? PersonGuid { get; set; }

	public Guid? UserGuid { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public int? HardwareCode { get; set; }
}
