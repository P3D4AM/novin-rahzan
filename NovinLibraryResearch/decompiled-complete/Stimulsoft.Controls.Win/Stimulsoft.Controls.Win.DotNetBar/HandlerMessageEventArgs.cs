using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class HandlerMessageEventArgs : EventArgs
{
	public IntPtr hWnd = IntPtr.Zero;

	public IntPtr wParam = IntPtr.Zero;

	public IntPtr lParam = IntPtr.Zero;

	public HandlerMessageEventArgs(IntPtr hwnd, IntPtr wparam, IntPtr lparam)
	{
		hWnd = hwnd;
		wParam = wparam;
		lParam = lparam;
	}
}
