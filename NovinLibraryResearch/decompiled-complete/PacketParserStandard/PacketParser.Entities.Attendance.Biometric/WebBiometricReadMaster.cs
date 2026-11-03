using System;
using System.Collections.Generic;

namespace PacketParser.Entities.Attendance.Biometric;

public class WebBiometricReadMaster
{
	public Guid DeviceGuid { get; set; }

	public int HardwareCode { get; set; }

	public EnAttendancePrivilage Privilage { get; set; }

	public EnAttendanceReadAction Action { get; set; }

	public List<WebBiometricDetail> Details { get; set; }
}
