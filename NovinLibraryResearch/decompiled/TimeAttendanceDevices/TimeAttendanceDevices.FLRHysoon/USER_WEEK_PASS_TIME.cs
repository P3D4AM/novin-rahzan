using System.Runtime.InteropServices;

namespace TimeAttendanceDevices.FLRHysoon;

internal struct USER_WEEK_PASS_TIME
{
	public int Size;

	public int UserId;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
	public byte[] WeekPassTime;

	public void Init()
	{
		Size = 16;
		UserId = 0;
		WeekPassTime = new byte[7];
	}
}
