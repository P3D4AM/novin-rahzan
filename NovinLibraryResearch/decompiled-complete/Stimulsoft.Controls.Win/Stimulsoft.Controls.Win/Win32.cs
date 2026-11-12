using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win;

[SuppressUnmanagedCodeSecurity]
internal sealed class Win32
{
	public enum DrawingOptions
	{
		PRF_CHECKVISIBLE,
		PRF_CHILDREN,
		PRF_CLIENT,
		PRF_ERASEBKGND,
		PRF_NONCLIENT,
		PRF_OWNED
	}

	public enum WindowExStyles
	{
		WS_EX_DLGMODALFRAME = 1,
		WS_EX_NOPARENTNOTIFY = 4,
		WS_EX_TOPMOST = 8,
		WS_EX_ACCEPTFILES = 16,
		WS_EX_TRANSPARENT = 32,
		WS_EX_MDICHILD = 64,
		WS_EX_TOOLWINDOW = 128,
		WS_EX_WINDOWEDGE = 256,
		WS_EX_CLIENTEDGE = 512,
		WS_EX_CONTEXTHELP = 1024,
		WS_EX_RIGHT = 4096,
		WS_EX_LEFT = 0,
		WS_EX_RTLREADING = 8192,
		WS_EX_LTRREADING = 0,
		WS_EX_LEFTSCROLLBAR = 16384,
		WS_EX_RIGHTSCROLLBAR = 0,
		WS_EX_CONTROLPARENT = 65536,
		WS_EX_STATICEDGE = 131072,
		WS_EX_APPWINDOW = 262144,
		WS_EX_OVERLAPPEDWINDOW = 768,
		WS_EX_PALETTEWINDOW = 392,
		WS_EX_LAYERED = 524288
	}

	public enum MousePosition
	{
		HTERROR = -2,
		HTTRANSPARENT = -1,
		HTNOWHERE = 0,
		HTCLIENT = 1,
		HTCAPTION = 2,
		HTSYSMENU = 3,
		HTGROWBOX = 4,
		HTSIZE = 4,
		HTMENU = 5,
		HTHSCROLL = 6,
		HTVSCROLL = 7,
		HTMINBUTTON = 8,
		HTMAXBUTTON = 9,
		HTLEFT = 10,
		HTRIGHT = 11,
		HTTOP = 12,
		HTTOPLEFT = 13,
		HTTOPRIGHT = 14,
		HTBOTTOM = 15,
		HTBOTTOMLEFT = 16,
		HTBOTTOMRIGHT = 17,
		HTBORDER = 18,
		HTOBJECT = 19,
		HTCLOSE = 20,
		HTHELP = 21
	}

	public enum VirtualKey
	{
		VK_LBUTTON = 1,
		VK_CANCEL = 3,
		VK_BACK = 8,
		VK_TAB = 9,
		VK_CLEAR = 12,
		VK_RETURN = 13,
		VK_SHIFT = 16,
		VK_CONTROL = 17,
		VK_MENU = 18,
		VK_CAPITAL = 20,
		VK_ESCAPE = 27,
		VK_SPACE = 32,
		VK_PRIOR = 33,
		VK_NEXT = 34,
		VK_END = 35,
		VK_HOME = 36,
		VK_LEFT = 37,
		VK_UP = 38,
		VK_RIGHT = 39,
		VK_DOWN = 40,
		VK_SELECT = 41,
		VK_EXECUTE = 43,
		VK_SNAPSHOT = 44,
		VK_HELP = 47,
		VK_0 = 48,
		VK_1 = 49,
		VK_2 = 50,
		VK_3 = 51,
		VK_4 = 52,
		VK_5 = 53,
		VK_6 = 54,
		VK_7 = 55,
		VK_8 = 56,
		VK_9 = 57,
		VK_A = 65,
		VK_B = 66,
		VK_C = 67,
		VK_D = 68,
		VK_E = 69,
		VK_F = 70,
		VK_G = 71,
		VK_H = 72,
		VK_I = 73,
		VK_J = 74,
		VK_K = 75,
		VK_L = 76,
		VK_M = 77,
		VK_N = 78,
		VK_O = 79,
		VK_P = 80,
		VK_Q = 81,
		VK_R = 82,
		VK_S = 83,
		VK_T = 84,
		VK_U = 85,
		VK_V = 86,
		VK_W = 87,
		VK_X = 88,
		VK_Y = 89,
		VK_Z = 90,
		VK_NUMPAD0 = 96,
		VK_NUMPAD1 = 97,
		VK_NUMPAD2 = 98,
		VK_NUMPAD3 = 99,
		VK_NUMPAD4 = 100,
		VK_NUMPAD5 = 101,
		VK_NUMPAD6 = 102,
		VK_NUMPAD7 = 103,
		VK_NUMPAD8 = 104,
		VK_NUMPAD9 = 105,
		VK_MULTIPLY = 106,
		VK_ADD = 107,
		VK_SEPARATOR = 108,
		VK_SUBTRACT = 109,
		VK_DECIMAL = 110,
		VK_DIVIDE = 111,
		VK_ATTN = 246,
		VK_CRSEL = 247,
		VK_EXSEL = 248,
		VK_EREOF = 249,
		VK_PLAY = 250,
		VK_ZOOM = 251,
		VK_NONAME = 252,
		VK_PA1 = 253,
		VK_OEM_CLEAR = 254,
		VK_LWIN = 91,
		VK_RWIN = 92,
		VK_APPS = 93,
		VK_LSHIFT = 160,
		VK_RSHIFT = 161,
		VK_LCONTROL = 162,
		VK_RCONTROL = 163,
		VK_LMENU = 164,
		VK_RMENU = 165
	}

	public enum ActivateState
	{
		WA_INACTIVE,
		WA_ACTIVE,
		WA_CLICKACTIVE
	}

	public enum MouseActivateFlags
	{
		MA_ACTIVATE = 1,
		MA_ACTIVATEANDEAT,
		MA_NOACTIVATE,
		MA_NOACTIVATEANDEAT
	}

	public enum Msg
	{
		WM_NULL = 0,
		WM_CREATE = 1,
		WM_DESTROY = 2,
		WM_MOVE = 3,
		WM_SIZE = 5,
		WM_ACTIVATE = 6,
		WM_SETFOCUS = 7,
		WM_KILLFOCUS = 8,
		WM_ENABLE = 10,
		WM_SETREDRAW = 11,
		WM_SETTEXT = 12,
		WM_GETTEXT = 13,
		WM_GETTEXTLENGTH = 14,
		WM_PAINT = 15,
		WM_CLOSE = 16,
		WM_QUERYENDSESSION = 17,
		WM_QUIT = 18,
		WM_QUERYOPEN = 19,
		WM_ERASEBKGND = 20,
		WM_SYSCOLORCHANGE = 21,
		WM_ENDSESSION = 22,
		WM_SHOWWINDOW = 24,
		WM_CTLCOLOR = 25,
		WM_WININICHANGE = 26,
		WM_SETTINGCHANGE = 26,
		WM_DEVMODECHANGE = 27,
		WM_ACTIVATEAPP = 28,
		WM_FONTCHANGE = 29,
		WM_TIMECHANGE = 30,
		WM_CANCELMODE = 31,
		WM_SETCURSOR = 32,
		WM_MOUSEACTIVATE = 33,
		WM_CHILDACTIVATE = 34,
		WM_QUEUESYNC = 35,
		WM_GETMINMAXINFO = 36,
		WM_PAINTICON = 38,
		WM_ICONERASEBKGND = 39,
		WM_NEXTDLGCTL = 40,
		WM_SPOOLERSTATUS = 42,
		WM_DRAWITEM = 43,
		WM_MEASUREITEM = 44,
		WM_DELETEITEM = 45,
		WM_VKEYTOITEM = 46,
		WM_CHARTOITEM = 47,
		WM_SETFONT = 48,
		WM_GETFONT = 49,
		WM_SETHOTKEY = 50,
		WM_GETHOTKEY = 51,
		WM_QUERYDRAGICON = 55,
		WM_COMPAREITEM = 57,
		WM_GETOBJECT = 61,
		WM_COMPACTING = 65,
		WM_COMMNOTIFY = 68,
		WM_WINDOWPOSCHANGING = 70,
		WM_WINDOWPOSCHANGED = 71,
		WM_POWER = 72,
		WM_COPYDATA = 74,
		WM_CANCELJOURNAL = 75,
		WM_NOTIFY = 78,
		WM_INPUTLANGCHANGEREQUEST = 80,
		WM_INPUTLANGCHANGE = 81,
		WM_TCARD = 82,
		WM_HELP = 83,
		WM_USERCHANGED = 84,
		WM_NOTIFYFORMAT = 85,
		WM_CONTEXTMENU = 123,
		WM_STYLECHANGING = 124,
		WM_STYLECHANGED = 125,
		WM_DISPLAYCHANGE = 126,
		WM_GETICON = 127,
		WM_SETICON = 128,
		WM_NCCREATE = 129,
		WM_NCDESTROY = 130,
		WM_NCCALCSIZE = 131,
		WM_NCHITTEST = 132,
		WM_NCPAINT = 133,
		WM_NCACTIVATE = 134,
		WM_GETDLGCODE = 135,
		WM_SYNCPAINT = 136,
		WM_NCMOUSEMOVE = 160,
		WM_NCLBUTTONDOWN = 161,
		WM_NCLBUTTONUP = 162,
		WM_NCLBUTTONDBLCLK = 163,
		WM_NCRBUTTONDOWN = 164,
		WM_NCRBUTTONUP = 165,
		WM_NCRBUTTONDBLCLK = 166,
		WM_NCMBUTTONDOWN = 167,
		WM_NCMBUTTONUP = 168,
		WM_NCMBUTTONDBLCLK = 169,
		WM_KEYDOWN = 256,
		WM_KEYUP = 257,
		WM_CHAR = 258,
		WM_DEADCHAR = 259,
		WM_SYSKEYDOWN = 260,
		WM_SYSKEYUP = 261,
		WM_SYSCHAR = 262,
		WM_SYSDEADCHAR = 263,
		WM_KEYLAST = 264,
		WM_IME_STARTCOMPOSITION = 269,
		WM_IME_ENDCOMPOSITION = 270,
		WM_IME_COMPOSITION = 271,
		WM_IME_KEYLAST = 271,
		WM_INITDIALOG = 272,
		WM_COMMAND = 273,
		WM_SYSCOMMAND = 274,
		WM_TIMER = 275,
		WM_HSCROLL = 276,
		WM_VSCROLL = 277,
		WM_INITMENU = 278,
		WM_INITMENUPOPUP = 279,
		WM_MENUSELECT = 287,
		WM_MENUCHAR = 288,
		WM_ENTERIDLE = 289,
		WM_MENURBUTTONUP = 290,
		WM_MENUDRAG = 291,
		WM_MENUGETOBJECT = 292,
		WM_UNINITMENUPOPUP = 293,
		WM_MENUCOMMAND = 294,
		WM_CTLCOLORMSGBOX = 306,
		WM_CTLCOLOREDIT = 307,
		WM_CTLCOLORLISTBOX = 308,
		WM_CTLCOLORBTN = 309,
		WM_CTLCOLORDLG = 310,
		WM_CTLCOLORSCROLLBAR = 311,
		WM_CTLCOLORSTATIC = 312,
		WM_MOUSEMOVE = 512,
		WM_LBUTTONDOWN = 513,
		WM_LBUTTONUP = 514,
		WM_LBUTTONDBLCLK = 515,
		WM_RBUTTONDOWN = 516,
		WM_RBUTTONUP = 517,
		WM_RBUTTONDBLCLK = 518,
		WM_MBUTTONDOWN = 519,
		WM_MBUTTONUP = 520,
		WM_MBUTTONDBLCLK = 521,
		WM_MOUSEWHEEL = 522,
		WM_PARENTNOTIFY = 528,
		WM_ENTERMENULOOP = 529,
		WM_EXITMENULOOP = 530,
		WM_NEXTMENU = 531,
		WM_SIZING = 532,
		WM_CAPTURECHANGED = 533,
		WM_MOVING = 534,
		WM_DEVICECHANGE = 537,
		WM_MDICREATE = 544,
		WM_MDIDESTROY = 545,
		WM_MDIACTIVATE = 546,
		WM_MDIRESTORE = 547,
		WM_MDINEXT = 548,
		WM_MDIMAXIMIZE = 549,
		WM_MDITILE = 550,
		WM_MDICASCADE = 551,
		WM_MDIICONARRANGE = 552,
		WM_MDIGETACTIVE = 553,
		WM_MDISETMENU = 560,
		WM_ENTERSIZEMOVE = 561,
		WM_EXITSIZEMOVE = 562,
		WM_DROPFILES = 563,
		WM_MDIREFRESHMENU = 564,
		WM_IME_SETCONTEXT = 641,
		WM_IME_NOTIFY = 642,
		WM_IME_CONTROL = 643,
		WM_IME_COMPOSITIONFULL = 644,
		WM_IME_SELECT = 645,
		WM_IME_CHAR = 646,
		WM_IME_REQUEST = 648,
		WM_IME_KEYDOWN = 656,
		WM_IME_KEYUP = 657,
		WM_MOUSEHOVER = 673,
		WM_MOUSELEAVE = 675,
		WM_XBUTTONDOWN = 523,
		WM_XBUTTONUP = 524,
		WM_XBUTTONDBLCLK = 525,
		WM_NCXBUTTONDOWN = 171,
		WM_NCXBUTTONUP = 172,
		WM_CUT = 768,
		WM_COPY = 769,
		WM_PASTE = 770,
		WM_CLEAR = 771,
		WM_UNDO = 772,
		WM_PaintFORMAT = 773,
		WM_PaintALLFORMATS = 774,
		WM_DESTROYCLIPBOARD = 775,
		WM_DRAWCLIPBOARD = 776,
		WM_PAINTCLIPBOARD = 777,
		WM_VSCROLLCLIPBOARD = 778,
		WM_SIZECLIPBOARD = 779,
		WM_ASKCBFORMATNAME = 780,
		WM_CHANGECBCHAIN = 781,
		WM_HSCROLLCLIPBOARD = 782,
		WM_QUERYNEWPALETTE = 783,
		WM_PALETTEISCHANGING = 784,
		WM_PALETTECHANGED = 785,
		WM_HOTKEY = 786,
		WM_PRINT = 791,
		WM_PRINTCLIENT = 792,
		WM_HANDHELDFIRST = 856,
		WM_HANDHELDLAST = 863,
		WM_AFXFIRST = 864,
		WM_AFXLAST = 895,
		WM_PENWINFIRST = 896,
		WM_PENWINLAST = 911,
		WM_APP = 32768,
		WM_USER = 1024,
		WM_REFLECT = 8192,
		EM_FORMATRANGE = 1081,
		EM_GETCHARFORMAT = 1082,
		EM_SETCHARFORMAT = 1092
	}

	public enum SetWindowPosZ
	{
		HWND_TOP = 0,
		HWND_BOTTOM = 1,
		HWND_TOPMOST = -1,
		HWND_NOTOPMOST = -2
	}

	[Flags]
	public enum SetWindowPosFlags
	{
		SWP_NOSIZE = 1,
		SWP_NOMOVE = 2,
		SWP_NOZORDER = 4,
		SWP_NOREDRAW = 8,
		SWP_NOACTIVATE = 0x10,
		SWP_FRAMECHANGED = 0x20,
		SWP_SHOWWINDOW = 0x40,
		SWP_HIDEWINDOW = 0x80,
		SWP_NOCOPYBITS = 0x100,
		SWP_NOOWNERZORDER = 0x200,
		SWP_NOSENDCHANGING = 0x400,
		SWP_DRAWFRAME = 0x20,
		SWP_NOREPOSITION = 0x200,
		SWP_DEFERERASE = 0x2000,
		SWP_ASYNCWINDOWPOS = 0x4000
	}

	public enum PenStyle
	{
		PS_SOLID,
		PS_DASH,
		PS_DOT,
		PS_DASHDOT,
		PS_DASHDOTDOT,
		PS_NULL,
		PS_INSIDEFRAME,
		PS_USERSTYLE,
		PS_ALTERNATE
	}

	public enum DrawMode
	{
		R2_BLACK = 1,
		R2_NOTMERGEPEN,
		R2_MASKNOTPEN,
		R2_NOTCOPYPEN,
		R2_MASKPENNOT,
		R2_NOT,
		R2_XORPEN,
		R2_NOTMASKPEN,
		R2_MASKPEN,
		R2_NOTXORPEN,
		R2_NOP,
		R2_MERGENOTPEN,
		R2_COPYPEN,
		R2_MERGEPENNOT,
		R2_MERGEPEN,
		R2_WHITE
	}

	public enum StockObject
	{
		WHITE_BRUSH = 0,
		LTGRAY_BRUSH = 1,
		GRAY_BRUSH = 2,
		DKGRAY_BRUSH = 3,
		BLACK_BRUSH = 4,
		NULL_BRUSH = 5,
		HOLLOW_BRUSH = 5,
		WHITE_PEN = 6,
		BLACK_PEN = 7,
		NULL_PEN = 8,
		OEM_FIXED_FONT = 10,
		ANSI_FIXED_FONT = 11,
		ANSI_VAR_FONT = 12,
		SYSTEM_FONT = 13,
		DEVICE_DEFAULT_FONT = 14,
		DEFAULT_PALETTE = 15,
		SYSTEM_FIXED_FONT = 16
	}

	public enum ColorType
	{
		COLOR_SCROLLBAR = 0,
		COLOR_BACKGROUND = 1,
		COLOR_ACTIVECAPTION = 2,
		COLOR_INACTIVECAPTION = 3,
		COLOR_MENU = 4,
		COLOR_WINDOW = 5,
		COLOR_WINDOWFRAME = 6,
		COLOR_MENUTEXT = 7,
		COLOR_WINDOWTEXT = 8,
		COLOR_CAPTIONTEXT = 9,
		COLOR_ACTIVEBORDER = 10,
		COLOR_INACTIVEBORDER = 11,
		COLOR_APPWORKSPACE = 12,
		COLOR_HIGHLIGHT = 13,
		COLOR_HIGHLIGHTTEXT = 14,
		COLOR_BTNFACE = 15,
		COLOR_BTNSHADOW = 16,
		COLOR_GRAYTEXT = 17,
		COLOR_BTNTEXT = 18,
		COLOR_INACTIVECAPTIONTEXT = 19,
		COLOR_BTNHIGHLIGHT = 20,
		COLOR_3DDKSHADOW = 21,
		COLOR_3DLIGHT = 22,
		COLOR_INFOTEXT = 23,
		COLOR_INFOBK = 24,
		COLOR_HOTLIGHT = 26,
		COLOR_GRADIENTACTIVECAPTION = 27,
		COLOR_GRADIENTINACTIVECAPTION = 28,
		COLOR_MENUHILIGHT = 29,
		COLOR_MENUBAR = 30
	}

	public enum ShowWindowCommand
	{
		SW_HIDE = 0,
		SW_SHOWNORMAL = 1,
		SW_NORMAL = 1,
		SW_SHOWMINIMIZED = 2,
		SW_SHOWMAXIMIZED = 3,
		SW_MAXIMIZE = 3,
		SW_SHOWNOACTIVATE = 4,
		SW_SHOW = 5,
		SW_MINIMIZE = 6,
		SW_SHOWMINNOACTIVE = 7,
		SW_SHOWNA = 8,
		SW_RESTORE = 9,
		SW_SHOWDEFAULT = 10,
		SW_FORCEMINIMIZE = 11,
		SW_MAX = 11
	}

	public struct RECT
	{
		public int left;

		public int top;

		public int right;

		public int bottom;

		public RECT(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public static RECT FromXYWH(int x, int y, int width, int height)
		{
			return new RECT(x, y, x + width, y + height);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public class POINT
	{
		public int x;

		public int y;

		public POINT()
		{
		}

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

	public struct CWPSTRUCT
	{
		public IntPtr lparam;

		public IntPtr wparam;

		public int message;

		public IntPtr hwnd;
	}

	internal struct WINDOWPOS
	{
		public IntPtr hwnd;

		public IntPtr hwndAfter;

		public int x;

		public int y;

		public int cx;

		public int cy;

		public uint flags;
	}

	internal struct NCCALCSIZE_PARAMS
	{
		public RECT rgc;

		public WINDOWPOS wndpos;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class SIZE
	{
		public int cx;

		public int cy;

		public SIZE()
		{
		}

		public SIZE(int cx, int cy)
		{
			this.cx = cx;
			this.cy = cy;
		}

		public Size ToSize()
		{
			return new Size(cx, cy);
		}
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern int GetSysColor(int color);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern bool LockWindowUpdate(IntPtr hWnd);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr GetDC(IntPtr hwnd);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr GetWindow(IntPtr hwnd, int wCmd);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern bool GetWindowRect(IntPtr hwnd, ref RECT lpRect);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern bool PtInRect(ref RECT lpRect, int x, int y);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern bool ReleaseDC(IntPtr hwnd, IntPtr hdc);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern bool SetWindowText(IntPtr hwnd, ref string lpString);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern int ShowWindow(IntPtr hWnd, short cmdShow);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern int PostMessage(IntPtr hwnd, int msg, int wParam, IntPtr lParam);

	[DllImport("USER32.DLL")]
	public static extern int GetCaretBlinkTime();

	public static Bitmap CaptureWindow(Control control)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		Bitmap val = new Bitmap(control.Width, control.Height);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			IntPtr hdc = val2.GetHdc();
			SendMessage(control.Handle, 791, (int)hdc, new IntPtr(12));
			val2.ReleaseHdc(hdc);
			return val;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern bool ScrollWindow(IntPtr hWnd, int xAmount, int yAmount, ref RECT rectScrollRegion, ref RECT rectClip);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern bool ScrollWindow(IntPtr hWnd, int xAmount, int yAmount, IntPtr rectScrollRegion, IntPtr rectClip);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern bool ScrollWindowEx(IntPtr hWnd, int nXAmount, int nYAmount, IntPtr rectScrollRegion, IntPtr rectClip, IntPtr hrgnUpdate, IntPtr prcUpdate, int flags);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern short GetKeyState(int nVirtKey);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr GetWindowDC(IntPtr handle);

	[DllImport("Gdi32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern int CallWindowProc(IntPtr wndProc, IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern bool UnhookWindowsHookEx(IntPtr hookHandle);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern int GetWindowThreadProcessId(IntPtr hwnd, int ID);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern int GetClassName(IntPtr hwnd, char[] className, int maxCount);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern int CallNextHookEx(IntPtr hookHandle, int code, IntPtr wparam, ref CWPSTRUCT cwp);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr CreatePen(int nStyle, int nWidth, int crColor);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern bool MoveToEx(IntPtr hdc, int x, int y, IntPtr pt);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern bool LineTo(IntPtr hdc, int x, int y);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern int SetROP2(IntPtr hDC, int nDrawMode);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr GetStockObject(int nIndex);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern bool Rectangle(IntPtr hdc, int left, int top, int right, int bottom);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr DeleteObject(IntPtr hDC);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern int SetBkColor(IntPtr hDC, int clr);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr CreateSolidBrush(int crColor);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	public static extern bool PatBlt(IntPtr hdc, int left, int top, int width, int height, int rop);

	private Win32()
	{
	}
}
