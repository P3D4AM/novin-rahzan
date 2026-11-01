using System;

namespace TimeAttendanceDAL.Dal.Enroll;

public class EnrollDet
{
	public int Id { get; set; }

	public DateTime DateM { get; set; }

	public int PersonNo { get; set; }

	public int DeviceId { get; set; }

	public byte[] EnrollData { get; set; }
}
