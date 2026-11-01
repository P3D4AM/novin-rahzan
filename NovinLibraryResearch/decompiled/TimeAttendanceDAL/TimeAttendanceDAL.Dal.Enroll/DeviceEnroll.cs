namespace TimeAttendanceDAL.Dal.Enroll;

public class DeviceEnroll
{
	public int EMachineNumber { get; set; }

	public int EnrollNumber { get; set; }

	public int FingerNumber { get; set; }

	public int Privilige { get; set; }

	public int Password1 { get; set; }

	public byte[] FPData { get; set; }

	public string CompanyName { get; set; }

	public string VerifyName { get; set; }
}
