using System;
using System.Runtime.InteropServices;

namespace Stimulsoft.Controls.DesktopIntegration;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct THUMBBUTTON
{
	[MarshalAs(UnmanagedType.U4)]
	public THBMASK dwMask;

	public uint iId;

	public uint iBitmap;

	public IntPtr hIcon;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
	public string szTip;

	[MarshalAs(UnmanagedType.U4)]
	public THBFLAGS dwFlags;
}
