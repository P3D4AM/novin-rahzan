using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class OSUtils
{
	public const int WM_CAPTURECHANGED = 533;

	public const int WM_THEMECHANGED = 794;

	public const int Message_CloseDropped = 1025;

	public const int Message_InitDialog = 272;

	public const int Message_NCPaint = 133;

	public const int Message_NCActivate = 134;

	public const int Message_SetCursor = 32;

	public const int Message_VScroll = 277;

	public const int Message_HScroll = 276;

	public const int WindowStyle_Border = 8388608;

	public const int WindowStyle_ClientEdge = 512;

	public const int Message_KeyDown = 256;

	public const int Message_SysKeyDown = 260;

	public const int Message_Char = 258;

	public const int Message_LButtonDown = 513;

	public const int Message_LButtonUp = 514;

	public const int Message_MouseMove = 512;

	public const int Message_Cut = 768;

	public const int Message_Copy = 769;

	public const int Message_Paste = 770;

	public const int Message_Clear = 771;

	public const int Message_CanUndo = 198;

	public const int Message_Undo = 772;

	public const int BorderFlags_Adjust = 8192;

	public const int BorderFlags_Mono = 32768;

	public const int BorderFlags_Flat = 16384;

	public const int BorderFlags_Soft = 4096;

	public static bool ShowWindowTopMost(IntPtr hwnd, int x, int y, int cx, int cy)
	{
		return Win32.SetWindowPos(hwnd, (IntPtr)(-1), x, y, cx, cy, 80);
	}

	public static Size GetScreenCaps()
	{
		IntPtr dC = Win32.GetDC(IntPtr.Zero);
		try
		{
			return new Size(Win32.GetDeviceCaps(dC, 88), Win32.GetDeviceCaps(dC, 90));
		}
		finally
		{
			Win32.ReleaseDC(IntPtr.Zero, dC);
		}
	}

	public static void SetScrollBar(IntPtr handle, bool flat, int size, int pageSize, bool isVertical)
	{
		if (flat)
		{
			Win32.FlatSB_SetScrollBarInfo(handle, size, pageSize, isVertical ? 1 : 0);
		}
		else
		{
			Win32.SetScrollBarInfo(handle, size, pageSize, isVertical ? 1 : 0);
		}
	}

	public static void SetScrollPos(IntPtr handle, bool flat, int pos, bool isVertical)
	{
		if (flat)
		{
			Win32.FlatSB_SetScrollPos(handle, isVertical ? 1 : 0, pos);
		}
		else
		{
			Win32.SetScrollPos(handle, isVertical ? 1 : 0, pos);
		}
	}

	public static int GetScrollPos(IntPtr handle, bool flat, bool isVertical)
	{
		if (flat)
		{
			return Win32.FlatSB_GetScrollPos(handle, isVertical ? 1 : 0);
		}
		return Win32.GetScrollPos(handle, isVertical ? 1 : 0);
	}

	public static int GetScrollSize(IntPtr handle, bool flat, bool isVertical)
	{
		if (flat)
		{
			return Win32.FlatSB_GetScrollSize(handle, isVertical ? 1 : 0);
		}
		return Win32.GetScrollSize(handle, isVertical ? 1 : 0);
	}

	public static int GetScrollSize(bool isVertical)
	{
		return Win32.GetSystemMetrics(isVertical ? 20 : 21);
	}

	public static int GetDoubleClickTime()
	{
		return Win32.GetDoubleClickTime();
	}

	public static IntPtr SendMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam)
	{
		return Win32.SendMessage(hwnd, msg, wparam, lparam);
	}

	public static bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam)
	{
		return Win32.PostMessage(hwnd, msg, wparam, lparam);
	}

	public static IntPtr SetMouseHook(HookHandler lpfn)
	{
		return Win32.SetWindowsHookEx(7, lpfn, IntPtr.Zero, Thread.CurrentThread.ManagedThreadId);
	}

	public static IntPtr SetWndProcHook(HookHandler lpfn)
	{
		return Win32.SetWindowsHookEx(4, lpfn, IntPtr.Zero, Thread.CurrentThread.ManagedThreadId);
	}

	public static bool ReleaseHook(IntPtr hhk)
	{
		return Win32.UnhookWindowsHookEx(hhk);
	}

	public static IntPtr CallNextHook(IntPtr hhk, int nCode, IntPtr wparam, IntPtr lparam)
	{
		return Win32.CallNextHookEx(hhk, nCode, wparam, lparam);
	}

	public static IntPtr GetMouseHookHandle(IntPtr lparam, out Point pt)
	{
		Win32.MOUSEHOOKSTRUCT mOUSEHOOKSTRUCT = (Win32.MOUSEHOOKSTRUCT)Marshal.PtrToStructure(lparam, typeof(Win32.MOUSEHOOKSTRUCT));
		pt = new Point(mOUSEHOOKSTRUCT.Pt.X, mOUSEHOOKSTRUCT.Pt.Y);
		return mOUSEHOOKSTRUCT.hwnd;
	}

	public static bool EnumChildWindows(IntPtr hwndParent, EnumChildProc lpEnumFunc, IntPtr lparam)
	{
		return Win32.EnumChildWindows(hwndParent, lpEnumFunc, lparam);
	}

	public static bool IsMouseMsg(int msg)
	{
		if (msg < 513 || msg > 520)
		{
			if (msg >= 161)
			{
				return msg <= 169;
			}
			return false;
		}
		return true;
	}

	public static string GetText(IntPtr hwnd)
	{
		return Win32.GetText(hwnd);
	}

	public static void SetText(IntPtr hwnd, string text)
	{
		Win32.SetText(hwnd, text);
	}

	public static string GetClassName(IntPtr hwnd)
	{
		return Win32.GetClassName(hwnd);
	}

	public static IntPtr GetWindowDC(IntPtr hwnd)
	{
		return Win32.GetWindowDC(hwnd);
	}

	public static int ReleaseDC(IntPtr hwnd, IntPtr hdc)
	{
		return Win32.ReleaseDC(hwnd, hdc);
	}

	public static bool CreateCaret(IntPtr handle, int nWidth, int nHeight)
	{
		return Win32.CreateCaret(handle, IntPtr.Zero, nWidth, nHeight);
	}

	public static bool ShowCaret(IntPtr handle)
	{
		return Win32.ShowCaret(handle);
	}

	public static bool DestroyCaret()
	{
		return Win32.DestroyCaret();
	}

	public static bool SetCaretPos(int x, int y)
	{
		return Win32.SetCaretPos(x, y);
	}

	public static IntPtr SetCursor(IntPtr handle)
	{
		return Win32.SetCursor(handle);
	}

	public static short LoWord(IntPtr value)
	{
		return (short)value.ToInt32();
	}

	public static short HiWord(IntPtr value)
	{
		return (short)(value.ToInt32() >> 16);
	}

	public static void ScrollWindow(IntPtr handle, int x, int y, Rectangle rect)
	{
		Win32.ScrollWindow(handle, x, y, rect);
	}

	public static bool InitCommonControls()
	{
		return Win32.InitCommonControls(8);
	}

	public static int ExcludeClipRect(IntPtr hdc, int l, int t, int r, int b)
	{
		return Win32.ExcludeClipRect(hdc, l, t, r, b);
	}

	public static ScrollEventType GetScrollType(int code)
	{
		return (ScrollEventType)(code switch
		{
			0 => 0, 
			1 => 1, 
			2 => 2, 
			3 => 3, 
			4 => 4, 
			5 => 5, 
			6 => 6, 
			7 => 7, 
			_ => 8, 
		});
	}

	public static bool InitializeFlatSB(IntPtr handle)
	{
		bool flag = Win32.InitializeFlatSB(handle);
		if (flag)
		{
			Win32.FlatSB_SetScrollProp(handle, 512, new IntPtr(2), setValue: true);
			Win32.FlatSB_SetScrollProp(handle, 256, new IntPtr(2), setValue: true);
		}
		return flag;
	}

	public static bool UninitializeFlatSB(IntPtr handle)
	{
		return Win32.UninitializeFlatSB(handle);
	}

	public static void MessageBeep()
	{
		Win32.MessageBeep(16);
	}

	public static void Beep(int freq, int duration)
	{
		Win32.Beep(freq, duration);
	}
}
