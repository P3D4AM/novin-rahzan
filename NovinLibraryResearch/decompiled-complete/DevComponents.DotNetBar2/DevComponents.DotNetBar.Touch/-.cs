using System;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar.Touch;

internal class _0EFE
{
	public struct GESTUREINFO
	{
		public uint cbSize;

		public uint dwFlags;

		public uint dwID;

		public IntPtr hwndTarget;

		public POINTS ptsLocation;

		public uint dwInstanceID;

		public uint dwSequenceID;

		public ulong ullArguments;

		public uint cbExtraArgs;
	}

	public struct GESTURECONFIG
	{
		public uint dwID;

		public uint dwWant;

		public uint dwBlock;

		public GESTURECONFIG(uint dwID, uint dwWant, uint dwBlock)
		{
			this.dwID = dwID;
			this.dwWant = dwWant;
			this.dwBlock = dwBlock;
		}
	}

	public enum TouchWindowFlag : uint
	{
		FineTouch = 1u,
		WantPalm
	}

	public struct TOUCHINPUT
	{
		public int x;

		public int y;

		public IntPtr hSource;

		public int dwID;

		public int dwFlags;

		public int dwMask;

		public int dwTime;

		public IntPtr dwExtraInfo;

		public int cxContact;

		public int cyContact;
	}

	public enum DigitizerIndex
	{
		SM_DIGITIZER = 94,
		SM_MAXIMUMTOUCHES
	}

	public delegate uint WindowProcDelegate(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam);

	[Flags]
	public enum DigitizerStatus : byte
	{
		IntegratedTouch = 1,
		ExternalTouch = 2,
		IntegratedPan = 4,
		ExternalPan = 8,
		MultiInput = 0x40,
		StackReady = 0x80
	}

	public struct POINTS
	{
		public short x;

		public short y;
	}

	public struct GESTURENOTIFYSTRUCT
	{
		public uint cbSize;

		public uint dwFlags;

		public IntPtr hwndTarget;

		public POINTS ptsLocation;

		public uint dwInstanceID;
	}

	public struct POINT
	{
		public int x;

		public int y;
	}

	public const int _1ACE = -4;

	public const uint _1ACF = 1u;

	public const uint _1AD0 = 2u;

	public const uint _1AD1 = 4u;

	public const uint _1AD2 = 1u;

	public const uint _1AD3 = 2u;

	public const uint _1AD4 = 3u;

	public const uint _1AD5 = 4u;

	public const uint _1AD6 = 5u;

	public const uint _1AD7 = 6u;

	public const uint _1AD8 = 7u;

	public const uint _1AD9 = 1u;

	public const uint _1ADA = 281u;

	public const uint _09BA = 282u;

	public const int _1ADB = 576;

	public const int _1ADC = 1;

	public const int _1ADD = 2;

	public const int _1ADE = 4;

	public const int _1ADF = 1;

	public const int _1AE0 = 2;

	public const int _1AE1 = 4;

	public const int _1AE2 = 8;

	public const int _1AE3 = 16;

	public const int _1AE4 = 32;

	public const int _1AE5 = 64;

	public const int _1AE6 = 128;

	[DllImport("user32")]
	public static extern bool SetGestureConfig(IntPtr hwnd, uint dwReserved, uint cIDs, GESTURECONFIG[] pGestureConfig, uint cbSize);

	[DllImport("user32")]
	public static extern bool RegisterTouchWindow(IntPtr hWnd, TouchWindowFlag flags);

	[DllImport("user32")]
	public static extern bool UnregisterTouchWindow(IntPtr hWnd);

	[DllImport("user32")]
	public static extern bool IsTouchWindow(IntPtr hWnd, out uint ulFlags);

	[DllImport("user32")]
	public static extern bool GetTouchInputInfo(IntPtr hTouchInput, int cInputs, [In][Out] TOUCHINPUT[] pInputs, int cbSize);

	[DllImport("user32")]
	public static extern void CloseTouchInputHandle(IntPtr lParam);

	[DllImport("user32")]
	public static extern bool SetProp(IntPtr hWnd, string lpString, IntPtr hData);

	[DllImport("user32", EntryPoint = "GetSystemMetrics")]
	public static extern int GetDigitizerCapabilities(DigitizerIndex index);

	[DllImport("user32")]
	public static extern bool GetGestureInfo(IntPtr hGestureInfo, ref GESTUREINFO pGestureInfo);

	[DllImport("user32")]
	public static extern bool CloseGestureInfoHandle(IntPtr hGestureInfo);

	[DllImport("user32", EntryPoint = "SetWindowLongPtr")]
	public static extern IntPtr SubclassWindow64(IntPtr hWnd, int nIndex, WindowProcDelegate dwNewLong);

	[DllImport("user32", EntryPoint = "SetWindowLong")]
	public static extern IntPtr SubclassWindow(IntPtr hWnd, int nIndex, WindowProcDelegate dwNewLong);

	[DllImport("user32", EntryPoint = "SetWindowLongPtr")]
	public static extern IntPtr SubclassWindow64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

	[DllImport("user32", EntryPoint = "SetWindowLong")]
	public static extern IntPtr SubclassWindow(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

	[DllImport("user32")]
	public static extern uint CallWindowProc(IntPtr prevWndFunc, IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam);

	public static DigitizerStatus _1AE7()
	{
		return (DigitizerStatus)GetDigitizerCapabilities(DigitizerIndex.SM_DIGITIZER);
	}

	public static ushort _1AE8(ushort _1AE9)
	{
		return (ushort)(((double)(int)_1AE9 + 6.2831853) / 12.5663706 * 65535.0);
	}

	public static double _1AEA(ushort _1AE9)
	{
		return (double)(int)_1AE9 / 65535.0 * 4.0 * 3.14159265 - 6.2831853;
	}

	public static ushort _1AEB(uint _1AEC)
	{
		return (ushort)(_1AEC & 0xFFFFu);
	}

	public static ushort _1AED(uint _1AEC)
	{
		return (ushort)((_1AEC >> 16) & 0xFFFFu);
	}

	public static uint _1AEE(ulong _1AEC)
	{
		return (uint)(_1AEC & 0xFFFFFFFFu);
	}

	public static uint _1AEF(ulong _1AEC)
	{
		return (uint)((_1AEC >> 32) & 0xFFFFFFFFu);
	}

	public static short _1AEB(int _1AEC)
	{
		return (short)_1AEC;
	}

	public static short _1AED(int _1AEC)
	{
		return (short)(_1AEC >> 16);
	}

	public static int _1AEE(long _1AEC)
	{
		return (int)_1AEC;
	}

	public static int _1AEF(long _1AEC)
	{
		return (int)(_1AEC >> 32);
	}
}
