using System;
using System.Runtime.InteropServices;

namespace Stimulsoft.Controls.Win.DotNetBar.Touch;

internal class WinApi
{
	public delegate uint WindowProcDelegate(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam);

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

	public enum DigitizerIndex
	{
		SM_DIGITIZER = 94,
		SM_MAXIMUMTOUCHES
	}

	public enum TouchWindowFlag : uint
	{
		FineTouch = 1u,
		WantPalm
	}

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

	public struct POINTS
	{
		public short x;

		public short y;
	}

	public struct POINT
	{
		public int x;

		public int y;
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

	public struct GESTURENOTIFYSTRUCT
	{
		public uint CbSize;

		public uint DwFlags;

		public IntPtr HwndTarget;

		public POINTS PtsLocation;

		public uint DwInstanceId;
	}

	public const int GWLP_WNDPROC = -4;

	public const uint GF_BEGIN = 1u;

	public const uint GF_INERTIA = 2u;

	public const uint GF_END = 4u;

	public const uint GID_BEGIN = 1u;

	public const uint GID_END = 2u;

	public const uint GID_ZOOM = 3u;

	public const uint GID_PAN = 4u;

	public const uint GID_ROTATE = 5u;

	public const uint GID_TWOFINGERTAP = 6u;

	public const uint GID_PRESSANDTAP = 7u;

	public const uint GC_ALLGESTURES = 1u;

	public const uint WM_GESTURE = 281u;

	public const uint WM_GESTURENOTIFY = 282u;

	public const int WM_TOUCH = 576;

	public const int TOUCHINPUTMASKF_TIMEFROMSYSTEM = 1;

	public const int TOUCHINPUTMASKF_EXTRAINFO = 2;

	public const int TOUCHINPUTMASKF_CONTACTAREA = 4;

	public const int TOUCHEVENTF_MOVE = 1;

	public const int TOUCHEVENTF_DOWN = 2;

	public const int TOUCHEVENTF_UP = 4;

	public const int TOUCHEVENTF_INRANGE = 8;

	public const int TOUCHEVENTF_PRIMARY = 16;

	public const int TOUCHEVENTF_NOCOALESCE = 32;

	public const int TOUCHEVENTF_PEN = 64;

	public const int TOUCHEVENTF_PALM = 128;

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

	[DllImport("user32")]
	public static extern uint CallWindowProc(IntPtr prevWndFunc, IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam);

	public static DigitizerStatus GetDigitizerStatus()
	{
		return (DigitizerStatus)GetDigitizerCapabilities(DigitizerIndex.SM_DIGITIZER);
	}

	public static ushort GID_ROTATE_ANGLE_TO_ARGUMENT(ushort arg)
	{
		return (ushort)(((double)(int)arg + 6.2831853) / 12.5663706 * 65535.0);
	}

	public static double GID_ROTATE_ANGLE_FROM_ARGUMENT(ushort arg)
	{
		return (double)(int)arg / 65535.0 * 4.0 * 3.14159265 - 6.2831853;
	}

	public static ushort LoWord(uint number)
	{
		return (ushort)(number & 0xFFFFu);
	}

	public static ushort HiWord(uint number)
	{
		return (ushort)((number >> 16) & 0xFFFFu);
	}

	public static uint LoDWord(ulong number)
	{
		return (uint)(number & 0xFFFFFFFFu);
	}

	public static uint HiDWord(ulong number)
	{
		return (uint)((number >> 32) & 0xFFFFFFFFu);
	}

	public static short LoWord(int number)
	{
		return (short)number;
	}

	public static short HiWord(int number)
	{
		return (short)(number >> 16);
	}

	public static int LoDWord(long number)
	{
		return (int)number;
	}

	public static int HiDWord(long number)
	{
		return (int)(number >> 32);
	}
}
