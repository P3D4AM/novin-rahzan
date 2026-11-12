using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Stimulsoft.BlocklyEditor.Wpf;

internal static class WindowExtensions
{
	private const int GWL_STYLE = -16;

	private const int WS_MAXIMIZEBOX = 65536;

	private const int WS_MINIMIZEBOX = 131072;

	[DllImport("user32.dll")]
	private static extern int GetWindowLong(IntPtr hwnd, int index);

	[DllImport("user32.dll")]
	private static extern int SetWindowLong(IntPtr hwnd, int index, int value);

	internal static void HideMinimizeAndMaximizeButtons(this Window window)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		IntPtr handle = new WindowInteropHelper(window).Handle;
		int windowLong = GetWindowLong(handle, -16);
		SetWindowLong(handle, -16, windowLong & -65537 & -131073);
	}
}
