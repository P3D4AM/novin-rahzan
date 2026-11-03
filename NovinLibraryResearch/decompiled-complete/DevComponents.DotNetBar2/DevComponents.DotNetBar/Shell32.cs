using System;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar;

public static class Shell32
{
	[DllImport("shell32.dll", SetLastError = true)]
	public static extern IntPtr SHAppBarMessage(ABM dwMessage, [In] ref APPBARDATA pData);
}
