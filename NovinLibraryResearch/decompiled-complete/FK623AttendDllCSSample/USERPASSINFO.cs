using System.Runtime.InteropServices;

internal struct USERPASSINFO
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	public byte[] UserPassID;
}
