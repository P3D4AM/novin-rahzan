using System;
using System.Drawing;

namespace Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons;

public static class ScreenCapture
{
	public static Bitmap GrabWindowBitmap(IntPtr hwnd, Size bitmapSize)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c1: Expected O, but got Unknown
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			UnsafeNativeMethods.GetClientSize(hwnd, out var size);
			intPtr = UnsafeNativeMethods.GetWindowDC(hwnd);
			Bitmap val = new Bitmap(bitmapSize.Width, bitmapSize.Height);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				IntPtr hdc = val2.GetHdc();
				uint operation = 1087111200u;
				Size nonClientArea = WindowUtilities.GetNonClientArea(hwnd);
				UnsafeNativeMethods.StretchBlt(hdc, 0, 0, ((Image)val).Width, ((Image)val).Height, intPtr, nonClientArea.Width, size.Height + nonClientArea.Height - 1, size.Width, -size.Height, operation);
				val2.ReleaseHdc();
				val2.DrawString("Windows 7 Bridge", new Font(FontFamily.GenericMonospace, 12f), (Brush)new SolidBrush(Color.Red), new PointF(10f, 10f));
				return val;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				UnsafeNativeMethods.ReleaseDC(hwnd, intPtr);
			}
		}
	}
}
