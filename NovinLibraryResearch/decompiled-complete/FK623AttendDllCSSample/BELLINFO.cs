using System.Runtime.InteropServices;

internal struct BELLINFO
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
	public byte[] mValid;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
	public byte[] mHour;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
	public byte[] mMinute;
}
