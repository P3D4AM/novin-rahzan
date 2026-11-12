using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Hook : IDisposable
{
	private delegate IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam);

	private delegate IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

	private struct POINT
	{
		public int x;

		public int y;

		public POINT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	private struct MOUSEHOOKSTRUCT
	{
		public POINT pt;

		public IntPtr hwnd;

		public ushort wHitTestCode;

		public IntPtr dwExtraInfo;

		public MOUSEHOOKSTRUCT(POINT pt, IntPtr hwnd, ushort wHitTestCode, IntPtr dwExtraInfo)
		{
			this.pt = pt;
			this.hwnd = hwnd;
			this.wHitTestCode = wHitTestCode;
			this.dwExtraInfo = dwExtraInfo;
		}
	}

	private const int WH_MOUSE = 7;

	private const int HC_ACTION = 0;

	private const int WM_MOUSEMOVE = 512;

	private const int WH_KEYBOARD = 2;

	private const int KF_EXTENDED = 256;

	private const int KF_DLGMODE = 2048;

	private const int KF_MENUMODE = 4096;

	private const int KF_ALTDOWN = 8192;

	private const int KF_REPEAT = 16384;

	private const int KF_UP = 32768;

	private IntPtr m_hMouseHook = IntPtr.Zero;

	private IntPtr m_hKeyboardHook = IntPtr.Zero;

	private MouseProc m_mouseHook;

	private KeyboardProc m_keyboardHook;

	private IMessageHandlerClient m_Client;

	public Hook(IMessageHandlerClient client)
	{
		m_Client = client;
		m_mouseHook = OnMouseHook;
		m_keyboardHook = OnKeyboardHook;
		m_hMouseHook = SetWindowsHookEx(7, m_mouseHook, IntPtr.Zero, GetCurrentThreadId());
		m_hKeyboardHook = SetWindowsHookExKeyboard(2, m_keyboardHook, IntPtr.Zero, GetCurrentThreadId());
		if (m_hMouseHook == IntPtr.Zero || m_hKeyboardHook == IntPtr.Zero)
		{
			throw new Win32Exception();
		}
	}

	public void Dispose()
	{
		if (m_hMouseHook != IntPtr.Zero)
		{
			IntPtr hMouseHook = m_hMouseHook;
			m_hMouseHook = IntPtr.Zero;
			UnhookWindowsHookEx(hMouseHook);
		}
		if (m_hKeyboardHook != IntPtr.Zero)
		{
			IntPtr hKeyboardHook = m_hKeyboardHook;
			m_hKeyboardHook = IntPtr.Zero;
			UnhookWindowsHookEx(hKeyboardHook);
		}
	}

	private IntPtr OnMouseHook(int nCode, IntPtr wParam, IntPtr lParam)
	{
		if (nCode == 0)
		{
			int num = WinApi.ToInt(wParam);
			if (num == 513 || num == 161 || num == 516 || num == 519 || num == 167 || num == 164)
			{
				MOUSEHOOKSTRUCT mOUSEHOOKSTRUCT = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MOUSEHOOKSTRUCT));
				m_Client.OnMouseDown(mOUSEHOOKSTRUCT.hwnd, IntPtr.Zero, lParam);
			}
		}
		return CallNextHookEx(m_hMouseHook, nCode, wParam, lParam);
	}

	private IntPtr OnKeyboardHook(int nCode, IntPtr wParam, IntPtr lParam)
	{
		if (nCode == 0)
		{
			int num = WinApi.ToInt(lParam);
			int num2 = WinApi.ToInt(wParam);
			int num3 = num >> 16;
			if (((uint)num3 & 0x2000u) != 0)
			{
				if (((uint)num3 & 0x8000u) != 0)
				{
					if (m_Client.OnSysKeyUp(IntPtr.Zero, wParam, lParam))
					{
						return (IntPtr)1;
					}
				}
				else if (m_Client.OnSysKeyDown(IntPtr.Zero, wParam, lParam))
				{
					return (IntPtr)1;
				}
			}
			else if ((num3 & 0x8000) == 0)
			{
				if (m_Client.OnKeyDown(IntPtr.Zero, wParam, lParam))
				{
					return (IntPtr)1;
				}
			}
			else if ((num2 == 18 || num2 == 121) && m_Client.OnSysKeyUp(IntPtr.Zero, wParam, lParam))
			{
				return (IntPtr)1;
			}
		}
		return CallNextHookEx(m_hKeyboardHook, nCode, wParam, lParam);
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr SetWindowsHookEx(int hookid, MouseProc pfnhook, IntPtr hinst, int threadid);

	[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowsHookEx", SetLastError = true)]
	private static extern IntPtr SetWindowsHookExKeyboard(int hookid, KeyboardProc pfnhook, IntPtr hinst, int threadid);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern bool UnhookWindowsHookEx(IntPtr hhook);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wparam, IntPtr lparam);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern int GetCurrentThreadId();
}
