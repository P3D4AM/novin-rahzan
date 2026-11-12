using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons;

public sealed class CustomWindowsManager
{
	private IntPtr _hwnd;

	private IntPtr _hwndParent;

	private ProxyWindow _proxyWindow;

	private IntPtr WindowToTellDwmAbout
	{
		get
		{
			if (_proxyWindow == null)
			{
				return _hwnd;
			}
			return ((Control)_proxyWindow).Handle;
		}
	}

	public event EventHandler<BitmapRequestedEventArgs> PeekRequested;

	public event EventHandler<BitmapRequestedEventArgs> ThumbnailRequested;

	public static CustomWindowsManager CreateWindowsManager(IntPtr hwnd)
	{
		return CreateWindowsManager(hwnd, IntPtr.Zero);
	}

	public static CustomWindowsManager CreateWindowsManager(IntPtr hwnd, IntPtr parentHwnd)
	{
		if (parentHwnd == IntPtr.Zero)
		{
			return new CustomWindowsManager(hwnd);
		}
		ProxyWindow proxyWindow = new ProxyWindow(hwnd);
		Windows7Taskbar.TaskbarList.UnregisterTab(parentHwnd);
		Windows7Taskbar.TaskbarList.RegisterTab(((Control)proxyWindow).Handle, parentHwnd);
		Windows7Taskbar.TaskbarList.SetTabOrder(((Control)proxyWindow).Handle, IntPtr.Zero);
		Windows7Taskbar.TaskbarList.ActivateTab(((Control)proxyWindow).Handle);
		return new CustomWindowsManager(proxyWindow, parentHwnd);
	}

	internal CustomWindowsManager(IntPtr hwnd)
	{
		_hwnd = hwnd;
		Windows7Taskbar.EnableCustomWindowPreview(hwnd);
	}

	internal CustomWindowsManager(ProxyWindow proxy, IntPtr hwndParent)
	{
		_hwnd = proxy.RealWindow;
		_hwndParent = hwndParent;
		_proxyWindow = proxy;
		_proxyWindow.WindowsManager = this;
		Windows7Taskbar.EnableCustomWindowPreview(WindowToTellDwmAbout);
	}

	public void InvalidatePreviews()
	{
		UnsafeNativeMethods.DwmInvalidateIconicBitmaps(WindowToTellDwmAbout);
	}

	public void DisablePreview()
	{
		Windows7Taskbar.DisableCustomWindowPreview(WindowToTellDwmAbout);
	}

	public void EnablePreview()
	{
		Windows7Taskbar.EnableCustomWindowPreview(WindowToTellDwmAbout);
	}

	public void WindowClosed()
	{
		Windows7Taskbar.TaskbarList.UnregisterTab(_hwnd);
		((Form)_proxyWindow).Close();
	}

	public void DispatchMessage(ref Message m)
	{
		if (((Message)(ref m)).Msg == 6 && _hwndParent != IntPtr.Zero && ((int)((Message)(ref m)).WParam == 1 || (int)((Message)(ref m)).WParam == 2))
		{
			UnsafeNativeMethods.SendMessage(_hwnd, (uint)((Message)(ref m)).Msg, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		}
		if (((Message)(ref m)).Msg == 274 && _hwndParent != IntPtr.Zero && (int)((Message)(ref m)).WParam == 61536)
		{
			UnsafeNativeMethods.SendMessage(_hwnd, 16u, IntPtr.Zero, IntPtr.Zero);
			WindowClosed();
		}
		if (((Message)(ref m)).Msg == 803)
		{
			int num = (int)((long)((Message)(ref m)).LParam >> 16);
			int num2 = (int)((long)((Message)(ref m)).LParam & 0xFFFF);
			BitmapRequestedEventArgs bitmapRequestedEventArgs = new BitmapRequestedEventArgs(num, num2, defaultDisplayFrame: true);
			this.ThumbnailRequested(this, bitmapRequestedEventArgs);
			if (bitmapRequestedEventArgs.UseWindowScreenshot)
			{
				UnsafeNativeMethods.GetClientSize(_hwnd, out var size);
				float num3 = (float)num / (float)num2;
				float num4 = (float)size.Width / (float)size.Height;
				if (num4 > num3)
				{
					num2 = (int)((float)num2 * (num3 / num4));
				}
				if (num4 < num3)
				{
					num = (int)((float)num * (num4 / num3));
				}
				bitmapRequestedEventArgs.Bitmap = ScreenCapture.GrabWindowBitmap(_hwnd, new Size(num, num2));
			}
			else if (!bitmapRequestedEventArgs.DoNotMirrorBitmap)
			{
				((Image)bitmapRequestedEventArgs.Bitmap).RotateFlip((RotateFlipType)6);
			}
			Windows7Taskbar.SetIconicThumbnail(WindowToTellDwmAbout, bitmapRequestedEventArgs.Bitmap);
			((Image)bitmapRequestedEventArgs.Bitmap).Dispose();
		}
		else if (((Message)(ref m)).Msg == 806)
		{
			if (!UnsafeNativeMethods.GetClientSize(_hwnd, out var size2))
			{
				size2 = new Size(50, 50);
			}
			BitmapRequestedEventArgs bitmapRequestedEventArgs2 = new BitmapRequestedEventArgs(size2.Width, size2.Height, _hwndParent == IntPtr.Zero);
			this.PeekRequested(this, bitmapRequestedEventArgs2);
			if (bitmapRequestedEventArgs2.UseWindowScreenshot)
			{
				bitmapRequestedEventArgs2.Bitmap = ScreenCapture.GrabWindowBitmap(_hwnd, size2);
			}
			else if (!bitmapRequestedEventArgs2.DoNotMirrorBitmap)
			{
				((Image)bitmapRequestedEventArgs2.Bitmap).RotateFlip((RotateFlipType)6);
			}
			if (_hwndParent != IntPtr.Zero)
			{
				Point parentOffsetOfChild = WindowUtilities.GetParentOffsetOfChild(_hwnd, _hwndParent);
				Windows7Taskbar.SetPeekBitmap(WindowToTellDwmAbout, bitmapRequestedEventArgs2.Bitmap, parentOffsetOfChild, bitmapRequestedEventArgs2.DisplayFrameAroundBitmap);
			}
			else
			{
				Windows7Taskbar.SetPeekBitmap(WindowToTellDwmAbout, bitmapRequestedEventArgs2.Bitmap, bitmapRequestedEventArgs2.DisplayFrameAroundBitmap);
			}
			((Image)bitmapRequestedEventArgs2.Bitmap).Dispose();
		}
	}
}
