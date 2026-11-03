using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Stimulsoft.Report.Components;

[SuppressUnmanagedCodeSecurity]
internal class StiPageWin32
{
	internal const short HORZSIZE = 4;

	internal const short VERTSIZE = 6;

	internal const short HORZRES = 8;

	internal const short VERTRES = 10;

	internal const short PHYSICALOFFSETX = 112;

	internal const short PHYSICALOFFSETY = 113;

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	internal static extern int GetDeviceCaps(IntPtr hDc, int nIndex);
}
