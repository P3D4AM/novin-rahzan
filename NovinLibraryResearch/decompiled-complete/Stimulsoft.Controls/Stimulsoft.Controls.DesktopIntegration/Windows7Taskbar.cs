using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Controls.DesktopIntegration;

public static class Windows7Taskbar
{
	public enum ThumbnailProgressState
	{
		NoProgress = 0,
		Indeterminate = 1,
		Normal = 2,
		Error = 4,
		Paused = 8
	}

	private static ITaskbarList3 taskbarList;

	private static IntPtr LastHwnd;

	public static bool IsWindowsVistaOrGreater { get; private set; }

	[CLSCompliant(false)]
	public static uint TaskbarButtonCreatedMessage => UnsafeNativeMethods.WM_TaskbarButtonCreated;

	internal static ITaskbarList3 TaskbarList
	{
		get
		{
			if (taskbarList == null)
			{
				lock (typeof(Windows7Taskbar))
				{
					if (taskbarList == null)
					{
						taskbarList = (ITaskbarList3)new CTaskbarList();
						taskbarList.HrInit();
					}
				}
			}
			return taskbarList;
		}
	}

	public static bool DisableTaskbarProgress { get; set; }

	public static void ShowTaskbarProgress(object form, ThumbnailProgressState state)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		ShowTaskbarProgress((form is Form) ? ((Control)(Form)form).Handle : IntPtr.Zero, state);
	}

	public static void ShowTaskbarProgress(IntPtr hwnd, ThumbnailProgressState state)
	{
		if (!DisableTaskbarProgress && IsWindowsVistaOrGreater)
		{
			HideTaskbarProgress();
			if (hwnd == IntPtr.Zero)
			{
				hwnd = GetHandleOfTheMainWindow();
			}
			if (hwnd != IntPtr.Zero)
			{
				LastHwnd = hwnd;
				TaskbarList.SetProgressState(LastHwnd, (TBPFLAG)state);
			}
		}
	}

	public static void HideTaskbarProgress()
	{
		if (!DisableTaskbarProgress && IsWindowsVistaOrGreater && LastHwnd != IntPtr.Zero)
		{
			TaskbarList.SetProgressState(LastHwnd, TBPFLAG.TBPF_NOPROGRESS);
			LastHwnd = IntPtr.Zero;
		}
	}

	private static IntPtr GetHandleOfTheMainWindow()
	{
		if (((ReadOnlyCollectionBase)(object)Application.OpenForms).Count > 0)
		{
			FormCollection openForms = Application.OpenForms;
			for (int num = ((ReadOnlyCollectionBase)(object)Application.OpenForms).Count - 1; num >= 0; num--)
			{
				Form val = openForms[num];
				if (!(val is IStiThreadForm) && val.ShowInTaskbar)
				{
					return ((Control)val).Handle;
				}
			}
		}
		return IntPtr.Zero;
	}

	private static void InternalEnableCustomWindowPreview(IntPtr hwnd, bool enable)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(4);
		Marshal.WriteInt32(intPtr, enable ? 1 : 0);
		try
		{
			int num = UnsafeNativeMethods.DwmSetWindowAttribute(hwnd, 10u, intPtr, 4u);
			if (num != 0)
			{
				throw Marshal.GetExceptionForHR(num);
			}
			num = UnsafeNativeMethods.DwmSetWindowAttribute(hwnd, 7u, intPtr, 4u);
			if (num != 0)
			{
				throw Marshal.GetExceptionForHR(num);
			}
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public static void EnableCustomWindowPreview(IntPtr hwnd)
	{
		InternalEnableCustomWindowPreview(hwnd, enable: true);
	}

	public static void DisableCustomWindowPreview(IntPtr hwnd)
	{
		InternalEnableCustomWindowPreview(hwnd, enable: false);
	}

	public static void SetIconicThumbnail(IntPtr hwnd, Bitmap bitmap)
	{
		int num = UnsafeNativeMethods.DwmSetIconicThumbnail(hwnd, bitmap.GetHbitmap(), 1u);
		if (num != 0)
		{
			throw Marshal.GetExceptionForHR(num);
		}
	}

	public static void SetPeekBitmap(IntPtr hwnd, Bitmap bitmap, bool displayFrame)
	{
		int num = UnsafeNativeMethods.DwmSetIconicLivePreviewBitmap(hwnd, bitmap.GetHbitmap(), IntPtr.Zero, displayFrame ? 1u : 0u);
		if (num != 0)
		{
			throw Marshal.GetExceptionForHR(num);
		}
	}

	public static void SetPeekBitmap(IntPtr hwnd, Bitmap bitmap, Point offset, bool displayFrame)
	{
		POINT ptClient = new POINT(offset.X, offset.Y);
		int num = UnsafeNativeMethods.DwmSetIconicLivePreviewBitmap(hwnd, bitmap.GetHbitmap(), ref ptClient, displayFrame ? 1u : 0u);
		if (num != 0)
		{
			throw Marshal.GetExceptionForHR(num);
		}
	}

	public static void SetThumbnailTooltip(IntPtr hWnd, string tooltip)
	{
		if (!IsWindowsVistaOrGreater || hWnd != IntPtr.Zero)
		{
			TaskbarList.SetThumbnailTooltip(hWnd, tooltip);
		}
	}

	static Windows7Taskbar()
	{
		LastHwnd = IntPtr.Zero;
		IsWindowsVistaOrGreater = Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 1;
	}
}
