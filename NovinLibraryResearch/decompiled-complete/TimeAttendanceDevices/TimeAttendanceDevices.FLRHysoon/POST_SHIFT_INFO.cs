using System.Runtime.InteropServices;

namespace TimeAttendanceDevices.FLRHysoon;

internal struct POST_SHIFT_INFO
{
	public int Size;

	public int Ver;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
	public SHIFT_TIME_SLOT[] ShiftTime;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public POST_NAME[] PostInfo;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
	public byte[] CompanyName;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	public byte[] Reserved;

	public void Init()
	{
		Size = 2476;
		Ver = 2;
		ShiftTime = new SHIFT_TIME_SLOT[24];
		PostInfo = new POST_NAME[16];
		CompanyName = new byte[128];
		Reserved = new byte[4];
	}
}
