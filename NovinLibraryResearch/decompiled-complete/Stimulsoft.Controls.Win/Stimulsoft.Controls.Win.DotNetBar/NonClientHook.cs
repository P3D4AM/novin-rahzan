using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class NonClientHook
{
	private delegate IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam);

	private struct POINT
	{
		public int x;

		public int y;

		public POINT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public Point ToPoint()
		{
			return new Point(x, y);
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

	private static ArrayList m_Clients;

	private static Hashtable m_FilterOnThread;

	private static ReaderWriterLock rwClientsListLock;

	private static MouseProc m_MouseHook;

	private const int WH_MOUSE = 7;

	private const int HC_ACTION = 0;

	private const int WH_CALLWNDPROCRET = 12;

	private const int WH_CALLWNDPROC = 4;

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr SetWindowsHookEx(int hookid, MouseProc pfnhook, IntPtr hinst, int threadid);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern bool UnhookWindowsHookEx(IntPtr hhook);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wparam, IntPtr lparam);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern int GetCurrentThreadId();

	static NonClientHook()
	{
		m_Clients = new ArrayList();
		m_FilterOnThread = new Hashtable();
		m_MouseHook = null;
		rwClientsListLock = new ReaderWriterLock();
		m_MouseHook = OnMouseHook;
	}

	public static void RegisterHook(ISkinHook client)
	{
		if (m_Clients.Contains(client))
		{
			return;
		}
		if (!m_FilterOnThread.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			HookThread();
		}
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = rwClientsListLock.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = rwClientsListLock.UpgradeToWriterLock(-1);
		}
		else
		{
			rwClientsListLock.AcquireWriterLock(-1);
		}
		try
		{
			m_Clients.Add(client);
		}
		finally
		{
			if (isReaderLockHeld)
			{
				rwClientsListLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				rwClientsListLock.ReleaseWriterLock();
			}
		}
	}

	public static void UnregisterHook(ISkinHook client)
	{
		if (!m_Clients.Contains(client))
		{
			return;
		}
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = rwClientsListLock.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = rwClientsListLock.UpgradeToWriterLock(-1);
		}
		else
		{
			rwClientsListLock.AcquireWriterLock(-1);
		}
		try
		{
			m_Clients.Remove(client);
			if (m_Clients.Count == 0)
			{
				UnHookThread();
			}
		}
		finally
		{
			if (isReaderLockHeld)
			{
				rwClientsListLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				rwClientsListLock.ReleaseWriterLock();
			}
		}
	}

	private static void HookThread()
	{
		if (!m_FilterOnThread.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			int currentThreadId = GetCurrentThreadId();
			IntPtr intPtr = SetWindowsHookEx(7, m_MouseHook, IntPtr.Zero, currentThreadId);
			m_FilterOnThread.Add(Thread.CurrentThread.GetHashCode(), intPtr);
		}
	}

	private static void UnHookThread()
	{
		if (m_FilterOnThread.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			IntPtr hhook = (IntPtr)m_FilterOnThread[Thread.CurrentThread.GetHashCode()];
			UnhookWindowsHookEx(hhook);
			m_FilterOnThread.Remove(Thread.CurrentThread.GetHashCode());
		}
	}

	private static ISkinHook[] GetMessageClients()
	{
		rwClientsListLock.AcquireReaderLock(-1);
		try
		{
			return (ISkinHook[])m_Clients.ToArray(typeof(ISkinHook));
		}
		finally
		{
			rwClientsListLock.ReleaseReaderLock();
		}
	}

	private unsafe static IntPtr OnMouseHook(int nCode, IntPtr wParam, IntPtr lParam)
	{
		try
		{
			if (nCode == 0)
			{
				int num = WinApi.ToInt(wParam);
				if (num == 512)
				{
					MOUSEHOOKSTRUCT* ptr = (MOUSEHOOKSTRUCT*)(void*)lParam;
					PostMouseMove(ptr->hwnd, ptr->pt.ToPoint());
				}
				if (num == 514)
				{
					MOUSEHOOKSTRUCT* ptr2 = (MOUSEHOOKSTRUCT*)(void*)lParam;
					PostMouseUp(ptr2->hwnd, ptr2->pt.ToPoint());
				}
			}
			IntPtr hhook = (IntPtr)m_FilterOnThread[Thread.CurrentThread.GetHashCode()];
			return CallNextHookEx(hhook, nCode, wParam, lParam);
		}
		catch
		{
		}
		return IntPtr.Zero;
	}

	public static bool PostMouseMove(IntPtr hWnd, Point mousePos)
	{
		ISkinHook[] messageClients = GetMessageClients();
		ISkinHook[] array = messageClients;
		foreach (ISkinHook skinHook in array)
		{
			skinHook.PostMouseMove(hWnd, mousePos);
		}
		return false;
	}

	public static bool PostMouseUp(IntPtr hWnd, Point mousePos)
	{
		ISkinHook[] messageClients = GetMessageClients();
		ISkinHook[] array = messageClients;
		foreach (ISkinHook skinHook in array)
		{
			skinHook.PostMouseUp(hWnd, mousePos);
		}
		return false;
	}
}
