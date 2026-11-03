using System.Runtime.InteropServices;

internal struct GROUPPASSINFO
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	public byte[] GroupPassID;
}
