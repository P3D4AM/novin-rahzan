using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Stimulsoft.Controls.DesktopIntegration;

[SuppressUnmanagedCodeSecurity]
internal static class UnsafeNativeMethods
{
	private static uint _uTBBCMsg;

	public static uint WM_TaskbarButtonCreated
	{
		get
		{
			if (_uTBBCMsg == 0)
			{
				_uTBBCMsg = RegisterWindowMessage("TaskbarButtonCreated");
			}
			return _uTBBCMsg;
		}
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern uint RegisterWindowMessage(string lpString);

	[DllImport("user32.dll")]
	public static extern int GetWindowText(IntPtr hwnd, StringBuilder str, int maxCount);

	[DllImport("DwmApi.dll")]
	internal static extern int DwmSetWindowAttribute(IntPtr hwnd, uint dwAttributeToSet, IntPtr pvAttributeValue, uint cbAttribute);

	[DllImport("dwmapi.dll")]
	public static extern int DwmInvalidateIconicBitmaps(IntPtr hwnd);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool GetClientRect(IntPtr hwnd, ref RECT rect);

	public static bool GetClientSize(IntPtr hwnd, out Size size)
	{
		RECT rect = default(RECT);
		if (!GetClientRect(hwnd, ref rect))
		{
			size = new Size(-1, -1);
			return false;
		}
		size = new Size(rect.right, rect.bottom);
		return true;
	}

	[DllImport("dwmapi.dll")]
	public static extern int DwmSetIconicThumbnail(IntPtr hwnd, IntPtr hbitmap, uint flags);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool ClientToScreen(IntPtr hwnd, ref POINT point);

	[DllImport("user32.dll")]
	public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

	[DllImport("dwmapi.dll")]
	public static extern int DwmSetIconicLivePreviewBitmap(IntPtr hwnd, IntPtr hbitmap, ref POINT ptClient, uint flags);

	[DllImport("dwmapi.dll")]
	public static extern int DwmSetIconicLivePreviewBitmap(IntPtr hwnd, IntPtr hbitmap, IntPtr ptClient, uint flags);

	[DllImport("user32.dll")]
	public static extern IntPtr GetWindowDC(IntPtr hwnd);

	[DllImport("gdi32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool StretchBlt(IntPtr hDestDC, int destX, int destY, int destWidth, int destHeight, IntPtr hSrcDC, int srcX, int srcY, int srcWidth, int srcHeight, uint operation);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool GetWindowRect(IntPtr hwnd, ref RECT rect);
}
