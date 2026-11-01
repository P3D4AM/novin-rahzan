using System.Runtime.InteropServices;

namespace TimeAttendanceDevices.FLRHysoon;

internal struct USER_INFO
{
	public int Size;

	public int Ver;

	public int UserId;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
	public byte[] UserName;

	public int PostId;

	public short YearAssigned;

	public short MonthAssigned;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
	public byte[] ShiftId;

	public void Init()
	{
		Size = 184;
		Ver = 2;
		UserId = 0;
		YearAssigned = 0;
		MonthAssigned = 0;
		UserName = new byte[128];
		PostId = 0;
		ShiftId = new byte[31];
	}
}
