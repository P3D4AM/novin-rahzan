using System.Runtime.InteropServices;

internal struct GROUPMATCHINFO
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
	public short[] GroupMatch;
}
