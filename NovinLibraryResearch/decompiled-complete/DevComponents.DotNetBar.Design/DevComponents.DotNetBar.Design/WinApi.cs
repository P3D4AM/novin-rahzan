using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar.Design;

internal static class WinApi
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class TEXTMETRIC
	{
		public int tmHeight;

		public int tmAscent;

		public int tmDescent;

		public int tmInternalLeading;

		public int tmExternalLeading;

		public int tmAveCharWidth;

		public int tmMaxCharWidth;

		public int tmWeight;

		public int tmOverhang;

		public int tmDigitizedAspectX;

		public int tmDigitizedAspectY;

		public char tmFirstChar;

		public char tmLastChar;

		public char tmDefaultChar;

		public char tmBreakChar;

		public byte tmItalic;

		public byte tmUnderlined;

		public byte tmStruckOut;

		public byte tmPitchAndFamily;

		public byte tmCharSet;
	}

	public struct RECT
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;

		public RECT(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public static RECT FromRectangle(Rectangle rectangle)
		{
			return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
		}
	}

	public enum WindowsMessages
	{
		WM_NCPAINT = 133,
		WM_NCCALCSIZE = 131,
		WM_NCACTIVATE = 134,
		WM_SETTEXT = 12,
		WM_INITMENUPOPUP = 279,
		WM_WINDOWPOSCHANGED = 71,
		WM_NCHITTEST = 132,
		WM_ERASEBKGND = 20,
		WM_NCMOUSEMOVE = 160,
		WM_NCLBUTTONDOWN = 161,
		WM_NCMOUSELEAVE = 674,
		WM_NCLBUTTONUP = 162,
		WM_NCMOUSEHOVER = 672,
		WM_SETCURSOR = 32,
		WM_SETICON = 128,
		WM_HSCROLL = 276,
		WM_VSCROLL = 277,
		WM_MOUSEWHEEL = 522,
		WM_STYLECHANGED = 125,
		WM_NCLBUTTONDBLCLK = 163,
		WM_MOUSEACTIVATE = 33,
		WM_MOUSEMOVE = 512,
		WM_MDISETMENU = 560,
		WM_MDIREFRESHMENU = 564,
		WM_KEYDOWN = 256,
		WM_SIZE = 5,
		WM_DWMCOMPOSITIONCHANGED = 798,
		WM_DRAWITEM = 43,
		SBM_SETPOS = 224,
		SBM_SETSCROLLINFO = 233,
		EM_GETMODIFY = 184,
		EM_SETMODIFY = 185,
		WM_LBUTTONUP = 514,
		WM_LBUTTONDOWN = 513,
		WM_LBUTTONDBLCLK = 515,
		WM_SYSTIMER = 280,
		WM_SETFOCUS = 7,
		WM_KILLFOCUS = 8,
		WM_PAINT = 15,
		WM_COMMAND = 273,
		WM_CTLCOLORBTN = 309,
		WM_CTLCOLORSCROLLBAR = 311,
		WM_MDIACTIVATE = 546,
		WM_CAPTURECHANGED = 533,
		CB_GETDROPPEDSTATE = 343,
		WVR_VALIDRECTS = 1024
	}

	public enum ComboNotificationCodes
	{
		CBN_DROPDOWN = 7,
		CBN_CLOSEUP
	}

	public enum MouseKeyState
	{
		MK_LBUTTON = 1,
		MK_RBUTTON = 2,
		MK_SHIFT = 4,
		MK_CONTROL = 8,
		MK_MBUTTON = 0x10,
		MK_XBUTTON1 = 0x20,
		MK_XBUTTON2 = 0x40
	}

	public enum WindowHitTestRegions
	{
		Error = -2,
		TransparentOrCovered = -1,
		NoWhere = 0,
		ClientArea = 1,
		TitleBar = 2,
		SystemMenu = 3,
		GrowBox = 4,
		Menu = 5,
		HorizontalScrollBar = 6,
		VerticalScrollBar = 7,
		MinimizeButton = 8,
		MaximizeButton = 9,
		LeftSizeableBorder = 10,
		RightSizeableBorder = 11,
		TopSizeableBorder = 12,
		TopLeftSizeableCorner = 13,
		TopRightSizeableCorner = 14,
		BottomSizeableBorder = 15,
		BottomLeftSizeableCorner = 16,
		BottomRightSizeableCorner = 17,
		NonSizableBorder = 18,
		Object = 19,
		CloseButton = 20,
		HelpButton = 21,
		SizeBox = 4,
		ReduceButton = 9,
		ZoomButton = 9
	}

	public enum GWL
	{
		GWL_WNDPROC = -4,
		GWL_HINSTANCE = -6,
		GWL_HWNDPARENT = -8,
		GWL_STYLE = -16,
		GWL_EXSTYLE = -20,
		GWL_USERDATA = -21,
		GWL_ID = -12
	}

	public enum RedrawWindowFlags : uint
	{
		RDW_INVALIDATE = 1u,
		RDW_INTERNALPAINT = 2u,
		RDW_ERASE = 4u,
		RDW_VALIDATE = 8u,
		RDW_NOINTERNALPAINT = 0x10u,
		RDW_NOERASE = 0x20u,
		RDW_NOCHILDREN = 0x40u,
		RDW_ALLCHILDREN = 0x80u,
		RDW_UPDATENOW = 0x100u,
		RDW_ERASENOW = 0x200u,
		RDW_FRAME = 0x400u,
		RDW_NOFRAME = 0x800u
	}

	[Flags]
	public enum WindowStyles
	{
		WS_VISIBLE = 0x10000000,
		WS_CAPTION = 0xC00000
	}

	public const int WM_LBUTTONDOWN = 513;

	public const int WM_LBUTTONUP = 514;

	public const int WM_RBUTTONDOWN = 516;

	public const int WM_RBUTTONUP = 517;

	public const int WM_MOUSEMOVE = 512;

	public const int WM_MOUSELEAVE = 675;

	public const int WM_HSCROLL = 276;

	public const int WM_VSCROLL = 277;

	public const int WM_LBUTTONDBLCLK = 515;

	public const int HWND_TOP = 0;

	public const int HWND_BOTTOM = 1;

	public const int HWND_TOPMOST = -1;

	public const int HWND_NOTOPMOST = -2;

	public const int SWP_NOMOVE = 2;

	public const int SWP_NOSIZE = 1;

	public const int SWP_NOACTIVATE = 16;

	public const int SWP_NOZORDER = 4;

	public const int SWP_SHOWWINDOW = 64;

	public const int SWP_HIDEWINDOW = 128;

	public const int SWP_FRAMECHANGED = 32;

	public const int SWP_NOOWNERZORDER = 512;

	public const int SWP_NOSENDCHANGING = 1024;

	public const int WM_USER = 1024;

	[DllImport("user32.dll")]
	public static extern IntPtr SetFocus(IntPtr hWnd);

	[DllImport("user32.dll")]
	public static extern IntPtr GetFocus();

	[DllImport("user32")]
	public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

	[DllImport("gdi32")]
	public static extern bool DeleteObject(IntPtr hObject);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern bool GetTextMetrics(HandleRef hdc, TEXTMETRIC tm);

	[DllImport("user32")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	public static int MAKELPARAM(int low, int high)
	{
		return (high << 16) | (low & 0xFFFF);
	}

	[DllImport("user32")]
	public static extern bool GetWindowRect(IntPtr hWnd, ref RECT r);

	[DllImport("user32")]
	public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

	public static int LOWORD(int n)
	{
		return (short)(n & 0xFFFF);
	}

	public static int HIWORD(int n)
	{
		return (short)((n >> 16) & 0xFFFF);
	}

	public static int LOWORD(IntPtr n)
	{
		return LOWORD((int)(long)n);
	}

	public static int HIWORD(IntPtr n)
	{
		return HIWORD((int)(long)n);
	}

	public static IntPtr CreateLParam(int loWord, int hiWord)
	{
		byte[] bytes = BitConverter.GetBytes(loWord);
		byte[] bytes2 = BitConverter.GetBytes(hiWord);
		byte[] value = new byte[4]
		{
			bytes[0],
			bytes[1],
			bytes2[0],
			bytes2[1]
		};
		return new IntPtr(BitConverter.ToInt32(value, 0));
	}
}
