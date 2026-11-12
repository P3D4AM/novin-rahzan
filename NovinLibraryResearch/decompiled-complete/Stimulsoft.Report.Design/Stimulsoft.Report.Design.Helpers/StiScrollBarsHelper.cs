using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.Helpers;

public sealed class StiScrollBarsHelper
{
	public const int GWL_STYLE = -16;

	public const int WS_VSCROLL = 2097152;

	public const int WS_HSCROLL = 1048576;

	[DllImport("user32.dll", SetLastError = true)]
	public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

	public static ScrollBars GetVisibleScrollbars(Control ctl)
	{
		int windowLong = GetWindowLong(ctl.Handle, -16);
		bool flag = (windowLong & 0x100000) != 0;
		bool flag2 = (windowLong & 0x200000) != 0;
		if (flag)
		{
			if (flag2)
			{
				return (ScrollBars)3;
			}
			return (ScrollBars)1;
		}
		if (flag2)
		{
			return (ScrollBars)2;
		}
		return (ScrollBars)0;
	}
}
