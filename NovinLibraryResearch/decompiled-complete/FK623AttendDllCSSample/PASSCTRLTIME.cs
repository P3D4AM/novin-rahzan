using System.Runtime.InteropServices;

internal struct PASSCTRLTIME
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
	public PASSTIME[] PassCtrlTime;
}
