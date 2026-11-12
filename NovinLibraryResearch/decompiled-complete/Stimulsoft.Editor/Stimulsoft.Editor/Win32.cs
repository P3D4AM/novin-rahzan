using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Stimulsoft.Editor;

internal class Win32
{
	public struct ABC
	{
		private int abcA;

		private uint abcB;

		private int abcC;

		public ABC(int a, uint b, int c)
		{
			abcA = a;
			abcB = b;
			abcC = c;
		}
	}

	public struct GdiRect
	{
		public int left;

		public int top;

		public int right;

		public int bottom;

		public GdiRect(int l, int t, int r, int b)
		{
			left = l;
			top = t;
			right = r;
			bottom = b;
		}

		public GdiRect(Rectangle rect)
		{
			left = rect.Left;
			top = rect.Top;
			right = rect.Right;
			bottom = rect.Bottom;
		}

		public Rectangle ToRectangle()
		{
			return new Rectangle(left, top, right - left, bottom - top);
		}
	}

	public struct LogBrush
	{
		public int style;

		public int color;

		public IntPtr hatch;

		public LogBrush(int style, int color, IntPtr hatch)
		{
			this.style = style;
			this.color = color;
			this.hatch = hatch;
		}
	}

	public struct GdiSize
	{
		public int cx;

		public int cy;

		public GdiSize(int cx, int cy)
		{
			this.cx = cx;
			this.cy = cy;
		}

		public GdiSize(Size size)
		{
			cx = size.Width;
			cy = size.Height;
		}

		public Size ToSize()
		{
			return new Size(cx, cy);
		}
	}

	public struct GdiPoint
	{
		public int X;

		public int Y;

		public GdiPoint(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	public struct ColorRef
	{
		public byte rgbRed;

		public byte rgbGreen;

		public byte rgbBlue;

		public byte rgbReserved;

		public Color Color => Color.FromArgb(rgbRed, rgbGreen, rgbBlue);
	}

	public struct MOUSEHOOKSTRUCT
	{
		public GdiPoint Pt;

		public IntPtr hwnd;

		public uint wHitTestCode;

		public IntPtr dwExtraInfo;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct TextMetrics
	{
		[FieldOffset(0)]
		public int Height;

		[FieldOffset(4)]
		public int Accent;

		[FieldOffset(8)]
		public int Descent;

		[FieldOffset(12)]
		public int publicLeading;

		[FieldOffset(16)]
		public int ExternalLeading;

		[FieldOffset(20)]
		public int AveCharWidth;

		[FieldOffset(24)]
		public int MaxCharWidth;

		[FieldOffset(28)]
		public int Weight;

		[FieldOffset(32)]
		public int Overhang;

		[FieldOffset(36)]
		public int DigitizedAspectX;

		[FieldOffset(40)]
		public int DigitizedAspectY;

		[FieldOffset(44)]
		public byte FirstChar;

		[FieldOffset(45)]
		public byte LastChar;

		[FieldOffset(46)]
		public byte DefaultChar;

		[FieldOffset(57)]
		public byte BreakChar;

		[FieldOffset(58)]
		public byte Italic;

		[FieldOffset(49)]
		public byte Underlined;

		[FieldOffset(50)]
		public byte StruckOut;

		[FieldOffset(51)]
		public byte PitchAndFamily;

		[FieldOffset(52)]
		public byte CharSet;

		[FieldOffset(52)]
		private int Reserved;
	}

	public struct MSG
	{
		public IntPtr hwnd;

		public int message;

		public IntPtr wparam;

		public IntPtr lparam;

		public uint time;

		public GdiPoint Pt;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct ScrollInfo
	{
		[FieldOffset(0)]
		public uint cbSize;

		[FieldOffset(4)]
		public uint fMask;

		[FieldOffset(8)]
		public int nMin;

		[FieldOffset(12)]
		public int nMax;

		[FieldOffset(16)]
		public uint nPage;

		[FieldOffset(20)]
		public int nPos;

		[FieldOffset(24)]
		public int nTrackPos;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct XFORM
	{
		[FieldOffset(0)]
		public float eM11;

		[FieldOffset(4)]
		public float eM12;

		[FieldOffset(8)]
		public float eM21;

		[FieldOffset(12)]
		public float eM22;

		[FieldOffset(16)]
		public float eDx;

		[FieldOffset(20)]
		public float eDy;

		public XFORM(float m11, float m12, float m21, float m22, float dx, float dy)
		{
			eM11 = m11;
			eM12 = m12;
			eM21 = m21;
			eM22 = m22;
			eDx = dx;
			eDy = dy;
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct INITCOMMONCONTROLSEX
	{
		[FieldOffset(0)]
		public int dwSize;

		[FieldOffset(8)]
		public int dwICC;
	}

	public const int TMPF_FIXED_PITCH = 1;

	public const int TMPF_VECTOR = 2;

	public const int TMPF_DEVICE = 8;

	public const int TMPF_TRUETYPE = 4;

	public const int OPAQUE = 2;

	public const int TRANSPARENT = 1;

	public const int GM_COMPATIBLE = 1;

	public const int GM_ADVANCED = 2;

	public const int DT_TOP = 0;

	public const int DT_LEFT = 0;

	public const int DT_CENTER = 1;

	public const int DT_RIGHT = 2;

	public const int DT_VCENTER = 4;

	public const int DT_BOTTOM = 8;

	public const int DT_SINGLELINE = 32;

	public const int DT_NOCLIP = 256;

	public const int DT_CALCRECT = 1024;

	public const int DT_NOPREFIX = 2048;

	public const int DT_HIDEPREFIX = 1048576;

	public const int DT_PATH_ELLIPSIS = 16384;

	public const int DT_END_ELLIPSIS = 32768;

	public const int DT_RTLREADING = 131072;

	public const int DT_WORD_ELLIPSIS = 262144;

	public const int ETO_OPAQUE = 2;

	public const int ETO_CLIPPED = 4;

	public const int PS_SOLID = 0;

	public const int PS_DASH = 1;

	public const int PS_DOT = 2;

	public const int PS_DASHDOT = 3;

	public const int PS_DASHDOTDOT = 4;

	public const int BS_SOLID = 0;

	public const int BS_PATTERN = 3;

	public const int BDR_RAISEDOUTER = 1;

	public const int BDR_SUNKENOUTER = 2;

	public const int BDR_RAISEDINNER = 4;

	public const int BDR_SUNKENINNER = 8;

	public const int BDR_OUTER = 3;

	public const int BDR_INNER = 12;

	public const int BDR_RAISED = 5;

	public const int BDR_SUNKEN = 10;

	public const int EDGE_RAISED = 5;

	public const int EDGE_SUNKEN = 10;

	public const int EDGE_ETCHED = 6;

	public const int EDGE_BUMP = 9;

	public const int BF_LEFT = 1;

	public const int BF_TOP = 2;

	public const int BF_RIGHT = 4;

	public const int BF_BOTTOM = 8;

	public const int BF_MIDDLE = 2048;

	public const int BF_SOFT = 4096;

	public const int BF_ADJUST = 8192;

	public const int BF_FLAT = 16384;

	public const int BF_MONO = 32768;

	public const int RGN_AND = 1;

	public const int RGN_OR = 2;

	public const int RGN_XOR = 3;

	public const int RGN_DIFF = 4;

	public const int RGN_COPY = 5;

	public const int RGN_MIN = 1;

	public const int RGN_MAX = 5;

	public const int SB_HORZ = 0;

	public const int SB_VERT = 1;

	public const int SIF_RANGE = 1;

	public const int SIF_PAGE = 2;

	public const int SIF_POS = 4;

	public const int SIF_DISABLENOSCROLL = 8;

	public const int SIF_TRACKPOS = 16;

	public const int SB_LINEUP = 0;

	public const int SB_LINELEFT = 0;

	public const int SB_LINEDOWN = 1;

	public const int SB_LINERIGHT = 1;

	public const int SB_PAGEUP = 2;

	public const int SB_PAGELEFT = 2;

	public const int SB_PAGEDOWN = 3;

	public const int SB_PAGERIGHT = 3;

	public const int SB_THUMBPOSITION = 4;

	public const int SB_THUMBTRACK = 5;

	public const int SB_TOP = 6;

	public const int SB_LEFT = 6;

	public const int SB_BOTTOM = 7;

	public const int SB_RIGHT = 7;

	public const int SB_ENDSCROLL = 8;

	public const int Scroll_Size = 28;

	public const int SM_CYVSCROLL = 20;

	public const int SM_CXHSCROLL = 21;

	public const int WSB_PROP_HSTYLE = 512;

	public const int WSB_PROP_VSTYLE = 256;

	public const int FSB_FLAT_MODE = 2;

	public const int WS_VSCROLL = 2097152;

	public const int WS_HSCROLL = 1048576;

	public const int WM_CLOSEDROPPED = 1025;

	public const int WM_MOUSEMOVE = 512;

	public const int WM_MINCLICK = 513;

	public const int WM_MAXCLICK = 520;

	public const int WM_MINNCCLICK = 161;

	public const int WM_MAXNCCLICK = 169;

	public const int WM_LBUTTONDOWN = 513;

	public const int WM_NCLBUTTONDOWN = 161;

	public const int WM_NCPAINT = 133;

	public const int WM_HSCROLL = 276;

	public const int WM_VSCROLL = 277;

	public const int WM_KEYDOWN = 256;

	public const int WM_CHAR = 258;

	public const int WM_SYSKEYDOWN = 260;

	public const int WM_SETCURSOR = 32;

	public const int WM_LBUTTONUP = 514;

	public const int WM_NCACTIVATE = 134;

	public const int WM_GETTEXT = 13;

	public const int WM_SETTEXT = 12;

	public const int WM_INITDIALOG = 272;

	public const int WM_SIZE = 5;

	public const int WM_NCDESTROY = 130;

	public const int WM_DESTROY = 2;

	public const int WM_CLOSE = 16;

	public const int WM_CUT = 768;

	public const int WM_COPY = 769;

	public const int WM_PASTE = 770;

	public const int WM_CLEAR = 771;

	public const int WM_CANUNDO = 198;

	public const int WM_UNDO = 772;

	public const int LOGPIXELSX = 88;

	public const int LOGPIXELSY = 90;

	public const int SM_DBCSENABLED = 42;

	public const int SW_SHOW = 5;

	public const int SW_HIDE = 0;

	public const int SWP_NOSIZE = 1;

	public const int SWP_NOMOVE = 2;

	public const int SWP_NOZORDER = 4;

	public const int SWP_NOACTIVATE = 16;

	public const int SWP_SHOWWINDOW = 64;

	public const int HWND_TOPMOST = -1;

	public const int GWL_WNDPROC = -4;

	public const int WS_EX_CLIENTEDGE = 512;

	public const int WS_BORDER = 8388608;

	public const int WS_EX_DLGMODALFRAME = 1;

	public const int WS_EX_WINDOWEDGE = 256;

	public const int WH_MOUSE = 7;

	public const int WH_CALLWNDPROC = 4;

	public const int WH_GETMESSAGE = 3;

	public const int MB_ICONHAND = 16;

	private Win32()
	{
	}

	[DllImport("comctl32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool InitCommonControlsEx(ref INITCOMMONCONTROLSEX iic);

	public static bool InitCommonControls(int icc)
	{
		INITCOMMONCONTROLSEX iic = default(INITCOMMONCONTROLSEX);
		iic.dwSize = 16;
		iic.dwICC = icc;
		return InitCommonControlsEx(ref iic);
	}

	[DllImport("user32.dll")]
	public static extern IntPtr GetDC(IntPtr hwnd);

	[DllImport("user32.dll")]
	public static extern IntPtr GetWindowDC(IntPtr hwnd);

	[DllImport("user32.dll")]
	public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

	[DllImport("gdi32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool DeleteDC(IntPtr hdc);

	[DllImport("gdi32.dll")]
	public static extern int GetDeviceCaps(IntPtr hdc, int index);

	[DllImport("gdi32.dll")]
	public static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

	[DllImport("gdi32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool DeleteObject(IntPtr p1);

	[DllImport("gdi32.dll")]
	public static extern IntPtr CreateSolidBrush(int color);

	[DllImport("gdi32.dll")]
	public static extern IntPtr CreatePen(int style, int width, int color);

	[DllImport("gdi32.dll")]
	public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

	[DllImport("gdi32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool SetWorldTransform(IntPtr hdc, ref XFORM p2);

	[DllImport("gdi32.dll")]
	public static extern int SetTextColor(IntPtr hdc, int color);

	[DllImport("gdi32.dll")]
	public static extern int SetBkColor(IntPtr hdc, int color);

	[DllImport("gdi32.dll")]
	public static extern int SetBkMode(IntPtr hdc, int bkMode);

	[DllImport("gdi32.dll")]
	public static extern int GetGraphicsMode(IntPtr hdc);

	[DllImport("gdi32.dll")]
	public static extern int SetGraphicsMode(IntPtr hdc, int mode);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
	[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool GetTextExtentPoint32(IntPtr hdc, string s, int count, ref GdiSize size);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
	[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetCharWidth32(IntPtr hdc, int iFirstChar, int iLastChar, IntPtr buffer);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
	[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetCharABCWidths(IntPtr hdc, int iFirstChar, int iLastChar, IntPtr buffer);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
	[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetTextExtentExPoint(IntPtr hdc, string s, int len, int maxExtent, IntPtr lpnFit, IntPtr alpDx, ref GdiSize size);

	[DllImport("gdi32.dll")]
	public static extern int GetTextMetrics(IntPtr hdc, ref TextMetrics metrics);

	[DllImport("user32.dll")]
	public static extern int GetSystemMetrics(int nIndex);

	[DllImport("user32.dll")]
	public static extern int GetDoubleClickTime();

	[DllImport("user32.dll")]
	public static extern int FillRect(IntPtr hdc, ref GdiRect rect, IntPtr brush);

	[DllImport("user32.dll")]
	public static extern int FrameRect(IntPtr hdc, ref GdiRect rect, IntPtr brush);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool DrawFocusRect(IntPtr hdc, ref GdiRect rect);

	[DllImport("gdi32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool RoundRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidth, int nHeight);

	[DllImport("comctl32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool ImageList_DrawEx(IntPtr handle, int index, IntPtr hdc, int x, int y, int dx, int dy, int bk, int fr, int style);

	[DllImport("gdi32.dll")]
	[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool PolyBezier(IntPtr hdc, IntPtr points, int count);

	[DllImport("gdi32.dll")]
	[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool Polygon(IntPtr hdc, IntPtr points, int count);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool DrawEdge(IntPtr hdc, ref GdiRect rect, int edge, int flags);

	[DllImport("gdi32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool MoveToEx(IntPtr hdc, int x, int y, IntPtr p);

	[DllImport("gdi32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool LineTo(IntPtr hdc, int x, int y);

	[DllImport("gdi32.dll")]
	public static extern int SetPixel(IntPtr hdc, int x, int y, int color);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
	[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	private static extern int ExtTextOut(IntPtr hdc, int x, int y, int options, ref GdiRect rect, string s, int len, IntPtr buffer);

	[DllImport("user32.dll", CharSet = CharSet.Unicode)]
	private static extern int DrawText(IntPtr hdc, string s, int len, ref GdiRect rect, int format);

	[DllImport("gdi32.dll")]
	public static extern int IntersectClipRect(IntPtr hdc, int l, int t, int r, int b);

	[DllImport("gdi32.dll")]
	public static extern int ExcludeClipRect(IntPtr hdc, int l, int t, int r, int b);

	[DllImport("gdi32.dll")]
	public static extern IntPtr CreateRectRgnIndirect(ref GdiRect rect);

	[DllImport("gdi32.dll")]
	public static extern int GetClipRgn(IntPtr handle, IntPtr rgn);

	[DllImport("gdi32.dll")]
	public static extern int SelectClipRgn(IntPtr handle, IntPtr rgn);

	[DllImport("user32.dll")]
	public static extern IntPtr SendMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

	[DllImport("user32.dll")]
	public static extern IntPtr SetWindowsHookEx(int idHook, HookHandler lpfn, IntPtr hMod, int dwThreadId);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool UnhookWindowsHookEx(IntPtr hhk);

	[DllImport("user32.dll")]
	public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wparam, IntPtr lparam);

	[DllImport("user32.dll")]
	private static extern int SetScrollInfo(IntPtr hwnd, int barFlag, ref ScrollInfo scrollInfo, [MarshalAs(UnmanagedType.Bool)] bool redraw);

	[DllImport("user32.dll")]
	private static extern int GetScrollInfo(IntPtr hwnd, int barFlag, ref ScrollInfo scrollInfo);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool ScrollWindow(IntPtr handle, int x, int y, ref GdiRect r, IntPtr clipR);

	[DllImport("comctl32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool FlatSB_GetScrollInfo(IntPtr hwnd, int barFlag, ref ScrollInfo scrollInfo);

	[DllImport("comctl32.dll")]
	private static extern int FlatSB_SetScrollInfo(IntPtr hwnd, int barFlag, ref ScrollInfo scrollInfo, [MarshalAs(UnmanagedType.Bool)] bool redraw);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool CreateCaret(IntPtr handle, IntPtr hBitmap, int nWidth, int nHeight);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool DestroyCaret();

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool SetCaretPos(int x, int y);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool ShowCaret(IntPtr handle);

	[DllImport("user32.dll")]
	public static extern IntPtr SetCursor(IntPtr handle);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool EnumChildWindows(IntPtr hwndParent, EnumChildProc lpEnumFunc, IntPtr lparam);

	[DllImport("user32.dll")]
	[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public static extern int GetClassName(IntPtr hwnd, IntPtr lpClassName, int nMaxCount);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool MessageBeep(int type);

	[DllImport("kernel32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool Beep(int freq, int duration);

	[DllImport("uxtheme.dll")]
	public static extern int CloseThemeData(IntPtr htheme);

	[DllImport("uxtheme.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool DrawThemeBackground(IntPtr theme, IntPtr hdc, int partID, int stateID, ref GdiRect rect, IntPtr clipRect);

	[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
	public static extern int GetCurrentThemeName(IntPtr themeFileName, int maxNameChars, IntPtr colorName, int maxColorName, IntPtr dummy1, IntPtr dummy2);

	[DllImport("uxtheme.dll")]
	public static extern int GetThemeColor(IntPtr htheme, int partID, int stateID, int propID, out ColorRef colorRef);

	[DllImport("uxtheme.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool IsAppThemed();

	[DllImport("uxtheme.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool IsThemeActive();

	[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
	public static extern IntPtr OpenThemeData(IntPtr hwnd, string classList);

	[DllImport("uxtheme.dll")]
	public static extern int GetThemeAppProperties();

	public static int ColorToGdiColor(Color color)
	{
		int num = color.ToArgb() & 0xFFFFFF;
		return (num >> 16) + (num & 0xFF00) + ((num & 0xFF) << 16);
	}

	public static int FillRect(IntPtr hdc, int x, int y, int w, int h, IntPtr brush)
	{
		GdiRect rect = new GdiRect(x, y, x + w, y + h);
		return FillRect(hdc, ref rect, brush);
	}

	public static int FrameRect(IntPtr hdc, int x, int y, int w, int h, IntPtr brush)
	{
		GdiRect rect = new GdiRect(x, y, x + w, y + h);
		return FrameRect(hdc, ref rect, brush);
	}

	public static bool DrawFocusRect(IntPtr hdc, int x, int y, int w, int h)
	{
		GdiRect rect = new GdiRect(x, y, x + w, y + h);
		return DrawFocusRect(hdc, ref rect);
	}

	public static bool PolyBezier(IntPtr hdc, Point[] points, int count)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(count * 8);
		try
		{
			for (int i = 0; i < count; i++)
			{
				Marshal.WriteInt32(intPtr, i * 8, points[i].X);
				Marshal.WriteInt32(intPtr, i * 8 + 4, points[i].Y);
			}
			return PolyBezier(hdc, intPtr, count);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public static bool Polygon(IntPtr hdc, Point[] points, int count)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(count * 8);
		try
		{
			for (int i = 0; i < count; i++)
			{
				Marshal.WriteInt32(intPtr, i * 8, points[i].X);
				Marshal.WriteInt32(intPtr, i * 8 + 4, points[i].Y);
			}
			return Polygon(hdc, intPtr, count);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public static int SetPixel(IntPtr hdc, int x, int y, Color color)
	{
		return SetPixel(hdc, x, y, ColorToGdiColor(color));
	}

	public static int ExtTextOut(IntPtr hdc, Rectangle rect, int x, int y, int options, string s, int len, int[] spacings)
	{
		GdiRect rect2 = new GdiRect(rect);
		if (len < 0)
		{
			len = s.Length;
		}
		IntPtr intPtr = ((spacings != null) ? Marshal.AllocHGlobal(len * 4) : IntPtr.Zero);
		try
		{
			if (intPtr != IntPtr.Zero)
			{
				int num = spacings.Length;
				for (int i = 0; i < len; i++)
				{
					Marshal.WriteInt32(intPtr, i * 4, (i < num) ? spacings[i] : 0);
				}
			}
			return ExtTextOut(hdc, x, y, options, ref rect2, s, len, intPtr);
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	public static bool DrawEdge(IntPtr hdc, ref Rectangle rect, int edge, int flags)
	{
		GdiRect rect2 = new GdiRect(rect);
		bool result = DrawEdge(hdc, ref rect2, edge, flags);
		rect = new Rectangle(rect2.left, rect2.top, rect2.right - rect2.left, rect2.bottom - rect2.top);
		return result;
	}

	public static int DrawText(IntPtr hdc, string s, int len, ref Rectangle rect, int format)
	{
		GdiRect rect2 = new GdiRect(rect);
		int result = DrawText(hdc, s, len, ref rect2, format);
		rect = rect2.ToRectangle();
		return result;
	}

	public static bool GetTextExtentPoint32(IntPtr hdc, string s, int count, ref Size size)
	{
		GdiSize size2 = new GdiSize(0, 0);
		bool textExtentPoint = GetTextExtentPoint32(hdc, s, count, ref size2);
		size.Width = size2.cx;
		size.Height = size2.cy;
		return textExtentPoint;
	}

	public static bool GetCharWidth32(IntPtr hdc, int iFirstChar, int iLastChar, ref int[] buffer)
	{
		bool flag = true;
		IntPtr intPtr = Marshal.AllocHGlobal(buffer.Length * 4);
		try
		{
			flag = GetCharWidth32(hdc, iFirstChar, iLastChar, intPtr);
			if (flag)
			{
				for (int i = 0; i < buffer.Length; i++)
				{
					buffer[i] = Marshal.ReadInt32(intPtr, i * 4);
				}
			}
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
		return flag;
	}

	public static bool GetCharABCWidths(IntPtr hdc, int iFirstChar, int iLastChar, ref int[] buffer)
	{
		bool flag = true;
		IntPtr intPtr = Marshal.AllocHGlobal(buffer.Length * 12);
		try
		{
			flag = GetCharABCWidths(hdc, iFirstChar, iLastChar, intPtr);
			if (flag)
			{
				for (int i = 0; i < buffer.Length; i++)
				{
					buffer[i] = Marshal.ReadInt32(intPtr, i * 12) + Marshal.ReadInt32(intPtr, i * 12 + 4) + Marshal.ReadInt32(intPtr, i * 12 + 8);
				}
			}
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
		return flag;
	}

	public static bool GetTextExtentExPoint(IntPtr hdc, string s, int len, ref int[] buffer, ref Size size)
	{
		bool flag = true;
		IntPtr intPtr = Marshal.AllocHGlobal(buffer.Length * 4);
		try
		{
			GdiSize size2 = default(GdiSize);
			flag = GetTextExtentExPoint(hdc, s, len, 0, IntPtr.Zero, intPtr, ref size2);
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = Marshal.ReadInt32(intPtr, i * 4);
			}
			size = size2.ToSize();
			return flag;
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public static string GetText(IntPtr hwnd)
	{
		int num = 255;
		IntPtr intPtr = Marshal.AllocHGlobal(num + 1);
		try
		{
			int num2 = (int)SendMessage(hwnd, 13, (IntPtr)num, intPtr);
			return (num2 > 0) ? Marshal.PtrToStringAnsi(intPtr, num2) : string.Empty;
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public static void SetText(IntPtr hwnd, string text)
	{
		SendMessage(hwnd, 12, IntPtr.Zero, Marshal.StringToHGlobalAnsi(text));
	}

	public static string GetClassName(IntPtr hwnd)
	{
		int num = 255;
		IntPtr intPtr = Marshal.AllocHGlobal(num + 1);
		try
		{
			int className = GetClassName(hwnd, intPtr, num);
			return (className > 0) ? Marshal.PtrToStringAnsi(intPtr, className) : string.Empty;
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public static IntPtr CreateRectRgnIndirect(Rectangle rect)
	{
		GdiRect rect2 = new GdiRect(rect.Left, rect.Top, rect.Right, rect.Bottom);
		return CreateRectRgnIndirect(ref rect2);
	}

	public static void ScrollWindow(IntPtr handle, int x, int y, Rectangle rect)
	{
		GdiRect r = new GdiRect(rect);
		ScrollWindow(handle, x, y, ref r, IntPtr.Zero);
	}

	public static void SetScrollSize(IntPtr handle, int code, int minPos, int maxPos, int pageSize)
	{
		ScrollInfo scrollInfo = default(ScrollInfo);
		scrollInfo.cbSize = 28u;
		scrollInfo.fMask = 3u;
		scrollInfo.nMin = minPos;
		scrollInfo.nMax = maxPos;
		scrollInfo.nPage = (uint)pageSize;
		scrollInfo.nPos = 0;
		scrollInfo.nTrackPos = 0;
		SetScrollInfo(handle, code, ref scrollInfo, redraw: true);
	}

	public static void SetScrollPos(IntPtr handle, int code, int pos)
	{
		ScrollInfo scrollInfo = default(ScrollInfo);
		scrollInfo.cbSize = 28u;
		scrollInfo.fMask = 4u;
		scrollInfo.nMin = 0;
		scrollInfo.nMax = 0;
		scrollInfo.nPage = 0u;
		scrollInfo.nPos = pos;
		scrollInfo.nTrackPos = 0;
		SetScrollInfo(handle, code, ref scrollInfo, redraw: true);
	}

	public static int GetScrollPos(IntPtr handle, int code)
	{
		ScrollInfo scrollInfo = default(ScrollInfo);
		scrollInfo.cbSize = 28u;
		scrollInfo.fMask = 16u;
		scrollInfo.nMin = 0;
		scrollInfo.nMax = 0;
		scrollInfo.nPage = 0u;
		scrollInfo.nPos = 0;
		scrollInfo.nTrackPos = 0;
		GetScrollInfo(handle, code, ref scrollInfo);
		return scrollInfo.nTrackPos;
	}

	public static int GetScrollSize(IntPtr handle, int code)
	{
		ScrollInfo scrollInfo = default(ScrollInfo);
		scrollInfo.cbSize = 28u;
		scrollInfo.fMask = 3u;
		scrollInfo.nMin = 0;
		scrollInfo.nMax = 0;
		scrollInfo.nPage = 0u;
		scrollInfo.nPos = 0;
		scrollInfo.nTrackPos = 0;
		GetScrollInfo(handle, code, ref scrollInfo);
		if (scrollInfo.nPage <= scrollInfo.nMax)
		{
			return (int)scrollInfo.nPage;
		}
		return -1;
	}

	public static void SetScrollBarInfo(IntPtr handle, int size, int pageSize, int code)
	{
		if (size < pageSize)
		{
			size = pageSize;
		}
		SetScrollSize(handle, code, 0, size, pageSize);
	}

	public static void FlatSB_SetScrollSize(IntPtr handle, int code, int minPos, int maxPos, int pageSize)
	{
		ScrollInfo scrollInfo = default(ScrollInfo);
		scrollInfo.cbSize = 28u;
		scrollInfo.fMask = 3u;
		scrollInfo.nMin = minPos;
		scrollInfo.nMax = maxPos;
		scrollInfo.nPage = (uint)pageSize;
		scrollInfo.nPos = 0;
		scrollInfo.nTrackPos = 0;
		FlatSB_SetScrollInfo(handle, code, ref scrollInfo, redraw: true);
	}

	public static void FlatSB_SetScrollPos(IntPtr handle, int code, int pos)
	{
		ScrollInfo scrollInfo = default(ScrollInfo);
		scrollInfo.cbSize = 28u;
		scrollInfo.fMask = 4u;
		scrollInfo.nMin = 0;
		scrollInfo.nMax = 0;
		scrollInfo.nPage = 0u;
		scrollInfo.nPos = pos;
		scrollInfo.nTrackPos = 0;
		FlatSB_SetScrollInfo(handle, code, ref scrollInfo, redraw: true);
	}

	public static int FlatSB_GetScrollPos(IntPtr handle, int code)
	{
		ScrollInfo scrollInfo = default(ScrollInfo);
		scrollInfo.cbSize = 28u;
		scrollInfo.fMask = 16u;
		scrollInfo.nMin = 0;
		scrollInfo.nMax = 0;
		scrollInfo.nPage = 0u;
		scrollInfo.nPos = 0;
		scrollInfo.nTrackPos = 0;
		FlatSB_GetScrollInfo(handle, code, ref scrollInfo);
		return scrollInfo.nTrackPos;
	}

	public static int FlatSB_GetScrollSize(IntPtr handle, int code)
	{
		ScrollInfo scrollInfo = default(ScrollInfo);
		scrollInfo.cbSize = 28u;
		scrollInfo.fMask = 3u;
		scrollInfo.nMin = 0;
		scrollInfo.nMax = 0;
		scrollInfo.nPage = 0u;
		scrollInfo.nPos = 0;
		scrollInfo.nTrackPos = 0;
		FlatSB_GetScrollInfo(handle, code, ref scrollInfo);
		if (scrollInfo.nPage <= scrollInfo.nMax)
		{
			return (int)scrollInfo.nPage;
		}
		return -1;
	}

	public static void FlatSB_SetScrollBarInfo(IntPtr handle, int size, int pageSize, int code)
	{
		if (size < pageSize)
		{
			size = pageSize;
		}
		FlatSB_SetScrollSize(handle, code, 0, size, pageSize);
	}

	[DllImport("comctl32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool InitializeFlatSB(IntPtr hwnd);

	[DllImport("comctl32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool UninitializeFlatSB(IntPtr hwnd);

	[DllImport("comctl32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool FlatSB_SetScrollProp(IntPtr hwnd, int index, IntPtr newValue, [MarshalAs(UnmanagedType.Bool)] bool setValue);
}
