using System.Collections.Generic;

namespace TimeAttendanceDevices;

public class EnrollDataDet
{
	public int EnrollNo { get; set; }

	public short DeviceNo { get; set; }

	public int apnBackupNumbar { get; set; }

	public int MachinPrivilage { get; set; }

	public int Enabled { get; set; }

	public List<byte> EnrollData { get; set; }

	public int Password { get; set; }
}
