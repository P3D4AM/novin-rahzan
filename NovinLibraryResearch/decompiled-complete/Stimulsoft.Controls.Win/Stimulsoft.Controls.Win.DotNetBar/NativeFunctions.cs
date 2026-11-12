using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[SuppressUnmanagedCodeSecurity]
public class NativeFunctions
{
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
	}

	public struct SIZE
	{
		public int cx;

		public int cy;
	}

	public struct TRACKMOUSEEVENT
	{
		public int cbSize;

		public uint dwFlags;

		public int dwHoverTime;

		public IntPtr hwndTrack;
	}

	public struct WINDOWPOS
	{
		public int hwnd;

		public int hwndInsertAfter;

		public int x;

		public int y;

		public int cx;

		public int cy;

		public int flags;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BLENDFUNCTION
	{
		public byte BlendOp;

		public byte BlendFlags;

		public byte SourceConstantAlpha;

		public byte AlphaFormat;
	}

	public enum Win32UpdateLayeredWindowsFlags
	{
		ULW_COLORKEY = 1,
		ULW_ALPHA = 2,
		ULW_OPAQUE = 4
	}

	public enum Win23AlphaFlags : byte
	{
		AC_SRC_OVER,
		AC_SRC_ALPHA
	}

	public struct OSVERSIONINFO
	{
		public int dwOSVersionInfoSize;

		public int dwMajorVersion;

		public int dwMinorVersion;

		public int dwBuildNumber;

		public int dwPlatformId;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string szCSDVersion;
	}

	public const uint SPI_GETMENUANIMATION = 4098u;

	public const uint SPI_GETMENUFADE = 4114u;

	public const uint SPI_GETMENUUNDERLINES = 4106u;

	public const uint SPI_GETDROPSHADOW = 4132u;

	public const uint SPI_GETCURSORSHADOW = 4122u;

	public const uint SPI_GETMENUDROPALIGNMENT = 27u;

	public const uint SM_CXBORDER = 5u;

	public const int LWA_COLORKEY = 1;

	public const int LWA_ALPHA = 2;

	private const int R2_NOTXORPEN = 10;

	public const int SND_ASYNC = 1;

	public const int SND_NODEFAULT = 2;

	public const uint BDR_RAISEDOUTER = 1u;

	public const uint BDR_SUNKENOUTER = 2u;

	public const uint BDR_RAISEDINNER = 4u;

	public const uint BDR_SUNKENINNER = 8u;

	public const uint BDR_OUTER = 3u;

	public const uint BDR_INNER = 12u;

	public const uint BDR_RAISED = 5u;

	public const uint BDR_SUNKEN = 10u;

	public const uint EDGE_RAISED = 5u;

	public const uint EDGE_SUNKEN = 10u;

	public const uint EDGE_ETCHED = 6u;

	public const uint EDGE_BUMP = 9u;

	public const uint BF_LEFT = 1u;

	public const uint BF_TOP = 2u;

	public const uint BF_RIGHT = 4u;

	public const uint BF_BOTTOM = 8u;

	public const uint BF_TOPLEFT = 3u;

	public const uint BF_TOPRIGHT = 6u;

	public const uint BF_BOTTOMLEFT = 9u;

	public const uint BF_BOTTOMRIGHT = 12u;

	public const uint BF_RECT = 15u;

	public const uint BF_DIAGONAL = 16u;

	public const uint BF_DIAGONAL_ENDTOPRIGHT = 22u;

	public const uint BF_DIAGONAL_ENDTOPLEFT = 19u;

	public const uint BF_DIAGONAL_ENDBOTTOMLEFT = 25u;

	public const uint BF_DIAGONAL_ENDBOTTOMRIGHT = 28u;

	public const uint BF_MIDDLE = 2048u;

	public const uint BF_SOFT = 4096u;

	public const uint BF_ADJUST = 8192u;

	public const uint BF_FLAT = 16384u;

	public const uint BF_MONO = 32768u;

	public const uint TME_HOVER = 1u;

	public const uint TME_LEAVE = 2u;

	public const uint TME_NONCLIENT = 16u;

	public const uint TME_QUERY = 1073741824u;

	public const uint TME_CANCEL = 2147483648u;

	public const uint HOVER_DEFAULT = uint.MaxValue;

	public const long WM_ACTIVATEAPP = 28L;

	public const int WM_LBUTTONDOWN = 513;

	public const int WM_LBUTTONUP = 514;

	public const int WM_LBUTTONDBLCLK = 515;

	public const int WM_NCLBUTTONDOWN = 161;

	public const int WM_NCMBUTTONDBLCLK = 169;

	public const int WM_NCACTIVATE = 134;

	public const int WM_RBUTTONDOWN = 516;

	public const int WM_RBUTTONUP = 517;

	public const int WM_MBUTTONDOWN = 519;

	public const int WM_NCRBUTTONDOWN = 164;

	public const int WM_NCMBUTTONDOWN = 167;

	public const int WM_CONTEXTMENU = 123;

	public const int WM_USER = 1024;

	public const int WM_MOUSEMOVE = 512;

	public const int WM_MOUSELEAVE = 675;

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

	public const int WM_SETFOCUS = 7;

	public const int WM_KILLFOCUS = 8;

	public const int WM_SHOWWINDOW = 24;

	public const int WM_SETREDRAW = 11;

	public const int WM_PAINT = 15;

	public const int WM_MOUSEWHEEL = 522;

	public const int WM_HSCROLL = 276;

	public const int WM_VSCROLL = 277;

	public const int WM_SYSKEYDOWN = 260;

	public const int WM_SYSKEYUP = 261;

	public const int WM_ACTIVATE = 6;

	public const int WA_INACTIVE = 0;

	public const int WA_ACTIVE = 1;

	public const int WA_CLICKACTIVE = 2;

	public const int AW_HOR_POSITIVE = 1;

	public const int AW_HOR_NEGATIVE = 2;

	public const int AW_VER_POSITIVE = 4;

	public const int AW_VER_NEGATIVE = 8;

	public const int AW_CENTER = 16;

	public const int AW_HIDE = 65536;

	public const int AW_ACTIVATE = 131072;

	public const int AW_SLIDE = 262144;

	public const int AW_BLEND = 524288;

	public const int WM_SYSCOMMAND = 274;

	public const int SC_CLOSE = 61536;

	public const int SC_CONTEXTHELP = 61824;

	public const int SC_NEXTWINDOW = 61504;

	public const int SC_PREVWINDOW = 61520;

	public const int SC_KEYMENU = 61696;

	public const int SC_RESTORE = 61728;

	public const int SC_MINIMIZE = 61472;

	public const int SC_MAXIMIZE = 61488;

	public const int SC_MOVE = 61456;

	public const int SC_SIZE = 61440;

	public const int WM_MOUSEACTIVATE = 33;

	public const int MA_NOACTIVATE = 3;

	public const int MA_NOACTIVATEANDEAT = 4;

	public const uint WS_POPUP = 2147483648u;

	public const uint WS_CLIPSIBLINGS = 67108864u;

	public const uint WS_CLIPCHILDREN = 33554432u;

	public const uint WS_EX_TOPMOST = 8u;

	public const uint WS_EX_TOOLWINDOW = 128u;

	public const int WM_THEMECHANGED = 794;

	public const int WM_PRINTCLIENT = 792;

	public const int WM_DISPLAYCHANGE = 126;

	public const int BITSPIXEL = 12;

	private static bool m_ShowKeyboardCues = true;

	private static bool m_ShowDropShadow = true;

	public static int ColorDepth = 0;

	private static bool m_CursorShadow = true;

	private static bool m_RightHandedMenus = false;

	public const int SM_REMOTESESSION = 4096;

	public static int BorderMultiplierFactor
	{
		get
		{
			int pvParam = 0;
			SystemParametersInfo(5u, 0u, ref pvParam, 0u);
			return pvParam;
		}
	}

	public static ePopupAnimation SystemMenuAnimation
	{
		get
		{
			bool pvParam = false;
			SystemParametersInfo(4098u, 0u, ref pvParam, 0u);
			if (pvParam)
			{
				pvParam = false;
				SystemParametersInfo(4114u, 0u, ref pvParam, 0u);
				if (pvParam)
				{
					return ePopupAnimation.Fade;
				}
				return ePopupAnimation.Slide;
			}
			return ePopupAnimation.None;
		}
	}

	public static bool ShowKeyboardCues => m_ShowKeyboardCues;

	public static bool ShowDropShadow => m_ShowDropShadow;

	public static bool AlphaBlendingSupported
	{
		get
		{
			if (Environment.OSVersion.Version.Major >= 5)
			{
				return true;
			}
			return false;
		}
	}

	public static bool CursorShadow => m_CursorShadow;

	public static bool RightHandedMenus => m_RightHandedMenus;

	[DllImport("user32.dll")]
	public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, ref POINT lpPoints, int cPoints);

	[DllImport("user32.dll")]
	private static extern int GetClassName(IntPtr hWnd, [Out] StringBuilder lpClassName, int nMaxCount);

	[DllImport("user32.dll")]
	public static extern IntPtr GetFocus();

	[DllImport("user32.dll")]
	public static extern IntPtr SetFocus(IntPtr hWnd);

	[DllImport("user32.dll")]
	public static extern IntPtr GetForegroundWindow();

	[DllImport("user32")]
	public static extern IntPtr WindowFromPoint(POINT p);

	[DllImport("user32")]
	public static extern IntPtr ChildWindowFromPoint(IntPtr parent, POINT p);

	[DllImport("user32.dll")]
	public static extern IntPtr RealChildWindowFromPoint(IntPtr hwndParent, POINT ptParentClientCoords);

	[DllImport("user32")]
	public static extern bool DrawIconEx(IntPtr hDC, int X, int Y, IntPtr hIcon, int width, int height, int frameIndex, IntPtr flickerFreeBrush, int flags);

	[DllImport("user32")]
	public static extern uint MapVirtualKey(uint uCode, uint uMapType);

	[DllImport("user32")]
	public static extern bool GetKeyboardState(byte[] state);

	[DllImport("user32.dll")]
	public static extern bool DrawCaption(IntPtr hwnd, IntPtr hdc, ref RECT lprc, eDrawCaption uFlags);

	[DllImport("user32")]
	public static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpKeyState, byte[] lpChar, uint uFlags);

	[DllImport("user32")]
	public static extern int SetFocus(int hWnd);

	[DllImport("user32")]
	public static extern bool IsWindow(int hWnd);

	[DllImport("user32")]
	public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT tme);

	[DllImport("user32")]
	public static extern bool DrawEdge(int hdc, ref RECT pRect, uint edge, uint grfFlags);

	[DllImport("user32")]
	public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

	[DllImport("user32")]
	public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

	[DllImport("user32")]
	public static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate, IntPtr hrgnUpdate, uint flags);

	[DllImport("user32")]
	public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref bool pvParam, uint fWinIni);

	[DllImport("user32")]
	public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int pvParam, uint fWinIni);

	[DllImport("gdi32")]
	public static extern int SetROP2(int hDC, int DrawMode);

	[DllImport("gdi32")]
	public static extern int SelectClipRgn(IntPtr hDC, int hRgn);

	[DllImport("gdi32")]
	public static extern int CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

	[DllImport("gdi32")]
	public static extern int CreateDC(string lpszDriver, int lpszDevice, int lpszOutput, int lpInitData);

	[DllImport("gdi32")]
	public static extern bool DeleteDC(int hDC);

	[DllImport("user32")]
	public static extern bool DrawFocusRect(int hDC, ref RECT r);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr GetDC(IntPtr hWnd);

	[DllImport("user32")]
	public static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32")]
	public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32.dll")]
	public static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

	[DllImport("user32.dll")]
	public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

	[DllImport("user32")]
	public static extern IntPtr GetDesktopWindow();

	[DllImport("user32")]
	public static extern IntPtr GetActiveWindow();

	[DllImport("user32")]
	public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

	[DllImport("winmm")]
	public static extern int sndPlaySound(string lpszSoundName, int uFlags);

	[DllImport("user32")]
	public static extern bool ValidateRect(IntPtr hWnd, ref RECT pRect);

	[DllImport("user32")]
	public static extern bool LockWindowUpdate(IntPtr hWndLock);

	[DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern int AnimateWindow(IntPtr hWnd, int dwTime, int dwFlags);

	[DllImport("gdi32", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern int GetDeviceCaps(int hdc, int nIndex);

	[DllImport("kernel32.Dll")]
	public static extern short GetVersionEx(ref OSVERSIONINFO o);

	public static void DrawReversibleDesktopRect(Rectangle rect, int iWidth)
	{
		RECT r = default(RECT);
		r.Left = rect.Left;
		r.Top = rect.Top;
		r.Right = rect.Right;
		r.Bottom = rect.Bottom;
		int hDC = CreateDC("DISPLAY", 0, 0, 0);
		int drawMode = SetROP2(hDC, 10);
		for (int i = 0; i < iWidth; i++)
		{
			DrawFocusRect(hDC, ref r);
			r.Left++;
			r.Top++;
			r.Right--;
			r.Bottom--;
		}
		SetROP2(hDC, drawMode);
		DeleteDC(hDC);
	}

	public static char GetAccessKey(string s)
	{
		int num = s.IndexOf('&', 0);
		while (num >= 0 && num < s.Length - 1)
		{
			num++;
			if (s[num] != '&')
			{
				return s.Substring(num, 1).ToLower()[0];
			}
			num = s.IndexOf('&', num);
		}
		return '\0';
	}

	public static void DrawRectangle(Graphics g, Pen pen, int x, int y, int Width, int Height)
	{
		Width--;
		Height--;
		g.DrawRectangle(pen, x, y, Width, Height);
	}

	public static void DrawRectangle(Graphics g, Pen pen, Rectangle r)
	{
		DrawRectangle(g, pen, r.X, r.Y, r.Width, r.Height);
	}

	internal static void RefreshSettings()
	{
		m_ShowKeyboardCues = SystemInformation.MenuAccessKeysUnderlined;
		m_ShowDropShadow = SystemInformation.IsDropShadowEnabled;
		m_CursorShadow = m_ShowDropShadow;
		m_RightHandedMenus = SystemInformation.RightAlignedMenus;
	}

	internal static void OnDisplayChange()
	{
		int num = CreateDC("DISPLAY", 0, 0, 0);
		ColorDepth = GetColorDepth(num);
		DeleteDC(num);
	}

	internal static int GetColorDepth(int hdc)
	{
		return GetDeviceCaps(hdc, 12);
	}

	[DllImport("user32.dll")]
	public static extern int GetSystemMetrics(int nIndex);

	public static bool IsTerminalSession()
	{
		return GetSystemMetrics(4096) != 0;
	}

	public static string GetClassName(IntPtr handle)
	{
		StringBuilder stringBuilder = new StringBuilder(150);
		if (GetClassName(handle, stringBuilder, stringBuilder.Capacity) != 0)
		{
			return stringBuilder.ToString();
		}
		return "";
	}

	public static int MAKELPARAM(int low, int high)
	{
		return (high << 16) | (low & 0xFFFF);
	}
}
