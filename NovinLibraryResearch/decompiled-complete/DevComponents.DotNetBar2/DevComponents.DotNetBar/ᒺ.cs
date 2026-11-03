using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar;

internal class ᒺ : IDisposable
{
	private delegate IntPtr ᓌ(int nCode, IntPtr wParam, IntPtr lParam);

	private delegate IntPtr ᓍ(int nCode, IntPtr wParam, IntPtr lParam);

	private struct ᓎ
	{
		public ᓒ ᓏ;

		public IntPtr _0F05;

		public ushort ᓐ;

		public IntPtr ᓑ;

		public ᓎ(ᓒ pt, IntPtr hwnd, ushort wHitTestCode, IntPtr dwExtraInfo)
		{
			ᓏ = pt;
			_0F05 = hwnd;
			ᓐ = wHitTestCode;
			ᓑ = dwExtraInfo;
		}
	}

	private struct ᓒ
	{
		public int و;

		public int ى;

		public ᓒ(int x, int y)
		{
			و = x;
			ى = y;
		}
	}

	private const int ᒻ = 7;

	private const int ᒼ = 0;

	private const int _08E6 = 512;

	private const int ᒽ = 2;

	private const int ᒾ = 256;

	private const int ᒿ = 2048;

	private const int ᓀ = 4096;

	private const int ᓁ = 8192;

	private const int ᓂ = 16384;

	private const int ᓃ = 32768;

	private IntPtr ᓄ = IntPtr.Zero;

	private IntPtr ᓅ = IntPtr.Zero;

	private ᓌ ᓆ;

	private ᓍ ᓇ;

	private ሎ ᓈ;

	public ᒺ(ሎ client)
	{
		ᓈ = client;
		ᓆ = ᓉ;
		ᓇ = ᓋ;
		ᓄ = SetWindowsHookEx(7, ᓆ, IntPtr.Zero, GetCurrentThreadId());
		ᓅ = SetWindowsHookExKeyboard(2, ᓇ, IntPtr.Zero, GetCurrentThreadId());
		if (ᓄ == IntPtr.Zero || ᓅ == IntPtr.Zero)
		{
			throw new Win32Exception();
		}
	}

	public void Dispose()
	{
		if (ᓄ != IntPtr.Zero)
		{
			IntPtr hhook = ᓄ;
			ᓄ = IntPtr.Zero;
			UnhookWindowsHookEx(hhook);
		}
		if (ᓅ != IntPtr.Zero)
		{
			IntPtr hhook2 = ᓅ;
			ᓅ = IntPtr.Zero;
			UnhookWindowsHookEx(hhook2);
		}
	}

	private IntPtr ᓉ(int ᓊ, IntPtr _੦, IntPtr _੧)
	{
		if (ᓊ == 0)
		{
			int num = _0EFE._0F11(_੦);
			if (num == 513 || num == 161 || num == 516 || num == 519 || num == 167 || num == 164)
			{
				ᓎ ᓎ = (ᓎ)Marshal.PtrToStructure(_੧, typeof(ᓎ));
				ᓈ.ሏ(ᓎ._0F05, IntPtr.Zero, _੧);
			}
		}
		return CallNextHookEx(ᓄ, ᓊ, _੦, _੧);
	}

	private IntPtr ᓋ(int ᓊ, IntPtr _੦, IntPtr _੧)
	{
		if (ᓊ == 0)
		{
			int num = _0EFE._0F11(_੧);
			int num2 = _0EFE._0F11(_੦);
			int num3 = num >> 16;
			if (((uint)num3 & 0x2000u) != 0)
			{
				if (((uint)num3 & 0x8000u) != 0)
				{
					if (ᓈ.ੳ(IntPtr.Zero, _੦, _੧))
					{
						return (IntPtr)1;
					}
				}
				else if (ᓈ._0A71(IntPtr.Zero, _੦, _੧))
				{
					return (IntPtr)1;
				}
			}
			else if ((num3 & 0x8000) == 0)
			{
				if (ᓈ._੪(IntPtr.Zero, _੦, _੧))
				{
					return (IntPtr)1;
				}
			}
			else if ((num2 == 18 || num2 == 121) && ᓈ.ੳ(IntPtr.Zero, _੦, _੧))
			{
				return (IntPtr)1;
			}
		}
		return CallNextHookEx(ᓅ, ᓊ, _੦, _੧);
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr SetWindowsHookEx(int hookid, ᓌ pfnhook, IntPtr hinst, int threadid);

	[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowsHookEx", SetLastError = true)]
	private static extern IntPtr SetWindowsHookExKeyboard(int hookid, ᓍ pfnhook, IntPtr hinst, int threadid);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern bool UnhookWindowsHookEx(IntPtr hhook);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wparam, IntPtr lparam);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern int GetCurrentThreadId();
}
