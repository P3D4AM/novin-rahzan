using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class WinApi
{
	public enum WindowsMessages
	{
		WM_DESTROY = 2,
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
		WM_STYLECHANGING = 124,
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
		WVR_VALIDRECTS = 1024,
		WM_GETMINMAXINFO = 36,
		WM_APPCOMMAND = 793
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
		WS_CAPTION = 0xC00000,
		WS_BORDER = 0x800000,
		WS_DLGFRAME = 0x400000,
		WS_THICKFRAME = 0x40000,
		WS_HSCROLL = 0x100000,
		WS_VSCROLL = 0x200000
	}

	public delegate uint WindowProcDelegate(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam);

	public enum SBOrientation
	{
		SB_HORZ,
		SB_VERT,
		SB_CTL,
		SB_BOTH
	}

	[Flags]
	internal enum ScrollInfoMask
	{
		SIF_RANGE = 1,
		SIF_PAGE = 2,
		SIF_POS = 4,
		SIF_DISABLENOSCROLL = 8,
		SIF_TRACKPOS = 0x10,
		SIF_ALL = 0x17
	}

	public enum ScrollBarCommands
	{
		SB_LINEUP = 0,
		SB_LINELEFT = 0,
		SB_LINEDOWN = 1,
		SB_LINERIGHT = 1,
		SB_PAGEUP = 2,
		SB_PAGELEFT = 2,
		SB_PAGEDOWN = 3,
		SB_PAGERIGHT = 3,
		SB_THUMBPOSITION = 4,
		SB_THUMBTRACK = 5,
		SB_TOP = 6,
		SB_LEFT = 6,
		SB_BOTTOM = 7,
		SB_RIGHT = 7,
		SB_ENDSCROLL = 8
	}

	public delegate void TimerDelegate(IntPtr hWnd, IntPtr uMsg, IntPtr idEvent, int dwTime);

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class STYLESTRUCT
	{
		public uint styleOld;

		public uint styleNew;
	}

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

	public struct SCROLLBARINFO
	{
		public int cbSize;

		public RECT rcScrollBar;

		public int dxyLineButton;

		public int xyThumbTop;

		public int xyThumbBottom;

		public int reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public int[] rgstate;
	}

	public enum eScrollBarDirection
	{
		SB_HORZ,
		SB_VERT
	}

	public enum eScrollInfoMask
	{
		SIF_RANGE = 1,
		SIF_PAGE = 2,
		SIF_POS = 4,
		SIF_DISABLENOSCROLL = 8,
		SIF_TRACKPOS = 16,
		SIF_ALL = 23
	}

	public struct SCROLLINFO
	{
		public int cbSize;

		public ScrollInfoMask fMask;

		public int nMin;

		public int nMax;

		public int nPage;

		public int nPos;

		public int nTrackPos;

		public override string ToString()
		{
			return $"nMin={nMin}, nMax={nMax}, nPage={nPage}, nPos={nPos}, nTrackPos={nTrackPos}";
		}
	}

	public struct POINT
	{
		public int x;

		public int y;

		public POINT(Point p)
		{
			x = p.X;
			y = p.Y;
		}

		public POINT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return $"{x},{y}";
		}
	}

	public enum ShowWindowCommands
	{
		Hide = 0,
		Normal = 1,
		ShowMinimized = 2,
		Maximize = 3,
		ShowMaximized = 3,
		ShowNoActivate = 4,
		Show = 5,
		Minimize = 6,
		ShowMinNoActive = 7,
		ShowNA = 8,
		Restore = 9,
		ShowDefault = 10,
		ForceMinimize = 11
	}

	public struct WINDOWPLACEMENT
	{
		public int length;

		public int flags;

		public int showCmd;

		public POINT ptMinPosition;

		public POINT ptMaxPosition;

		public RECT rcNormalPosition;
	}

	public struct NCCALCSIZE_PARAMS
	{
		public RECT rgrc0;

		public RECT rgrc1;

		public RECT rgrc2;

		public IntPtr lppos;
	}

	public struct WINDOWPOS
	{
		public IntPtr hwnd;

		public IntPtr hwndInsertAfter;

		public int x;

		public int y;

		public int cx;

		public int cy;

		public int flags;

		public override string ToString()
		{
			return $"x={x}, y={y}, right={cx}, bottom={cy}";
		}
	}

	[Serializable]
	public struct RECT
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;

		public int Height => Bottom - Top;

		public int Width => Right - Left;

		public Size Size => new Size(Width, Height);

		public Point Location => new Point(Left, Top);

		public RECT(int left_, int top_, int right_, int bottom_)
		{
			Left = left_;
			Top = top_;
			Right = right_;
			Bottom = bottom_;
		}

		public RECT(Rectangle r)
		{
			Left = r.Left;
			Top = r.Top;
			Right = r.Right;
			Bottom = r.Bottom;
		}

		public Rectangle ToRectangle()
		{
			return Rectangle.FromLTRB(Left, Top, Right, Bottom);
		}

		public static RECT FromRectangle(Rectangle rectangle)
		{
			return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
		}

		public override int GetHashCode()
		{
			return Left ^ ((Top << 13) | (Top >> 19)) ^ ((Width << 26) | (Width >> 6)) ^ ((Height << 7) | (Height >> 25));
		}

		public static implicit operator Rectangle(RECT rect)
		{
			return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
		}

		public static implicit operator RECT(Rectangle rect)
		{
			return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
		}

		public override string ToString()
		{
			return "Left=" + Left + ", Top=" + Top + ", Right=" + Right + ", Bottom=" + Bottom;
		}
	}

	public struct PAINTSTRUCT
	{
		public IntPtr hdc;

		public int fErase;

		public RECT rcPaint;

		public int fRestore;

		public int fIncUpdate;

		public int Reserved1;

		public int Reserved2;

		public int Reserved3;

		public int Reserved4;

		public int Reserved5;

		public int Reserved6;

		public int Reserved7;

		public int Reserved8;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class BITMAPINFO
	{
		public int biSize;

		public int biWidth;

		public int biHeight;

		public short biPlanes;

		public short biBitCount;

		public int biCompression;

		public int biSizeImage;

		public int biXPelsPerMeter;

		public int biYPelsPerMeter;

		public int biClrUsed;

		public int biClrImportant;

		public byte bmiColors_rgbBlue;

		public byte bmiColors_rgbGreen;

		public byte bmiColors_rgbRed;

		public byte bmiColors_rgbReserved;
	}

	public enum eObjectId : uint
	{
		OBJID_CLIENT = 4294967292u,
		OBJID_VSCROLL = 4294967291u,
		OBJID_HSCROLL = 4294967290u
	}

	public enum eStateFlags
	{
		STATE_SYSTEM_INVISIBLE = 32768,
		STATE_SYSTEM_OFFSCREEN = 65536,
		STATE_SYSTEM_PRESSED = 8,
		STATE_SYSTEM_UNAVAILABLE = 1
	}

	public enum AppCommands
	{
		APPCOMMAND_BROWSER_BACKWARD = 1,
		APPCOMMAND_BROWSER_FORWARD
	}

	public struct MARGINS
	{
		public int cxLeftWidth;

		public int cxRightWidth;

		public int cyTopHeight;

		public int cyBottomHeight;
	}

	public enum DWMWINDOWATTRIBUTE
	{
		DWMWA_NCRENDERING_ENABLED = 1,
		DWMWA_NCRENDERING_POLICY,
		DWMWA_TRANSITIONS_FORCEDISABLED,
		DWMWA_ALLOW_NCPAINT,
		DWMWA_LAST
	}

	public enum DWMNCRENDERINGPOLICY
	{
		DWMNCRP_USEWINDOWSTYLE,
		DWMNCRP_DISABLED,
		DWMNCRP_ENABLED,
		DWMNCRP_LAST
	}

	public enum WVR
	{
		ALIGNTOP = 16,
		ALIGNLEFT = 32,
		ALIGNBOTTOM = 64,
		ALIGNRIGHT = 128,
		HREDRAW = 256,
		VREDRAW = 512,
		VALIDRECTS = 1024,
		REDRAW = 768
	}

	private static int FAPPCOMMAND_MASK = 61440;

	public static bool IsGlassEnabled
	{
		get
		{
			if (Environment.OSVersion.Version.Major < 6)
			{
				return false;
			}
			return DwmIsCompositionEnabled();
		}
	}

	[DllImport("user32", EntryPoint = "SetWindowLongPtr")]
	public static extern IntPtr SubclassWindow64(IntPtr hWnd, GWL nIndex, WindowProcDelegate dwNewLong);

	[DllImport("user32", EntryPoint = "SetWindowLong")]
	public static extern IntPtr SubclassWindow(IntPtr hWnd, GWL nIndex, WindowProcDelegate dwNewLong);

	[DllImport("user32", EntryPoint = "SetWindowLongPtr")]
	public static extern IntPtr SubclassWindow64(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);

	[DllImport("user32", EntryPoint = "SetWindowLong")]
	public static extern IntPtr SubclassWindow(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern uint CallWindowProc(IntPtr prevWndFunc, IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam);

	[DllImport("user32.dll")]
	public static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

	public static char ToAscii(uint virtualKey)
	{
		StringBuilder stringBuilder = new StringBuilder(2);
		byte[] array = new byte[256];
		NativeFunctions.GetKeyboardState(array);
		int num = ToAscii(virtualKey, 0u, array, stringBuilder, 0u);
		if (num == 1)
		{
			return stringBuilder[0];
		}
		return '\0';
	}

	[DllImport("user32.dll")]
	private static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpKeyState, [Out] StringBuilder lpChar, uint uFlags);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	public static extern short GetKeyState(int keyCode);

	[DllImport("user32.dll")]
	public static extern bool IsZoomed(IntPtr hWnd);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

	[DllImport("gdi32.dll")]
	public static extern int ExcludeClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

	[DllImport("user32")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern bool GetTextMetrics(HandleRef hdc, TEXTMETRIC tm);

	[DllImport("user32.dll")]
	public static extern bool SetMenu(IntPtr hWnd, IntPtr hMenu);

	[DllImport("user32.dll")]
	public static extern IntPtr GetMenu(IntPtr hWnd);

	[DllImport("user32")]
	public static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);

	[DllImport("user32")]
	public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);

	[DllImport("user32")]
	public static extern bool GetWindowRect(IntPtr hWnd, ref RECT r);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern int GetScrollBarInfo(IntPtr hWnd, uint idObject, ref SCROLLBARINFO psbi);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool GetScrollInfo(IntPtr hwnd, SBOrientation fnBar, ref SCROLLINFO lpsi);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	public static extern int SetScrollInfo(IntPtr hWnd, SBOrientation fnBar, ref SCROLLINFO si, bool redraw);

	[DllImport("user32.dll")]
	public static extern IntPtr GetWindowDC(IntPtr hWnd);

	[DllImport("user32.dll")]
	public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool SetForegroundWindow(IntPtr hWnd);

	[DllImport("user32.dll")]
	public static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

	public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
	{
		if (IntPtr.Size == 8)
		{
			return GetWindowLongPtr64(hWnd, nIndex);
		}
		return new IntPtr(GetWindowLong32(hWnd, nIndex));
	}

	[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
	public static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
	public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	public static IntPtr SetWindowLongPtr(IntPtr hwnd, int nIndex, IntPtr dwNewLong)
	{
		if (IntPtr.Size == 8)
		{
			return SetWindowLongPtr64(hwnd, nIndex, dwNewLong);
		}
		return new IntPtr(SetWindowLongPtr32(hwnd, nIndex, dwNewLong.ToInt32()));
	}

	[DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
	private static extern int SetWindowLongPtr32(IntPtr hWnd, int nIndex, int dwNewLong);

	[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
	private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	public static extern int MapWindowPoints(IntPtr hwndFrom, IntPtr hwndTo, ref POINT lpPoints, [MarshalAs(UnmanagedType.U4)] int cPoints);

	[DllImport("user32")]
	public static extern bool PostMessage(int hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32")]
	public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern bool DeleteDC(IntPtr hDC);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr CreateDIBSection(IntPtr hdc, BITMAPINFO pbmi, uint iUsage, int ppvBits, IntPtr hSection, uint dwOffset);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

	[DllImport("gdi32")]
	public static extern bool DeleteObject(IntPtr hObject);

	[DllImport("gdi32")]
	public static extern bool DeleteObject(int hObject);

	[DllImport("user32.dll")]
	public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerDelegate lpTimerFunc);

	[DllImport("user32.dll")]
	public static extern bool KillTimer(IntPtr hWnd, UIntPtr uIDEvent);

	internal static bool ModifyHwndStyle(IntPtr hwnd, int removeStyle, int addStyle)
	{
		int num = GetWindowLongPtr(hwnd, -16).ToInt32();
		int num2 = (num & ~removeStyle) | addStyle;
		if (num == num2)
		{
			return false;
		}
		SetWindowLong(hwnd, -16, num2);
		return true;
	}

	[DllImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

	[DllImport("Gdi32.dll")]
	public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

	public static int GetAppCommandLParam(IntPtr lParam)
	{
		return HIWORD(lParam) & ~FAPPCOMMAND_MASK;
	}

	public static int LOWORD(int n)
	{
		return (short)(n & 0xFFFF);
	}

	public static int HIWORD(int n)
	{
		return (n >> 16) & 0xFFFF;
	}

	public static int LOWORD(IntPtr n)
	{
		return LOWORD((int)(long)n);
	}

	public static int HIWORD(IntPtr n)
	{
		return (short)((int)n >>> 16);
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

	public static int ToInt(IntPtr ptr)
	{
		int result = 0;
		try
		{
			result = (int)(long)ptr;
		}
		catch
		{
		}
		return result;
	}

	[DllImport("dwmapi.dll")]
	public static extern int DwmSetWindowAttribute(IntPtr hWnd, int dwAttribute, ref int attributeValue, int sizeOfValueRetrieved);

	[DllImport("dwmapi.dll")]
	public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, ref int pvAttribute, int sizeOfValueRetrieved);

	[DllImport("dwmapi.dll", PreserveSig = false)]
	private static extern bool DwmIsCompositionEnabled();

	[DllImport("dwmapi.dll")]
	public static extern int DwmDefWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, out IntPtr plResult);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW")]
	public static extern IntPtr DefWindowProc(IntPtr hWnd, WindowsMessages Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW")]
	public static extern IntPtr DefWindowProc(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("dwmapi.dll")]
	public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMargins);

	public static void ExtendGlass(IntPtr handle, int glassHeight)
	{
		MARGINS pMargins = default(MARGINS);
		pMargins.cxLeftWidth = 0;
		pMargins.cxRightWidth = 0;
		pMargins.cyTopHeight = glassHeight;
		pMargins.cyBottomHeight = 0;
		int num = DwmExtendFrameIntoClientArea(handle, ref pMargins);
	}
}
