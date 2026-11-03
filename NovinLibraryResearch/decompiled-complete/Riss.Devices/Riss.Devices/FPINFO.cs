using System.Runtime.InteropServices;

namespace Riss.Devices;

public struct FPINFO
{
	public uint ID;

	public byte Valid;

	public byte Manager;

	public byte FingerNum;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1397)]
	public byte[] Reserved;
}
