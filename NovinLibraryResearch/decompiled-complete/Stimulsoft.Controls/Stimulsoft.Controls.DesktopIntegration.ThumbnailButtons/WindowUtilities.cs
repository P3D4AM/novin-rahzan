using System;
using System.Drawing;

namespace Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons;

internal static class WindowUtilities
{
	public static Point GetParentOffsetOfChild(IntPtr hwnd, IntPtr hwndParent)
	{
		POINT point = new POINT(0, 0);
		UnsafeNativeMethods.ClientToScreen(hwnd, ref point);
		POINT point2 = new POINT(0, 0);
		UnsafeNativeMethods.ClientToScreen(hwndParent, ref point2);
		return new Point(point.X - point2.X, point.Y - point2.Y);
	}

	public static Size GetNonClientArea(IntPtr hwnd)
	{
		POINT point = new POINT(0, 0);
		UnsafeNativeMethods.ClientToScreen(hwnd, ref point);
		RECT rect = default(RECT);
		UnsafeNativeMethods.GetWindowRect(hwnd, ref rect);
		return new Size(point.X - rect.left, point.Y - rect.top);
	}
}
