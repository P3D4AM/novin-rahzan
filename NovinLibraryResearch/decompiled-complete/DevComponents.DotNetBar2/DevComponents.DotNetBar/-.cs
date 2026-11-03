using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Presentation;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Ribbon;
using DevComponents.DotNetBar.TextMarkup;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

internal interface _103D
{
	void છ(Graphics _٥);
}
internal interface _1DDD
{
	void _1DD9(IntPtr _0A65, Point ఋ);

	void _1DDB(IntPtr _0A65, Point ఋ);
}
internal class _0A79 : _0EEE
{
	protected override string ThemeClass => "WINDOW";

	public _0A79(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _༣ _070B, ཉ య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _༣ _070B, ཉ య, ThemeTextFormat _0EE9, bool _0EEA)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, _0EE9, _0EEA);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _༣ _070B, ཉ య, ThemeTextFormat _0EE9)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, _0EE9, drawdisabled: false);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _༣ _070B, ཉ య)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, ThemeTextFormat.Top, drawdisabled: false);
	}

	public void _0EEB(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _༠ _070B, _༢ య, ThemeTextFormat _0EE9, DTTOPTS _0EEC)
	{
		InternalDrawTextEx(_٥, _06EA, _0EE7, _0EE8, _070B, య, _0EE9, _0EEC);
	}

	public IntPtr _0EED(Graphics _٥, _༣ _070B, ཉ య, Rectangle ल)
	{
		return InternalGetThemeBackgroundRegion(_٥, _070B, య, ल);
	}
}
internal abstract class _0EEE : IDisposable
{
	public struct RECT
	{
		private int _0EFA;

		private int _0EFB;

		private int _0EFC;

		private int _0EFD;

		public RECT(Rectangle r)
		{
			_0EFA = r.Left;
			_0EFB = r.Top;
			_0EFC = r.Right;
			_0EFD = r.Bottom;
		}

		public void Offset(int x, int y)
		{
			_0EFA += x;
			_0EFB += y;
		}
	}

	public struct COLORREF
	{
		public uint ColorDWORD;

		public COLORREF(Color color)
		{
			ColorDWORD = (uint)(color.R + (color.G << 8) + (color.B << 16));
		}

		public Color GetColor()
		{
			return Color.FromArgb((int)(0xFF & ColorDWORD), (int)(0xFF00 & ColorDWORD) >> 8, (int)(0xFF0000 & ColorDWORD) >> 16);
		}

		public void SetColor(Color color)
		{
			ColorDWORD = (uint)(color.R + (color.G << 8) + (color.B << 16));
		}
	}

	public struct DTTOPTS
	{
		public int dwSize;

		public int dwFlags;

		public COLORREF crText;

		public COLORREF crBorder;

		public COLORREF crShadow;

		public int iTextShadowType;

		public _0EFE.POINT ptShadowOffset;

		public int iBorderSize;

		public int iFontPropId;

		public int iColorPropId;

		public int iStateId;

		public bool fApplyOverlay;

		public int iGlowSize;

		public int pfnDrawTextCallback;

		public int lParam;
	}

	public struct MARGINS
	{
		private int _0F1C;

		private int _0F1D;

		private int _0F1E;

		private int _0F1F;
	}

	public struct SIZE
	{
		public int Width;

		public int Height;
	}

	public enum DTT_VALIDBITS
	{
		DTT_TEXTCOLOR = 1,
		DTT_BORDERCOLOR = 2,
		DTT_SHADOWCOLOR = 4,
		DTT_SHADOWTYPE = 8,
		DTT_SHADOWOFFSET = 0x10,
		DTT_BORDERSIZE = 0x20,
		DTT_FONTPROP = 0x40,
		DTT_COLORPROP = 0x80,
		DTT_STATEID = 0x100,
		DTT_CALCRECT = 0x200,
		DTT_APPLYOVERLAY = 0x400,
		DTT_GLOWSIZE = 0x800,
		DTT_CALLBACK = 0x1000,
		DTT_COMPOSITED = 0x2000
	}

	public enum ThemeFontId
	{
		TMT_CAPTIONFONT = 801,
		TMT_SMALLCAPTIONFONT,
		TMT_MENUFONT,
		TMT_STATUSFONT,
		TMT_MSGBOXFONT,
		TMT_ICONTITLEFONT
	}

	protected Control ӭ;

	protected IntPtr _0EEF = IntPtr.Zero;

	public static bool _0EF0 = __0EF5();

	protected abstract string ThemeClass { get; }

	public static int _0EF1(Graphics _٥, Rectangle ശ, string _06EA, ThemeTextFormat _0EE9, Color ໆ, Color _0EF2, int _0EF3, int _0EF4)
	{
		int num = 0;
		IntPtr hdc = _٥.GetHdc();
		try
		{
			RECT r = new RECT(ശ);
			return DrawShadowText(hdc, _06EA, (uint)_06EA.Length, ref r, (int)_0EE9, new COLORREF(ໆ), new COLORREF(_0EF2), _0EF3, _0EF4);
		}
		finally
		{
			_٥.ReleaseHdc(hdc);
		}
	}

	[DllImport("ComCtl32.dll", CharSet = CharSet.Auto)]
	public static extern int DrawShadowText(IntPtr hdc, string text, uint textLength, ref RECT r, int flags, COLORREF textColor, COLORREF shadowColor, int shadowXOffset, int shadowYOffset);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern IntPtr OpenThemeData(IntPtr hWnd, string pszClassList);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern IntPtr OpenThemeDataEx(IntPtr hWnd, string pszClassList, int flags);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int CloseThemeData(IntPtr hTheme);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, ref RECT pClipRect);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, IntPtr pCliprect);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int EnableTheming(bool fEnable);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int DrawThemeEdge(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pDestRect, uint uEdge, uint uFlags, ref RECT pContentRect);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int DrawThemeIcon(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, IntPtr himl, int iImageIndex);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int DrawThemeParentBackground(IntPtr hwnd, IntPtr hdc, ref RECT prc);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int DrawThemeText(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string pszText, int iCharCount, int dwTextFlags, int dwTextFlags2, ref RECT pRect);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	public static extern int DrawThemeTextEx(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string pszText, int iCharCount, int dwTextFlags, ref RECT pRect, ref DTTOPTS options);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int EnableThemeDialogTexture(IntPtr hwnd, long dwFlags);

	[DllImport("uxtheme", CharSet = CharSet.Unicode, ExactSpelling = true)]
	public static extern int GetCurrentThemeName(string stringThemeName, int lengthThemeName, string stringColorName, int lengthColorName, string stringSizeName, int lengthSizeName);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern long GetThemeAppProperties();

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int GetThemeBackgroundContentRect(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pBoundingRect, ref RECT pContentRect);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int GetThemeBackgroundExtent(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pContentRect, ref RECT pExtentRect);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int GetThemeBackgroundRegion(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, ref IntPtr pRegion);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int GetThemeBool(IntPtr hTheme, int iPartId, int iStateId, int iPropId, ref bool pfVal);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int GetThemeColor(IntPtr hTheme, int iPartId, int iStateId, int iPropId, ref long pColor);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int GetThemeDocumentationProperty(string pszThemeName, string pszPropertyName, string pszValueBuff, int cchMaxValChars);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int GetThemeEnumValue(IntPtr hTheme, int iPartId, int iStateId, int iPropId, ref int piVal);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int GetThemeFilename(IntPtr hTheme, int iPartId, int iStateId, int iPropId, ref string pszThemeFilename, int cchMaxBuffChars);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int GetThemeInt(IntPtr hTheme, int iPartId, int iStateId, int iPropId, ref int piVal);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	private static extern int GetThemeMargins(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, int iPropId, ref RECT prc, ref MARGINS pMargins);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern bool IsThemeActive();

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	internal static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int SetWindowTheme(IntPtr hwnd, IntPtr pszSubAppName, string pszSubIdList);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int SetWindowTheme(IntPtr hwnd, IntPtr pszSubAppName, IntPtr pszSubIdList);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern bool IsThemePartDefined(IntPtr hTheme, int iPartId, int iStateId);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int GetThemePartSize(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT prc, int eSize, ref SIZE psz);

	[DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
	protected static extern int GetThemePartSize(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, IntPtr prc, int eSize, ref SIZE psz);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	protected static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

	public _0EEE(Control parent)
	{
		ӭ = parent;
		_0EEF = OpenThemeData(parent.Handle, ThemeClass);
	}

	~_0EEE()
	{
		if (_0EEF != IntPtr.Zero)
		{
			Dispose();
		}
	}

	private static bool __0EF5()
	{
		if (!BarFunctions.ThemedOS)
		{
			return false;
		}
		return IsThemeActive();
	}

	public static void _0EF6()
	{
		_0EF0 = __0EF5();
	}

	public static int _0EF7(Control _0EF8, string _0EF9)
	{
		return SetWindowTheme(_0EF8.Handle, IntPtr.Zero, _0EF9);
	}

	protected virtual void InternalDrawBackground(Graphics g, _༠ part, _༢ state, Rectangle r)
	{
		RECT pRect = new RECT(r);
		IntPtr hdc = g.GetHdc();
		try
		{
			DrawThemeBackground(_0EEF, hdc, part._༡, state._༡, ref pRect, IntPtr.Zero);
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
	}

	protected virtual void InternalDrawBackground(Graphics g, _༠ part, _༢ state, Rectangle r, Rectangle clip)
	{
		RECT pRect = new RECT(r);
		RECT pClipRect = new RECT(clip);
		IntPtr hdc = g.GetHdc();
		try
		{
			DrawThemeBackground(_0EEF, hdc, part._༡, state._༡, ref pRect, ref pClipRect);
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
	}

	protected virtual IntPtr InternalGetThemeBackgroundRegion(Graphics g, _༠ part, _༢ state, Rectangle r)
	{
		RECT pRect = new RECT(r);
		IntPtr hdc = g.GetHdc();
		IntPtr pRegion = IntPtr.Zero;
		try
		{
			GetThemeBackgroundRegion(_0EEF, hdc, part._༡, state._༡, ref pRect, ref pRegion);
			return pRegion;
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
	}

	protected virtual void InternalDrawText(Graphics g, string text, Font font, Rectangle layoutRect, _༠ part, _༢ state, ThemeTextFormat format, bool drawdisabled)
	{
		RECT pRect = new RECT(layoutRect);
		IntPtr hdc = g.GetHdc();
		IntPtr intPtr = font.ToHfont();
		IntPtr hgdiobj = SelectObject(hdc, intPtr);
		DrawThemeText(_0EEF, hdc, part._༡, state._༡, text, text.Length, (int)format, drawdisabled ? 1 : 0, ref pRect);
		SelectObject(hdc, hgdiobj);
		_0EFE.DeleteObject(intPtr);
		g.ReleaseHdc(hdc);
	}

	protected virtual void InternalDrawTextEx(Graphics g, string text, Font font, Rectangle layoutRect, _༠ part, _༢ state, ThemeTextFormat format, DTTOPTS options)
	{
		RECT pRect = new RECT(layoutRect);
		IntPtr hdc = g.GetHdc();
		IntPtr intPtr = font.ToHfont();
		IntPtr hgdiobj = SelectObject(hdc, intPtr);
		options.dwSize = Marshal.SizeOf((object)options);
		DrawThemeTextEx(_0EEF, hdc, part._༡, state._༡, text, text.Length, (int)format, ref pRect, ref options);
		SelectObject(hdc, hgdiobj);
		_0EFE.DeleteObject(intPtr);
		g.ReleaseHdc(hdc);
	}

	public virtual bool IsPartDefined(_༠ part, _༢ state)
	{
		return IsThemePartDefined(_0EEF, part._༡, state._༡);
	}

	public void Dispose()
	{
		if (_0EEF != IntPtr.Zero)
		{
			CloseThemeData(_0EEF);
			_0EEF = IntPtr.Zero;
		}
	}

	public virtual Size ThemeMinSize(Graphics g, _༠ part, _༢ state)
	{
		SIZE psz = default(SIZE);
		IntPtr hdc = g.GetHdc();
		try
		{
			GetThemePartSize(_0EEF, hdc, part._༡, state._༡, IntPtr.Zero, 0, ref psz);
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
		return new Size(psz.Width, psz.Height);
	}

	public virtual Size ThemeTrueSize(Graphics g, _༠ part, _༢ state)
	{
		SIZE psz = default(SIZE);
		IntPtr hdc = g.GetHdc();
		try
		{
			GetThemePartSize(_0EEF, hdc, part._༡, state._༡, IntPtr.Zero, 1, ref psz);
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
		return new Size(psz.Width, psz.Height);
	}

	public virtual Size ThemeDrawSize(Graphics g, _༠ part, _༢ state)
	{
		SIZE psz = default(SIZE);
		IntPtr hdc = g.GetHdc();
		try
		{
			GetThemePartSize(_0EEF, hdc, part._༡, state._༡, IntPtr.Zero, 2, ref psz);
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
		return new Size(psz.Width, psz.Height);
	}
}
internal class _0EFE
{
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

	public delegate uint WindowProcDelegate(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam);

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

		public static RECT FromXYWH(int x, int y, int width, int height)
		{
			return new RECT(x, y, x + width, y + height);
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

	public enum SBOrientation
	{
		SB_HORZ,
		SB_VERT,
		SB_CTL,
		SB_BOTH
	}

	public struct SCROLLINFO
	{
		public int cbSize;

		public _0F15 fMask;

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

	[Flags]
	internal enum _0F15
	{
		_0F16 = 1,
		_0F17 = 2,
		_0F18 = 4,
		_0F19 = 8,
		_0F1A = 0x10,
		_0F1B = 0x17
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

	public delegate void TimerDelegate(IntPtr hWnd, IntPtr uMsg, IntPtr idEvent, int dwTime);

	public struct WINDOWPLACEMENT
	{
		public int length;

		public int flags;

		public int showCmd;

		public POINT ptMinPosition;

		public POINT ptMaxPosition;

		public RECT rcNormalPosition;
	}

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
		WM_APPCOMMAND = 793,
		WM_USER = 1024,
		WM_REFLECT = 8192,
		WM_REFLECTCOMMAND = 8465,
		WM_DPICHANGED = 736,
		WM_SETREDRAW = 11,
		WM_MOVE = 3
	}

	public struct MARGINS
	{
		public int cxLeftWidth;

		public int cxRightWidth;

		public int cyTopHeight;

		public int cyBottomHeight;
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

	public struct MINMAXINFO
	{
		public Point reserved;

		public Size maxSize;

		public Point maxPosition;

		public Size minTrackSize;

		public Size maxTrackSize;
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

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class STYLESTRUCT
	{
		public uint styleOld;

		public uint styleNew;
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

	private static int _0EFF = 61440;

	public static bool ষ
	{
		get
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Invalid comparison between Unknown and I4
			if (Environment.OSVersion.Version.Major < 6)
			{
				return false;
			}
			if (DwmIsCompositionEnabled())
			{
				return (int)Application.VisualStyleState == 3;
			}
			return false;
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

	public static char ༀ(uint _0F01)
	{
		StringBuilder stringBuilder = new StringBuilder(2);
		byte[] array = new byte[256];
		_089F.GetKeyboardState(array);
		int num = ToAscii(_0F01, 0u, array, stringBuilder, 0u);
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

	[DllImport("user32")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

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

	public static IntPtr _0F02(IntPtr _0A65, int _0F03)
	{
		if (IntPtr.Size == 8)
		{
			return GetWindowLongPtr64(_0A65, _0F03);
		}
		return new IntPtr(GetWindowLong32(_0A65, _0F03));
	}

	[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
	public static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
	public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	public static IntPtr _0F04(IntPtr _0F05, int _0F03, IntPtr _0F06)
	{
		if (IntPtr.Size == 8)
		{
			return SetWindowLongPtr64(_0F05, _0F03, _0F06);
		}
		return new IntPtr(SetWindowLongPtr32(_0F05, _0F03, _0F06.ToInt32()));
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

	internal static bool _0F07(IntPtr _0F05, int _0F08, int _0F09)
	{
		int num = _0F02(_0F05, -16).ToInt32();
		int num2 = (num & ~_0F08) | _0F09;
		if (num == num2)
		{
			return false;
		}
		SetWindowLong(_0F05, -16, num2);
		return true;
	}

	[DllImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	public static extern int ScrollWindowEx(HandleRef hWnd, int nXAmount, int nYAmount, ref RECT rectScrollRegion, ref RECT rectClip, IntPtr hrgnUpdate, ref RECT prcUpdate, int flags);

	[DllImport("Gdi32.dll")]
	public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

	public static short _0F0A(IntPtr _੧)
	{
		return (short)((short)_0F0D(_੧) & ~_0EFF);
	}

	public static int _0F0B(int _0F0C)
	{
		return (short)(_0F0C & 0xFFFF);
	}

	public static int _0F0D(int _0F0C)
	{
		return (_0F0C >> 16) & 0xFFFF;
	}

	public static int _0F0B(IntPtr _0F0C)
	{
		return (short)(long)_0F0C;
	}

	public static int _0F0D(IntPtr _0F0C)
	{
		return (short)((long)_0F0C >> 16);
	}

	public static IntPtr _0F0E(int _0F0F, int _0F10)
	{
		byte[] bytes = BitConverter.GetBytes(_0F0F);
		byte[] bytes2 = BitConverter.GetBytes(_0F10);
		byte[] value = new byte[4]
		{
			bytes[0],
			bytes[1],
			bytes2[0],
			bytes2[1]
		};
		return new IntPtr(BitConverter.ToInt32(value, 0));
	}

	public static int _0F11(IntPtr _0F12)
	{
		int result = 0;
		try
		{
			result = (int)(long)_0F12;
		}
		catch
		{
		}
		return result;
	}

	[DllImport("dwmapi.dll")]
	public static extern int DwmSetWindowAttribute(IntPtr hWnd, int dwAttribute, ref int attributeValue, int sizeOfValueRetrived);

	[DllImport("dwmapi.dll")]
	public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, ref int pvAttribute, int sizeOfValueRetrived);

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

	public static void _0F13(IntPtr ह, int _0F14)
	{
		MARGINS pMargins = default(MARGINS);
		pMargins.cxLeftWidth = 0;
		pMargins.cxRightWidth = 0;
		pMargins.cyTopHeight = _0F14;
		pMargins.cyBottomHeight = 0;
		DwmExtendFrameIntoClientArea(ह, ref pMargins);
	}
}
internal class _0A7A : _0EEE
{
	protected override string ThemeClass => "REBAR";

	public _0A7A(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, ཤ _070B, ཫ య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _0A7B : _0EEE
{
	protected override string ThemeClass => "Toolbar";

	public _0A7B(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _0F6F _070B, _0F76 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _0F6F _070B, _0F76 య, ThemeTextFormat _0EE9, bool _0EEA)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, _0EE9, _0EEA);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _0F6F _070B, _0F76 య, ThemeTextFormat _0EE9)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, _0EE9, drawdisabled: false);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _0F6F _070B, _0F76 య)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, ThemeTextFormat.Top, drawdisabled: false);
	}
}
internal class _0F6F : _༠
{
	public static _0F6F _0F70 = new _0F6F(1);

	public static _0F6F _0F71 = new _0F6F(2);

	public static _0F6F _0F72 = new _0F6F(3);

	public static _0F6F _0F73 = new _0F6F(4);

	public static _0F6F _0F74 = new _0F6F(5);

	public static _0F6F _0F75 = new _0F6F(6);

	protected _0F6F(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _0F76 : _༢
{
	public static _0F76 ཊ = new _0F76(1);

	public static _0F76 _0F77 = new _0F76(2);

	public static _0F76 _0F78 = new _0F76(3);

	public static _0F76 ड़ = new _0F76(4);

	public static _0F76 _0F79 = new _0F76(5);

	public static _0F76 _0F7A = new _0F76(6);

	protected _0F76(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _0A7C : _0EEE
{
	protected override string ThemeClass => "HEADER";

	public _0A7C(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _0F7B _070B, _0F80 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _0F7B _070B, _0F80 య, ThemeTextFormat _0EE9, bool _0EEA)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, _0EE9, _0EEA);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _0F7B _070B, _0F80 య, ThemeTextFormat _0EE9)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, _0EE9, drawdisabled: false);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _0F7B _070B, _0F80 య)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, ThemeTextFormat.Top, drawdisabled: false);
	}
}
internal class _0F7B : _༠
{
	public static _0F7B _0F7C = new _0F7B(1);

	public static _0F7B _0F7D = new _0F7B(2);

	public static _0F7B _0F7E = new _0F7B(3);

	public static _0F7B _0F7F = new _0F7B(4);

	protected _0F7B(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _0F80 : _༢
{
	public static _0F80 _0F81 = new _0F80(1);

	public static _0F80 _0F82 = new _0F80(2);

	public static _0F80 _0F83 = new _0F80(3);

	public static _0F80 _0F84 = new _0F80(1);

	public static _0F80 _0F85 = new _0F80(2);

	protected _0F80(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _0A7D : _0EEE
{
	protected override string ThemeClass => "SCROLLBAR";

	public _0A7D(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _0F86 _070B, _0F90 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _0F86 : _༠
{
	public static _0F86 _0F87 = new _0F86(1);

	public static _0F86 ྈ = new _0F86(2);

	public static _0F86 ྉ = new _0F86(3);

	public static _0F86 ྊ = new _0F86(4);

	public static _0F86 ྋ = new _0F86(5);

	public static _0F86 ྌ = new _0F86(6);

	public static _0F86 _0F8D = new _0F86(7);

	public static _0F86 _0F8E = new _0F86(8);

	public static _0F86 ཧ = new _0F86(9);

	public static _0F86 _0F8F = new _0F86(10);

	protected _0F86(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _0F90 : _༢
{
	public static _0F90 _0F91 = new _0F90(1);

	public static _0F90 _0F92 = new _0F90(2);

	public static _0F90 _0F93 = new _0F90(3);

	public static _0F90 _0F94 = new _0F90(4);

	public static _0F90 _0F95 = new _0F90(5);

	public static _0F90 _0F96 = new _0F90(6);

	public static _0F90 _0F97 = new _0F90(7);

	public static _0F90 _0F98 = new _0F90(8);

	public static _0F90 _0F99 = new _0F90(9);

	public static _0F90 _0F9A = new _0F90(10);

	public static _0F90 _0F9B = new _0F90(11);

	public static _0F90 _0F9C = new _0F90(12);

	public static _0F90 _0F9D = new _0F90(13);

	public static _0F90 _0F9E = new _0F90(14);

	public static _0F90 _0F9F = new _0F90(15);

	public static _0F90 _0FA0 = new _0F90(16);

	public static _0F90 _0FA1 = new _0F90(0);

	public static _0F90 _0FA2 = new _0F90(0);

	public static _0F90 _0FA3 = new _0F90(1);

	public static _0F90 _0FA4 = new _0F90(2);

	public static _0F90 _0FA5 = new _0F90(3);

	public static _0F90 _0FA6 = new _0F90(4);

	public static _0F90 _0FA7 = new _0F90(1);

	public static _0F90 _0FA8 = new _0F90(2);

	public static _0F90 _0FA9 = new _0F90(3);

	public static _0F90 _0FAA = new _0F90(4);

	public static _0F90 _0FAB = new _0F90(1);

	public static _0F90 _0FAC = new _0F90(2);

	protected _0F90(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _0A7E : _0EEE
{
	protected override string ThemeClass => "EXPLORERBAR";

	public _0A7E(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _0FAD _070B, _0FBA య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _0FAD _070B, _0FBA య, ThemeTextFormat _0EE9, bool _0EEA)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, _0EE9, _0EEA);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _0FAD _070B, _0FBA య, ThemeTextFormat _0EE9)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, _0EE9, drawdisabled: false);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _0FAD _070B, _0FBA య)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, ThemeTextFormat.Top, drawdisabled: false);
	}
}
internal class _0FAD : _༠
{
	public static _0FAD _0FAE = new _0FAD(1);

	public static _0FAD _0FAF = new _0FAD(2);

	public static _0FAD _0FB0 = new _0FAD(3);

	public static _0FAD _0FB1 = new _0FAD(4);

	public static _0FAD _0FB2 = new _0FAD(5);

	public static _0FAD _0FB3 = new _0FAD(6);

	public static _0FAD _0FB4 = new _0FAD(7);

	public static _0FAD _0FB5 = new _0FAD(8);

	public static _0FAD _0FB6 = new _0FAD(9);

	public static _0FAD _0FB7 = new _0FAD(10);

	public static _0FAD _0FB8 = new _0FAD(11);

	public static _0FAD _0FB9 = new _0FAD(12);

	protected _0FAD(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _0FBA : _༢
{
	public static _0FBA _0FBB = new _0FBA(1);

	public static _0FBA _0FBC = new _0FBA(1);

	public static _0FBA _0FBD = new _0FBA(2);

	public static _0FBA _0FBE = new _0FBA(3);

	public static _0FBA _0FBF = new _0FBA(1);

	public static _0FBA _0FC0 = new _0FBA(2);

	public static _0FBA _0FC1 = new _0FBA(3);

	public static _0FBA _0FC2 = new _0FBA(4);

	public static _0FBA _0FC3 = new _0FBA(5);

	public static _0FBA _0FC4 = new _0FBA(6);

	public static _0FBA _0FC5 = new _0FBA(1);

	public static _0FBA _0FC6 = new _0FBA(2);

	public static _0FBA _0FC7 = new _0FBA(3);

	public static _0FBA _0FC8 = new _0FBA(1);

	public static _0FBA _0FC9 = new _0FBA(1);

	public static _0FBA _0FCA = new _0FBA(2);

	public static _0FBA _0FCB = new _0FBA(3);

	public static _0FBA _0FCC = new _0FBA(1);

	public static _0FBA _0FCD = new _0FBA(2);

	public static _0FBA _0FCE = new _0FBA(3);

	public static _0FBA _0FCF = new _0FBA(1);

	public static _0FBA _0FD0 = new _0FBA(1);

	public static _0FBA _0FD1 = new _0FBA(1);

	public static _0FBA _0FD2 = new _0FBA(2);

	public static _0FBA _0FD3 = new _0FBA(3);

	public static _0FBA _0FD4 = new _0FBA(1);

	public static _0FBA _0FD5 = new _0FBA(2);

	public static _0FBA _0FD6 = new _0FBA(3);

	public static _0FBA _0FD7 = new _0FBA(1);

	protected _0FBA(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _0A7F : _0EEE
{
	protected override string ThemeClass => "PROGRESS";

	public _0A7F(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _0FD8 _070B, _0FDD య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _0FD8 : _༠
{
	public static _0FD8 _0FD9 = new _0FD8(1);

	public static _0FD8 _0FDA = new _0FD8(2);

	public static _0FD8 _0FDB = new _0FD8(3);

	public static _0FD8 _0FDC = new _0FD8(4);

	protected _0FD8(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _0FDD : _༢
{
	public static _0FDD ཊ = new _0FDD(1);

	protected _0FDD(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _0A80 : _0EEE
{
	protected override string ThemeClass => "Button";

	public _0A80(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _0FDE _070B, _0FE4 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _0FDE _070B, _0FE4 య, ThemeTextFormat _0EE9, bool _0EEA)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, _0EE9, _0EEA);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _0FDE _070B, _0FE4 య, ThemeTextFormat _0EE9)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, _0EE9, drawdisabled: false);
	}

	public void _0EE6(Graphics _٥, string _06EA, Font _0EE7, Rectangle _0EE8, _0FDE _070B, _0FE4 య)
	{
		InternalDrawText(_٥, _06EA, _0EE7, _0EE8, _070B, య, ThemeTextFormat.Top, drawdisabled: false);
	}
}
internal class _0FDE : _༠
{
	public static _0FDE _0FDF = new _0FDE(1);

	public static _0FDE _0FE0 = new _0FDE(2);

	public static _0FDE _0FE1 = new _0FDE(3);

	public static _0FDE _0FE2 = new _0FDE(4);

	public static _0FDE _0FE3 = new _0FDE(5);

	protected _0FDE(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _0FE4 : _༢
{
	public static _0FE4 _0FE5 = new _0FE4(1);

	public static _0FE4 _0FE6 = new _0FE4(2);

	public static _0FE4 _0FE7 = new _0FE4(3);

	public static _0FE4 _0FE8 = new _0FE4(4);

	public static _0FE4 _0FE9 = new _0FE4(5);

	public static _0FE4 _0FEA = new _0FE4(1);

	public static _0FE4 _0FEB = new _0FE4(2);

	public static _0FE4 _0FEC = new _0FE4(3);

	public static _0FE4 _0FED = new _0FE4(4);

	public static _0FE4 _0FEE = new _0FE4(5);

	public static _0FE4 _0FEF = new _0FE4(6);

	public static _0FE4 _0FF0 = new _0FE4(7);

	public static _0FE4 _0FF1 = new _0FE4(8);

	public static _0FE4 _0FF2 = new _0FE4(1);

	public static _0FE4 _0FF3 = new _0FE4(2);

	public static _0FE4 _0FF4 = new _0FE4(3);

	public static _0FE4 _0FF5 = new _0FE4(4);

	public static _0FE4 _0FF6 = new _0FE4(5);

	public static _0FE4 _0FF7 = new _0FE4(6);

	public static _0FE4 _0FF8 = new _0FE4(7);

	public static _0FE4 _0FF9 = new _0FE4(8);

	public static _0FE4 _0FFA = new _0FE4(9);

	public static _0FE4 _0FFB = new _0FE4(10);

	public static _0FE4 _0FFC = new _0FE4(11);

	public static _0FE4 _0FFD = new _0FE4(12);

	public static _0FE4 _0FFE = new _0FE4(1);

	public static _0FE4 _0FFF = new _0FE4(2);

	protected _0FE4(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _103E : Control
{
	private Control ဿ;

	private _103D ਜ;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			createParams.ExStyle |= 0x20;
			return createParams;
		}
	}

	public _103E(_103D renderer)
	{
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)1, false);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).BackColor = Color.Transparent;
		ਜ = renderer;
	}

	protected override void Dispose(bool disposing)
	{
		if (ဿ != null)
		{
			ဿ.Resize -= _၀;
			ဿ = null;
		}
		((Control)this).Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (ਜ != null)
		{
			ਜ.છ(e.Graphics);
		}
		((Control)this).OnPaint(e);
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (ဿ != null)
		{
			ဿ.Resize -= _၀;
		}
		ဿ = ((Control)this).Parent;
		if (ဿ != null)
		{
			ဿ.Resize += _၀;
		}
		((Control)this).OnParentChanged(e);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	private void _၀(object _٠, EventArgs ؿ)
	{
		if (ဿ != null)
		{
			((Control)this).Bounds = new Rectangle(0, 0, ဿ.Width, ဿ.Height);
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 132)
		{
			((Message)(ref m)).Result = new IntPtr(-1);
		}
		else
		{
			((Control)this).WndProc(ref m);
		}
	}
}
[SuppressUnmanagedCodeSecurity]
internal class _089F
{
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

	public struct TRACKMOUSEEVENT
	{
		public int cbSize;

		public uint dwFlags;

		public int dwHoverTime;

		public IntPtr hwndTrack;
	}

	public struct SIZE
	{
		public int cx;

		public int cy;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BLENDFUNCTION
	{
		public byte BlendOp;

		public byte BlendFlags;

		public byte SourceConstantAlpha;

		public byte AlphaFormat;
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

	[Flags]
	public enum WindowFromPointFlags
	{
		CWP_ALL = 0,
		CWP_SKIPINVISIBLE = 1,
		CWP_SKIPDISABLED = 2,
		CWP_SKIPTRANSPARENT = 4
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

	public const uint ࢧ = 4098u;

	public const uint ࢨ = 4114u;

	public const uint ࢩ = 4106u;

	public const uint ࢪ = 4132u;

	public const uint ࢫ = 4122u;

	public const uint ࢬ = 27u;

	public const uint ࢭ = 5u;

	public const int ࢮ = 1;

	public const int ࢯ = 2;

	private const int ࢰ = 10;

	public const int ࢱ = 1;

	public const int ࢲ = 2;

	public const uint ࢳ = 1u;

	public const uint ࢴ = 2u;

	public const uint _08B5 = 4u;

	public const uint ࢶ = 8u;

	public const uint ࢷ = 3u;

	public const uint ࢸ = 12u;

	public const uint ࢹ = 5u;

	public const uint ࢺ = 10u;

	public const uint ࢻ = 5u;

	public const uint ࢼ = 10u;

	public const uint ࢽ = 6u;

	public const uint ࢾ = 9u;

	public const uint ࢿ = 1u;

	public const uint ࣀ = 2u;

	public const uint ࣁ = 4u;

	public const uint ࣂ = 8u;

	public const uint ࣃ = 3u;

	public const uint ࣄ = 6u;

	public const uint ࣅ = 9u;

	public const uint ࣆ = 12u;

	public const uint ࣇ = 15u;

	public const uint _08C8 = 16u;

	public const uint _08C9 = 22u;

	public const uint _08CA = 19u;

	public const uint _08CB = 25u;

	public const uint _08CC = 28u;

	public const uint _08CD = 2048u;

	public const uint _08CE = 4096u;

	public const uint _08CF = 8192u;

	public const uint _08D0 = 16384u;

	public const uint _08D1 = 32768u;

	public const uint _08D2 = 1u;

	public const uint _08D3 = 2u;

	public const uint _08D4 = 16u;

	public const uint _08D5 = 1073741824u;

	public const uint _08D6 = 2147483648u;

	public const uint _08D7 = uint.MaxValue;

	public const long _08D8 = 28L;

	public const int _08D9 = 513;

	public const int _08DA = 514;

	public const int _08DB = 515;

	public const int _08DC = 161;

	public const int _08DD = 169;

	public const int _08DE = 134;

	public const int _08DF = 516;

	public const int _08E0 = 517;

	public const int _08E1 = 519;

	public const int _08E2 = 164;

	public const int _08E3 = 167;

	public const int _08E4 = 123;

	public const int _08E5 = 1024;

	public const int _08E6 = 512;

	public const int _08E7 = 675;

	public const int _08E8 = 0;

	public const int _08E9 = 1;

	public const int _08EA = -1;

	public const int _08EB = -2;

	public const int _08EC = 2;

	public const int _08ED = 1;

	public const int _08EE = 16;

	public const int _08EF = 4;

	public const int _08F0 = 64;

	public const int _08F1 = 128;

	public const int _08F2 = 32;

	public const int _08F3 = 512;

	public const int _08F4 = 1024;

	public const int _08F5 = 7;

	public const int _08F6 = 8;

	public const int _08F7 = 24;

	public const int _08F8 = 11;

	public const int _08F9 = 15;

	public const int _08FA = 522;

	public const int _08FB = 276;

	public const int _08FC = 277;

	public const int _08FD = 260;

	public const int _08FE = 261;

	public const int _08FF = 6;

	public const int _0900 = 0;

	public const int _0901 = 1;

	public const int _0902 = 2;

	public const int _0903 = 1;

	public const int ऄ = 2;

	public const int अ = 4;

	public const int आ = 8;

	public const int इ = 16;

	public const int ई = 65536;

	public const int उ = 131072;

	public const int ऊ = 262144;

	public const int ऋ = 524288;

	public const int ऌ = 274;

	public const int ऍ = 61536;

	public const int ऎ = 61824;

	public const int ए = 61504;

	public const int ऐ = 61520;

	public const int ऑ = 61696;

	public const int ऒ = 61728;

	public const int ओ = 61472;

	public const int औ = 61488;

	public const int क = 61456;

	public const int ख = 61440;

	public const int ग = 33;

	public const int घ = 3;

	public const int ङ = 4;

	public const uint च = 2147483648u;

	public const uint छ = 67108864u;

	public const uint ज = 33554432u;

	public const uint झ = 8u;

	public const uint ञ = 128u;

	public const int ट = 794;

	public const int ठ = 792;

	public const int ड = 126;

	public const int ढ = 12;

	public const int ण = 4096;

	private static Color त = Color.Empty;

	internal static bool थ = false;

	private static bool द = true;

	private static bool ध = true;

	public static int न = 0;

	private static bool ऩ = true;

	private static bool प = false;

	public static int ࢠ
	{
		get
		{
			int pvParam = 0;
			SystemParametersInfo(5u, 0u, ref pvParam, 0u);
			return pvParam;
		}
	}

	public static ePopupAnimation ࢡ
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

	public static bool ࢢ => द;

	public static bool ࢣ => ध;

	public static bool ࢤ
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

	public static bool ࢥ => ऩ;

	public static bool ࢦ => प;

	internal static bool फ()
	{
		//Discarded unreachable code: IL_004b, IL_0051, IL_006e, IL_01b5, IL_01ba, IL_01c6
		Color control = SystemColors.Control;
		Color color = control;
		color.ToArgb();
		control.ToArgb();
		थ = true;
		त = Color.Black;
		return false;
	}

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

	[DllImport("user32.dll")]
	public static extern IntPtr ChildWindowFromPointEx(IntPtr hWndParent, POINT pt, uint uFlags);

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
	public static extern bool DrawCaption(IntPtr hwnd, IntPtr hdc, ref RECT lprc, ऽ uFlags);

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

	[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

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

	public static void ब(Rectangle भ, int म)
	{
		RECT r = default(RECT);
		r.Left = भ.Left;
		r.Top = भ.Top;
		r.Right = भ.Right;
		r.Bottom = भ.Bottom;
		int hDC = CreateDC("DISPLAY", 0, 0, 0);
		int drawMode = SetROP2(hDC, 10);
		for (int i = 0; i < म; i++)
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

	public static char य(string ٮ)
	{
		int num = ٮ.IndexOf('&', 0);
		while (num >= 0 && num < ٮ.Length - 1)
		{
			num++;
			if (ٮ[num] != '&')
			{
				return ٮ.Substring(num, 1).ToLower()[0];
			}
			num = ٮ.IndexOf('&', num);
		}
		return '\0';
	}

	public static void र(Graphics _٥, Pen ऱ, int و, int ى, int ܐ, int _0711)
	{
		ܐ--;
		_0711--;
		_٥.DrawRectangle(ऱ, و, ى, ܐ, _0711);
	}

	public static void र(Graphics _٥, Pen ऱ, Rectangle ल)
	{
		र(_٥, ऱ, ल.X, ल.Y, ल.Width, ल.Height);
	}

	internal static void ळ()
	{
		द = SystemInformation.MenuAccessKeysUnderlined;
		ध = SystemInformation.IsDropShadowEnabled;
		ऩ = ध;
		प = SystemInformation.RightAlignedMenus;
	}

	internal static void ऴ()
	{
		int num = CreateDC("DISPLAY", 0, 0, 0);
		न = व(num);
		DeleteDC(num);
	}

	internal static int व(int श)
	{
		return GetDeviceCaps(श, 12);
	}

	[DllImport("user32.dll")]
	public static extern int GetSystemMetrics(int nIndex);

	public static bool ष()
	{
		return 0 != GetSystemMetrics(4096);
	}

	public static string स(IntPtr ह)
	{
		StringBuilder stringBuilder = new StringBuilder(150);
		if (GetClassName(ह, stringBuilder, stringBuilder.Capacity) != 0)
		{
			return stringBuilder.ToString();
		}
		return "";
	}

	public static int _093A(int _093B, int _093C)
	{
		return (_093C << 16) | (_093B & 0xFFFF);
	}
}
internal enum _17B6
{
	_0749,
	ሪ,
	_074A,
	ራ
}
internal enum _0883
{
	_0884,
	_0885,
	_0886,
	_0887
}
internal class _192E : _192F
{
	protected virtual Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
	{
		return _1932._1933(button, pa);
	}

	protected virtual Rectangle GetDisplayRectangle(ButtonItem button)
	{
		return button.DisplayRectangle;
	}

	public override void PaintButton(ButtonItem button, ItemPaintArgs pa)
	{
		bool flag = IsOnMenu(button, pa);
		if (flag && button.Parent is ItemContainer)
		{
			flag = false;
		}
		bool isOnMenuBar = pa.IsOnMenuBar;
		bool isThemed = button.IsThemed;
		Graphics graphics = pa.Graphics;
		Region clip = graphics.Clip;
		Rectangle displayRectangle = GetDisplayRectangle(button);
		try
		{
			graphics.SetClip(displayRectangle, (CombineMode)1);
			if (!pa.IsOnMenu && !isOnMenuBar && isThemed)
			{
				if (pa.ContainerControl is ButtonX)
				{
					_1934._1935(button, pa);
				}
				else
				{
					ThemedButtonItemPainter.PaintButton(button, pa);
				}
				return;
			}
			Color textColor = GetTextColor(button, pa);
			ᇎ ᇎ2 = button._0EC5(textColor);
			Rectangle imageRectangle = GetImageRectangle(button, pa, ᇎ2);
			PaintButtonBackground(button, pa, ᇎ2);
			Rectangle customizeMenuCheckRectangle = GetCustomizeMenuCheckRectangle(button, pa);
			Rectangle checkRectangle = GetCheckRectangle(button, pa, ᇎ2);
			Rectangle mouseOverRectangle = GetMouseOverRectangle(button, pa, ᇎ2);
			bool flag2 = button.IsMouseOver;
			if (button.Expanded && !flag)
			{
				flag2 = false;
			}
			if (flag && button.Expanded && pa.ContainerControl != null && pa.ContainerControl.Parent != null && !pa.ContainerControl.Parent.Bounds.Contains(Control.MousePosition))
			{
				flag2 = true;
			}
			if (button.HotTrackingStyle != eHotTrackingStyle.None && (flag2 || (button.IsMouseDown && !button.DesignMode)))
			{
				PaintButtonMouseOver(button, pa, ᇎ2, mouseOverRectangle);
			}
			if (flag && button.IsOnCustomizeMenu && button.Visible && !button.SystemItem)
			{
				PaintCustomizeCheck(button, pa, customizeMenuCheckRectangle);
			}
			if (button.Checked && !button.IsOnCustomizeMenu && (button.GetEnabled(pa.ContainerControl) || flag))
			{
				PaintButtonCheck(button, pa, ᇎ2, checkRectangle);
			}
			if ((ᇎ2 != null || !string.IsNullOrEmpty(button.SymbolRealized)) && button.ButtonStyle != eButtonStyle.TextOnlyAlways)
			{
				PaintButtonImage(button, pa, ᇎ2, imageRectangle);
			}
			if (!(button is ApplicationButton) || button.EffectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(button.EffectiveStyle))
			{
				PaintButtonText(button, pa, textColor, ᇎ2);
			}
			PaintExpandButton(button, pa);
			if (button.Focused && button.DesignMode)
			{
				Rectangle ल = displayRectangle;
				ल.Inflate(-1, -1);
				_1A6B._1A6C(graphics, ल, pa.Colors.ItemDesignTimeBorder);
			}
			ᇎ2?.Dispose();
		}
		finally
		{
			if (clip != null)
			{
				graphics.Clip = clip;
				clip.Dispose();
			}
			else
			{
				graphics.ResetClip();
			}
		}
	}

	protected virtual bool IsOnMenu(ButtonItem button, ItemPaintArgs pa)
	{
		bool flag = pa.IsOnMenu;
		if (flag && button.Parent is ItemContainer)
		{
			flag = false;
		}
		return flag;
	}

	public override void PaintButtonImage(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle imagebounds)
	{
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		bool flag = IsOnMenu(button, pa);
		if (imagebounds.Width <= 0 || imagebounds.Height <= 0)
		{
			return;
		}
		if (!string.IsNullOrEmpty(button.SymbolRealized))
		{
			Font font = Symbols.GetFont(button.SymbolSize, button.SymbolSet);
			Color color = ((button.SymbolColor.IsEmpty || !button.Enabled) ? GetTextColor(button, pa) : button.SymbolColor);
			if (flag)
			{
				TextDrawing.DrawStringLegacy(pa.Graphics, button.SymbolRealized, font, color, new Rectangle(imagebounds.X + imagebounds.Width / 2, imagebounds.Y, 0, 0), eTextFormat.HorizontalCenter);
			}
			else
			{
				TextDrawing.DrawStringLegacy(pa.Graphics, button.SymbolRealized, font, color, new Rectangle(imagebounds.X, imagebounds.Y - ((button.ImagePosition != eImagePosition.Bottom && !(Dpi.Factor.Width > 1f) && pa.ໞ) ? 2 : 0), 0, 0), eTextFormat.Default);
			}
			return;
		}
		if (button.EnableImageAnimation && button._๗)
		{
			if (!button._๘)
			{
				button.ເ(image.ᇑ);
			}
			ImageAnimator.UpdateFrames();
		}
		if (flag)
		{
			image.ᇗ(pa.Graphics, imagebounds);
		}
		else if (!button.IsMouseOver && button.HotTrackingStyle == eHotTrackingStyle.Color)
		{
			ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
			{
				new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
				new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
				new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
				new float[5] { 0f, 0f, 0f, 1f, 0f },
				new float[5] { 0.2f, 0.2f, 0.2f, 0f, 1f }
			});
			ImageAttributes val = new ImageAttributes();
			val.SetColorMatrix(colorMatrix);
			image.ᇗ(pa.Graphics, imagebounds, 0, 0, image.ᇕ, image.ᇔ, (GraphicsUnit)2, val);
		}
		else
		{
			image.ᇗ(pa.Graphics, imagebounds);
		}
	}

	public override Rectangle GetImageRectangle(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		Rectangle result = Rectangle.Empty;
		bool flag = IsOnMenu(button, pa);
		if (image != null || !string.IsNullOrEmpty(button.SymbolRealized))
		{
			Size imageSize = button.ImageSize;
			Rectangle displayRectangle = GetDisplayRectangle(button);
			if (pa.RightToLeft && flag)
			{
				result = new Rectangle(displayRectangle.Right - (button._0E60.X + imageSize.Width + 2), button._0E60.Y + displayRectangle.Y, imageSize.Width, imageSize.Height);
			}
			else
			{
				result = ((flag || (button.ImagePosition != eImagePosition.Top && button.ImagePosition != eImagePosition.Bottom)) ? new Rectangle(button._0E60.X, button._0E60.Y, button._0E60.Width, button._0E60.Height) : new Rectangle(button._0E60.X, button._0E60.Y, displayRectangle.Width, button._0E60.Height));
				result.Offset(displayRectangle.Left, displayRectangle.Top);
				if ((button.EffectiveStyle == eDotNetBarStyle.Windows7 || button.EffectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(button.EffectiveStyle)) && (button is ApplicationButton || button is MetroAppButton))
				{
					result.Offset(14, (result.Height - imageSize.Height) / 2);
				}
				else
				{
					result.Offset((result.Width - imageSize.Width) / 2, (result.Height - imageSize.Height) / 2);
				}
				result.Width = imageSize.Width;
				result.Height = imageSize.Height;
			}
		}
		return result;
	}

	public override Rectangle GetCheckRectangle(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		Rectangle result = Rectangle.Empty;
		bool flag = IsOnMenu(button, pa);
		Rectangle displayRectangle = GetDisplayRectangle(button);
		if (flag)
		{
			if (pa.RightToLeft)
			{
				result = new Rectangle(displayRectangle.Right - button._0E60.Width + 1, displayRectangle.Y, button._0E60.Width - 2, displayRectangle.Height);
				result.Inflate(-1, -1);
			}
			else
			{
				result = new Rectangle(displayRectangle.X, displayRectangle.Y, button._0E60.Width - 2, displayRectangle.Height);
				result.Inflate(-1, -1);
			}
		}
		else if (button.HotTrackingStyle == eHotTrackingStyle.Image && image != null)
		{
			result = GetImageRectangle(button, pa, image);
			result.Inflate(2, 2);
		}
		else
		{
			result = displayRectangle;
		}
		return result;
	}

	public override Rectangle GetCustomizeMenuCheckRectangle(ButtonItem button, ItemPaintArgs pa)
	{
		bool flag = IsOnMenu(button, pa);
		Rectangle result = Rectangle.Empty;
		if (flag && button.IsOnCustomizeMenu && button.Visible && !button.SystemItem)
		{
			Rectangle displayRectangle = GetDisplayRectangle(button);
			result = new Rectangle(displayRectangle.Left, displayRectangle.Top, displayRectangle.Height, displayRectangle.Height);
			result.Inflate(-1, -1);
		}
		return result;
	}

	public override void PaintCustomizeCheck(ButtonItem button, ItemPaintArgs pa, Rectangle r)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		Color color = pa.Colors.ItemCheckedBackground;
		Graphics graphics = pa.Graphics;
		if (button.IsMouseOver && !pa.Colors.ItemHotBackground2.IsEmpty)
		{
			LinearGradientBrush val = BarFunctions.ॴ(r, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
			try
			{
				graphics.FillRectangle((Brush)(object)val, r);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			if (button.IsMouseOver)
			{
				color = pa.Colors.ItemHotBackground;
			}
			if (!pa.Colors.ItemCheckedBackground2.IsEmpty && !button.IsMouseOver)
			{
				LinearGradientBrush val2 = BarFunctions.ॴ(r, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
				try
				{
					graphics.FillRectangle((Brush)(object)val2, r);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else
			{
				SolidBrush val3 = new SolidBrush(color);
				try
				{
					graphics.FillRectangle((Brush)(object)val3, r);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		Pen val4 = new Pen(pa.Colors.ItemCheckedBorder, (float)Dpi.Width1);
		DisplayHelp.DrawRectangle(graphics, val4, r);
		val4.Dispose();
		val4 = new Pen(pa.Colors.ItemCheckedText);
		Point[] array = new Point[3];
		array[0].X = r.Left + (r.Width - Dpi.Width5) / 2 - Dpi.Width1;
		array[0].Y = r.Top + (r.Height - Dpi.Height6) / 2 + Dpi.Height3;
		array[1].X = array[0].X + Dpi.Width2;
		array[1].Y = array[0].Y + Dpi.Height2;
		array[2].X = array[1].X + Dpi.Width4;
		array[2].Y = array[1].Y - Dpi.Height4;
		graphics.DrawLines(val4, array);
		array[0].X++;
		array[1].X++;
		array[2].X++;
		graphics.DrawLines(val4, array);
		val4.Dispose();
	}

	public override void PaintButtonMouseOver(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r)
	{
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		bool isMouseDown = button.IsMouseDown;
		bool flag = IsOnMenu(button, pa);
		Graphics graphics = pa.Graphics;
		Brush val = null;
		Pen val2 = null;
		if (isMouseDown && !flag)
		{
			val = (Brush)((!pa.Colors.ItemPressedBackground2.IsEmpty) ? ((object)BarFunctions.ॴ(r, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle)) : ((object)new SolidBrush(pa.Colors.ItemPressedBackground)));
			val2 = new Pen(pa.Colors.ItemPressedBorder, 1f);
		}
		else if (_192F._1930(button, pa))
		{
			val = (Brush)(pa.Colors.ItemHotBackground2.IsEmpty ? ((object)new SolidBrush(pa.Colors.ItemHotBackground)) : ((object)BarFunctions.ॴ(r, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle)));
			val2 = new Pen(pa.Colors.ItemHotBorder, 1f);
		}
		else if (flag)
		{
			val2 = new Pen(pa.Colors.ItemHotBorder, 1f);
		}
		if (val != null)
		{
			graphics.FillRectangle(val, r);
		}
		if (val2 != null)
		{
			DisplayHelp.DrawRectangle(graphics, val2, r);
		}
		if (val != null)
		{
			val.Dispose();
		}
		if (val2 != null)
		{
			val2.Dispose();
		}
	}

	protected virtual void PaintButtonCheckBackground(ButtonItem button, ItemPaintArgs pa, Rectangle r)
	{
		Graphics graphics = pa.Graphics;
		bool flag = IsOnMenu(button, pa);
		if (!button.IsMouseOver || flag)
		{
			DisplayHelp.FillRectangle(graphics, r, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
			DisplayHelp.DrawRectangle(graphics, pa.Colors.ItemCheckedBorder, r);
		}
	}

	public override void PaintButtonCheck(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		if (r.IsEmpty)
		{
			return;
		}
		bool flag = IsOnMenu(button, pa);
		Graphics graphics = pa.Graphics;
		PaintButtonCheckBackground(button, pa, r);
		if (((image != null || !string.IsNullOrEmpty(button.SymbolRealized)) && button.ButtonStyle != eButtonStyle.TextOnlyAlways) || !flag)
		{
			return;
		}
		Pen val = new Pen(_192F._1930(button, pa) ? pa.Colors.ItemCheckedText : pa.Colors.ItemDisabledText, (float)Dpi.Width1);
		try
		{
			Point[] array = new Point[3];
			array[0].X = r.Left + (r.Width - Dpi.Width5) / 2 - Dpi.Width1;
			array[0].Y = r.Top + (r.Height - Dpi.Height6) / 2 + Dpi.Height3;
			array[1].X = array[0].X + Dpi.Width2;
			array[1].Y = array[0].Y + Dpi.Height2;
			array[2].X = array[1].X + Dpi.Width4;
			array[2].Y = array[1].Y - Dpi.Height4;
			graphics.DrawLines(val, array);
			array[0].X += Dpi.Width1;
			array[1].X += Dpi.Width1;
			array[2].X += Dpi.Width1;
			graphics.DrawLines(val, array);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public override Rectangle GetMouseOverRectangle(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		Rectangle displayRectangle = GetDisplayRectangle(button);
		if (button.HotTrackingStyle == eHotTrackingStyle.None || button.HotTrackingStyle == eHotTrackingStyle.Color)
		{
			return Rectangle.Empty;
		}
		if (button.HotTrackingStyle == eHotTrackingStyle.Image && image != null)
		{
			displayRectangle = GetImageRectangle(button, pa, image);
			displayRectangle.Inflate(2, 2);
			return displayRectangle;
		}
		if (IsOnMenu(button, pa))
		{
			displayRectangle.X++;
			displayRectangle.Width -= 2;
		}
		return displayRectangle;
	}

	public override eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		eTextFormat eTextFormat2 = pa.ButtonStringFormat;
		if (!IsOnMenu(button, pa))
		{
			if ((pa.ContainerControl is RibbonStrip && ((image == null && string.IsNullOrEmpty(button.SymbolRealized)) || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)) || button._ບ)
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			else if (pa.ContainerControl is ButtonX)
			{
				ButtonX buttonX = pa.ContainerControl as ButtonX;
				if (buttonX.TextAlignment == eButtonTextAlignment.Center)
				{
					eTextFormat2 |= eTextFormat.HorizontalCenter;
				}
				else if (buttonX.TextAlignment == eButtonTextAlignment.Left)
				{
					eTextFormat2 = eTextFormat2;
				}
				else if (buttonX.TextAlignment == eButtonTextAlignment.Right && ((image == null && string.IsNullOrEmpty(button.SymbolRealized)) || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom))
				{
					eTextFormat2 |= eTextFormat.Right;
				}
				if (!buttonX.UseMnemonic)
				{
					eTextFormat2 |= eTextFormat.NoPrefix;
				}
			}
			else if (pa.IsOnMenuBar || ((pa.ContainerControl is Bar || pa.ContainerControl is ButtonX || button.Orientation == eOrientation.Vertical) && image == null && string.IsNullOrEmpty(button.SymbolRealized)) || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			if (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
			{
				if (image != null || !string.IsNullOrEmpty(button.SymbolRealized))
				{
					eTextFormat2 |= eTextFormat.WordBreak;
				}
				eTextFormat2 &= ~(eTextFormat2 & eTextFormat.VerticalCenter);
			}
		}
		if (pa.RightToLeft)
		{
			eTextFormat2 |= eTextFormat.RightToLeft;
		}
		return eTextFormat2;
	}

	protected virtual bool IsTextCentered(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		if (!IsOnMenu(button, pa))
		{
			Control containerControl = pa.ContainerControl;
			if ((containerControl is RibbonStrip && ((image == null && string.IsNullOrEmpty(button.SymbolRealized)) || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)) || button.Name.StartsWith("sysgallery") || button._ບ)
			{
				return true;
			}
			if (containerControl is ButtonX)
			{
				ButtonX buttonX = containerControl as ButtonX;
				if (buttonX.TextAlignment == eButtonTextAlignment.Center)
				{
					return true;
				}
			}
			else if (pa.IsOnMenuBar || (containerControl is Bar && image == null && string.IsNullOrEmpty(button.SymbolRealized)) || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual Rectangle GetTextRectangle(ButtonItem button, ItemPaintArgs pa, eTextFormat stringFormat, ᇎ image)
	{
		_ = pa.Graphics;
		bool flag = IsOnMenu(button, pa);
		bool isOnMenuBar = pa.IsOnMenuBar;
		Rectangle displayRectangle = GetDisplayRectangle(button);
		Rectangle result = button.TextDrawRect;
		Rectangle rectangle = button._0E60;
		bool rightToLeft = pa.RightToLeft;
		if (flag || button.ButtonStyle != 0 || (image == null && string.IsNullOrEmpty(button.SymbolRealized)) || (!flag && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)))
		{
			if (flag)
			{
				result = ((!rightToLeft) ? new Rectangle(result.X, result.Y, displayRectangle.Width - rectangle.Right - 26, result.Height) : ((!button.IsOnCustomizeMenu) ? new Rectangle(17, result.Y, displayRectangle.Width - rectangle.Width - 28, result.Height) : new Rectangle(displayRectangle.Height, result.Y, displayRectangle.Width - rectangle.Width - 11 - displayRectangle.Height, result.Height)));
			}
			else if ((button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom) && button.Orientation != eOrientation.Vertical)
			{
				result = new Rectangle(1, result.Y + 2, displayRectangle.Width - 2, result.Height);
				if (button.SplitButton || pa.ContainerControl is RibbonBar)
				{
					result.Y += 2;
				}
			}
			if (image == null && string.IsNullOrEmpty(button.SymbolRealized) && (stringFormat & eTextFormat.HorizontalCenter) != eTextFormat.HorizontalCenter && !flag && !isOnMenuBar && result.X == 0 && !pa.RightToLeft)
			{
				result.X = 3;
			}
			result.Offset(displayRectangle.Left, displayRectangle.Top);
			if (button.Orientation == eOrientation.Vertical && !flag)
			{
				if (result.Bottom > displayRectangle.Bottom)
				{
					result.Height = displayRectangle.Bottom - result.Y;
				}
			}
			else if (result.Right > displayRectangle.Right)
			{
				result.Width = displayRectangle.Right - result.Left;
			}
		}
		return result;
	}

	public override void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, ᇎ image)
	{
		if (!button._0AD4)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		eTextFormat stringFormat = GetStringFormat(button, pa, image);
		bool flag = IsOnMenu(button, pa);
		bool isOnMenuBar = pa.IsOnMenuBar;
		Rectangle displayRectangle = GetDisplayRectangle(button);
		Rectangle textRectangle = GetTextRectangle(button, pa, stringFormat, image);
		Font val = button._໘(pa, _໙: false);
		bool rightToLeft = pa.RightToLeft;
		if (flag || button.ButtonStyle != 0 || (image == null && string.IsNullOrEmpty(button.SymbolRealized)) || (!flag && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)))
		{
			if (button.Orientation == eOrientation.Vertical && !flag)
			{
				graphics.RotateTransform(90f);
				if (button.TextMarkupBody == null)
				{
					TextDrawing.DrawStringLegacy(graphics, _192F.ಱ(button.Text), val, textColor, new Rectangle(textRectangle.Top, -textRectangle.Right, textRectangle.Height, textRectangle.Width), stringFormat);
				}
				else
				{
					MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, val, textColor, rightToLeft);
					markupDrawContext.HotKeyPrefixVisible = (stringFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
					button.TextMarkupBody.Bounds = new Rectangle(textRectangle.Top, -textRectangle.Right, button.TextMarkupBody.Bounds.Width, button.TextMarkupBody.Bounds.Height);
					button.TextMarkupBody.Render(markupDrawContext);
				}
				graphics.ResetTransform();
			}
			else
			{
				if (button.TextMarkupBody == null)
				{
					if (pa.GlassEnabled && (button.Parent is ᱭ || (button.Parent is ය && (button.EffectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(button.EffectiveStyle)))) && !(pa.ContainerControl is Ꮮ))
					{
						if (!pa.ໟ)
						{
							_21F8._21FC(graphics, button.Text, val, textRectangle, TextDrawing.GetTextFormat(stringFormat), textColor, _21FE: true, !button.IsMouseOver && !ColorFunctions.IsEqual(textColor, Color.White), 7);
						}
					}
					else
					{
						TextDrawing.DrawString(graphics, _192F.ಱ(button.Text), val, textColor, textRectangle, stringFormat);
					}
				}
				else
				{
					MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(graphics, val, textColor, rightToLeft);
					markupDrawContext2.HotKeyPrefixVisible = (stringFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
					markupDrawContext2.ContextObject = button;
					Rectangle bounds = new Rectangle((rightToLeft && flag) ? (textRectangle.X + textRectangle.Width - button.TextMarkupBody.Bounds.Width) : textRectangle.X, textRectangle.Y + (textRectangle.Height - button.TextMarkupBody.Bounds.Height) / 2 + 1, button.TextMarkupBody.Bounds.Width, button.TextMarkupBody.Bounds.Height);
					if (pa.ContainerControl is RibbonBar && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom))
					{
						bounds = new Rectangle(textRectangle.X + (textRectangle.Width - button.TextMarkupBody.Bounds.Width) / 2, textRectangle.Y, button.TextMarkupBody.Bounds.Width, button.TextMarkupBody.Bounds.Height);
					}
					else if (IsTextCentered(button, pa, image))
					{
						bounds.Offset((textRectangle.Width - bounds.Width) / 2, 0);
					}
					if (button._ບ)
					{
						bounds.Y--;
					}
					button.TextMarkupBody.Bounds = bounds;
					button.TextMarkupBody.Render(markupDrawContext2);
				}
				if (!button.DesignMode && button.Focused && !flag && !isOnMenuBar && (!(pa.ContainerControl is ButtonX) || ((ButtonX)(object)pa.ContainerControl).FocusCuesEnabled))
				{
					Rectangle rectangle = displayRectangle;
					rectangle.Inflate(-2, -2);
					ControlPaint.DrawFocusRectangle(graphics, rectangle);
				}
			}
		}
		if (button._0E5F != "" && flag && !button.IsOnCustomizeDialog)
		{
			stringFormat |= eTextFormat.HidePrefix;
			stringFormat |= eTextFormat.Right;
			TextDrawing.DrawString(graphics, button._0E5F, val, textColor, textRectangle, stringFormat);
		}
	}

	protected virtual Rectangle GetTotalSubItemsRect(ButtonItem button)
	{
		return button.ໜ();
	}

	public override void PaintExpandButton(ButtonItem button, ItemPaintArgs pa)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		bool flag = IsOnMenu(button, pa);
		Rectangle displayRectangle = GetDisplayRectangle(button);
		bool isMouseOver = button.IsMouseOver;
		Color textColor = GetTextColor(button, pa);
		SolidBrush val = new SolidBrush(textColor);
		try
		{
			if ((button.SubItems.Count <= 0 && button.PopupType != ePopupType.Container) || !button.ShowSubItems)
			{
				return;
			}
			if (flag)
			{
				Point[] array = new Point[3];
				if (pa.RightToLeft)
				{
					array[0].X = displayRectangle.Left + 8;
					array[0].Y = displayRectangle.Top + (displayRectangle.Height - 8) / 2;
					array[1].X = array[0].X;
					array[1].Y = array[0].Y + 8;
					array[2].X = array[0].X - 4;
					array[2].Y = array[0].Y + 4;
				}
				else
				{
					array[0].X = displayRectangle.Left + displayRectangle.Width - 12;
					array[0].Y = displayRectangle.Top + (displayRectangle.Height - 8) / 2;
					array[1].X = array[0].X;
					array[1].Y = array[0].Y + 8;
					array[2].X = array[0].X + 4;
					array[2].Y = array[0].Y + 4;
				}
				graphics.FillPolygon((Brush)(object)val, array);
			}
			else
			{
				if (button.SubItemsRect.IsEmpty)
				{
					return;
				}
				if (_192F._1930(button, pa) && (isMouseOver || button.Checked) && !button.Expanded && button.HotTrackingStyle != eHotTrackingStyle.None && button.HotTrackingStyle != eHotTrackingStyle.Image && !button.AutoExpandOnClick)
				{
					Rectangle totalSubItemsRect = GetTotalSubItemsRect(button);
					totalSubItemsRect.Offset(displayRectangle.Location);
					Pen val2 = new Pen(isMouseOver ? pa.Colors.ItemHotBorder : pa.Colors.ItemCheckedBorder);
					try
					{
						DisplayHelp.DrawRectangle(graphics, val2, totalSubItemsRect);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				_192F._1931(button, pa);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected virtual void PaintMenuItemSide(ButtonItem button, ItemPaintArgs pa, Rectangle sideRect)
	{
		Graphics graphics = pa.Graphics;
		Region val = graphics.Clip.Clone();
		graphics.SetClip(sideRect);
		sideRect.Inflate(0, 1);
		if (button.MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !button.RecentlyUsed)
		{
			DisplayHelp.FillRectangle(graphics, sideRect, pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
		}
		else
		{
			DisplayHelp.FillRectangle(graphics, sideRect, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
		}
		if (val != null)
		{
			graphics.Clip = val;
		}
		else
		{
			graphics.ResetClip();
		}
	}

	public override void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		bool flag = IsOnMenu(button, pa);
		Rectangle displayRectangle = GetDisplayRectangle(button);
		if (flag)
		{
			Rectangle sideRect = new Rectangle(displayRectangle.Left, displayRectangle.Top, button._0E60.Right, displayRectangle.Height);
			if (pa.RightToLeft)
			{
				sideRect = new Rectangle(displayRectangle.Right - button._0E60.Right, displayRectangle.Top, button._0E60.Right, displayRectangle.Height);
			}
			PaintMenuItemSide(button, pa, sideRect);
		}
		else if (!pa.Colors.ItemBackground.IsEmpty)
		{
			if (pa.Colors.ItemBackground2.IsEmpty)
			{
				SolidBrush val = new SolidBrush(pa.Colors.ItemBackground);
				try
				{
					graphics.FillRectangle((Brush)(object)val, displayRectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else
			{
				LinearGradientBrush val2 = BarFunctions.ॴ(displayRectangle, pa.Colors.ItemBackground, pa.Colors.ItemBackground2, pa.Colors.ItemBackgroundGradientAngle);
				try
				{
					graphics.FillRectangle((Brush)(object)val2, displayRectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		else if (!_192F._1930(button, pa) && !pa.Colors.ItemDisabledBackground.IsEmpty)
		{
			SolidBrush val3 = new SolidBrush(pa.Colors.ItemDisabledBackground);
			try
			{
				graphics.FillRectangle((Brush)(object)val3, displayRectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		Rectangle rectangle = displayRectangle;
		if ((!_192F._1930(button, pa) && !button.DesignMode) || !button.Expanded || flag)
		{
			return;
		}
		if (pa.Colors.ItemExpandedBackground2.IsEmpty)
		{
			Rectangle rectangle2 = displayRectangle;
			if (!pa.Colors.ItemExpandedShadow.IsEmpty)
			{
				rectangle2.Width -= 2;
			}
			SolidBrush val4 = new SolidBrush(pa.Colors.ItemExpandedBackground);
			try
			{
				graphics.FillRectangle((Brush)(object)val4, rectangle2);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		else
		{
			LinearGradientBrush val5 = BarFunctions.ॴ(new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width - 2, rectangle.Height), pa.Colors.ItemExpandedBackground, pa.Colors.ItemExpandedBackground2, pa.Colors.ItemExpandedBackgroundGradientAngle);
			Rectangle rectangle3 = new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
			if (!pa.Colors.ItemExpandedShadow.IsEmpty)
			{
				rectangle3.Width -= 2;
			}
			graphics.FillRectangle((Brush)(object)val5, rectangle3);
			((Brush)val5).Dispose();
		}
		Point[] array = ((button.Orientation != 0 || button.PopupSide != 0) ? new Point[5] : new Point[4]);
		array[0].X = rectangle.Left;
		array[0].Y = rectangle.Top + rectangle.Height - 1;
		array[1].X = rectangle.Left;
		array[1].Y = rectangle.Top;
		if (button.Orientation == eOrientation.Horizontal)
		{
			if (!pa.Colors.ItemExpandedShadow.IsEmpty)
			{
				array[2].X = rectangle.Left + rectangle.Width - 3;
			}
			else
			{
				array[2].X = rectangle.Right - 1;
			}
		}
		else
		{
			array[2].X = rectangle.Left + rectangle.Width - 1;
		}
		array[2].Y = rectangle.Top;
		if (button.Orientation == eOrientation.Horizontal)
		{
			if (!pa.Colors.ItemExpandedShadow.IsEmpty)
			{
				array[3].X = rectangle.Left + rectangle.Width - 3;
			}
			else
			{
				array[3].X = rectangle.Right - 1;
			}
		}
		else
		{
			array[3].X = rectangle.Left + rectangle.Width - 1;
		}
		array[3].Y = rectangle.Top + rectangle.Height - 1;
		if (button.Orientation == eOrientation.Vertical || button.PopupSide != 0)
		{
			array[4].X = rectangle.Left;
			array[4].Y = rectangle.Top + rectangle.Height - 1;
		}
		if (!pa.Colors.ItemExpandedBorder.IsEmpty)
		{
			Pen val6 = new Pen(pa.Colors.ItemExpandedBorder, 1f);
			try
			{
				graphics.DrawLines(val6, array);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		if (!pa.Colors.ItemExpandedShadow.IsEmpty && button.Orientation == eOrientation.Horizontal)
		{
			SolidBrush val7 = new SolidBrush(pa.Colors.ItemExpandedShadow);
			try
			{
				graphics.FillRectangle((Brush)(object)val7, rectangle.Left + rectangle.Width - 2, rectangle.Top + 2, 2, rectangle.Height - 2);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
	}
}
internal abstract class _192F
{
	public abstract void PaintButton(ButtonItem button, ItemPaintArgs pa);

	public abstract void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, ᇎ image);

	public abstract void PaintButtonCheck(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r);

	public abstract void PaintButtonImage(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle imagebounds);

	public abstract void PaintButtonMouseOver(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r);

	public abstract void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, ᇎ image);

	public abstract void PaintCustomizeCheck(ButtonItem button, ItemPaintArgs pa, Rectangle r);

	public abstract void PaintExpandButton(ButtonItem button, ItemPaintArgs pa);

	public abstract Rectangle GetCheckRectangle(ButtonItem button, ItemPaintArgs pa, ᇎ image);

	public abstract Rectangle GetCustomizeMenuCheckRectangle(ButtonItem button, ItemPaintArgs pa);

	public abstract Rectangle GetImageRectangle(ButtonItem button, ItemPaintArgs pa, ᇎ image);

	public abstract Rectangle GetMouseOverRectangle(ButtonItem button, ItemPaintArgs pa, ᇎ image);

	public abstract eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, ᇎ image);

	public static string ಱ(string _06EA)
	{
		return _06EA;
	}

	public static bool _1930(BaseItem _06EC, ItemPaintArgs _0BD2)
	{
		return _06EC.GetEnabled(_0BD2.ContainerControl);
	}

	public static Point[] ረ(Rectangle ल, ePopupSide _0C74)
	{
		Point[] array = new Point[3];
		Size size = new Size(Dpi.Width4, Dpi.Height3);
		switch (_0C74)
		{
		case ePopupSide.Right:
			array[0].X = ल.Left + 1;
			array[0].Y = ल.Top + (ल.Height - size.Height) / 2 - 1;
			array[1].X = array[0].X;
			array[1].Y = array[0].Y + Dpi.Height6;
			array[2].X = array[0].X + Dpi.Width3;
			array[2].Y = array[0].Y + Dpi.Width3;
			break;
		case ePopupSide.Left:
			array[0].X = ल.Left + Dpi.Width3;
			array[0].Y = ल.Top + (ल.Height - size.Height) / 2 - 1;
			array[1].X = array[0].X;
			array[1].Y = array[0].Y + Dpi.Height6;
			array[2].X = array[0].X - Dpi.Width3;
			array[2].Y = array[0].Y + Dpi.Height3;
			break;
		case ePopupSide.Top:
			array[0].X = ल.Left + (ल.Width - size.Width) / 2;
			array[0].Y = ल.Top + (ल.Height - size.Height) / 2 + size.Height;
			array[1].X = array[0].X + Dpi.Width5;
			array[1].Y = array[0].Y + 1;
			array[2].X = array[0].X + Dpi.Width2;
			array[2].Y = array[0].Y - Dpi.Height3;
			break;
		case ePopupSide.Default:
		case ePopupSide.Bottom:
			array[0].X = ल.Left + (ल.Width - size.Width) / 2;
			array[0].Y = ल.Top + (ल.Height - size.Height) / 2 + 1;
			array[1].X = array[0].X + Dpi.Width5;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + Dpi.Width2;
			array[2].Y = array[0].Y + Dpi.Height3;
			break;
		}
		return array;
	}

	public static void _1931(ButtonItem ᤉ, ItemPaintArgs _0BD2)
	{
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		Graphics graphics = _0BD2.Graphics;
		Rectangle displayRectangle = ᤉ.DisplayRectangle;
		Point[] array = new Point[3];
		Rectangle subItemsRect = ᤉ.SubItemsRect;
		if (ᤉ.PopupSide == ePopupSide.Default)
		{
			if (ᤉ.Orientation == eOrientation.Horizontal)
			{
				array[0].X = displayRectangle.Left + subItemsRect.Left + (subItemsRect.Width - Dpi.Width5) / 2;
				array[0].Y = displayRectangle.Top + subItemsRect.Top + (subItemsRect.Height - Dpi.Height3) / 2 + 1;
				array[1].X = array[0].X + Dpi.Width5;
				array[1].Y = array[0].Y;
				array[2].X = array[0].X + Dpi.Width2;
				array[2].Y = array[0].Y + Dpi.Height3;
			}
			else
			{
				array[0].X = displayRectangle.Left + subItemsRect.Left + subItemsRect.Width / 2;
				array[0].Y = displayRectangle.Top + subItemsRect.Top + subItemsRect.Height / 2 - Dpi.Height3;
				array[1].X = array[0].X;
				array[1].Y = array[0].Y + Dpi.Height6;
				array[2].X = array[0].X + Dpi.Width3;
				array[2].Y = array[0].Y + Dpi.Height3;
			}
		}
		else
		{
			switch (ᤉ.PopupSide)
			{
			case ePopupSide.Right:
				array[0].X = displayRectangle.Left + subItemsRect.Left + subItemsRect.Width / 2;
				array[0].Y = displayRectangle.Top + subItemsRect.Top + subItemsRect.Height / 2 - Dpi.Height3;
				array[1].X = array[0].X;
				array[1].Y = array[0].Y + Dpi.Height6;
				array[2].X = array[0].X + Dpi.Width3;
				array[2].Y = array[0].Y + Dpi.Height3;
				break;
			case ePopupSide.Left:
				array[0].X = displayRectangle.Left + subItemsRect.Left + subItemsRect.Width / 2 + Dpi.Width3;
				array[0].Y = displayRectangle.Top + subItemsRect.Top + subItemsRect.Height / 2 - Dpi.Height3;
				array[1].X = array[0].X;
				array[1].Y = array[0].Y + Dpi.Height6;
				array[2].X = array[0].X - Dpi.Width3;
				array[2].Y = array[0].Y + Dpi.Height3;
				break;
			case ePopupSide.Top:
				array[0].X = displayRectangle.Left + subItemsRect.Left + (subItemsRect.Width - Dpi.Width5) / 2;
				array[0].Y = displayRectangle.Top + subItemsRect.Top + (subItemsRect.Height - Dpi.Height3) / 2 + Dpi.Height4;
				array[1].X = array[0].X + Dpi.Width6;
				array[1].Y = array[0].Y;
				array[2].X = array[0].X + Dpi.Width3;
				array[2].Y = array[0].Y - Dpi.Height4;
				break;
			case ePopupSide.Bottom:
				array[0].X = displayRectangle.Left + subItemsRect.Left + (subItemsRect.Width - Dpi.Width5) / 2 + 1;
				array[0].Y = displayRectangle.Top + subItemsRect.Top + (subItemsRect.Height - Dpi.Height3) / 2 + 1;
				array[1].X = array[0].X + Dpi.Width5;
				array[1].Y = array[0].Y;
				array[2].X = array[0].X + Dpi.Width2;
				array[2].Y = array[0].Y + Dpi.Height3;
				break;
			}
		}
		if (_1930(ᤉ, _0BD2))
		{
			SolidBrush val = new SolidBrush(_0BD2.Colors.ItemText);
			try
			{
				graphics.FillPolygon((Brush)(object)val, array);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		SolidBrush val2 = new SolidBrush(_0BD2.Colors.ItemDisabledText);
		try
		{
			graphics.FillPolygon((Brush)(object)val2, array);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
internal class _1932
{
	public static Color _1933(ButtonItem ᤉ, ItemPaintArgs _0BD2)
	{
		Color controlText = SystemColors.ControlText;
		if (!_192F._1930(ᤉ, _0BD2))
		{
			if (!_0BD2.Colors.ItemDisabledText.IsEmpty)
			{
				return _0BD2.Colors.ItemDisabledText;
			}
			return SystemColors.ControlDark;
		}
		if (ᤉ.IsMouseDown && !_0BD2.Colors.ItemPressedText.IsEmpty)
		{
			return _0BD2.Colors.ItemPressedText;
		}
		if (ᤉ.IsMouseOver)
		{
			if (!ᤉ.HotForeColor.IsEmpty)
			{
				return ᤉ.HotForeColor;
			}
			return _0BD2.Colors.ItemHotText;
		}
		if (ᤉ.Expanded && !_0BD2.Colors.ItemExpandedText.IsEmpty)
		{
			return _0BD2.Colors.ItemExpandedText;
		}
		if (ᤉ.Checked && !_0BD2.Colors.ItemCheckedText.IsEmpty)
		{
			return _0BD2.Colors.ItemCheckedText;
		}
		if (!ᤉ.ForeColor.IsEmpty)
		{
			return ᤉ.ForeColor;
		}
		if (ᤉ.IsThemed && ᤉ.IsOnMenuBar && _0BD2.Colors.ItemText == SystemColors.ControlText)
		{
			return SystemColors.MenuText;
		}
		return _0BD2.Colors.ItemText;
	}
}
internal class _1934
{
	public static void _1935(ButtonItem ᤉ, ItemPaintArgs _0BD2)
	{
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Expected O, but got Unknown
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Expected O, but got Unknown
		Graphics graphics = _0BD2.Graphics;
		_0A80 obj = _0BD2._0A80;
		_0FDE _070B = _0FDE._0FDF;
		_0FE4 obj2 = _0FE4._0FE5;
		Color color = _1932._1933(ᤉ, _0BD2);
		ButtonX buttonX = _0BD2.ContainerControl as ButtonX;
		bool flag = true;
		if (buttonX != null)
		{
			flag = buttonX.FocusCuesEnabled;
		}
		Rectangle rectangle = Rectangle.Empty;
		Rectangle displayRectangle = ᤉ.DisplayRectangle;
		Font val = null;
		ᇎ ᇎ2 = ᤉ._0EC5();
		eTextFormat eTextFormat2 = ಲ(ᤉ, _0BD2, ᇎ2);
		val = ᤉ._໘(_0BD2, _໙: false);
		bool flag2 = (ᤉ.SubItems.Count > 0 || ᤉ.PopupType == ePopupType.Container) && ᤉ.ShowSubItems && !ᤉ.SubItemsRect.IsEmpty;
		if (ᇎ2 != null)
		{
			rectangle = ((ᤉ.ImagePosition != eImagePosition.Top && ᤉ.ImagePosition != eImagePosition.Bottom) ? new Rectangle(ᤉ._0E60.X, ᤉ._0E60.Y, ᤉ._0E60.Width, ᤉ._0E60.Height) : new Rectangle(ᤉ._0E60.X, ᤉ._0E60.Y, displayRectangle.Width, ᤉ._0E60.Height));
			rectangle.Offset(displayRectangle.Left, displayRectangle.Top);
			rectangle.Offset((rectangle.Width - ᤉ.ImageSize.Width) / 2, (rectangle.Height - ᤉ.ImageSize.Height) / 2);
			rectangle.Width = ᤉ.ImageSize.Width;
			rectangle.Height = ᤉ.ImageSize.Height;
		}
		if (!_192F._1930(ᤉ, _0BD2))
		{
			obj2 = _0FE4._0FE8;
		}
		else if (ᤉ.IsMouseDown || ᤉ.Expanded)
		{
			obj2 = _0FE4._0FE7;
		}
		else if (ᤉ.IsMouseOver && ᤉ.Checked)
		{
			obj2 = _0FE4._0FE7;
		}
		else if (ᤉ.IsMouseOver)
		{
			obj2 = _0FE4._0FE6;
		}
		else if (ᤉ.Checked || ᤉ.Expanded)
		{
			obj2 = _0FE4._0FE7;
		}
		else if (ᤉ.Focused || _0BD2.ContainerControl.Focused)
		{
			obj2 = _0FE4._0FE9;
		}
		Rectangle ल = ᤉ.DisplayRectangle;
		if (ᤉ.HotTrackingStyle == eHotTrackingStyle.Image && ᇎ2 != null)
		{
			ल = rectangle;
			ल.Inflate(3, 3);
		}
		if (ᤉ.HotTrackingStyle != eHotTrackingStyle.None)
		{
			obj._0EE5(graphics, _070B, obj2, ल);
		}
		if (ᇎ2 != null && ᤉ.ButtonStyle != eButtonStyle.TextOnlyAlways)
		{
			if (obj2 == _0FE4._0FE5 && ᤉ.HotTrackingStyle == eHotTrackingStyle.Color)
			{
				ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
				{
					new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
					new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
					new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0.2f, 0.2f, 0.2f, 0f, 1f }
				});
				ImageAttributes val2 = new ImageAttributes();
				val2.SetColorMatrix(colorMatrix);
				ᇎ2.ᇗ(graphics, rectangle, 0, 0, ᇎ2.ᇕ, ᇎ2.ᇔ, (GraphicsUnit)2, val2);
			}
			else if (obj2 == _0FE4._0FE5 && !ᇎ2.ᇏ)
			{
				ImageAttributes val3 = new ImageAttributes();
				val3.SetGamma(0.7f, (ColorAdjustType)1);
				ᇎ2.ᇗ(graphics, rectangle, 0, 0, ᇎ2.ܐ, ᇎ2._0711, (GraphicsUnit)2, val3);
			}
			else
			{
				ᇎ2.ᇗ(graphics, rectangle);
			}
		}
		if (ᤉ.ButtonStyle == eButtonStyle.ImageAndText || ᤉ.ButtonStyle == eButtonStyle.TextOnlyAlways || ᇎ2 == null)
		{
			Rectangle bounds = ᤉ.TextDrawRect;
			if (ᤉ.ImagePosition == eImagePosition.Top || ᤉ.ImagePosition == eImagePosition.Bottom)
			{
				bounds = ((ᤉ.Orientation != eOrientation.Vertical) ? new Rectangle(ᤉ.TextDrawRect.X, ᤉ.TextDrawRect.Y, ᤉ.TextDrawRect.Width, ᤉ.TextDrawRect.Height) : new Rectangle(ᤉ.TextDrawRect.X, ᤉ.TextDrawRect.Y, ᤉ.TextDrawRect.Width, ᤉ.TextDrawRect.Height));
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			bounds.Offset(displayRectangle.Left, displayRectangle.Top);
			if (ᤉ.Orientation == eOrientation.Vertical)
			{
				graphics.RotateTransform(90f);
				TextDrawing.DrawStringLegacy(graphics, _192F.ಱ(ᤉ.Text), val, color, new Rectangle(bounds.Top, -bounds.Right, bounds.Height, bounds.Width), eTextFormat2);
				graphics.ResetTransform();
			}
			else
			{
				if (bounds.Right > ᤉ.DisplayRectangle.Right)
				{
					bounds.Width = ᤉ.DisplayRectangle.Right - bounds.Left;
				}
				TextDrawing.DrawString(graphics, _192F.ಱ(ᤉ.Text), val, color, bounds, eTextFormat2);
				if (!ᤉ.DesignMode && ᤉ.Focused && flag && !_0BD2.IsOnMenu && !_0BD2.IsOnMenuBar)
				{
					Rectangle bounds2 = ᤉ.Bounds;
					bounds2.Inflate(-3, -3);
					ControlPaint.DrawFocusRectangle(graphics, bounds2);
				}
			}
		}
		if (flag2)
		{
			_192F._1931(ᤉ, _0BD2);
		}
		ᇎ2?.Dispose();
	}

	private static eTextFormat ಲ(ButtonItem ᤉ, ItemPaintArgs _0BD2, ᇎ ແ)
	{
		eTextFormat eTextFormat2 = _0BD2.ButtonStringFormat;
		if (!_1936(ᤉ, _0BD2) && (ແ == null || ᤉ.ImagePosition == eImagePosition.Top || ᤉ.ImagePosition == eImagePosition.Bottom))
		{
			eTextFormat2 |= eTextFormat.HorizontalCenter;
		}
		return eTextFormat2;
	}

	private static bool _1936(ButtonItem ᤉ, ItemPaintArgs _0BD2)
	{
		bool flag = _0BD2.IsOnMenu;
		if (flag && ᤉ.Parent is ItemContainer)
		{
			flag = false;
		}
		return flag;
	}
}
internal enum _1937
{
	ሬ,
	ड़,
	_1938,
	_0F78
}
internal class _1A6B
{
	public static void _1A6C(Graphics _٥, Rectangle ल, Color _0BDE)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		bool flag = false;
		if ((int)_٥.SmoothingMode == 4)
		{
			flag = true;
			_٥.SmoothingMode = (SmoothingMode)0;
		}
		_٥.DrawRectangle(Pens.White, ल);
		Pen val = new Pen(_0BDE, 1f);
		try
		{
			val.DashStyle = (DashStyle)2;
			_٥.DrawRectangle(val, ल);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (flag)
		{
			_٥.SmoothingMode = (SmoothingMode)4;
		}
	}

	public static void _1A6C(Graphics _٥, GraphicsPath _1A6D, Color _0BDE)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		bool flag = false;
		if ((int)_٥.SmoothingMode == 4)
		{
			flag = true;
			_٥.SmoothingMode = (SmoothingMode)0;
		}
		_٥.DrawPath(Pens.White, _1A6D);
		Pen val = new Pen(_0BDE, 1f);
		try
		{
			val.DashStyle = (DashStyle)2;
			_٥.DrawPath(val, _1A6D);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (flag)
		{
			_٥.SmoothingMode = (SmoothingMode)4;
		}
	}
}
internal class _1BE6
{
	private static ArrayList _1BE7;

	private static Hashtable _1BE8;

	private static ReaderWriterLock _1BE9;

	static _1BE6()
	{
		_1BE7 = new ArrayList();
		_1BE8 = new Hashtable();
		_1BE9 = new ReaderWriterLock();
	}

	public static void _1BEA(ሎ ᖲ)
	{
		if (_1BE7.Contains(ᖲ))
		{
			return;
		}
		if (!_1BE8.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			_0A3B();
		}
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = _1BE9.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = _1BE9.UpgradeToWriterLock(-1);
		}
		else
		{
			_1BE9.AcquireWriterLock(-1);
		}
		try
		{
			_1BE7.Add(ᖲ);
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_1BE9.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_1BE9.ReleaseWriterLock();
			}
		}
	}

	public static void _1BEB(ሎ ᖲ)
	{
		if (!_1BE7.Contains(ᖲ))
		{
			return;
		}
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = _1BE9.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = _1BE9.UpgradeToWriterLock(-1);
		}
		else
		{
			_1BE9.AcquireWriterLock(-1);
		}
		bool flag = false;
		try
		{
			_1BE7.Remove(ᖲ);
			flag = _1BE7.Count == 0;
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_1BE9.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_1BE9.ReleaseWriterLock();
			}
		}
		if (BarUtilities.AutoRemoveMessageFilter && flag && _1BE8.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			_1BEC();
		}
	}

	private static void _0A3B()
	{
		if (!_1BE8.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			_1BF0 obj = new _1BF0();
			Application.AddMessageFilter((IMessageFilter)(object)obj);
			_1BE8.Add(Thread.CurrentThread.GetHashCode(), obj);
		}
	}

	private static void _1BEC()
	{
		if (_1BE8.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			_1BF0 obj = _1BE8[Thread.CurrentThread.GetHashCode()] as _1BF0;
			Application.RemoveMessageFilter((IMessageFilter)(object)obj);
			_1BE8.Remove(Thread.CurrentThread.GetHashCode());
		}
	}

	private static ሎ _1BED()
	{
		_1BE9.AcquireReaderLock(-1);
		try
		{
			foreach (ሎ item in _1BE7)
			{
				if (item._0A63)
				{
					return item;
				}
			}
		}
		finally
		{
			_1BE9.ReleaseReaderLock();
		}
		return null;
	}

	private static ሎ[] _1BEE()
	{
		_1BE9.AcquireReaderLock(-1);
		try
		{
			return (ሎ[])_1BE7.ToArray(typeof(ሎ));
		}
		finally
		{
			_1BE9.ReleaseReaderLock();
		}
	}

	public static bool _0A71(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		ሎ ሎ2 = _1BED();
		if (ሎ2 != null)
		{
			if (!_1BEF(ሎ2) && ሎ2._0A71(_0A65, _੦, _੧))
			{
				return true;
			}
			return false;
		}
		ሎ[] array = _1BEE();
		ሎ[] array2 = array;
		foreach (ሎ ሎ3 in array2)
		{
			if (!_1BEF(ሎ3) && ሎ3._0A71(_0A65, _੦, _੧))
			{
				return true;
			}
		}
		return false;
	}

	public static bool ੳ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		ሎ ሎ2 = _1BED();
		if (ሎ2 != null)
		{
			if (!_1BEF(ሎ2) && ሎ2.ੳ(_0A65, _੦, _੧))
			{
				return true;
			}
			return false;
		}
		ሎ[] array = _1BEE();
		ሎ[] array2 = array;
		foreach (ሎ ሎ3 in array2)
		{
			if (!_1BEF(ሎ3) && ሎ3.ੳ(_0A65, _੦, _੧))
			{
				return true;
			}
		}
		return false;
	}

	public static bool _੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		ሎ[] array = _1BEE();
		ሎ[] array2 = array;
		foreach (ሎ ሎ2 in array2)
		{
			if (!_1BEF(ሎ2) && ሎ2._੪(_0A65, _੦, _੧))
			{
				return true;
			}
		}
		return false;
	}

	public static bool ሏ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		ሎ[] array = _1BEE();
		ሎ[] array2 = array;
		foreach (ሎ ሎ2 in array2)
		{
			if (!_1BEF(ሎ2) && ሎ2.ሏ(_0A65, _੦, _੧))
			{
				return true;
			}
		}
		return false;
	}

	private static bool _1BEF(ሎ ᖲ)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		if (ᖲ is DotNetBarManager { ParentForm: not null } dotNetBarManager && ((Control)dotNetBarManager.ParentForm).InvokeRequired)
		{
			return true;
		}
		if (ᖲ is Control && ((Control)ᖲ).InvokeRequired)
		{
			return true;
		}
		return false;
	}

	public static bool ሐ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		ሎ[] array = _1BEE();
		ሎ[] array2 = array;
		foreach (ሎ ሎ2 in array2)
		{
			if (!_1BEF(ሎ2) && ሎ2.ሐ(_0A65, _੦, _੧))
			{
				return true;
			}
		}
		return false;
	}

	public static bool _੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		ሎ ሎ2 = _1BED();
		if (ሎ2 != null)
		{
			if (!_1BEF(ሎ2) && ሎ2._੨(_0A65, _੦, _੧))
			{
				return true;
			}
			return false;
		}
		ሎ[] array = _1BEE();
		ሎ[] array2 = array;
		foreach (ሎ ሎ3 in array2)
		{
			if (!_1BEF(ሎ3) && ሎ3._੨(_0A65, _੦, _੧))
			{
				return true;
			}
		}
		return false;
	}
}
internal class _1BF0 : IMessageFilter
{
	public bool PreFilterMessage(ref Message m)
	{
		switch (((Message)(ref m)).Msg)
		{
		case 260:
			return _1BE6._0A71(((Message)(ref m)).HWnd, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		case 261:
			return _1BE6.ੳ(((Message)(ref m)).HWnd, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		case 256:
			return _1BE6._੪(((Message)(ref m)).HWnd, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		case 161:
		case 164:
		case 167:
		case 513:
		case 516:
		case 519:
			return _1BE6.ሏ(((Message)(ref m)).HWnd, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		case 512:
			return _1BE6.ሐ(((Message)(ref m)).HWnd, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		case 522:
			return _1BE6._੨(((Message)(ref m)).HWnd, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		default:
			return false;
		}
	}

	public void _1BF1(ref Message Ⴘ)
	{
	}
}
internal enum _1886
{
	ᘶ = -1,
	_1C8C = 1,
	_1C8D = 3,
	ᆤ = 0,
	_1C8E = 4,
	_1C8F = 2,
	Ა = 10
}
internal class _1DE0
{
	private delegate IntPtr ᓌ(int nCode, IntPtr wParam, IntPtr lParam);

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

		public Point _1DE7()
		{
			return new Point(و, ى);
		}
	}

	private const int ᒻ = 7;

	private const int ᒼ = 0;

	private const int _1DE1 = 12;

	private const int _1DE2 = 4;

	private static ArrayList _1BE7;

	private static Hashtable _1BE8;

	private static ReaderWriterLock _1BE9;

	private static ᓌ _1DE3;

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr SetWindowsHookEx(int hookid, ᓌ pfnhook, IntPtr hinst, int threadid);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern bool UnhookWindowsHookEx(IntPtr hhook);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wparam, IntPtr lparam);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern int GetCurrentThreadId();

	static _1DE0()
	{
		_1BE7 = new ArrayList();
		_1BE8 = new Hashtable();
		_1DE3 = null;
		_1BE9 = new ReaderWriterLock();
		_1DE3 = ᓉ;
	}

	public static void ᶷ(_1DDD ᖲ)
	{
		if (_1BE7.Contains(ᖲ))
		{
			return;
		}
		if (!_1BE8.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			_1DE5();
		}
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = _1BE9.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = _1BE9.UpgradeToWriterLock(-1);
		}
		else
		{
			_1BE9.AcquireWriterLock(-1);
		}
		try
		{
			_1BE7.Add(ᖲ);
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_1BE9.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_1BE9.ReleaseWriterLock();
			}
		}
	}

	public static void _1DE4(_1DDD ᖲ)
	{
		if (!_1BE7.Contains(ᖲ))
		{
			return;
		}
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = _1BE9.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = _1BE9.UpgradeToWriterLock(-1);
		}
		else
		{
			_1BE9.AcquireWriterLock(-1);
		}
		try
		{
			_1BE7.Remove(ᖲ);
			if (_1BE7.Count == 0)
			{
				_1DE6();
			}
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_1BE9.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_1BE9.ReleaseWriterLock();
			}
		}
	}

	private static void _1DE5()
	{
		if (!_1BE8.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			int currentThreadId = GetCurrentThreadId();
			IntPtr intPtr = SetWindowsHookEx(7, _1DE3, IntPtr.Zero, currentThreadId);
			_1BE8.Add(Thread.CurrentThread.GetHashCode(), intPtr);
		}
	}

	private static void _1DE6()
	{
		if (_1BE8.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			IntPtr hhook = (IntPtr)_1BE8[Thread.CurrentThread.GetHashCode()];
			UnhookWindowsHookEx(hhook);
			_1BE8.Remove(Thread.CurrentThread.GetHashCode());
		}
	}

	private static _1DDD[] _1BEE()
	{
		_1BE9.AcquireReaderLock(-1);
		try
		{
			return (_1DDD[])_1BE7.ToArray(typeof(_1DDD));
		}
		finally
		{
			_1BE9.ReleaseReaderLock();
		}
	}

	private unsafe static IntPtr ᓉ(int ᓊ, IntPtr _੦, IntPtr _੧)
	{
		try
		{
			if (ᓊ == 0)
			{
				int num = _0EFE._0F11(_੦);
				if (num == 512)
				{
					ᓎ* ptr = (ᓎ*)(void*)_੧;
					_1DD9(ptr->_0F05, ptr->ᓏ._1DE7());
				}
				if (num == 514)
				{
					ᓎ* ptr2 = (ᓎ*)(void*)_੧;
					_1DDB(ptr2->_0F05, ptr2->ᓏ._1DE7());
				}
			}
			IntPtr hhook = (IntPtr)_1BE8[Thread.CurrentThread.GetHashCode()];
			return CallNextHookEx(hhook, ᓊ, _੦, _੧);
		}
		catch
		{
		}
		return IntPtr.Zero;
	}

	public static bool _1DD9(IntPtr _0A65, Point ఋ)
	{
		_1DDD[] array = _1BEE();
		_1DDD[] array2 = array;
		foreach (_1DDD obj in array2)
		{
			obj._1DD9(_0A65, ఋ);
		}
		return false;
	}

	public static bool _1DDB(IntPtr _0A65, Point ఋ)
	{
		_1DDD[] array = _1BEE();
		_1DDD[] array2 = array;
		foreach (_1DDD obj in array2)
		{
			obj._1DDB(_0A65, ఋ);
		}
		return false;
	}
}
[ToolboxItem(false)]
internal class _1DFE : UserControl
{
	private bool ݡ;

	private bool ݠ;

	private eOrientation Ⴐ;

	private eItemAlignment Ḁ;

	private Timer ဟ;

	public ColorScheme _ḁ;

	private _0A7D _09F4;

	public bool _193C;

	public bool Ḃ = true;

	public bool ḃ;

	public eOrientation ᡥ
	{
		get
		{
			return Ⴐ;
		}
		set
		{
			if (Ⴐ != value)
			{
				Ⴐ = value;
				((Control)this).Refresh();
			}
		}
	}

	public eItemAlignment _1DFF
	{
		get
		{
			return Ḁ;
		}
		set
		{
			if (Ḁ != value)
			{
				Ḁ = value;
				((Control)this).Refresh();
			}
		}
	}

	private _0A7D _0A7D
	{
		get
		{
			if (_09F4 == null)
			{
				_09F4 = new _0A7D((Control)(object)this);
			}
			return _09F4;
		}
	}

	public _1DFE()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).TabStop = false;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Expected O, but got Unknown
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Expected O, but got Unknown
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		if (ḃ)
		{
			Ḅ(e);
			return;
		}
		if (ݡ)
		{
			if (_ḁ == null)
			{
				SolidBrush val = new SolidBrush(ColorFunctions.HoverBackColor(graphics));
				try
				{
					graphics.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				_089F.र(graphics, SystemPens.Highlight, ((Control)this).ClientRectangle);
			}
			else
			{
				if (!_ḁ.ItemHotBackground2.IsEmpty)
				{
					LinearGradientBrush val2 = BarFunctions.ॴ(((Control)this).ClientRectangle, _ḁ.ItemHotBackground, _ḁ.ItemHotBackground2, _ḁ.ItemHotBackgroundGradientAngle);
					graphics.FillRectangle((Brush)(object)val2, ((Control)this).ClientRectangle);
					((Brush)val2).Dispose();
				}
				else
				{
					SolidBrush val3 = new SolidBrush(_ḁ.ItemHotBackground);
					try
					{
						graphics.FillRectangle((Brush)(object)val3, ((Control)this).DisplayRectangle);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				_089F.र(graphics, new Pen(_ḁ.ItemHotBorder), ((Control)this).ClientRectangle);
			}
		}
		else if (_ḁ == null)
		{
			SolidBrush val4 = new SolidBrush(SystemColors.Control);
			try
			{
				graphics.FillRectangle((Brush)(object)val4, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			_089F.र(graphics, SystemPens.ControlDark, ((Control)this).ClientRectangle);
		}
		else if (_ḁ.ItemBackground.IsEmpty)
		{
			SolidBrush val5 = new SolidBrush(_ḁ.BarBackground);
			try
			{
				graphics.FillRectangle((Brush)(object)val5, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		else if (_ḁ.ItemBackground2.IsEmpty)
		{
			SolidBrush val6 = new SolidBrush(_ḁ.ItemBackground);
			try
			{
				graphics.FillRectangle((Brush)(object)val6, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		else
		{
			LinearGradientBrush val7 = BarFunctions.ॴ(((Control)this).DisplayRectangle, _ḁ.ItemBackground, _ḁ.ItemBackground2, _ḁ.ItemBackgroundGradientAngle);
			try
			{
				graphics.FillRectangle((Brush)(object)val7, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		if (Ⴐ == eOrientation.Horizontal)
		{
			if (Ḁ == eItemAlignment.Far)
			{
				Point[] array = new Point[3];
				array[0].X = ((Control)this).ClientRectangle.Width / 2 - 2;
				array[0].Y = (((Control)this).ClientRectangle.Height - 8) / 2;
				array[1].X = array[0].X;
				array[1].Y = array[0].Y + 8;
				array[2].X = array[0].X + 4;
				array[2].Y = array[0].Y + 4;
				if (_ḁ == null)
				{
					graphics.FillPolygon(SystemBrushes.ControlText, array);
				}
				else
				{
					graphics.FillPolygon((Brush)new SolidBrush(_ḁ.ItemText), array);
				}
			}
			else
			{
				Point[] array2 = new Point[3];
				array2[0].X = ((Control)this).ClientRectangle.Width / 2 + 2;
				array2[0].Y = (((Control)this).ClientRectangle.Height - 8) / 2;
				array2[1].X = array2[0].X;
				array2[1].Y = array2[0].Y + 8;
				array2[2].X = array2[0].X - 4;
				array2[2].Y = array2[0].Y + 4;
				if (_ḁ == null)
				{
					graphics.FillPolygon(SystemBrushes.ControlText, array2);
				}
				else
				{
					graphics.FillPolygon((Brush)new SolidBrush(_ḁ.ItemText), array2);
				}
			}
		}
		else if (Ḁ == eItemAlignment.Far)
		{
			Point[] array3 = new Point[3];
			array3[0].X = ((Control)this).ClientRectangle.Width / 2 - 3;
			array3[0].Y = (((Control)this).ClientRectangle.Height - 4) / 2;
			array3[1].X = array3[0].X + 7;
			array3[1].Y = array3[0].Y;
			array3[2].X = array3[0].X + 3;
			array3[2].Y = array3[0].Y + 4;
			if (_ḁ == null)
			{
				graphics.FillPolygon(SystemBrushes.ControlText, array3);
			}
			else
			{
				graphics.FillPolygon((Brush)new SolidBrush(_ḁ.ItemText), array3);
			}
		}
		else
		{
			Point[] array4 = new Point[3];
			array4[0].X = ((Control)this).ClientRectangle.Width / 2 - 3;
			array4[0].Y = (((Control)this).ClientRectangle.Height + 3) / 2;
			array4[1].X = array4[0].X + 7;
			array4[1].Y = array4[0].Y;
			array4[2].X = array4[0].X + 3;
			array4[2].Y = array4[0].Y - 4;
			if (_ḁ == null)
			{
				graphics.FillPolygon(SystemBrushes.ControlText, array4);
			}
			else
			{
				graphics.FillPolygon((Brush)new SolidBrush(_ḁ.ItemText), array4);
			}
		}
	}

	private void Ḅ(PaintEventArgs ؿ)
	{
		Graphics graphics = ؿ.Graphics;
		if (_193C)
		{
			_0A7D obj = _0A7D;
			_0F86 _070B = _0F86._0F87;
			_0F90 obj2 = _0F90._0F91;
			obj2 = (ݡ ? ((Ⴐ == eOrientation.Vertical) ? ((Ḁ != 0) ? _0F90._0F96 : _0F90._0F92) : ((Ḁ != 0) ? _0F90._0F9E : _0F90._0F9A)) : ((Ⴐ == eOrientation.Vertical) ? ((Ḁ != 0) ? _0F90._0F95 : _0F90._0F91) : ((Ḁ != 0) ? _0F90._0F9D : _0F90._0F99)));
			obj._0EE5(graphics, _070B, obj2, ((Control)this).DisplayRectangle);
			return;
		}
		if (ݠ)
		{
			BarFunctions._८(graphics, ((Control)this).DisplayRectangle, (Border3DStyle)8);
		}
		else
		{
			BarFunctions._८(graphics, ((Control)this).DisplayRectangle, (Border3DStyle)5);
		}
		if (Ⴐ == eOrientation.Vertical)
		{
			ᥕ(graphics, ((Control)this).DisplayRectangle, SystemColors.ControlText, Ḁ == eItemAlignment.Near);
		}
	}

	private void ᥕ(Graphics _٥, Rectangle भ, Color _0BDE, bool ᥖ)
	{
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		Point[] array = new Point[3];
		if (ᥖ)
		{
			array[0].X = भ.Left + (भ.Width - 9) / 2;
			array[0].Y = भ.Top + भ.Height / 2 + 1;
			array[1].X = array[0].X + 8;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + 4;
			array[2].Y = array[0].Y - 5;
		}
		else
		{
			array[0].X = भ.Left + (भ.Width - 7) / 2;
			array[0].Y = भ.Top + (भ.Height - 4) / 2;
			array[1].X = array[0].X + 7;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + 3;
			array[2].Y = array[0].Y + 4;
		}
		_٥.FillPolygon((Brush)new SolidBrush(_0BDE), array);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		if (!ݡ)
		{
			ݡ = true;
			((Control)this).Refresh();
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (ဟ != null)
		{
			ဟ.Stop();
			((Component)(object)ဟ).Dispose();
			ဟ = null;
		}
		if (ݡ)
		{
			ݡ = false;
			((Control)this).Refresh();
		}
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		if (Ḃ)
		{
			_1037();
		}
	}

	private void _1037()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (ဟ == null)
		{
			ဟ = new Timer();
			ဟ.Interval = 200;
			ဟ.Tick += _1038;
			ဟ.Start();
		}
	}

	private void _1038(object _٠, EventArgs ؿ)
	{
		((Control)this).InvokeOnClick((Control)(object)this, new EventArgs());
	}

	protected override void Dispose(bool disposing)
	{
		if (ဟ != null)
		{
			ဟ.Stop();
			((Component)(object)ဟ).Dispose();
			ဟ = null;
		}
		((ContainerControl)this).Dispose(disposing);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_0A78();
		((Control)this).OnHandleDestroyed(e);
	}

	private void _0A78()
	{
		if (_09F4 != null)
		{
			_09F4.Dispose();
			_09F4 = null;
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 794)
		{
			చ();
		}
		else if (((Message)(ref m)).Msg == 33)
		{
			((Message)(ref m)).Result = new IntPtr(3);
			return;
		}
		((UserControl)this).WndProc(ref m);
	}

	private void చ()
	{
		if (_09F4 != null)
		{
			_09F4.Dispose();
			_09F4 = new _0A7D((Control)(object)this);
		}
	}
}
internal class _2082 : ῲ
{
	internal override Size _2001 => new Size(3, 2);

	public _2082(SuperTabStripItem tabStripItem)
		: base(tabStripItem)
	{
	}
}
internal class _209D : ῲ
{
	private int ₙ => Dpi.Width4;

	private int ₚ => Dpi.Width25;

	private int ₛ => Dpi.Width4;

	private int ₜ => Dpi.Width14;

	internal override Size Ό
	{
		get
		{
			if (!base._1FD4.ᓙ || base._1FD4.HorizontalText)
			{
				return Dpi.Size(new Size(52, 20));
			}
			return Dpi.Size(new Size(36, 20));
		}
	}

	internal override Size _2001 => Dpi.Size(new Size(5, 3));

	internal override int _136F
	{
		get
		{
			if (!base._1FD4.ᓙ)
			{
				return ₚ;
			}
			return ₜ;
		}
	}

	internal override int _1FFF
	{
		get
		{
			switch (base._1FD4.TabAlignment)
			{
			case eTabStripAlignment.Top:
			case eTabStripAlignment.Bottom:
				return ₙ;
			case eTabStripAlignment.Right:
				if (base._1FD4.HorizontalText)
				{
					return ₛ;
				}
				return ₙ;
			default:
				return ₛ;
			}
		}
	}

	public _209D(SuperTabStripItem tabStripItem)
		: base(tabStripItem)
	{
	}

	protected override void NextBlockPosition(LayoutManagerPositionEventArgs e)
	{
		int num = base.ᇃ.IndexOf((BaseItem)e.Block);
		if (num >= 0 && num + 1 < base.ᇃ.Count && base.ᇃ[num] is SuperTabItem && base.ᇃ[num + 1] is SuperTabItem)
		{
			e.NextPosition = e.CurrentPosition;
			if (base._1FD4.ᓙ)
			{
				e.NextPosition.Y += e.Block.Bounds.Height - ₜ;
			}
			else
			{
				e.NextPosition.X += e.Block.Bounds.Width - ₚ;
			}
			e.Cancel = true;
		}
	}

	internal override Rectangle _2013(BaseItem _06EC, BaseItem _2014)
	{
		Rectangle result = base._2013(_06EC, _2014);
		if (_06EC is SuperTabItem && _2014 is SuperTabItem)
		{
			if (base._1FD4.ᓙ)
			{
				result.Y -= ₜ;
			}
			else
			{
				result.X -= ₚ;
			}
		}
		return result;
	}

	internal override void _2012(ref Size ߒ)
	{
		base._2012(ref ߒ);
		if (!base._1FD4.ᓙ || !base._1FD4.HorizontalText)
		{
			ߒ.Height += Dpi.Height7;
		}
	}

	protected override void DrawStripBorder(ItemPaintArgs p, SuperTabColorTable ct)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = p.Graphics;
		Rectangle ዲ = ((base._1FD4.SelectedTab != null) ? base._1FD4.SelectedTab.DisplayRectangle : Rectangle.Empty);
		Rectangle rectangle = _20A0();
		eTabState ὡ = ῠ();
		SuperTabItemStateColorTable superTabItemStateColorTable = base._1FD4.SelectedTab.ὠ(ὡ);
		if (superTabItemStateColorTable.Background.Colors != null)
		{
			Brush val = (Brush)new SolidBrush(superTabItemStateColorTable.Background.Colors[superTabItemStateColorTable.Background.Colors.Length - 1]);
			try
			{
				SmoothingMode smoothingMode = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)3;
				graphics.FillRectangle(val, rectangle);
				graphics.SmoothingMode = smoothingMode;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (!ct.InnerBorder.IsEmpty)
		{
			_209E(graphics, ct, rectangle, ዲ);
		}
		if (!ct.OuterBorder.IsEmpty)
		{
			_209F(graphics, ct, rectangle, ዲ);
		}
	}

	private void _209E(Graphics _٥, SuperTabColorTable ዅ, Rectangle ल, Rectangle ዲ)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Pen val = new Pen(ዅ.InnerBorder);
		try
		{
			switch (base._1FD4.TabAlignment)
			{
			case eTabStripAlignment.Top:
				if (ዲ.X > ल.X)
				{
					_٥.DrawLine(val, ल.X, ल.Y + Dpi.Height1, ዲ.X + Dpi.Width4, ल.Y + Dpi.Height1);
				}
				if (ዲ.Right < ल.Right)
				{
					_٥.DrawLine(val, ዲ.Right - Dpi.Width4, ल.Y + Dpi.Height1, ल.Right - Dpi.Width1, ल.Y + Dpi.Height1);
				}
				_٥.DrawLine(val, ल.X + Dpi.Width1, ल.Y + Dpi.Height1, ल.X + Dpi.Width1, ल.Y + Dpi.Height7);
				_٥.DrawLine(val, ल.Right - Dpi.Width2, ल.Y + Dpi.Height1, ल.Right - Dpi.Width2, ल.Y + Dpi.Height7);
				break;
			case eTabStripAlignment.Bottom:
				if (ዲ.X > ल.X)
				{
					_٥.DrawLine(val, ल.X, ल.Bottom - 1, ዲ.X + 3, ल.Bottom - 1);
				}
				if (ዲ.Right < ल.Right)
				{
					_٥.DrawLine(val, ዲ.Right - 3, ल.Bottom - 1, ल.Right - 1, ल.Bottom - 1);
				}
				_٥.DrawLine(val, ल.X + 1, ल.Bottom - 2, ल.X + 1, ल.Bottom - 7);
				_٥.DrawLine(val, ल.Right - 2, ल.Bottom - 2, ल.Right - 2, ल.Bottom - 7);
				break;
			case eTabStripAlignment.Left:
				if (ዲ.Y > 0)
				{
					_٥.DrawLine(val, ल.X + 1, ल.Y, ल.X + 1, ዲ.Y + 3);
				}
				if (ዲ.Bottom < ल.Bottom)
				{
					_٥.DrawLine(val, ल.X + 1, ዲ.Bottom + 3, ल.X + 1, ल.Bottom - 3);
				}
				_٥.DrawLine(val, ल.X, ल.Y + 1, ल.X + 7, ल.Y + 1);
				_٥.DrawLine(val, ल.X, ल.Bottom - 2, ल.X + 7, ल.Bottom - 2);
				break;
			case eTabStripAlignment.Right:
				if (ዲ.Y > 0)
				{
					_٥.DrawLine(val, ल.Right - 1, ल.Y, ल.Right - 1, ዲ.Y + 3);
				}
				if (ዲ.Bottom < ल.Bottom)
				{
					_٥.DrawLine(val, ल.Right - 1, ዲ.Bottom + 2, ल.Right - 1, ल.Bottom - 1);
				}
				_٥.DrawLine(val, ल.X, ल.Y + 1, ल.Right, ल.Y + 1);
				_٥.DrawLine(val, ल.X, ल.Bottom - 2, ल.Right, ल.Bottom - 2);
				break;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void _209F(Graphics _٥, SuperTabColorTable ዅ, Rectangle ल, Rectangle ዲ)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Pen val = new Pen(ዅ.OuterBorder);
		try
		{
			switch (base._1FD4.TabAlignment)
			{
			case eTabStripAlignment.Top:
				if (ዲ.X > ल.X)
				{
					_٥.DrawLine(val, ल.X, ल.Y, ዲ.X + 4, ल.Y);
				}
				if (ዲ.Right < ल.Right)
				{
					_٥.DrawLine(val, ዲ.Right - 4, ल.Y, ल.Right - 2, ल.Y);
				}
				_٥.DrawLine(val, ल.X, ल.Y, ल.X, ल.Y + 7);
				_٥.DrawLine(val, ल.Right - 1, ल.Y, ल.Right - 1, ल.Y + 7);
				break;
			case eTabStripAlignment.Bottom:
				if (ዲ.X > ल.X)
				{
					_٥.DrawLine(val, ल.X, ल.Bottom, ዲ.X, ल.Bottom);
				}
				if (ዲ.Right < ल.Right)
				{
					_٥.DrawLine(val, ዲ.Right, ल.Bottom, ल.Right - 2, ल.Bottom);
				}
				_٥.DrawLine(val, ल.X, ल.Bottom, ल.X, ल.Bottom - 7);
				_٥.DrawLine(val, ल.Right - 1, ल.Bottom, ल.Right - 1, ल.Bottom - 7);
				break;
			case eTabStripAlignment.Left:
				if (ዲ.Y > 0)
				{
					_٥.DrawLine(val, ल.X, ल.Y, ल.X, ዲ.Y + 3);
				}
				if (ዲ.Bottom < ल.Bottom)
				{
					_٥.DrawLine(val, ल.X, ዲ.Bottom + 2, ल.X, ल.Bottom - 3);
				}
				_٥.DrawLine(val, ल.X, ल.Y, ल.X + 7, ल.Y);
				_٥.DrawLine(val, ल.X, ल.Bottom - 1, ल.X + 7, ल.Bottom - 1);
				break;
			case eTabStripAlignment.Right:
				if (ዲ.Y > 0)
				{
					_٥.DrawLine(val, ल.Right, ल.Y, ल.Right, ዲ.Y);
				}
				if (ዲ.Bottom < ल.Bottom)
				{
					_٥.DrawLine(val, ल.Right, ዲ.Bottom + 3, ल.Right, ल.Bottom - 1);
				}
				_٥.DrawLine(val, ल.X, ल.Y, ल.Right, ल.Y);
				_٥.DrawLine(val, ल.X, ल.Bottom - 1, ल.Right, ल.Bottom - 1);
				break;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private Rectangle _20A0()
	{
		Rectangle bounds = base._1FD4.Bounds;
		switch (base._1FD4.TabAlignment)
		{
		case eTabStripAlignment.Top:
			bounds.Y = bounds.Bottom - Dpi.Height7 - Dpi.Height1;
			break;
		case eTabStripAlignment.Bottom:
			bounds.Y -= Dpi.Height1;
			bounds.Height = Dpi.Height7 + Dpi.Height1;
			break;
		case eTabStripAlignment.Left:
			bounds.X = bounds.Right - Dpi.Width7 - Dpi.Width1;
			break;
		default:
			bounds.X -= Dpi.Width1;
			bounds.Width = Dpi.Width7 + Dpi.Width1;
			break;
		}
		return bounds;
	}

	private eTabState ῠ()
	{
		SuperTabItem selectedTab = base._1FD4.SelectedTab;
		if (!selectedTab.Enabled)
		{
			return eTabState.Disabled;
		}
		if (!selectedTab.IsSelected)
		{
			return eTabState.Default;
		}
		return eTabState.Selected;
	}
}
internal class _20A1 : ῲ
{
	internal override Size _2001 => new Size(3, 2);

	public _20A1(SuperTabStripItem tabStripItem)
		: base(tabStripItem)
	{
	}
}
internal class _20A2 : ῲ
{
	private const int _20A3 = 0;

	private const int ₗ = 20;

	internal override int _136F => 20;

	internal override int _1FFF => 0;

	internal override bool _2000 => true;

	internal override Size _2001 => new Size(3, 2);

	internal override Size Ό
	{
		get
		{
			if (base._1FD4.ᓙ && base._1FD4.HorizontalText)
			{
				return new Size(33, 35);
			}
			return new Size(33, 16);
		}
	}

	public _20A2(SuperTabStripItem tabStripItem)
		: base(tabStripItem)
	{
	}

	protected override void NextBlockPosition(LayoutManagerPositionEventArgs e)
	{
		int num = base.ᇃ.IndexOf((BaseItem)e.Block);
		if (num >= 0 && num + 1 < base.ᇃ.Count && base.ᇃ[num] is SuperTabItem && base.ᇃ[num + 1] is SuperTabItem)
		{
			e.NextPosition = e.CurrentPosition;
			if (base._1FD4.ᓙ)
			{
				e.NextPosition.Y += e.Block.Bounds.Height - 20;
			}
			else
			{
				e.NextPosition.X += e.Block.Bounds.Width - 20;
			}
			e.Cancel = true;
		}
	}

	internal override Rectangle _2013(BaseItem _06EC, BaseItem _2014)
	{
		Rectangle result = base._2013(_06EC, _2014);
		if (_06EC is SuperTabItem && _2014 is SuperTabItem)
		{
			if (base.ᓙ)
			{
				result.Y -= 20;
			}
			else
			{
				result.X -= 20;
			}
		}
		return result;
	}
}
internal class _20A4 : ῲ
{
	private const int ₗ = -3;

	public _20A4(SuperTabStripItem tabStripItem)
		: base(tabStripItem)
	{
		tabStripItem.ControlBox.MenuBox.Style = eDotNetBarStyle.Office2010;
	}

	protected override void NextBlockPosition(LayoutManagerPositionEventArgs e)
	{
		int num = base.ᇃ.IndexOf((BaseItem)e.Block);
		if (num >= 0 && num + 1 < base.ᇃ.Count && base.ᇃ[num] is SuperTabItem && base.ᇃ[num + 1] is SuperTabItem)
		{
			e.NextPosition = e.CurrentPosition;
			if (base._1FD4.ᓙ)
			{
				e.NextPosition.Y += e.Block.Bounds.Height - -3;
			}
			else
			{
				e.NextPosition.X += e.Block.Bounds.Width - -3;
			}
			e.Cancel = true;
		}
	}

	internal override Rectangle _2013(BaseItem _06EC, BaseItem _2014)
	{
		Rectangle result = base._2013(_06EC, _2014);
		if (_06EC is SuperTabItem && _2014 is SuperTabItem)
		{
			if (base._1FD4.ᓙ)
			{
				result.Y -= -3;
			}
			else
			{
				result.X -= -3;
			}
		}
		return result;
	}
}
internal class _20B1
{
	private struct _2A8B
	{
		public Size ᆾ;

		public Size _2A8C;
	}

	public static void _0CC6(ButtonItem ᤉ)
	{
		int width = Dpi.Width8;
		bool flag = ᤉ.IsOnMenu;
		if (flag && ᤉ.Parent is ItemContainer)
		{
			flag = false;
		}
		bool flag2 = false;
		Size empty = Size.Empty;
		if (!string.IsNullOrEmpty(ᤉ.SymbolRealized))
		{
			flag2 = true;
			empty = ᤉ.ImageSize;
		}
		else
		{
			using (ᇎ ᇎ2 = ᤉ._0EC5())
			{
				if (ᇎ2 != null)
				{
					flag2 = true;
				}
			}
			empty = _20BB(ᤉ, flag2, flag, _20B9: false);
		}
		bool isRightToLeft = ᤉ.IsRightToLeft;
		Rectangle displayRectangle = ᤉ.DisplayRectangle;
		if (flag)
		{
			empty.Height += Dpi.Width2;
			empty.Width += Dpi.Width7;
			if (ᤉ.IsOnCustomizeMenu && !isRightToLeft)
			{
				ᤉ._0E60 = new Rectangle(displayRectangle.Height + 2, Math.Max(0, (displayRectangle.Height - empty.Height) / 2), empty.Width, empty.Height);
			}
			else
			{
				ᤉ._0E60 = new Rectangle(0, Math.Max(0, (displayRectangle.Height - empty.Height) / 2), empty.Width, empty.Height);
			}
			ᤉ.TextDrawRect = new Rectangle(ᤉ._0E60.Right + 8, Math.Max(0, (displayRectangle.Height - ᤉ.TextDrawRect.Height) / 2), ᤉ.TextDrawRect.Width, ᤉ.TextDrawRect.Height);
			return;
		}
		int val = 0;
		if (!ᤉ.SubItemsRect.IsEmpty)
		{
			Rectangle subItemsRect = ᤉ.SubItemsRect;
			if (ᤉ.ContainerControl is RibbonBar && (ᤉ.ImagePosition == eImagePosition.Top || ᤉ.ImagePosition == eImagePosition.Bottom))
			{
				subItemsRect = new Rectangle(0, displayRectangle.Height - subItemsRect.Height, displayRectangle.Width, subItemsRect.Height);
				displayRectangle.Height -= subItemsRect.Height;
			}
			else if (ᤉ.Orientation == eOrientation.Horizontal)
			{
				if (isRightToLeft)
				{
					subItemsRect = new Rectangle(0, 0, subItemsRect.Width, displayRectangle.Height);
					val = subItemsRect.Width;
				}
				else
				{
					subItemsRect = new Rectangle(displayRectangle.Width - subItemsRect.Width, 0, subItemsRect.Width, displayRectangle.Height);
					displayRectangle.Width -= subItemsRect.Width;
				}
			}
			else
			{
				subItemsRect = new Rectangle(0, 0, displayRectangle.Width, subItemsRect.Height);
				displayRectangle.Height -= subItemsRect.Height;
			}
			ᤉ.SubItemsRect = subItemsRect;
		}
		if (!flag2 || ᤉ.ButtonStyle == eButtonStyle.TextOnlyAlways)
		{
			int num = displayRectangle.Height - 2;
			ᤉ.TextDrawRect = new Rectangle(2, Math.Max(0, (displayRectangle.Height - num) / 2), displayRectangle.Width - 4, num);
		}
		else if ((ᤉ.ButtonStyle == eButtonStyle.Default && ᤉ.ImagePosition != eImagePosition.Top && ᤉ.ImagePosition != eImagePosition.Bottom) || displayRectangle.Width < empty.Width + width)
		{
			ᤉ._0E60 = new Rectangle(Math.Max(0, (displayRectangle.Width - ᤉ._0E60.Width) / 2), Math.Max(0, (displayRectangle.Height - ᤉ._0E60.Height) / 2), ᤉ._0E60.Width, ᤉ._0E60.Height);
			if (displayRectangle.Width < empty.Width + width)
			{
				ᤉ.TextDrawRect = Rectangle.Empty;
			}
		}
		else if ((ᤉ.ImagePosition == eImagePosition.Left && !isRightToLeft) || (ᤉ.ImagePosition == eImagePosition.Right && isRightToLeft))
		{
			ᤉ._0E60 = new Rectangle(0, Math.Max(0, (displayRectangle.Height - ᤉ._0E60.Height) / 2), ᤉ._0E60.Width, ᤉ._0E60.Height);
			ᤉ.TextDrawRect = new Rectangle(ᤉ._0E60.Right - 2, Math.Max(0, (displayRectangle.Height - ᤉ.TextDrawRect.Height) / 2), Math.Min(ᤉ.TextDrawRect.Width, displayRectangle.Width - (ᤉ._0E60.Right - 2)), ᤉ.TextDrawRect.Height);
		}
		else if ((ᤉ.ImagePosition == eImagePosition.Right && !isRightToLeft) || (ᤉ.ImagePosition == eImagePosition.Left && isRightToLeft))
		{
			ᤉ._0E60 = new Rectangle(displayRectangle.Width - ᤉ._0E60.Width, Math.Max(0, (displayRectangle.Height - ᤉ.TextDrawRect.Height) / 2), ᤉ._0E60.Width, ᤉ._0E60.Height);
			ᤉ.TextDrawRect = new Rectangle(Math.Max(val, ᤉ._0E60.X - ᤉ.TextDrawRect.Width + 2), Math.Max(0, (displayRectangle.Height - ᤉ.TextDrawRect.Height) / 2), Math.Min(ᤉ.TextDrawRect.Width, ᤉ._0E60.X), ᤉ.TextDrawRect.Height);
		}
		else if (ᤉ.ImagePosition == eImagePosition.Top)
		{
			int y = Math.Max(2, (displayRectangle.Height - (ᤉ.TextDrawRect.Height + ᤉ._0E60.Height - 2)) / 2);
			if (ᤉ.Name != "sysOverflowButton" && ᤉ.Parent is ItemContainer && ((ItemContainer)ᤉ.Parent).LayoutOrientation == eOrientation.Horizontal && ((ItemContainer)ᤉ.Parent).VerticalItemAlignment == eVerticalItemsAlignment.Top)
			{
				y = 2;
			}
			ᤉ._0E60 = new Rectangle(0, y, displayRectangle.Width, ᤉ._0E60.Height);
			ᤉ.TextDrawRect = new Rectangle(Math.Max(0, (displayRectangle.Width - ᤉ.TextDrawRect.Width) / 2), ᤉ._0E60.Bottom, ᤉ.TextDrawRect.Width, ᤉ.TextDrawRect.Height);
		}
		else if (ᤉ.ImagePosition == eImagePosition.Bottom)
		{
			int num2 = Math.Max(0, (displayRectangle.Height - (ᤉ.TextDrawRect.Height + ᤉ._0E60.Height - 2)) / 2);
			ᤉ._0E60 = new Rectangle(0, displayRectangle.Height - num2 - ᤉ._0E60.Height, displayRectangle.Width, ᤉ._0E60.Height);
			ᤉ.TextDrawRect = new Rectangle(Math.Max(0, (displayRectangle.Width - ᤉ._0E60.Width) / 2), Math.Max(0, ᤉ._0E60.Y + 2 - ᤉ.TextDrawRect.Height), ᤉ.TextDrawRect.Width, Math.Min(ᤉ.TextDrawRect.Height, ᤉ._0E60.Y));
		}
	}

	public static Size _20B2(BaseItem _06EC, Graphics _٥, int ᜨ, Font _0EE7, eTextFormat _20B3, bool _20B4)
	{
		return _20B2(_06EC, _٥, ᜨ, _0EE7, _20B3, _20B4, _20B5: false, eImagePosition.Left);
	}

	public static Size _20B2(BaseItem _06EC, Graphics _٥, int ᜨ, Font _0EE7, eTextFormat _20B3, bool _20B4, bool _20B5, eImagePosition _20B6)
	{
		if (_06EC.Text == "" && _06EC.TextMarkupBody == null)
		{
			return Size.Empty;
		}
		Size empty = Size.Empty;
		if (_06EC.TextMarkupBody == null)
		{
			return TextDrawing.MeasureString(_٥, _192F.ಱ(_06EC.Text), _0EE7, ᜨ, _20B3);
		}
		Size availableSize = new Size(ᜨ, 1);
		if (ᜨ == 0)
		{
			availableSize.Width = 1600;
		}
		MarkupDrawContext markupDrawContext = new MarkupDrawContext(_٥, _0EE7, Color.Empty, rightToLeft: false);
		_06EC.TextMarkupBody.Measure(availableSize, markupDrawContext);
		availableSize = _06EC.TextMarkupBody.Bounds.Size;
		if (ᜨ != 0 && (!_20B5 || _20B6 != eImagePosition.Top))
		{
			availableSize.Width = ᜨ;
		}
		markupDrawContext.RightToLeft = _20B4;
		_06EC.TextMarkupBody._0CC6(new Rectangle(0, 0, availableSize.Width, availableSize.Height), markupDrawContext);
		return _06EC.TextMarkupBody.Bounds.Size;
	}

	public static void _20B7(ButtonItem ᤉ)
	{
		_20B7(ᤉ, _20B9: false);
	}

	private static bool _20B8(ButtonItem ᤉ)
	{
		if (ᤉ.TextMarkupBody == null)
		{
			if (ᤉ.Text.IndexOf(' ') <= 0)
			{
				return ᤉ.Text.IndexOf(Environment.NewLine) > 0;
			}
			return true;
		}
		if (ᤉ.Text.IndexOf(' ') <= 0 || ᤉ.TextMarkupBody._0CBA)
		{
			if (ᤉ.Text.Split(new char[1] { ' ' }).Length > 2 && ᤉ.TextMarkupBody._0CBA)
			{
				return ᤉ.ButtonStyle != eButtonStyle.Default;
			}
			return false;
		}
		return true;
	}

	public static void _20B7(ButtonItem ᤉ, bool _20B9)
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Invalid comparison between Unknown and I4
		object containerControl = ᤉ.ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || val.Disposing || val.IsDisposed)
		{
			return;
		}
		if (val is ButtonX && ᤉ._ງ)
		{
			_20C0(ᤉ);
			return;
		}
		if (ᤉ.FixedSize.Width > 0 && ᤉ.FixedSize.Height > 0)
		{
			Size size = Dpi.Size(ᤉ.FixedSize);
			ᤉ.SetDisplayRectangle(new Rectangle(ᤉ.DisplayRectangle.Location, size));
			_20C0(ᤉ);
			return;
		}
		bool flag = ᤉ.IsOnMenu;
		if (flag && ᤉ.Parent is ItemContainer)
		{
			flag = false;
		}
		bool flag2 = false;
		bool flag3 = false;
		eDotNetBarStyle effectiveStyle = ᤉ.EffectiveStyle;
		bool flag4 = false;
		if (_20B9 && (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Metro || effectiveStyle == eDotNetBarStyle.Metro))
		{
			flag4 = true;
		}
		if (!flag4)
		{
			if (!string.IsNullOrEmpty(ᤉ.SymbolRealized))
			{
				flag2 = true;
				flag3 = true;
			}
			else
			{
				using ᇎ ᇎ2 = ᤉ._0EC5();
				if (ᇎ2 != null || _20B9)
				{
					flag2 = true;
				}
			}
		}
		eImagePosition eImagePosition2 = ᤉ.ImagePosition;
		bool flag5 = (int)val.RightToLeft == 1;
		Rectangle textDrawRect = Rectangle.Empty;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rectangle2 = Rectangle.Empty;
		Rectangle rectangle3 = new Rectangle(ᤉ.DisplayRectangle.Location, Size.Empty);
		if (flag5 && ᤉ.Orientation == eOrientation.Horizontal)
		{
			switch (eImagePosition2)
			{
			case eImagePosition.Left:
				eImagePosition2 = eImagePosition.Right;
				break;
			case eImagePosition.Right:
				eImagePosition2 = eImagePosition.Left;
				break;
			}
		}
		int num = 0;
		if (ᤉ._ງ)
		{
			num = ᤉ.DisplayRectangle.Width - 4;
		}
		rectangle3.Width = 0;
		rectangle3.Height = 0;
		Graphics val2 = BarFunctions.CreateGraphics(val);
		try
		{
			eTextFormat eTextFormat2 = ᗎ(ᤉ);
			Size empty = Size.Empty;
			if (flag3)
			{
				Font font = Symbols.GetFont(ᤉ.SymbolSize, ᤉ.SymbolSet);
				empty = (ᤉ.ImageSize = ((!ᤉ.IsOnMenu && !(val is SideNavStrip)) ? TextDrawing.MeasureStringLegacy(val2, ᤉ.SymbolRealized, font, Size.Empty, eTextFormat.Default) : TextDrawing.MeasureStringLegacy(val2, "\uf00a", font, Size.Empty, eTextFormat.Default)));
			}
			else
			{
				empty = _20BB(ᤉ, flag2, flag, _20B9);
			}
			bool flag6 = false;
			if (ᤉ._ງ && flag2 && (eImagePosition2 == eImagePosition.Left || eImagePosition2 == eImagePosition.Right))
			{
				num -= empty.Width + Dpi.Width10;
			}
			else if (ᤉ.RibbonWordWrap && flag2 && eImagePosition2 == eImagePosition.Top && val is RibbonBar && _20B8(ᤉ))
			{
				num = empty.Width + Dpi.Width4;
				eTextFormat2 |= eTextFormat.WordBreak;
				flag6 = true;
			}
			Font val3 = ᤉ._໘(null, _໙: true);
			SizeF sizeF = SizeF.Empty;
			if ((ᤉ.Text != "" || ᤉ.TextMarkupBody != null) && (!flag2 || flag || ᤉ.ButtonStyle != 0 || (ᤉ.ImagePosition != 0 && flag2)))
			{
				sizeF = _20B2(ᤉ, val2, num, val3, eTextFormat2, flag5, flag6, eImagePosition2);
				int num2 = 0;
				int num3 = Math.Max(14, empty.Width / 2);
				while (flag6 && (double)sizeF.Height > (double)val3.Height * 2.2 && num2 < 4)
				{
					num += num3;
					sizeF = _20B2(ᤉ, val2, num, val3, eTextFormat2, flag5, flag6, eImagePosition2);
					num2++;
				}
				if (num2 > 0)
				{
					num += num3;
					sizeF = _20B2(ᤉ, val2, num, val3, eTextFormat2, flag5, flag6, eImagePosition2);
				}
				if (val is RibbonBar && flag2 && eImagePosition2 == eImagePosition.Top)
				{
					sizeF.Height += Dpi.Width2;
				}
				if (_20B9 && flag4)
				{
					sizeF.Width = Dpi.Width((int)Math.Max(51f, sizeF.Width + 6f));
				}
			}
			if (flag)
			{
				if (empty.IsEmpty)
				{
					empty = new Size(Dpi.Width16, Dpi.Height16);
				}
				empty.Height += Dpi.Width2;
				empty.Width += Dpi.Width7;
				if (sizeF.Height > (float)empty.Height)
				{
					rectangle3.Height = (int)sizeF.Height + Dpi.Height4;
				}
				else
				{
					rectangle3.Height = empty.Height + Dpi.Height4;
				}
				rectangle3.Height += Dpi.Height(ᤉ._0E62);
				rectangle = ((!ᤉ.IsOnCustomizeMenu || flag5) ? new Rectangle(0, (rectangle3.Height - empty.Height) / 2, empty.Width, empty.Height) : new Rectangle(rectangle3.Height + Dpi.Width2, (rectangle3.Height - empty.Height) / 2, empty.Width, empty.Height));
				rectangle3.Width = (int)sizeF.Width;
				if (ᤉ._0E5F != "")
				{
					Size size3 = TextDrawing.MeasureString(val2, ᤉ._0E5F, val3, 0, eTextFormat2);
					rectangle3.Width += size3.Width + 14;
				}
				textDrawRect = new Rectangle(rectangle.Right + 8, 2, rectangle3.Width, rectangle3.Height - 4);
				rectangle3.Width += rectangle.Right + Dpi.Width34;
				rectangle3.Width += Dpi.Width(ᤉ._0E63);
			}
			else
			{
				bool isThemed = ᤉ.IsThemed;
				if (StyleManager.Style == eStyle.OfficeMobile2014)
				{
					empty.Width += Dpi.Width6;
					empty.Height += Dpi.Height4;
				}
				else if (StyleManager.Style == eStyle.Office2016)
				{
					empty.Width += Dpi.Width4;
					empty.Height += Dpi.Width2;
				}
				if (ᤉ.Orientation == eOrientation.Horizontal && (eImagePosition2 == eImagePosition.Left || eImagePosition2 == eImagePosition.Right))
				{
					empty.Width += Dpi.Width(ᤉ.ImagePaddingHorizontal);
					if (sizeF.Height > (float)empty.Height)
					{
						rectangle3.Height = (int)sizeF.Height + Dpi.Height(ᤉ.ImagePaddingVertical);
					}
					else
					{
						rectangle3.Height = empty.Height + Dpi.Height(ᤉ.ImagePaddingVertical);
					}
					rectangle3.Height += Dpi.Height(ᤉ._0E62);
					if (isThemed && !ᤉ.IsOnMenuBar)
					{
						rectangle3.Height += Dpi.Height4;
					}
					rectangle = Rectangle.Empty;
					if (ᤉ.ButtonStyle != eButtonStyle.TextOnlyAlways && flag2)
					{
						rectangle = new Rectangle(0, (rectangle3.Height - empty.Height) / 2, empty.Width, empty.Height);
					}
					textDrawRect = Rectangle.Empty;
					if (ᤉ.ButtonStyle != 0 || !flag2)
					{
						if (rectangle.Right > 0)
						{
							rectangle3.Width = (int)sizeF.Width + 1;
							textDrawRect = new Rectangle(rectangle.Right - 2, 2, rectangle3.Width, rectangle3.Height - 4);
						}
						else
						{
							rectangle3.Width = (int)sizeF.Width + Dpi.Width6;
							if (!flag2 && ᤉ.IsOnMenuBar)
							{
								rectangle3.Width += Dpi.Width6;
								textDrawRect = new Rectangle(2, 2, rectangle3.Width, rectangle3.Height - 4);
							}
							else
							{
								textDrawRect = new Rectangle(2, 2, rectangle3.Width + Dpi.Width(ᤉ._0E63 - 4), rectangle3.Height - 4);
							}
						}
					}
					rectangle3.Width += rectangle.Right;
					if (eImagePosition2 == eImagePosition.Right && rectangle.Right > 0 && flag2)
					{
						textDrawRect.X = 3;
						rectangle.X = rectangle3.Width - rectangle.Width;
					}
					rectangle3.Width += Dpi.Width(ᤉ._0E63);
				}
				else if (ᤉ.Orientation == eOrientation.Horizontal)
				{
					if (sizeF.Width > (float)empty.Width)
					{
						rectangle3.Width = (int)sizeF.Width + Dpi.Width(ᤉ.ImagePaddingHorizontal);
					}
					else
					{
						rectangle3.Width = empty.Width + Dpi.Width(ᤉ.ImagePaddingHorizontal);
					}
					rectangle3.Height = (int)((float)empty.Height + sizeF.Height + (float)Dpi.Height(ᤉ.ImagePaddingVertical));
					rectangle3.Width += Dpi.Width(ᤉ._0E63);
					rectangle3.Height += Dpi.Height(ᤉ._0E62);
					if (eImagePosition2 == eImagePosition.Top)
					{
						rectangle = new Rectangle(0, Dpi.Height(ᤉ._0E62) / 2 + 2, rectangle3.Width, empty.Height);
						textDrawRect = new Rectangle((int)((float)rectangle3.Width - sizeF.Width) / 2, rectangle.Bottom, (int)sizeF.Width, (int)sizeF.Height + 5);
					}
					else
					{
						textDrawRect = new Rectangle((int)((float)rectangle3.Width - sizeF.Width) / 2, Dpi.Height(ᤉ._0E62) / 2, (int)sizeF.Width, (int)sizeF.Height + 2);
						rectangle = new Rectangle(0, textDrawRect.Bottom, rectangle3.Width, empty.Height + 5);
					}
				}
				else
				{
					if (sizeF.Height > (float)empty.Width && ᤉ.ButtonStyle != 0)
					{
						rectangle3.Width = (int)sizeF.Height + 6;
					}
					else
					{
						rectangle3.Width = empty.Width + 10;
					}
					rectangle3.Width += Dpi.Width(ᤉ._0E63);
					if (ᤉ.ButtonStyle != 0 || !flag2)
					{
						if (flag2)
						{
							rectangle3.Height = (int)((float)empty.Height + sizeF.Width + (float)Dpi.Width12);
						}
						else
						{
							rectangle3.Height = (int)(sizeF.Width + (float)Dpi.Width6);
						}
					}
					else
					{
						rectangle3.Height = empty.Height + Dpi.Height6;
					}
					if (eImagePosition2 == eImagePosition.Top || eImagePosition2 == eImagePosition.Left)
					{
						if (flag2)
						{
							rectangle = new Rectangle(0, 0, rectangle3.Width, empty.Height + Dpi.Height6);
						}
						textDrawRect = new Rectangle((int)((float)rectangle3.Width - sizeF.Height) / 2, rectangle.Bottom + Dpi.Width2, (int)sizeF.Height, (int)sizeF.Width + Dpi.Width5);
					}
					else
					{
						textDrawRect = new Rectangle((int)((float)rectangle3.Width - sizeF.Height) / 2, Dpi.Height5, (int)sizeF.Height, (int)sizeF.Width + Dpi.Width5);
						if (flag2)
						{
							rectangle = new Rectangle(0, textDrawRect.Bottom - Dpi.Height3, rectangle3.Width, empty.Height + Dpi.Height5);
						}
					}
					rectangle3.Height += Dpi.Height(ᤉ._0E62);
				}
				if (_20BA(ᤉ))
				{
					rectangle2 = _20BE(ᤉ, rectangle3, flag5);
					Rectangle b = rectangle2;
					b.Offset(rectangle3.Location);
					if (flag5 && (!(val is RibbonBar) || (ᤉ.ImagePosition != eImagePosition.Top && ᤉ.ImagePosition != eImagePosition.Bottom)))
					{
						if (!textDrawRect.IsEmpty)
						{
							textDrawRect.Offset(rectangle2.Width + 1, 0);
						}
						if (!rectangle.IsEmpty && ᤉ.Orientation == eOrientation.Horizontal && (eImagePosition2 == eImagePosition.Left || eImagePosition2 == eImagePosition.Right))
						{
							rectangle.Offset(rectangle2.Width, 0);
						}
						rectangle3.X += rectangle2.Width;
					}
					rectangle3 = Rectangle.Union(rectangle3, b);
				}
			}
		}
		finally
		{
			val2.TextRenderingHint = (TextRenderingHint)0;
			val2.SmoothingMode = (SmoothingMode)0;
			val2.Dispose();
		}
		val = null;
		ᤉ.SetDisplayRectangle(rectangle3);
		ᤉ._0E60 = rectangle;
		ᤉ.TextDrawRect = textDrawRect;
		ᤉ.SubItemsRect = rectangle2;
	}

	private static bool _20BA(ButtonItem ᤉ)
	{
		if ((ᤉ.SubItems.Count > 0 || ᤉ.PopupType == ePopupType.Container) && ᤉ.ShowSubItems && !ᤉ.IsOnMenuBar)
		{
			if (ᤉ.TextMarkupBody != null && ᤉ.TextMarkupBody._0CBA)
			{
				return ᤉ.ButtonStyle == eButtonStyle.Default;
			}
			return true;
		}
		return false;
	}

	private static Size _20BB(ButtonItem ᤉ, bool _20BC, bool _20BD, bool _20B9)
	{
		Size size = Size.Empty;
		size = ((ᤉ.Parent != null && ᤉ.ImageFixedSize.IsEmpty && ᤉ.UseParentSubItemsImageSize) ? ((!(ᤉ.Parent is ImageItem { SubItemsImageSize: { IsEmpty: false } } imageItem)) ? ᤉ.ImageSize : ((_20BC && !_20BD) ? ((ᤉ.Orientation != 0) ? new Size(Math.Max(imageItem.SubItemsImageSize.Width, ᤉ.ImageSize.Width), ᤉ.ImageSize.Height) : new Size(ᤉ.ImageSize.Width, Math.Max(imageItem.SubItemsImageSize.Height, ᤉ.ImageSize.Height))) : new Size(imageItem.SubItemsImageSize.Width, imageItem.SubItemsImageSize.Height))) : ((_20B9 && !ᤉ.ImageFixedSize.IsEmpty) ? ᤉ.ImageFixedSize : (ᤉ.ImageFixedSize.IsEmpty ? ᤉ.ImageSize : Dpi.Size(ᤉ.ImageFixedSize))));
		if (_20B9)
		{
			if (GlobalManager.Renderer is Office2007Renderer { ColorTable: not null, ColorTable: var colorTable } && colorTable.RibbonControl.StartButtonDefault != null)
			{
				if (size.Width < colorTable.RibbonControl.StartButtonDefault.Width)
				{
					size.Width = colorTable.RibbonControl.StartButtonDefault.Width;
				}
				if (size.Height < colorTable.RibbonControl.StartButtonDefault.Height)
				{
					size.Height = colorTable.RibbonControl.StartButtonDefault.Height;
				}
			}
			size = Dpi.ImageSize(size);
		}
		return size;
	}

	public static Rectangle _20BE(ButtonItem ᤉ, Rectangle _20BF, bool _20B4)
	{
		Rectangle empty = Rectangle.Empty;
		int num = Dpi.Width(ᤉ.SubItemsExpandWidth);
		return (ᤉ.ContainerControl is RibbonBar && (ᤉ.ImagePosition == eImagePosition.Top || ᤉ.ImagePosition == eImagePosition.Bottom)) ? new Rectangle(0, _20BF.Height, _20BF.Width, num) : ((ᤉ.Orientation != 0) ? new Rectangle(0, _20BF.Height - 2, _20BF.Width, num) : (ᤉ.IsThemed ? ((!_20B4) ? new Rectangle(_20BF.Width, 0, num, _20BF.Height) : new Rectangle(0, 0, num, _20BF.Height)) : ((!_20B4) ? new Rectangle(_20BF.Width + 1, 0, num, _20BF.Height) : new Rectangle(0, 0, num, _20BF.Height))));
	}

	public static eTextFormat ᗎ(ButtonItem ᤉ)
	{
		eTextFormat eTextFormat2 = eTextFormat.Default;
		eTextFormat2 = (ᤉ._ງ ? (eTextFormat2 | eTextFormat.WordBreak) : (eTextFormat2 | eTextFormat.SingleLine));
		return eTextFormat2 | eTextFormat.VerticalCenter;
	}

	public static void _20C0(ButtonItem ᤉ)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		object containerControl = ᤉ.ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		ButtonX buttonX = ᤉ.ContainerControl as ButtonX;
		if (!BarFunctions.IsHandleValid(val))
		{
			return;
		}
		bool flag = ᤉ.IsOnMenu;
		if (flag && ᤉ.Parent is ItemContainer)
		{
			flag = false;
		}
		bool flag2 = false;
		if (!string.IsNullOrEmpty(ᤉ.SymbolRealized))
		{
			flag2 = true;
		}
		else
		{
			using ᇎ ᇎ2 = ᤉ._0EC5();
			if (ᇎ2 != null)
			{
				flag2 = true;
			}
		}
		eImagePosition eImagePosition2 = ᤉ.ImagePosition;
		bool flag3 = (int)val.RightToLeft == 1;
		Rectangle textDrawRect = Rectangle.Empty;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle subItemsRect = Rectangle.Empty;
		Rectangle bounds = ᤉ.Bounds;
		if (ᤉ.SubItems.Count > 0 && ᤉ.ShowSubItems && (ᤉ.TextMarkupBody == null || !ᤉ.TextMarkupBody._0CBA || ᤉ.ButtonStyle == eButtonStyle.Default))
		{
			int num = Dpi.Width(ᤉ.SubItemsExpandWidth);
			if (ᤉ.Orientation == eOrientation.Horizontal)
			{
				subItemsRect = ((!flag3) ? new Rectangle(bounds.Width - num, 0, num, bounds.Height) : new Rectangle(0, 0, num, bounds.Height));
				if (flag3)
				{
					bounds.X += num + 1;
				}
				bounds.Width -= num + 1;
			}
			else
			{
				subItemsRect = new Rectangle(0, bounds.Height - num, bounds.Width, num);
				bounds.Height -= num + 1;
			}
		}
		if (flag3 && ᤉ.Orientation == eOrientation.Horizontal)
		{
			switch (eImagePosition2)
			{
			case eImagePosition.Left:
				eImagePosition2 = eImagePosition.Right;
				break;
			case eImagePosition.Right:
				eImagePosition2 = eImagePosition.Left;
				break;
			}
		}
		int num2 = 0;
		num2 = bounds.Width;
		Graphics val2 = BarFunctions.CreateGraphics(val);
		try
		{
			Size empty = Size.Empty;
			if (string.IsNullOrEmpty(ᤉ.SymbolRealized))
			{
				empty = (ᤉ.ImageFixedSize.IsEmpty ? ᤉ.ImageSize : ᤉ.ImageFixedSize);
			}
			else
			{
				Font font = Symbols.GetFont(ᤉ.SymbolSize, ᤉ.SymbolSet);
				empty = TextDrawing.MeasureStringLegacy(val2, ᤉ.SymbolRealized, font, Size.Empty, eTextFormat.Default);
				int num3 = (int)Math.Ceiling((float)font.FontFamily.GetCellDescent(font.Style) * font.Size / (float)font.FontFamily.GetEmHeight(font.Style));
				empty.Height -= num3;
				ᤉ.ImageSize = empty;
			}
			if (flag2 && (eImagePosition2 == eImagePosition.Left || eImagePosition2 == eImagePosition.Right))
			{
				num2 = ((buttonX == null) ? (num2 - (empty.Width + 8)) : (num2 - (empty.Width + buttonX.ImageTextSpacing * 2 + 3)));
			}
			if (flag2 && !empty.IsEmpty && buttonX != null && buttonX.ImageTextSpacing != 0)
			{
				if (eImagePosition2 == eImagePosition.Left || eImagePosition2 == eImagePosition.Right)
				{
					empty.Width += buttonX.ImageTextSpacing * 2;
				}
				else
				{
					empty.Height += buttonX.ImageTextSpacing * 2;
				}
			}
			Font val3 = ᤉ._໘(null, _໙: true);
			SizeF sizeF = SizeF.Empty;
			eTextFormat eTextFormat2 = eTextFormat.VerticalCenter;
			if (BarFunctions.IsWindowsXP && BarUtilities.UseTextRenderer)
			{
				eTextFormat2 |= eTextFormat.LeftAndRightPadding;
			}
			if (buttonX != null || (val is RibbonBar && ᤉ.RibbonWordWrap))
			{
				eTextFormat2 |= eTextFormat.WordBreak;
			}
			if (buttonX != null && !buttonX.UseMnemonic)
			{
				eTextFormat2 |= eTextFormat.NoPrefix;
			}
			if (ᤉ.Text != "")
			{
				if (ᤉ.TextMarkupBody == null)
				{
					if (ᤉ.Orientation == eOrientation.Vertical && !flag)
					{
						sizeF = TextDrawing.MeasureStringLegacy(val2, _192F.ಱ(ᤉ.Text), val3, new Size(num2, 0), eTextFormat2);
					}
					else
					{
						sizeF = TextDrawing.MeasureString(val2, _192F.ಱ(ᤉ.Text), val3, num2, eTextFormat2);
						if (BarFunctions.IsWindowsXP && BarUtilities.UseTextRenderer)
						{
							sizeF.Width += 2f;
						}
					}
				}
				else
				{
					Size availableSize = new Size(num2, 1);
					if (num2 == 0)
					{
						availableSize.Width = 1600;
					}
					MarkupDrawContext markupDrawContext = new MarkupDrawContext(val2, val3, Color.Empty, rightToLeft: false);
					markupDrawContext.RightToLeft = flag3;
					ᤉ.TextMarkupBody.Measure(availableSize, markupDrawContext);
					availableSize = ᤉ.TextMarkupBody.Bounds.Size;
					ᤉ.TextMarkupBody._0CC6(new Rectangle(0, 0, availableSize.Width, availableSize.Height), markupDrawContext);
					sizeF = ᤉ.TextMarkupBody.Bounds.Size;
				}
			}
			if (ᤉ.Orientation == eOrientation.Horizontal && (eImagePosition2 == eImagePosition.Left || eImagePosition2 == eImagePosition.Right))
			{
				if (ᤉ.ButtonStyle != 0 || !flag2)
				{
					empty.Width += 4;
				}
				rectangle = Rectangle.Empty;
				if (ᤉ.ButtonStyle != eButtonStyle.TextOnlyAlways && flag2)
				{
					rectangle = ((eImagePosition2 == eImagePosition.Left) ? ((buttonX != null && buttonX.TextAlignment == eButtonTextAlignment.Left) ? new Rectangle(2, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : ((buttonX != null && buttonX.TextAlignment == eButtonTextAlignment.Right) ? new Rectangle(bounds.Width - (empty.Width + (int)Math.Ceiling(sizeF.Width) + 4), (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : ((buttonX != null) ? new Rectangle((int)((float)bounds.Width - (sizeF.Width + (float)empty.Width)) / 2, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : ((!subItemsRect.IsEmpty || ᤉ.ButtonStyle != 0) ? new Rectangle(0, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : new Rectangle((bounds.Width - empty.Width) / 2, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height))))) : ((buttonX != null && buttonX.TextAlignment == eButtonTextAlignment.Left) ? new Rectangle((int)sizeF.Width + 4, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : ((buttonX != null && buttonX.TextAlignment == eButtonTextAlignment.Right) ? new Rectangle(bounds.Width - empty.Width, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : ((buttonX == null) ? new Rectangle(bounds.Width - empty.Width, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : new Rectangle(bounds.Width - (int)((float)bounds.Width - (sizeF.Width + (float)empty.Width)) / 2 - empty.Width, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height)))));
				}
				textDrawRect = Rectangle.Empty;
				if (ᤉ.ButtonStyle != 0 || !flag2)
				{
					textDrawRect = (flag2 ? ((eImagePosition2 == eImagePosition.Left) ? ((buttonX != null && buttonX.TextAlignment == eButtonTextAlignment.Center) ? new Rectangle(rectangle.Right + 1, (int)(((float)bounds.Height - sizeF.Height) / 2f), (int)sizeF.Width, (int)sizeF.Height) : ((buttonX == null || (buttonX.TextAlignment != eButtonTextAlignment.Right && buttonX.TextAlignment != 0)) ? new Rectangle(rectangle.Right + 1, (int)(((float)bounds.Height - sizeF.Height) / 2f), bounds.Width - 3 - rectangle.Width, (int)sizeF.Height) : new Rectangle(rectangle.Right + 1, (int)(((float)bounds.Height - sizeF.Height) / 2f), bounds.Width - 3 - rectangle.Width, (int)sizeF.Height))) : ((buttonX != null && buttonX.TextAlignment == eButtonTextAlignment.Center) ? new Rectangle((int)((float)rectangle.X - sizeF.Width) - 1, (int)(((float)bounds.Height - sizeF.Height) / 2f), (int)sizeF.Width, (int)sizeF.Height) : ((buttonX == null || buttonX.TextAlignment != eButtonTextAlignment.Right) ? new Rectangle(3, (int)(((float)bounds.Height - sizeF.Height) / 2f), rectangle.X - 2, (int)sizeF.Height) : new Rectangle(rectangle.X - ((int)sizeF.Width + 4), (int)(((float)bounds.Height - sizeF.Height) / 2f), (int)sizeF.Width + 2, (int)sizeF.Height)))) : (((buttonX == null || buttonX.TextAlignment != eButtonTextAlignment.Center) && !ᤉ._ບ) ? new Rectangle(3, (int)(((float)bounds.Height - sizeF.Height) / 2f), bounds.Width - 6, (int)sizeF.Height) : new Rectangle((int)(((float)bounds.Width - sizeF.Width) / 2f), (int)(((float)bounds.Height - sizeF.Height) / 2f), (int)sizeF.Width, (int)sizeF.Height)));
				}
			}
			else if (ᤉ.Orientation == eOrientation.Horizontal)
			{
				if (eImagePosition2 == eImagePosition.Top)
				{
					rectangle = new Rectangle(0, (int)((float)bounds.Height - ((float)empty.Height + sizeF.Height)) / 2, bounds.Width, empty.Height);
					textDrawRect = new Rectangle(0, rectangle.Bottom, (int)sizeF.Width, (int)sizeF.Height + 5);
				}
				else
				{
					textDrawRect = new Rectangle((int)((float)bounds.Width - sizeF.Width) / 2, (int)((float)bounds.Height - ((float)empty.Height + sizeF.Height)) / 2, (int)sizeF.Width, (int)sizeF.Height);
					rectangle = new Rectangle(0, textDrawRect.Bottom, bounds.Width, empty.Height + 5);
				}
			}
			else if (eImagePosition2 == eImagePosition.Top || eImagePosition2 == eImagePosition.Left)
			{
				if (flag2)
				{
					rectangle = ((!subItemsRect.IsEmpty || ᤉ.ButtonStyle != 0) ? new Rectangle(0, 0, bounds.Width, empty.Height + 6) : new Rectangle((bounds.Width - empty.Width) / 2, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height));
				}
				textDrawRect = new Rectangle((int)((float)bounds.Width - sizeF.Height) / 2, rectangle.Bottom + 2, (int)sizeF.Height, (int)sizeF.Width + 5);
			}
			else
			{
				textDrawRect = new Rectangle((int)((float)bounds.Width - sizeF.Width) / 2, 0, (int)sizeF.Height, (int)sizeF.Width + 5);
				if (flag2)
				{
					rectangle = new Rectangle(0, textDrawRect.Bottom + 2, bounds.Width, empty.Height + 5);
				}
			}
		}
		finally
		{
			val2.TextRenderingHint = (TextRenderingHint)0;
			val2.SmoothingMode = (SmoothingMode)0;
			val2.Dispose();
		}
		ᤉ._0E60 = rectangle;
		ᤉ.TextDrawRect = textDrawRect;
		ᤉ.SubItemsRect = subItemsRect;
	}
}
internal class _20E7
{
	private Timer ᆪ;

	private ArrayList _20E8 = new ArrayList();

	private ReaderWriterLock _20E9 = new ReaderWriterLock();

	public _20E7()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		ᆪ = new Timer();
		ᆪ.Enabled = false;
		ᆪ.Interval = 800;
		ᆪ.Tick += _20EA;
	}

	private void _20EA(object _٠, EventArgs ؿ)
	{
		ᆪ.Stop();
		_20EB();
	}

	public void _1A17()
	{
		if (ᆪ != null)
		{
			ᆪ.Stop();
			_20EB();
			((Component)(object)ᆪ).Dispose();
			ᆪ = null;
		}
	}

	private void _20EB()
	{
		if (_20E8.Count <= 0)
		{
			return;
		}
		bool isReaderLockHeld = _20E9.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = _20E9.UpgradeToWriterLock(-1);
		}
		else
		{
			_20E9.AcquireWriterLock(-1);
		}
		try
		{
			foreach (PopupItem item in _20E8)
			{
				item.ClosePopup();
			}
			_20E8.Clear();
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_20E9.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_20E9.ReleaseWriterLock();
			}
		}
	}

	public void _20EC(PopupItem _06EC)
	{
		if (ᆪ == null)
		{
			return;
		}
		bool isReaderLockHeld = _20E9.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = _20E9.UpgradeToWriterLock(-1);
		}
		else
		{
			_20E9.AcquireWriterLock(-1);
		}
		try
		{
			_20E8.Add(_06EC);
			ᆪ.Start();
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_20E9.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_20E9.ReleaseWriterLock();
			}
		}
	}

	public void _20ED()
	{
		bool isReaderLockHeld = _20E9.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = _20E9.UpgradeToWriterLock(-1);
		}
		else
		{
			_20E9.AcquireWriterLock(-1);
		}
		try
		{
			_20E8.Clear();
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_20E9.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_20E9.ReleaseWriterLock();
			}
		}
	}
}
internal class _20EF : NativeWindow
{
	private const int _20F1 = 560;

	private const int _20F2 = 564;

	public bool _20F3;

	public event EventHandler _20F0;

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 560 || ((Message)(ref m)).Msg == 564)
		{
			if (this._20F0 != null)
			{
				this._20F0(this, new EventArgs());
			}
			if (_20F3)
			{
				_20F3 = false;
				return;
			}
		}
		((NativeWindow)this).WndProc(ref m);
	}
}
internal class _2189
{
	internal static void ᡟ(BaseItem ઇ, KeyEventArgs ᨍ)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Invalid comparison between Unknown and I4
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Invalid comparison between Unknown and I4
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Invalid comparison between Unknown and I4
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Invalid comparison between Unknown and I4
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Invalid comparison between Unknown and I4
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Invalid comparison between Unknown and I4
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Invalid comparison between Unknown and I4
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Invalid comparison between Unknown and I4
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Invalid comparison between Unknown and I4
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		if (ઇ.SubItems.Count == 0 || ᨍ.Handled)
		{
			return;
		}
		BaseItem baseItem = ઇ.ExpandedItem();
		if (baseItem != null)
		{
			baseItem.InternalKeyDown(ᨍ);
			if (ᨍ.Handled)
			{
				return;
			}
		}
		eOrientation eOrientation2 = ઇ.Orientation;
		if (ઇ is ItemContainer)
		{
			eOrientation2 = ((ItemContainer)ઇ).LayoutOrientation;
		}
		if ((eOrientation2 == eOrientation.Horizontal && ((int)ᨍ.KeyCode == 37 || (int)ᨍ.KeyCode == 39 || (ઇ.HotSubItem == null && ((int)ᨍ.KeyCode == 40 || (int)ᨍ.KeyCode == 38)))) || (eOrientation2 == eOrientation.Vertical && ((int)ᨍ.KeyCode == 38 || (int)ᨍ.KeyCode == 40 || (ઇ.HotSubItem == null && ((int)ᨍ.KeyCode == 39 || (int)ᨍ.KeyCode == 37)))))
		{
			if (ઇ.HotSubItem != null)
			{
				ઇ.HotSubItem.InternalMouseLeave();
				if (ઇ.AutoExpand && ઇ.HotSubItem.Expanded)
				{
					ઇ.HotSubItem.Expanded = false;
				}
			}
			if ((int)ᨍ.KeyCode == 37 || (int)ᨍ.KeyCode == 38)
			{
				int num = 0;
				if (ઇ.HotSubItem != null)
				{
					num = ઇ.SubItems.IndexOf(ઇ.HotSubItem) - 1;
				}
				if (num < 0)
				{
					num = ઇ.SubItems.Count - 1;
				}
				BaseItem baseItem2 = null;
				bool flag = false;
				do
				{
					for (int num2 = num; num2 >= 0; num2--)
					{
						baseItem2 = ઇ.SubItems[num2];
						if (_218A(baseItem2))
						{
							num = num2;
							break;
						}
					}
					if (!_218A(ઇ.SubItems[num]))
					{
						if (!flag)
						{
							num = ઇ.SubItems.Count - 1;
							flag = true;
						}
						else
						{
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
				while (flag);
				ઇ.HotSubItem = ઇ.SubItems[num];
			}
			else
			{
				int i = 0;
				if (ઇ.HotSubItem != null)
				{
					i = ઇ.SubItems.IndexOf(ઇ.HotSubItem) + 1;
				}
				for (; i < ઇ.SubItems.Count && !_218A(ઇ.SubItems[i]); i++)
				{
				}
				if (i >= ઇ.SubItems.Count)
				{
					i = 0;
				}
				BaseItem baseItem3 = null;
				for (int j = i; j < ઇ.SubItems.Count; j++)
				{
					baseItem3 = ઇ.SubItems[j];
					if (_218A(baseItem3))
					{
						i = j;
						break;
					}
				}
				ઇ.HotSubItem = ઇ.SubItems[i];
			}
			if (ઇ.HotSubItem != null)
			{
				if (ઇ.HotSubItem is ItemContainer)
				{
					((ItemContainer)ઇ.HotSubItem)._1030();
				}
				else
				{
					ઇ.HotSubItem.InternalMouseEnter();
					ઇ.HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, ઇ.HotSubItem.LeftInternal + 1, ઇ.HotSubItem.TopInternal + 1, 0));
					if (ઇ.ContainerControl is IScrollableItemControl scrollableItemControl)
					{
						scrollableItemControl.KeyboardItemSelected(ઇ.HotSubItem);
					}
				}
			}
			ᨍ.Handled = true;
			return;
		}
		if ((int)ᨍ.KeyCode == 27)
		{
			if (baseItem != null)
			{
				baseItem.Expanded = false;
				ᨍ.Handled = true;
				return;
			}
			object containerControl = ઇ.ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val is Bar)
			{
				Bar bar = val as Bar;
				if (bar.BarState == eBarState.Popup)
				{
					bar.ParentItem.Expanded = false;
				}
				else if (ઇ.AutoExpand)
				{
					ઇ.AutoExpand = false;
				}
				else if (((Control)bar).Focused || bar._09A9)
				{
					bar._09A9 = false;
					bar.ReleaseFocus();
				}
				ᨍ.Handled = true;
			}
			else if (val is ItemControl)
			{
				ItemControl itemControl = val as ItemControl;
				if (ઇ.AutoExpand)
				{
					ઇ.AutoExpand = false;
				}
				else if (((Control)itemControl).Focused || itemControl._09A9)
				{
					itemControl._09A9 = false;
					itemControl.ReleaseFocus();
				}
			}
			return;
		}
		BaseItem baseItem4 = ઇ.ExpandedItem();
		if (baseItem4 != null)
		{
			baseItem4.InternalKeyDown(ᨍ);
			return;
		}
		int num3 = 0;
		if (ᨍ.Shift)
		{
			try
			{
				byte[] array = new byte[256];
				if (_089F.GetKeyboardState(array))
				{
					byte[] array2 = new byte[2];
					if (_089F.ToAscii((uint)ᨍ.KeyValue, 0u, array, array2, 0u) != 0)
					{
						num3 = array2[0];
					}
				}
			}
			catch (Exception)
			{
				num3 = 0;
			}
		}
		if (num3 == 0)
		{
			num3 = (int)_089F.MapVirtualKey((uint)ᨍ.KeyValue, 2u);
		}
		if (ઇ.HotSubItem != null)
		{
			ઇ.HotSubItem.InternalKeyDown(ᨍ);
		}
	}

	private static bool _218A(BaseItem _218B)
	{
		if (_218B == null)
		{
			return false;
		}
		if (!_218B.Visible || !_218B.GetEnabled() || (_218B is ListBoxItem && ((ListBoxItem)_218B).IsSelected))
		{
			return false;
		}
		if (_218B is LabelItem)
		{
			return false;
		}
		return true;
	}
}
internal class _218C : EventArgs
{
	public IntPtr _0A65 = IntPtr.Zero;

	public IntPtr _੦ = IntPtr.Zero;

	public IntPtr _੧ = IntPtr.Zero;

	public _218C(IntPtr hwnd, IntPtr wparam, IntPtr lparam)
	{
		_0A65 = hwnd;
		_੦ = wparam;
		_੧ = lparam;
	}
}
internal delegate void _218D(object sender, _218C e);
internal class _21E3 : ImageItem
{
	private int __21E7;

	private bool __21E8;

	internal int _21E4 => __21E7;

	internal LabelItem _21E5 => m_SubItems[0] as LabelItem;

	internal CrumbBarOverflowButton _21E6 => (CrumbBarOverflowButton)m_SubItems[1];

	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override bool Expanded
	{
		get
		{
			return m_Expanded;
		}
		set
		{
			base.Expanded = value;
			if (!value)
			{
				BaseItem.CollapseSubItems(this);
			}
		}
	}

	public _21E3()
	{
		LabelItem item = new LabelItem("sys_crumbbarimagelabel");
		SubItems.Add(item);
		CrumbBarOverflowButton item2 = new CrumbBarOverflowButton("sys_crumbbaroverflowbutton");
		SubItems.Add(item2);
		m_IsContainer = true;
	}

	public override void RecalcSize()
	{
		if (__21E8)
		{
			return;
		}
		_ = m_Rect.Location;
		Size size = m_Rect.Size;
		__21E7 = 0;
		if (m_SubItems == null)
		{
			return;
		}
		_21E9();
		ButtonItem buttonItem = _21E6;
		LabelItem labelItem = _21E5;
		buttonItem.Visible = false;
		Rectangle rect = m_Rect;
		if (labelItem.Visible)
		{
			labelItem.RecalcSize();
			if (labelItem.HeightInternal > __21E7)
			{
				__21E7 = labelItem.HeightInternal;
			}
			labelItem.HeightInternal = size.Height;
			rect.X += labelItem.WidthInternal;
			rect.Width -= labelItem.WidthInternal;
			labelItem.Displayed = true;
		}
		Point location = rect.Location;
		bool flag = false;
		BaseItem[] array = new BaseItem[m_SubItems.Count - 2];
		if (m_SubItems.Count > 2)
		{
			m_SubItems._0AF6(array, 2);
		}
		for (int num = array.Length - 1; num >= 0; num--)
		{
			BaseItem baseItem = array[num];
			if (!baseItem.Visible)
			{
				baseItem.Displayed = false;
			}
			else
			{
				if (!flag)
				{
					baseItem.LeftInternal = location.X;
					baseItem.TopInternal = location.Y;
					baseItem.HeightInternal = rect.Height;
					baseItem.RecalcSize();
					if (baseItem.HeightInternal > __21E7)
					{
						__21E7 = baseItem.HeightInternal;
					}
					baseItem.HeightInternal = rect.Height;
					if (location.X + baseItem.WidthInternal > rect.Right || (location.X + baseItem.WidthInternal + 16 > rect.Right && num > 0))
					{
						flag = true;
					}
					else
					{
						location.X += baseItem.WidthInternal;
						baseItem.Displayed = true;
					}
				}
				if (flag)
				{
					__21E8 = true;
					m_SubItems.Remove(baseItem);
					buttonItem.SubItems.Insert(0, baseItem);
					__21E8 = false;
				}
			}
		}
		if (flag)
		{
			buttonItem.Visible = true;
			buttonItem.Displayed = true;
			buttonItem.HeightInternal = rect.Height;
			buttonItem.RecalcSize();
			buttonItem.LeftInternal = rect.X;
			buttonItem.TopInternal = rect.Y;
			buttonItem.HeightInternal = rect.Height;
			rect.X += buttonItem.WidthInternal;
			rect.Width -= buttonItem.WidthInternal;
		}
		location = rect.Location;
		for (int i = 2; i < m_SubItems.Count; i++)
		{
			BaseItem baseItem2 = m_SubItems[i];
			if (baseItem2.Visible)
			{
				if (baseItem2.WidthInternal + location.X > rect.Right)
				{
					baseItem2.Displayed = false;
					location.X = rect.Right;
				}
				else
				{
					baseItem2.LeftInternal = location.X;
					location.X += baseItem2.WidthInternal;
				}
			}
		}
		base.RecalcSize();
	}

	internal void _21E9()
	{
		ButtonItem buttonItem = _21E6;
		if (buttonItem.SubItems.Count != 0)
		{
			BaseItem[] array = new BaseItem[buttonItem.SubItems.Count];
			buttonItem.SubItems.CopyTo(array, 0);
			buttonItem.SubItems.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				m_SubItems.Insert(i + 2, array[i]);
			}
		}
	}

	internal void _21EA()
	{
		__21E8 = true;
		while (m_SubItems.Count > 2)
		{
			m_SubItems.RemoveAt(m_SubItems.Count - 1);
		}
		__21E8 = false;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (SuspendLayout)
		{
			return;
		}
		_ = pa.Graphics;
		if (m_NeedRecalcSize)
		{
			RecalcSize();
		}
		if (m_SubItems == null)
		{
			return;
		}
		foreach (BaseItem subItem in m_SubItems)
		{
			if (subItem.Visible && subItem.Displayed)
			{
				subItem.Paint(pa);
			}
		}
	}

	public override BaseItem Copy()
	{
		_21E3 _21E32 = new _21E3();
		CopyToItem(_21E32);
		return _21E32;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		_21E3 objCopy = copy as _21E3;
		base.CopyToItem(objCopy);
	}

	protected internal override void OnSubItemExpandChange(BaseItem item)
	{
		base.OnSubItemExpandChange(item);
		if (item.Expanded)
		{
			Expanded = true;
		}
		else
		{
			base.Expanded = false;
		}
	}
}
internal class _21EB : ᯊ
{
	protected override Rectangle GetTextRectangle(ButtonItem button, ItemPaintArgs pa, eTextFormat stringFormat, ᇎ image)
	{
		Rectangle textRectangle = base.GetTextRectangle(button, pa, stringFormat, image);
		if (image == null && string.IsNullOrEmpty(button.SymbolRealized))
		{
			textRectangle.Inflate(-3, 0);
		}
		return textRectangle;
	}

	public override eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		eTextFormat stringFormat = base.GetStringFormat(button, pa, image);
		return stringFormat & ~(stringFormat & eTextFormat.EndEllipsis);
	}

	protected override void PaintState(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r, bool isMouseDown)
	{
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		if (r.IsEmpty || !_192F._1930(button, pa) || r.Width == 0 || r.Height == 0)
		{
			return;
		}
		if (!(button is RibbonTabItem ribbonTabItem) || IsOnMenu(button, pa))
		{
			base.PaintState(button, pa, image, r, isMouseDown);
			return;
		}
		_ = pa.IsOnMenu;
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = ጧ(ribbonTabItem);
		if (office2007RibbonTabItemColorTable == null)
		{
			return;
		}
		bool controlExpanded = pa.ControlExpanded;
		Office2007RibbonTabItemStateColorTable office2007RibbonTabItemStateColorTable = ῡ(office2007RibbonTabItemColorTable, ribbonTabItem, controlExpanded);
		if (office2007RibbonTabItemStateColorTable == null)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		int cornerSize = office2007RibbonTabItemColorTable.CornerSize;
		Region clip = graphics.Clip;
		try
		{
			Rectangle rectangle = r;
			rectangle.Inflate(1, 0);
			graphics.SetClip(rectangle, (CombineMode)0);
			if (office2007RibbonTabItemStateColorTable != null)
			{
				eDotNetBarStyle effectiveStyle = button.EffectiveStyle;
				GraphicsPath val = ᔹ(r, cornerSize, _21ED: true, effectiveStyle);
				try
				{
					DisplayHelp.FillPath(graphics, val, office2007RibbonTabItemStateColorTable.Background);
					DisplayHelp.DrawGradientPathBorder(graphics, val, office2007RibbonTabItemStateColorTable.OuterBorder, 1);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				if (!StyleManager.IsMetro(effectiveStyle) && ribbonTabItem.Checked && controlExpanded && ribbonTabItem.ᎇ)
				{
					SmoothingMode smoothingMode = graphics.SmoothingMode;
					graphics.SmoothingMode = (SmoothingMode)0;
					if ((double)base.ᘽ.RibbonControl.TabsBackground.Start.GetBrightness() > 0.5 && !office2007RibbonTabItemStateColorTable.OuterBorder.IsEmpty)
					{
						GraphicsPath val2 = new GraphicsPath();
						try
						{
							val2.AddRectangle(new Rectangle(r.Right - 1, r.Y + cornerSize + 1, 1, r.Height - cornerSize - 3));
							DisplayHelp.FillPath(graphics, val2, Color.FromArgb(96, office2007RibbonTabItemStateColorTable.OuterBorder.Start), Color.FromArgb(32, office2007RibbonTabItemStateColorTable.OuterBorder.End), 90);
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
						GraphicsPath val3 = new GraphicsPath();
						try
						{
							val3.AddRectangle(new Rectangle(r.X + 1, r.Y + cornerSize + 1, 1, r.Height - cornerSize - 3));
							DisplayHelp.FillPath(graphics, val3, Color.FromArgb(32, office2007RibbonTabItemStateColorTable.OuterBorder.Start), Color.FromArgb(8, office2007RibbonTabItemStateColorTable.OuterBorder.End), 90);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					graphics.SmoothingMode = smoothingMode;
				}
				Rectangle ल = r;
				ल.Inflate(-1, 0);
				ल.Height--;
				ल.Y++;
				GraphicsPath val4 = ᔹ(ल, cornerSize, _21ED: true, effectiveStyle);
				try
				{
					DisplayHelp.DrawGradientPathBorder(graphics, val4, office2007RibbonTabItemStateColorTable.InnerBorder, 1);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				if (ribbonTabItem.Checked && controlExpanded && ribbonTabItem.ᎇ)
				{
					SolidBrush val5 = new SolidBrush(office2007RibbonTabItemStateColorTable.InnerBorder.Start);
					try
					{
						SmoothingMode smoothingMode2 = graphics.SmoothingMode;
						graphics.SmoothingMode = (SmoothingMode)3;
						graphics.FillRectangle((Brush)(object)val5, new Rectangle(ल.X + cornerSize, ल.Y + 1, ल.Width - cornerSize * 2, 2));
						graphics.SmoothingMode = smoothingMode2;
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				float num = 0.6f;
				float num2 = 0.4f;
				Rectangle rectangle2 = r;
				Rectangle rectangle3 = new Rectangle(rectangle2.X, rectangle2.Y + (int)((float)rectangle2.Height * num), rectangle2.Width, (int)((float)rectangle2.Height * num2));
				if (!office2007RibbonTabItemStateColorTable.BackgroundHighlight.IsEmpty)
				{
					Rectangle rectangle4 = new Rectangle(rectangle3.X, rectangle3.Y, rectangle2.Width, rectangle2.Height);
					GraphicsPath val6 = new GraphicsPath();
					val6.AddEllipse(rectangle4);
					PathGradientBrush val7 = new PathGradientBrush(val6);
					val7.CenterColor = office2007RibbonTabItemStateColorTable.BackgroundHighlight.Start;
					val7.SurroundColors = new Color[1] { office2007RibbonTabItemStateColorTable.BackgroundHighlight.End };
					val7.CenterPoint = new PointF(rectangle4.X + rectangle4.Width / 2, rectangle2.Bottom + 2);
					Blend val8 = new Blend();
					val8.Factors = new float[3] { 0f, 0.8f, 1f };
					val8.Positions = new float[3] { 0f, 0.55f, 1f };
					val7.Blend = val8;
					graphics.FillRectangle((Brush)(object)val7, rectangle3);
					((Brush)val7).Dispose();
					val6.Dispose();
				}
			}
			if (ribbonTabItem._136C && !ribbonTabItem.Checked && !ribbonTabItem.IsMouseOver && office2007RibbonTabItemColorTable != null && office2007RibbonTabItemColorTable.Selected != null)
			{
				Color start = base.ᘽ.RibbonControl.OuterBorder.Start;
				if (!start.IsEmpty)
				{
					DisplayHelp.DrawGradientLine(graphics, new Point(r.Right - 1, r.Y), new Point(r.Right - 1, r.Bottom - 1), Color.Transparent, start, 90, 1, new float[3] { 0f, 0.8f, 1f }, new float[3] { 0f, 0.5f, 1f });
				}
			}
			graphics.Clip = clip;
		}
		finally
		{
			if (clip != null)
			{
				clip.Dispose();
			}
		}
	}

	private GraphicsPath ᔹ(Rectangle ल, int _21EC, bool _21ED, eDotNetBarStyle _0657)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		int num = ((_0657 == eDotNetBarStyle.Windows7 || _0657 == eDotNetBarStyle.Office2010) ? 1 : 2);
		if (StyleManager.IsMetro(_0657))
		{
			num = 0;
		}
		if (_21ED)
		{
			val.AddLine(ल.X, ल.Bottom, ल.X + num, ल.Bottom - num);
		}
		else
		{
			val.AddLine(ल.X, ल.Bottom, ल.X, ल.Y + _21EC);
		}
		ल.Inflate(-num, 0);
		ElementStyleDisplay.AddCornerArc(val, ल, _21EC, eCornerArc.TopLeft);
		ElementStyleDisplay.AddCornerArc(val, ल, _21EC, eCornerArc.TopRight);
		if (_21ED)
		{
			val.AddLine(ल.Right, ल.Bottom - num, ल.Right + num, ल.Bottom);
		}
		else
		{
			val.AddLine(ल.Right, ल.Y + _21EC, ल.Right, ल.Bottom);
		}
		return val;
	}

	protected override Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
	{
		if (!_192F._1930(button, pa) || !(button is RibbonTabItem))
		{
			return base.GetTextColor(button, pa);
		}
		RibbonTabItem ᆰ = button as RibbonTabItem;
		Color result = Color.Empty;
		Office2007RibbonTabItemStateColorTable office2007RibbonTabItemStateColorTable = ῡ(ጧ(ᆰ), ᆰ, pa.ControlExpanded);
		if (office2007RibbonTabItemStateColorTable != null)
		{
			if (pa.GlassEnabled && !office2007RibbonTabItemStateColorTable.GlassText.IsEmpty)
			{
				return office2007RibbonTabItemStateColorTable.GlassText;
			}
			result = office2007RibbonTabItemStateColorTable.Text;
		}
		if (result.IsEmpty)
		{
			return base.GetTextColor(button, pa);
		}
		return result;
	}

	private Office2007RibbonTabItemColorTable ጧ(RibbonTabItem ᆰ)
	{
		return ጧ(base.ᘽ, ᆰ);
	}

	internal static Office2007RibbonTabItemColorTable ጧ(Office2007ColorTable ቯ, RibbonTabItem ᆰ)
	{
		if (ቯ == null)
		{
			return null;
		}
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = ቯ.RibbonTabItemColors[ᆰ.ຽ()];
		if (office2007RibbonTabItemColorTable == null && ቯ.RibbonTabItemColors.Count > 0)
		{
			office2007RibbonTabItemColorTable = ቯ.RibbonTabItemColors[0];
		}
		return office2007RibbonTabItemColorTable;
	}

	internal static Office2007RibbonTabItemColorTable ጧ(Office2007ColorTable ቯ)
	{
		if (ቯ == null)
		{
			return null;
		}
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = ቯ.RibbonTabItemColors["Default"];
		if (office2007RibbonTabItemColorTable == null && ቯ.RibbonTabItemColors.Count > 0)
		{
			office2007RibbonTabItemColorTable = ቯ.RibbonTabItemColors[0];
		}
		return office2007RibbonTabItemColorTable;
	}

	internal static Office2007RibbonTabItemStateColorTable ῡ(Office2007RibbonTabItemColorTable _21EE, RibbonTabItem ᆰ, bool _21EF)
	{
		if (_21EE == null)
		{
			return null;
		}
		Office2007RibbonTabItemStateColorTable result = null;
		if (!ᆰ.ᎇ)
		{
			result = _21EE.Default;
		}
		else if (ᆰ.Checked && ᆰ.IsMouseOver && _21EF)
		{
			result = _21EE.SelectedMouseOver;
		}
		else if (ᆰ.Checked && _21EF)
		{
			result = _21EE.Selected;
		}
		else if (ᆰ.IsMouseOver)
		{
			result = _21EE.MouseOver;
		}
		else if (ᆰ.GetEnabled())
		{
			result = _21EE.Default;
		}
		return result;
	}

	public override void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, ᇎ image)
	{
		eDotNetBarStyle effectiveStyle = button.EffectiveStyle;
		if ((effectiveStyle != eDotNetBarStyle.Office2010 && !StyleManager.IsMetro(effectiveStyle)) || !pa.GlassEnabled)
		{
			base.PaintButtonText(button, pa, textColor, image);
			return;
		}
		Rectangle textRectangle = GetTextRectangle(button, pa, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter, image);
		ThemeTextFormat _21FD = (ThemeTextFormat)1048613;
		bool _21FF = true;
		_21F8._21FC(pa.Graphics, button.Text, pa.Font, textRectangle, _21FD, textColor, _21FE: true, _21FF, 10);
	}
}
internal enum _21F0
{
	ᘶ,
	_21F1,
	_21F2,
	_1BF3,
	_21F3,
	_21F4,
	_21F5,
	_21F6,
	_21F7,
	_20AE
}
internal class _21F8 : _2205, ᙓ
{
	private class _2EF2 : IComparer
	{
		private int _1C24(object و, object ى)
		{
			if (و is Rectangle && ى is Rectangle)
			{
				return ((Rectangle)و).Width - ((Rectangle)ى).Width;
			}
			return new CaseInsensitiveComparer().Compare(و, ى);
		}

		int IComparer.Compare(object و, object ى)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ᰤ
			return this._1C24(و, ى);
		}
	}

	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void PaintBackground(RibbonControlRendererEventArgs e)
	{
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Expected O, but got Unknown
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		RibbonControl ribbonControl = e.RibbonControl;
		Office2007RibbonColorTable ribbonControl2 = ຊ.RibbonControl;
		Office2007RibbonTabItemStateColorTable office2007RibbonTabItemStateColorTable = null;
		PaintCaptionBackground(e, ((Control)ribbonControl).DisplayRectangle);
		if (ribbonControl.Expanded && ribbonControl.RibbonStrip._0D63)
		{
			int cornerSize = ribbonControl2.CornerSize;
			Rectangle empty = Rectangle.Empty;
			if (ribbonControl.SelectedRibbonTabItem == null || !ribbonControl.Expanded)
			{
				empty = Rectangle.Empty;
			}
			else
			{
				empty = ribbonControl.SelectedRibbonTabItem.DisplayRectangle;
				office2007RibbonTabItemStateColorTable = _21EB.ῡ(_21EB.ጧ(ᘽ, ribbonControl.SelectedRibbonTabItem), ribbonControl.SelectedRibbonTabItem, ribbonControl.Expanded);
			}
			Rectangle _21FA = new Rectangle(((Control)ribbonControl).ClientRectangle.X, ((Control)ribbonControl).ClientRectangle.Bottom - cornerSize, ((Control)ribbonControl).ClientRectangle.Width - 1, cornerSize);
			if (!empty.IsEmpty)
			{
				_21FA = new Rectangle(((Control)ribbonControl).ClientRectangle.X, empty.Bottom, ((Control)ribbonControl).ClientRectangle.Width - 1, ((Control)ribbonControl).ClientRectangle.Bottom - empty.Bottom);
			}
			GraphicsPath val = _21F9(_21FA, cornerSize);
			try
			{
				Region val2 = null;
				if (graphics.Clip != null)
				{
					val2 = graphics.Clip;
				}
				if (!empty.IsEmpty)
				{
					if (office2007RibbonTabItemStateColorTable != null)
					{
						Pen val3 = new Pen(office2007RibbonTabItemStateColorTable.Background.End.IsEmpty ? office2007RibbonTabItemStateColorTable.Background.Start : office2007RibbonTabItemStateColorTable.Background.End, 1f);
						try
						{
							graphics.DrawLine(val3, empty.X + 1, empty.Bottom, empty.Right - 1, empty.Bottom);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					graphics.SetClip(new Rectangle(empty.X + 1, empty.Bottom, empty.Width - 1, 1), (CombineMode)4);
					if (StyleManager.IsMetro(ribbonControl.EffectiveStyle) && ribbonControl.GetApplicationButton() is ApplicationButton { Displayed: not false, Bounds: var bounds } applicationButton)
					{
						graphics.TranslateTransform(0f, 1f);
						applicationButton.Paint(e.က);
						graphics.TranslateTransform(0f, -1f);
						bounds.Inflate(0, 2);
						bounds.Width -= 3;
						graphics.SetClip(bounds, (CombineMode)4);
					}
				}
				Pen val4 = new Pen(ribbonControl2.OuterBorder.Start, 1f);
				try
				{
					graphics.DrawPath(val4, val);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				if (!empty.IsEmpty)
				{
					if (val2 != null)
					{
						graphics.Clip = val2;
					}
					else
					{
						graphics.ResetClip();
					}
				}
				if (val2 != null)
				{
					val2.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			_21FA.Y++;
			if (ribbonControl2.InnerBorder == null)
			{
				return;
			}
			GraphicsPath val5 = _21F9(_21FA, cornerSize);
			try
			{
				Pen val6 = new Pen(ribbonControl2.InnerBorder.Start, 1f);
				try
				{
					graphics.DrawPath(val6, val5);
					return;
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		int num = ((Control)ribbonControl).ClientRectangle.Bottom - 2;
		if (!ribbonControl2.TabDividerBorder.IsEmpty)
		{
			Pen val7 = new Pen(ribbonControl2.TabDividerBorder, 1f);
			try
			{
				graphics.DrawLine(val7, 0, num, ((Control)ribbonControl).ClientRectangle.Right, num);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
			num++;
		}
		if (!ribbonControl2.TabDividerBorderLight.IsEmpty)
		{
			Pen val8 = new Pen(ribbonControl2.TabDividerBorderLight, 1f);
			try
			{
				graphics.DrawLine(val8, 0, num, ((Control)ribbonControl).ClientRectangle.Right, num);
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
	}

	public override void PaintCaptionBackground(RibbonControlRendererEventArgs e, Rectangle displayRect)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		RibbonControl ribbonControl = e.RibbonControl;
		Office2007RibbonColorTable ribbonControl2 = ຊ.RibbonControl;
		int num = _21FB(ribbonControl.Items);
		if (num == -1)
		{
			num = ribbonControl.RibbonStrip.ක() + 2;
		}
		if (num > 2)
		{
			num = ((!e.GlassEnabled || ribbonControl.EffectiveStyle != eDotNetBarStyle.Windows7) ? (num - 3) : (num - 2));
			if (!e.GlassEnabled)
			{
				if (!ribbonControl2.TabDividerBorder.IsEmpty)
				{
					Pen val = new Pen(ribbonControl2.TabDividerBorder, 1f);
					try
					{
						graphics.DrawLine(val, 0, num, displayRect.Right + 12, num);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					num++;
				}
				if (!ribbonControl2.TabDividerBorderLight.IsEmpty)
				{
					Pen val2 = new Pen(ribbonControl2.TabDividerBorderLight, 1f);
					try
					{
						graphics.DrawLine(val2, 0, num, displayRect.Right + 12, num);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
			}
			else if (ribbonControl.EffectiveStyle != eDotNetBarStyle.Office2010 && !StyleManager.IsMetro(ribbonControl.EffectiveStyle))
			{
				Pen val3 = new Pen(ຊ.Form.Active.BorderColors[0], 1f);
				try
				{
					graphics.DrawLine(val3, 0, num, displayRect.Right + 12, num);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		if (!ribbonControl.RibbonStrip.CaptionVisible)
		{
			return;
		}
		Office2007FormStateColorTable office2007FormStateColorTable = ຊ.Form.Active;
		Form val4 = ((Control)ribbonControl).FindForm();
		if (!ribbonControl.Ꮬ && val4 != null && ((val4 != Form.ActiveForm && val4.MdiParent == null) || (val4.MdiParent != null && val4.MdiParent.ActiveMdiChild != val4)))
		{
			office2007FormStateColorTable = ຊ.Form.Inactive;
		}
		Rectangle rectangle = new Rectangle(displayRect.X, displayRect.Y, displayRect.Width, num - 1);
		if (rectangle.Height <= 0)
		{
			rectangle.Height = ribbonControl.RibbonStrip.ක();
		}
		if (!e.GlassEnabled)
		{
			Rectangle rectangle2 = rectangle;
			if (val4 is RibbonForm)
			{
				rectangle2.Width = ((Control)(RibbonForm)(object)val4).Width;
			}
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)0;
			if (!office2007FormStateColorTable.CaptionTopBackground.End.IsEmpty && !office2007FormStateColorTable.CaptionBottomBackground.End.IsEmpty)
			{
				LinearGradientBrush val5 = new LinearGradientBrush(rectangle2, office2007FormStateColorTable.CaptionTopBackground.Start, office2007FormStateColorTable.CaptionBottomBackground.End, (float)office2007FormStateColorTable.CaptionTopBackground.GradientAngle);
				try
				{
					ColorBlend val6 = new ColorBlend(4);
					val6.Colors = new Color[4]
					{
						office2007FormStateColorTable.CaptionTopBackground.Start,
						office2007FormStateColorTable.CaptionTopBackground.End,
						office2007FormStateColorTable.CaptionBottomBackground.Start,
						office2007FormStateColorTable.CaptionBottomBackground.End
					};
					val6.Positions = new float[4] { 0f, 0.4f, 0.4f, 1f };
					val5.InterpolationColors = val6;
					graphics.FillRectangle((Brush)(object)val5, rectangle2);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			else
			{
				LinearGradientBrush val7 = new LinearGradientBrush(rectangle2, office2007FormStateColorTable.CaptionTopBackground.Start, office2007FormStateColorTable.CaptionBottomBackground.Start, (float)office2007FormStateColorTable.CaptionTopBackground.GradientAngle);
				try
				{
					graphics.FillRectangle((Brush)(object)val7, rectangle2);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
			graphics.SmoothingMode = smoothingMode;
		}
		ribbonControl.RibbonStrip._0D65 = rectangle;
	}

	private GraphicsPath _21F9(Rectangle _21FA, int _21EC)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		if (_21EC > 1)
		{
			if (_21FA.Height > _21EC)
			{
				val.AddLine(_21FA.X, _21FA.Bottom, _21FA.X, _21FA.Y + _21EC);
			}
			ElementStyleDisplay.AddCornerArc(val, _21FA, _21EC, eCornerArc.TopLeft);
			val.AddLine(_21FA.X + _21EC, _21FA.Y, _21FA.Right - _21EC, _21FA.Y);
			ElementStyleDisplay.AddCornerArc(val, _21FA, _21EC, eCornerArc.TopRight);
			if (_21FA.Height > _21EC)
			{
				val.AddLine(_21FA.Right, _21FA.Y + _21EC, _21FA.Right, _21FA.Bottom);
			}
		}
		else
		{
			val.AddLine(_21FA.X, _21FA.Y, _21FA.Right, _21FA.Y);
		}
		return val;
	}

	private int _21FB(SubItemsCollection _0BDE)
	{
		foreach (BaseItem item in _0BDE)
		{
			if (item is RibbonTabItem && item.Visible)
			{
				return item.Bounds.Top;
			}
		}
		return -1;
	}

	public override void PaintCaptionText(RibbonControlRendererEventArgs e)
	{
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Invalid comparison between Unknown and I4
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Invalid comparison between Unknown and I4
		RibbonStrip ribbonStrip = e.RibbonControl.RibbonStrip;
		if (!ribbonStrip.CaptionVisible || ribbonStrip._0D65.IsEmpty)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		bool flag = false;
		bool flag2 = true;
		Office2007FormStateColorTable office2007FormStateColorTable = ຊ.Form.Active;
		Form val = ((Control)ribbonStrip).FindForm();
		bool flag3 = true;
		if (val is RibbonForm && !((RibbonForm)(object)val).RenderFormText)
		{
			flag3 = false;
		}
		if (val != null && ((val != Form.ActiveForm && val.MdiParent == null) || (val.MdiParent != null && val.MdiParent.ActiveMdiChild != val)))
		{
			office2007FormStateColorTable = ຊ.Form.Inactive;
			flag2 = false;
		}
		string titleText = e.RibbonControl.TitleText;
		string text = titleText;
		bool flag4 = e.RibbonControl.RibbonStrip._0D62 != null;
		if (flag4)
		{
			text = e.RibbonControl.RibbonStrip._0D62._0CBB;
		}
		if (val != null)
		{
			if (titleText == "")
			{
				titleText = ((Control)val).Text;
				text = titleText;
			}
			flag = (int)val.WindowState == 2;
		}
		Rectangle ഡ = ribbonStrip._0D65;
		if (!ribbonStrip.QuickToolbarBounds.IsEmpty)
		{
			DisplayHelp.ഠ(ref ഡ, ribbonStrip.QuickToolbarBounds);
		}
		else
		{
			BaseItem applicationButton = e.RibbonControl.GetApplicationButton();
			if (applicationButton != null && applicationButton.Visible && applicationButton.Displayed)
			{
				DisplayHelp.ഠ(ref ഡ, applicationButton.Bounds);
			}
			else
			{
				DisplayHelp.ഠ(ref ഡ, new Rectangle(0, 0, 22, 22));
			}
		}
		if (!ribbonStrip._൦.IsEmpty)
		{
			DisplayHelp.ഠ(ref ഡ, ribbonStrip._൦);
		}
		ArrayList arrayList = new ArrayList(5);
		ArrayList arrayList2 = new ArrayList(5);
		if (ribbonStrip.TabGroupsVisible)
		{
			foreach (RibbonTabItemGroup tabGroup in ribbonStrip.TabGroups)
			{
				foreach (Rectangle displayPosition in tabGroup.DisplayPositions)
				{
					if (arrayList.Count > 0)
					{
						arrayList2.Clear();
						Rectangle[] array = new Rectangle[arrayList.Count];
						arrayList.CopyTo(array);
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i].IntersectsWith(displayPosition))
							{
								arrayList2.Add(i);
								Rectangle[] c = DisplayHelp.ExcludeRectangle(array[i], displayPosition);
								arrayList.AddRange(c);
							}
						}
						foreach (int item in arrayList2)
						{
							arrayList.RemoveAt(item);
						}
					}
					else if (displayPosition.IntersectsWith(ഡ))
					{
						Rectangle[] array2 = DisplayHelp.ExcludeRectangle(ഡ, displayPosition);
						if (array2.Length > 0)
						{
							arrayList.AddRange(array2);
							break;
						}
					}
				}
			}
		}
		Font captionFont = SystemFonts.CaptionFont;
		bool flag5 = true;
		if (ribbonStrip.CaptionFont != null)
		{
			captionFont.Dispose();
			captionFont = ribbonStrip.CaptionFont;
			flag5 = false;
		}
		Size empty = Size.Empty;
		empty = ((!flag4) ? TextDrawing.MeasureString(graphics, text, captionFont) : e.RibbonControl.RibbonStrip._0D62.Bounds.Size);
		if (arrayList.Count > 0)
		{
			ribbonStrip._0D64 = (Rectangle[])arrayList.ToArray(typeof(Rectangle));
			if ((int)((Control)e.RibbonControl).RightToLeft == 0)
			{
				arrayList.Reverse();
			}
			ഡ = Rectangle.Empty;
			foreach (Rectangle item2 in arrayList)
			{
				if (item2.Width >= empty.Width)
				{
					ഡ = item2;
					break;
				}
				if (item2.Width > ഡ.Width)
				{
					ഡ = item2;
				}
			}
		}
		else
		{
			ribbonStrip._0D64 = new Rectangle[1] { ഡ };
		}
		if (ഡ.Width > 6 && ഡ.Height > 6 && flag3)
		{
			if (e.GlassEnabled && e.က != null && e.က._0A79 != null && !e.RibbonControl.Ꮬ)
			{
				if (!e.က.ໟ || flag)
				{
					_2202(graphics, text, captionFont, ഡ, flag);
				}
			}
			else if (!flag4)
			{
				TextDrawing.DrawString(graphics, text, captionFont, office2007FormStateColorTable.CaptionText, ഡ, eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.NoPrefix | eTextFormat.VerticalCenter);
			}
			else
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, captionFont, office2007FormStateColorTable.CaptionText, (int)((Control)e.RibbonControl).RightToLeft == 1, ഡ, hotKeyPrefixVisible: false);
				markupDrawContext.AllowMultiLine = false;
				markupDrawContext.IgnoreFormattingColors = !flag2;
				_0CB4 obj = e.RibbonControl.RibbonStrip._0D62;
				if (e.RibbonControl.RibbonStrip.අ != ഡ)
				{
					obj.Measure(ഡ.Size, markupDrawContext);
					obj._0CC6(ഡ, markupDrawContext);
					e.RibbonControl.RibbonStrip.අ = ഡ;
					Rectangle bounds = obj.Bounds;
					if (bounds.Width < ഡ.Width)
					{
						bounds.Offset((ഡ.Width - bounds.Width) / 2, 0);
					}
					if (bounds.Height < ഡ.Height)
					{
						bounds.Offset(0, (ഡ.Height - bounds.Height) / 2);
					}
					obj.Bounds = bounds;
				}
				Region clip = graphics.Clip;
				graphics.SetClip(ഡ, (CombineMode)1);
				obj.Render(markupDrawContext);
				graphics.Clip = clip;
				if (clip != null)
				{
					clip.Dispose();
				}
			}
		}
		if (flag5)
		{
			captionFont.Dispose();
		}
	}

	public static void _21FC(Graphics _٥, string _06EA, Font _0EE7, Rectangle भ, ThemeTextFormat _21FD)
	{
		_21FC(_٥, _06EA, _0EE7, भ, _21FD, SystemColors.ControlText, _21FE: false, _21FF: true, 16);
	}

	public static void _21FC(Graphics _٥, string _06EA, Font _0EE7, Rectangle भ, ThemeTextFormat _21FD, Color ໆ, bool _21FE, bool _21FF, int _2200)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		IntPtr hdc = _٥.GetHdc();
		try
		{
			IntPtr intPtr = _0EFE.CreateCompatibleDC(hdc);
			try
			{
				_0EFE.BITMAPINFO bITMAPINFO = new _0EFE.BITMAPINFO();
				bITMAPINFO.biWidth = भ.Width;
				bITMAPINFO.biHeight = -भ.Height;
				bITMAPINFO.biPlanes = 1;
				bITMAPINFO.biBitCount = 32;
				bITMAPINFO.biSize = Marshal.SizeOf((object)bITMAPINFO);
				IntPtr hObject = _0EFE.CreateDIBSection(hdc, bITMAPINFO, 0u, 0, IntPtr.Zero, 0u);
				_0EFE.SelectObject(intPtr, hObject);
				IntPtr hObject2 = _0EE7.ToHfont();
				_0EFE.SelectObject(intPtr, hObject2);
				_0EEE.RECT pRect = new _0EEE.RECT(new Rectangle(0, 0, भ.Width, भ.Height));
				VisualStyleRenderer val = new VisualStyleRenderer(Caption.Active);
				_0EEE.DTTOPTS options = default(_0EEE.DTTOPTS);
				options.iGlowSize = _2200;
				options.crText = new _0EEE.COLORREF(ໆ);
				options.dwFlags = 0x2000 | (_21FF ? 2048 : 0) | 1;
				options.dwSize = Marshal.SizeOf((object)options);
				if (_21FE)
				{
					_0EFE.BitBlt(intPtr, 0, 0, भ.Width, भ.Height, hdc, भ.Left, भ.Top, 13369376u);
				}
				_0EEE.DrawThemeTextEx(val.Handle, intPtr, 0, 0, _06EA, -1, (int)_21FD, ref pRect, ref options);
				_0EFE.BitBlt(hdc, भ.Left, भ.Top, भ.Width, भ.Height, intPtr, 0, 0, 13369376u);
				_0EFE.DeleteObject(hObject2);
				_0EFE.DeleteObject(hObject);
			}
			finally
			{
				_0EFE.DeleteDC(intPtr);
			}
		}
		finally
		{
			_٥.ReleaseHdc(hdc);
		}
	}

	public static void _2201(Graphics _٥, string _06EA, Font _0EE7, Rectangle भ, ThemeTextFormat _21FD, Color ໆ, Color _0EF2, bool _21FE)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		Point point = default(Point);
		Matrix transform = _٥.Transform;
		try
		{
			Point[] array = new Point[1] { point };
			transform.TransformPoints(array);
			point = array[0];
		}
		finally
		{
			((IDisposable)transform)?.Dispose();
		}
		IntPtr hdc = _٥.GetHdc();
		try
		{
			IntPtr intPtr = _0EFE.CreateCompatibleDC(hdc);
			try
			{
				_0EFE.BITMAPINFO bITMAPINFO = new _0EFE.BITMAPINFO();
				bITMAPINFO.biWidth = भ.Width;
				bITMAPINFO.biHeight = -भ.Height;
				bITMAPINFO.biPlanes = 1;
				bITMAPINFO.biBitCount = 32;
				bITMAPINFO.biSize = Marshal.SizeOf((object)bITMAPINFO);
				IntPtr hObject = _0EFE.CreateDIBSection(hdc, bITMAPINFO, 0u, 0, IntPtr.Zero, 0u);
				_0EFE.SelectObject(intPtr, hObject);
				IntPtr hObject2 = _0EE7.ToHfont();
				_0EFE.SelectObject(intPtr, hObject2);
				_0EEE.RECT pRect = new _0EEE.RECT(new Rectangle(0, 0, भ.Width, भ.Height));
				VisualStyleRenderer val = new VisualStyleRenderer(Caption.Active);
				_0EEE.DTTOPTS options = default(_0EEE.DTTOPTS);
				options.crText = new _0EEE.COLORREF(_0EF2);
				options.dwFlags = 8193;
				options.dwSize = Marshal.SizeOf((object)options);
				if (_21FE)
				{
					_0EFE.BitBlt(intPtr, 0, 0, भ.Width, भ.Height, hdc, भ.Left + point.X, भ.Top + point.Y, 13369376u);
				}
				pRect.Offset(1, 2);
				_0EEE.DrawThemeTextEx(val.Handle, intPtr, 0, 0, _06EA, -1, (int)_21FD, ref pRect, ref options);
				pRect.Offset(-1, -1);
				options.crText = new _0EEE.COLORREF(ໆ);
				_0EEE.DrawThemeTextEx(val.Handle, intPtr, 0, 0, _06EA, -1, (int)_21FD, ref pRect, ref options);
				_0EFE.BitBlt(hdc, भ.Left + point.X, भ.Top + point.Y, भ.Width, भ.Height, intPtr, 0, 0, 13369376u);
				_0EFE.DeleteObject(hObject2);
				_0EFE.DeleteObject(hObject);
			}
			finally
			{
				_0EFE.DeleteDC(intPtr);
			}
		}
		finally
		{
			_٥.ReleaseHdc(hdc);
		}
	}

	private void _2202(Graphics _٥, string _06EA, Font _0EE7, Rectangle ഡ, bool _2203)
	{
		if (_2203 && !BarFunctions.IsWindows7)
		{
			ഡ.Offset(0, 3);
			TextDrawing.DrawString(_٥, _06EA, _0EE7, Color.White, ഡ, eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.NoPrefix | eTextFormat.VerticalCenter);
			return;
		}
		if (_2203 && BarFunctions.IsWindows7)
		{
			ഡ.Y += 6;
			ഡ.Height -= 6;
		}
		ThemeTextFormat _21FD = (ThemeTextFormat)32805;
		_21FC(_٥, _06EA, _0EE7, ഡ, _21FD);
	}

	public override void PaintQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Invalid comparison between Unknown and I4
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Expected O, but got Unknown
		//IL_076c: Unknown result type (might be due to invalid IL or missing references)
		RibbonStrip ribbonStrip = e.RibbonControl.RibbonStrip;
		RibbonControl ribbonControl = e.RibbonControl;
		if (!ribbonStrip.CaptionVisible)
		{
			return;
		}
		eDotNetBarStyle effectiveStyle = ribbonControl.EffectiveStyle;
		Graphics graphics = e.Graphics;
		bool flag = (int)((Control)ribbonStrip).RightToLeft == 1;
		bool flag2 = true;
		Office2007QuickAccessToolbarStateColorTable office2007QuickAccessToolbarStateColorTable = ຊ.QuickAccessToolbar.Active;
		Form val = ((Control)ribbonStrip).FindForm();
		bool flag3 = false;
		if (val != null)
		{
			flag3 = (int)val.WindowState == 2;
		}
		if (val != null && !e.RibbonControl.Ꮬ && ((val != Form.ActiveForm && val.MdiParent == null) || (val.MdiParent != null && val.MdiParent.ActiveMdiChild != val)))
		{
			office2007QuickAccessToolbarStateColorTable = ຊ.QuickAccessToolbar.Inactive;
		}
		int num = 0;
		int num2 = 0;
		Size size = Size.Empty;
		for (int num3 = ribbonStrip.QuickToolbarItems.Count - 1; num3 >= 0; num3--)
		{
			BaseItem baseItem = ribbonStrip.QuickToolbarItems[num3];
			if (baseItem.Visible && (baseItem.Displayed || ((RibbonStripContainerItem)ribbonStrip.GetBaseItemContainer()).MetroBackstageOpen))
			{
				if (baseItem is QatCustomizeItem)
				{
					size = baseItem.DisplayRectangle.Size;
				}
				if (baseItem.ItemAlignment == eItemAlignment.Near && baseItem.Visible && num3 > 0)
				{
					num = ((!flag) ? baseItem.DisplayRectangle.Right : baseItem.DisplayRectangle.X);
					break;
				}
				if (baseItem.ItemAlignment == eItemAlignment.Far && baseItem.Visible)
				{
					num2 = ((!flag) ? baseItem.DisplayRectangle.X : baseItem.DisplayRectangle.Right);
				}
			}
		}
		if (num == 0)
		{
			flag2 = false;
		}
		if (ribbonStrip.CaptionContainerItem is ᱭ && ((ᱭ)ribbonStrip.CaptionContainerItem)._0DBC != null)
		{
			num = ((!flag) ? ((ᱭ)ribbonStrip.CaptionContainerItem)._0DBC.DisplayRectangle.Right : ((ᱭ)ribbonStrip.CaptionContainerItem)._0DBC.DisplayRectangle.X);
			size = ((ᱭ)ribbonStrip.CaptionContainerItem)._0DBC.DisplayRectangle.Size;
		}
		Rectangle ल = ribbonStrip._0D65;
		if (num2 > 0)
		{
			if (flag)
			{
				ribbonStrip._൦ = new Rectangle(ल.X, ल.Y, num2, ल.Height);
			}
			else
			{
				ribbonStrip._൦ = new Rectangle(num2, ल.Y, ल.Right - num2, ल.Height);
			}
		}
		if (num == 0 || ल.Height <= 0 || ल.Width <= 0)
		{
			return;
		}
		if (!ribbonControl.QatPositionedBelowRibbon)
		{
			BaseItem applicationButton = ribbonControl.GetApplicationButton();
			if (applicationButton != null)
			{
				int num4 = ribbonStrip.QuickToolbarItems.IndexOf(applicationButton);
				if (ribbonStrip.QuickToolbarItems.Count - 1 > num4)
				{
					BaseItem baseItem2 = ribbonStrip.QuickToolbarItems[num4 + 1];
					if (flag)
					{
						ल.Width -= ल.Right - baseItem2.DisplayRectangle.Right;
					}
					else
					{
						ल.Width -= baseItem2.DisplayRectangle.X - ल.X;
						ल.X = baseItem2.DisplayRectangle.X;
					}
				}
			}
			ल.Height = ((ᱭ)ribbonControl.RibbonStrip.CaptionContainerItem).ᱮ + 6;
		}
		int num5 = ल.Right - num;
		if (flag)
		{
			ल.Width = num5;
			if (flag)
			{
				ल.X += num;
			}
		}
		else
		{
			ल.Width -= num5;
		}
		if (e.GlassEnabled)
		{
			ल.Inflate(0, -3);
			ल.Offset(0, 2);
		}
		else
		{
			ल.Inflate(0, -2);
			if (flag3)
			{
				ल.Y++;
				ल.Height--;
			}
		}
		GraphicsPath val2 = _2204(ल, flag);
		if (val2 == null)
		{
			return;
		}
		Rectangle rectangle2 = (ribbonStrip.QuickToolbarBounds = Rectangle.Ceiling(val2.GetBounds()));
		ल.Width -= size.Width + size.Height / 2;
		if (flag)
		{
			ल.X += size.Width + size.Height / 2 + 3;
		}
		val2.Dispose();
		val2 = _2204(ल, flag);
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		if (!ribbonControl.QatPositionedBelowRibbon && flag2 && ल.Width > 6)
		{
			bool flag4 = false;
			if (e.GlassEnabled)
			{
				graphics.SmoothingMode = (SmoothingMode)2;
				if (e.GlassEnabled && flag3 && effectiveStyle == eDotNetBarStyle.Office2007)
				{
					DisplayHelp.FillPath(graphics, val2, Color.FromArgb(72, Color.LightGray), Color.Empty);
				}
				if (!office2007QuickAccessToolbarStateColorTable.GlassBorder.IsEmpty)
				{
					DisplayHelp.DrawGradientPathBorder(graphics, val2, office2007QuickAccessToolbarStateColorTable.GlassBorder, 1);
				}
				graphics.SmoothingMode = smoothingMode;
				if (e.GlassEnabled)
				{
					flag4 = true;
				}
			}
			else if (!office2007QuickAccessToolbarStateColorTable.OutterBorderColor.IsEmpty)
			{
				graphics.SmoothingMode = (SmoothingMode)2;
				ल.X++;
				ल.Y--;
				ल.Height += 2;
				GraphicsPath val3 = _2204(ल, flag);
				try
				{
					Pen val4 = new Pen(office2007QuickAccessToolbarStateColorTable.OutterBorderColor);
					try
					{
						graphics.DrawPath(val4, val3);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				ल.X--;
				ल.Y++;
				ल.Height -= 2;
				graphics.SmoothingMode = smoothingMode;
				if (e.GlassEnabled)
				{
					flag4 = true;
				}
			}
			Region clip = graphics.Clip;
			graphics.SetClip(val2, (CombineMode)1);
			Rectangle r = new Rectangle(rectangle2.X, rectangle2.Y, rectangle2.Width, (int)((double)ल.Height * 0.2));
			Rectangle r2 = new Rectangle(rectangle2.X, r.Bottom, rectangle2.Width, rectangle2.Height - r.Height);
			graphics.SmoothingMode = (SmoothingMode)0;
			if (!e.GlassEnabled)
			{
				DisplayHelp.FillRectangle(graphics, r, office2007QuickAccessToolbarStateColorTable.TopBackground);
				DisplayHelp.FillRectangle(graphics, r2, office2007QuickAccessToolbarStateColorTable.BottomBackground);
			}
			else if (!office2007QuickAccessToolbarStateColorTable.GlassBorder.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, rectangle2, Color.FromArgb(64, Color.White));
			}
			graphics.SmoothingMode = smoothingMode;
			rectangle2.Height -= 2;
			rectangle2.Width += 2;
			rectangle2.Y++;
			rectangle2.X++;
			graphics.SetClip(rectangle2);
			graphics.Clip = clip;
			if (clip != null)
			{
				clip.Dispose();
			}
			if (!office2007QuickAccessToolbarStateColorTable.MiddleBorderColor.IsEmpty && (!e.GlassEnabled || !flag4))
			{
				graphics.SmoothingMode = (SmoothingMode)2;
				val2 = _2204(ल, flag);
				Pen val5 = new Pen(office2007QuickAccessToolbarStateColorTable.MiddleBorderColor);
				try
				{
					graphics.DrawPath(val5, val2);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				val2.Dispose();
				val2 = null;
				graphics.SmoothingMode = smoothingMode;
			}
		}
		if (val2 != null)
		{
			val2.Dispose();
		}
	}

	private GraphicsPath _2204(Rectangle ल, bool _20B4)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		ल.Offset(-1, 0);
		ल.Height--;
		ल.Width--;
		if (ल.Width < 2 || ल.Height < 2)
		{
			return null;
		}
		GraphicsPath val = new GraphicsPath();
		int num = 11;
		if (_20B4)
		{
			val.AddCurve(new Point[3]
			{
				new Point(ल.Right + num, ल.Y),
				new Point(ल.Right + 2, ल.Y + 10),
				new Point(ल.Right, ल.Bottom)
			}, 0.6f);
			val.AddLine(ल.Right, ल.Bottom, ल.X, ल.Bottom);
			val.AddArc(ल.X - (num + 1), ल.Y, 20, ल.Height, 90f, 180f);
			val.AddLine(ल.X, ल.Y, ल.Right + num, ल.Y);
		}
		else
		{
			val.AddCurve(new Point[3]
			{
				new Point(ल.X - num, ल.Y),
				new Point(ल.X - 2, ल.Y + 10),
				new Point(ल.X, ल.Bottom)
			}, 0.6f);
			val.AddLine(ल.X, ल.Bottom, ल.Right, ल.Bottom);
			val.AddArc(ल.Right - (num + 1), ल.Y, 20, ल.Height, 90f, -180f);
			val.AddLine(ल.Right, ल.Y, ल.X - num, ल.Y);
		}
		val.CloseAllFigures();
		return val;
	}
}
internal class _2205
{
	public virtual void PaintBackground(RibbonControlRendererEventArgs e)
	{
	}

	public virtual void PaintCaptionBackground(RibbonControlRendererEventArgs e, Rectangle displayRect)
	{
	}

	public virtual void PaintCaptionText(RibbonControlRendererEventArgs e)
	{
	}

	public virtual void PaintQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
	{
	}
}
internal class _2206 : _192E
{
	public override void PaintButtonMouseOver(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r)
	{
		PaintButtonCheck(button, pa, image, r);
	}

	public override void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		if (IsOnMenu(button, pa))
		{
			base.PaintButtonBackground(button, pa, image);
		}
		else
		{
			PaintButtonCheck(button, pa, image, button.DisplayRectangle);
		}
	}

	public override void PaintButtonCheck(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r)
	{
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		if (IsOnMenu(button, pa))
		{
			base.PaintButtonCheck(button, pa, image, r);
		}
		else
		{
			if (!button.IsMouseOver && !button.Checked)
			{
				return;
			}
			Color color = pa.Colors.BarBackground2;
			Color color2 = pa.Colors.BarBackground;
			Color barFloatingBorder = pa.Colors.BarFloatingBorder;
			int num = pa.Colors.BarBackgroundGradientAngle;
			if (button.IsMouseOver && !button.Checked)
			{
				color = pa.Colors.ItemHotBackground2;
				color2 = pa.Colors.ItemHotBackground;
				num = pa.Colors.ItemHotBackgroundGradientAngle;
			}
			else if (button.Checked)
			{
				color = Color.White;
				color2 = pa.Colors.BarBackground;
				num = pa.Colors.ItemPressedBackgroundGradientAngle;
			}
			ControlPaint.LightLight(color2);
			Color color3 = Color.FromArgb(100, ControlPaint.Dark(color));
			int num2 = 4;
			Graphics graphics = pa.Graphics;
			GraphicsPath val = ᔹ(r, num2);
			val.CloseAllFigures();
			LinearGradientBrush val2 = DisplayHelp.CreateLinearGradientBrush(r, color, color2, num);
			try
			{
				val2.GammaCorrection = true;
				graphics.FillPath((Brush)(object)val2, val);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			val.Dispose();
			GraphicsPath val3 = (val = _2207(r, num2 - 1));
			try
			{
				Pen val4 = new Pen(color3, 1f);
				try
				{
					graphics.DrawPath(val4, val);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			GraphicsPath val5 = (val = _2208(r, num2 - 1));
			try
			{
				Pen val6 = new Pen(color3, 1f);
				try
				{
					graphics.DrawPath(val6, val);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			GraphicsPath val7 = (val = ᔹ(r, num2));
			try
			{
				Pen val8 = new Pen(barFloatingBorder, 1f);
				try
				{
					graphics.DrawPath(val8, val);
				}
				finally
				{
					((IDisposable)val8)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
	}

	private GraphicsPath _2207(Rectangle ल, int ഹ)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		ल.X++;
		ल.Y++;
		ल.Width -= 2;
		ल.Height--;
		GraphicsPath val = new GraphicsPath();
		val.AddLine(ल.X, ल.Bottom, ल.X, ल.Y + ഹ);
		ᒇ ᒇ2 = ElementStyleDisplay.സ(ल, ഹ, eCornerArc.TopLeft);
		val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
		val.AddLine(ल.X + ഹ, ल.Y, ल.Right - ഹ, ल.Y);
		return val;
	}

	private GraphicsPath _2208(Rectangle ल, int ഹ)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		ल.X++;
		ल.Y++;
		ल.Width -= 3;
		ल.Height--;
		GraphicsPath val = new GraphicsPath();
		ᒇ ᒇ2 = ElementStyleDisplay.സ(ल, ഹ, eCornerArc.TopRight);
		val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
		val.AddLine(ल.Right, ल.Y + ഹ, ल.Right, ल.Bottom);
		return val;
	}

	private GraphicsPath ᔹ(Rectangle ल, int ഹ)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		ल.Width--;
		GraphicsPath val = new GraphicsPath();
		val.AddLine(ल.X, ल.Bottom, ल.X, ल.Y + ഹ);
		ᒇ ᒇ2 = ElementStyleDisplay.സ(ल, ഹ, eCornerArc.TopLeft);
		val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
		val.AddLine(ल.X + ഹ, ल.Y, ल.Right - ഹ, ल.Y);
		ᒇ2 = ElementStyleDisplay.സ(ल, ഹ, eCornerArc.TopRight);
		val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
		val.AddLine(ल.Right, ल.Y + ഹ, ल.Right, ल.Bottom);
		return val;
	}

	public override void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, ᇎ image)
	{
		base.PaintButtonText(button, pa, textColor, image);
	}
}
internal class _220F : ButtonItem
{
	public RibbonBar _21F1;

	protected override void OnCommandChanged()
	{
	}

	protected override void OnExternalSizeChange()
	{
		base.OnExternalSizeChange();
		_2210();
	}

	public override void RecalcSize()
	{
		base.RecalcSize();
		_2210();
	}

	private void _2210()
	{
		Rectangle subItemsRect = base.SubItemsRect;
		if (!subItemsRect.IsEmpty)
		{
			subItemsRect.Y += 8;
			subItemsRect.Height -= 4;
			base.SubItemsRect = subItemsRect;
		}
	}
}
internal sealed class _23CE
{
	private const string _23D1 = "Shell_TrayWnd";

	private Rectangle __1734 = Rectangle.Empty;

	private TaskbarPosition __1361 = TaskbarPosition.Unknown;

	private bool __23D2;

	private bool __2076;

	public Rectangle ঋ
	{
		get
		{
			return __1734;
		}
		private set
		{
			__1734 = value;
		}
	}

	public TaskbarPosition _0AF4
	{
		get
		{
			return __1361;
		}
		private set
		{
			__1361 = value;
		}
	}

	public Point ᙞ => ঋ.Location;

	public Size ᇐ => ঋ.Size;

	public bool _23CF
	{
		get
		{
			return __23D2;
		}
		private set
		{
			__23D2 = value;
		}
	}

	public bool _23D0
	{
		get
		{
			return __2076;
		}
		private set
		{
			__2076 = value;
		}
	}

	public _23CE()
	{
		IntPtr hWnd = User32.FindWindow("Shell_TrayWnd", null);
		APPBARDATA pData = new APPBARDATA
		{
			cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA)),
			hWnd = hWnd
		};
		IntPtr intPtr = Shell32.SHAppBarMessage(ABM.GetTaskbarPos, ref pData);
		if (intPtr == IntPtr.Zero)
		{
			throw new InvalidOperationException();
		}
		_0AF4 = (TaskbarPosition)pData.uEdge;
		ঋ = Rectangle.FromLTRB(pData.rc.left, pData.rc.top, pData.rc.right, pData.rc.bottom);
		pData.cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA));
		int num = Shell32.SHAppBarMessage(ABM.GetState, ref pData).ToInt32();
		_23CF = (num & 2) == 2;
		_23D0 = (num & 1) == 1;
	}
}
internal class _23D7
{
	public BaseItem _0CCA;

	private bool _23D9;

	protected bool _23D8
	{
		get
		{
			return _23D9;
		}
		set
		{
			_23D9 = value;
		}
	}

	public virtual void RecordSetting(BaseItem item)
	{
		_0CCA = item;
		_23D9 = true;
	}

	public virtual void RestoreSettings()
	{
		_23D9 = false;
	}
}
internal class _23DA : ISimpleElement
{
	private Rectangle ݗ = Rectangle.Empty;

	private Rectangle ݚ = Rectangle.Empty;

	private Rectangle ݘ = Rectangle.Empty;

	private Image _0E66;

	private eSimplePartAlignment ݜ;

	private string ө = "";

	private int _23DB;

	private Size _ᢧ;

	public int FixedWidth
	{
		get
		{
			return _23DB;
		}
		set
		{
			_23DB = value;
		}
	}

	public bool ImageVisible => _0E66 != null;

	public Size ImageLayoutSize
	{
		get
		{
			if (_0E66 == null)
			{
				return Size.Empty;
			}
			return _0E66.Size;
		}
	}

	public eSimplePartAlignment ImageAlignment
	{
		get
		{
			return ݜ;
		}
		set
		{
			ݜ = value;
		}
	}

	public int ImageTextSpacing => 1;

	public bool TextVisible => true;

	public string Text
	{
		get
		{
			return ө;
		}
		set
		{
			ө = value;
		}
	}

	public Rectangle Bounds
	{
		get
		{
			return ݗ;
		}
		set
		{
			ݗ = value;
		}
	}

	public Rectangle TextBounds
	{
		get
		{
			return ݘ;
		}
		set
		{
			ݘ = value;
		}
	}

	public Rectangle ImageBounds
	{
		get
		{
			return ݚ;
		}
		set
		{
			ݚ = value;
		}
	}

	public Image Image
	{
		get
		{
			return _0E66;
		}
		set
		{
			_0E66 = value;
		}
	}

	public Size TextSize
	{
		get
		{
			return _ᢧ;
		}
		set
		{
			_ᢧ = value;
		}
	}
}
internal class _249A
{
	public void _139A(ItemContainer ઇ, ItemPaintArgs _065C)
	{
		foreach (BaseItem subItem in ઇ.SubItems)
		{
			if (!subItem.Visible || !subItem.Displayed)
			{
				continue;
			}
			if (subItem.BeginGroup && _065C.ໝ != null)
			{
				_065C.ໝ.DrawItemContainerSeparator(new ItemContainerSeparatorRendererEventArgs(_065C.Graphics, ઇ, subItem));
			}
			if (_065C.ClipRectangle.IsEmpty || _065C.ClipRectangle.IntersectsWith(subItem.DisplayRectangle))
			{
				Region clip = _065C.Graphics.Clip;
				_065C.Graphics.SetClip(subItem.DisplayRectangle, (CombineMode)1);
				if (!_065C.Graphics.IsClipEmpty)
				{
					subItem.Paint(_065C);
				}
				_065C.Graphics.Clip = clip;
				if (clip != null)
				{
					clip.Dispose();
				}
			}
		}
	}

	public void _139A(BaseItem ઇ, ItemPaintArgs _065C)
	{
		foreach (BaseItem subItem in ઇ.SubItems)
		{
			if (subItem.Visible && subItem.Displayed && (_065C.ClipRectangle.IsEmpty || _065C.ClipRectangle.IntersectsWith(subItem.DisplayRectangle)))
			{
				Region clip = _065C.Graphics.Clip;
				_065C.Graphics.SetClip(subItem.DisplayRectangle, (CombineMode)1);
				subItem.Paint(_065C);
				_065C.Graphics.Clip = clip;
				if (clip != null)
				{
					clip.Dispose();
				}
			}
		}
	}

	public void _139A(StepItemContainer ઇ, ItemPaintArgs _065C)
	{
		StepItem stepItem = null;
		foreach (BaseItem subItem in ઇ.SubItems)
		{
			if (!subItem.Visible || !subItem.Displayed || (!_065C.ClipRectangle.IsEmpty && !_065C.ClipRectangle.IntersectsWith(subItem.DisplayRectangle)))
			{
				continue;
			}
			if (subItem is StepItem && ((StepItem)subItem).IsMouseOver)
			{
				stepItem = (StepItem)subItem;
				continue;
			}
			Region clip = _065C.Graphics.Clip;
			_065C.Graphics.SetClip(subItem.DisplayRectangle, (CombineMode)1);
			subItem.Paint(_065C);
			_065C.Graphics.Clip = clip;
			if (clip != null)
			{
				clip.Dispose();
			}
		}
		stepItem?.Paint(_065C);
	}
}
internal class _24AD : _24AE, ᙓ
{
	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void PaintBackground(ItemContainerRendererEventArgs e)
	{
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		if (!BarFunctions.IsOffice2007Style(e.ItemContainer.EffectiveStyle))
		{
			return;
		}
		ItemContainer itemContainer = e.ItemContainer;
		Office2007ItemGroupColorTable itemGroup = ຊ.ItemGroup;
		if ((itemContainer.SubItems.Count == 0 && (itemContainer.WidthInternal <= 0 || itemContainer.HeightInternal <= 0)) || itemGroup == null)
		{
			return;
		}
		Rectangle displayRectangle = itemContainer.DisplayRectangle;
		if (!itemContainer.BeginGroup || (itemContainer.IsOnMenu && !(itemContainer.Parent is ItemContainer)))
		{
			if (itemContainer.BackgroundStyle.Class == ElementStyleClassKeys.Office2007StatusBarBackground2Key && e.က != null && e.က.ContainerControl is Bar && ((Bar)(object)e.က.ContainerControl).BarType == eBarType.StatusBar)
			{
				displayRectangle.Y = 1;
				if (!ຊ.Bar.StatusBarTopBorderLight.IsEmpty)
				{
					displayRectangle.Y++;
				}
				displayRectangle.Height = e.က.ContainerControl.Height - 1;
				if (itemContainer.Parent.SubItems.IndexOf(itemContainer) == itemContainer.Parent.SubItems.Count - 1)
				{
					if (e.က.RightToLeft)
					{
						displayRectangle.Width += displayRectangle.X;
						displayRectangle.X = 0;
					}
					else
					{
						displayRectangle.Width += e.က.ContainerControl.Width - displayRectangle.Right;
					}
				}
			}
			Region clip = e.Graphics.Clip;
			e.Graphics.SetClip(displayRectangle);
			ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(itemContainer.BackgroundStyle, e.Graphics, displayRectangle));
			e.Graphics.Clip = clip;
			if (clip != null)
			{
				clip.Dispose();
			}
			return;
		}
		displayRectangle.Width -= itemContainer.BackgroundStyle.MarginHorizontal;
		displayRectangle.Height -= itemContainer.BackgroundStyle.MarginVertical;
		displayRectangle.X += itemContainer.BackgroundStyle.MarginLeft;
		displayRectangle.Y += itemContainer.BackgroundStyle.MarginTop;
		Graphics graphics = e.Graphics;
		int roundCornerSize = 2;
		if (itemGroup.OuterBorder != null && !itemGroup.OuterBorder.IsEmpty)
		{
			DisplayHelp.DrawRoundGradientRectangle(graphics, displayRectangle, itemGroup.OuterBorder, 1, roundCornerSize);
		}
		if (itemGroup.InnerBorder != null && !itemGroup.InnerBorder.IsEmpty)
		{
			displayRectangle.Inflate(-1, -1);
			DisplayHelp.DrawRoundGradientRectangle(graphics, displayRectangle, itemGroup.InnerBorder, 1, roundCornerSize);
		}
		float num = 0.4f;
		Rectangle r = displayRectangle;
		r.Inflate(-1, -1);
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)0;
		if (itemGroup.TopBackground != null && !itemGroup.TopBackground.IsEmpty)
		{
			DisplayHelp.FillRectangle(graphics, r, itemGroup.TopBackground);
		}
		int num2 = (int)((float)r.Height * num);
		r.Height -= num2;
		r.Y += num2;
		if (itemGroup.BottomBackground != null && !itemGroup.BottomBackground.IsEmpty)
		{
			DisplayHelp.FillRectangle(graphics, r, itemGroup.BottomBackground);
		}
		graphics.SmoothingMode = smoothingMode;
		bool flag = itemContainer.LayoutOrientation == eOrientation.Vertical;
		Pen val = new Pen(itemGroup.ItemGroupDividerLight, 1f);
		try
		{
			Pen val2 = new Pen(itemGroup.ItemGroupDividerDark);
			try
			{
				int num3 = itemContainer.SubItems.Count - 1;
				for (int i = 0; i <= num3; i++)
				{
					BaseItem baseItem = itemContainer.SubItems[i];
					if (baseItem.Visible && baseItem.DisplayRectangle.Right != itemContainer.DisplayRectangle.Right && i != num3)
					{
						if (flag)
						{
							graphics.DrawLine(val2, itemContainer.DisplayRectangle.X + 1, baseItem.DisplayRectangle.Bottom - 1, itemContainer.DisplayRectangle.Right - 1, baseItem.DisplayRectangle.Bottom - 1);
							graphics.DrawLine(val, itemContainer.DisplayRectangle.X + 1, baseItem.DisplayRectangle.Bottom, itemContainer.DisplayRectangle.Right, baseItem.DisplayRectangle.Bottom);
						}
						else
						{
							graphics.DrawLine(val2, baseItem.DisplayRectangle.Right - 1, baseItem.DisplayRectangle.Y + 1, baseItem.DisplayRectangle.Right - 1, baseItem.DisplayRectangle.Bottom - 2);
							graphics.DrawLine(val, baseItem.DisplayRectangle.Right, baseItem.DisplayRectangle.Y + 1, baseItem.DisplayRectangle.Right, baseItem.DisplayRectangle.Bottom - 2);
						}
					}
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public override void PaintItemSeparator(ItemContainerSeparatorRendererEventArgs e)
	{
		if (!(e.Item is ItemContainer))
		{
			Graphics graphics = e.Graphics;
			BaseItem item = e.Item;
			Size size = Size.Empty;
			if (item is ImageItem)
			{
				size = ((ImageItem)item).ImageSize;
			}
			ItemContainer itemContainer = e.ItemContainer;
			Color itemSeparator = ຊ.LegacyColors.ItemSeparator;
			Color itemSeparatorShade = ຊ.LegacyColors.ItemSeparatorShade;
			Point start = Point.Empty;
			Point end = Point.Empty;
			Point empty = Point.Empty;
			Point empty2 = Point.Empty;
			if (itemContainer.LayoutOrientation == eOrientation.Horizontal)
			{
				start = new Point(item.DisplayRectangle.X - 2, item.DisplayRectangle.Y + 3);
				end = new Point(start.X, item.DisplayRectangle.Bottom - 4);
				empty = new Point(start.X + 1, start.Y);
				empty2 = new Point(end.X + 1, end.Y);
			}
			else if (item.IsOnMenu)
			{
				start = new Point(item.DisplayRectangle.X + size.Width, item.DisplayRectangle.Y - 2);
				end = new Point(item.DisplayRectangle.Right - 1, start.Y);
				empty = new Point(start.X, start.Y + 1);
				empty2 = new Point(end.X, end.Y + 1);
			}
			else
			{
				start = new Point(item.DisplayRectangle.X + 3, item.DisplayRectangle.Y - 2);
				end = new Point(item.DisplayRectangle.Right - 4, start.Y);
				empty = new Point(start.X, start.Y + 1);
				empty2 = new Point(end.X, end.Y + 1);
			}
			DisplayHelp.DrawLine(graphics, start, end, itemSeparator, 1);
			DisplayHelp.DrawLine(graphics, empty, empty2, itemSeparatorShade, 1);
		}
	}
}
internal abstract class _24AE
{
	public abstract void PaintBackground(ItemContainerRendererEventArgs e);

	public abstract void PaintItemSeparator(ItemContainerSeparatorRendererEventArgs e);
}
internal class _24AF : _24B2, ᙓ
{
	private float _24B0 = 0.4f;

	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void PaintDockedBackground(ToolbarRendererEventArgs e)
	{
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Expected O, but got Unknown
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Bar bar = e.Bar;
		ItemPaintArgs က = e.က;
		Rectangle bounds = e.Bounds;
		Rectangle rectangle = bounds;
		rectangle.Inflate(1, 1);
		ColorScheme legacyColors = ຊ.LegacyColors;
		if (bar.LayoutType == eLayoutType.DockContainer || bar.LayoutType == eLayoutType.TaskList)
		{
			if (bar._0AF8)
			{
				DisplayHelp.FillRectangle(graphics, rectangle, legacyColors.BarBackground, legacyColors.BarBackground2, legacyColors.BarBackgroundGradientAngle);
			}
			else
			{
				DisplayHelp.FillRectangle(graphics, rectangle, legacyColors.BarCaptionInactiveBackground, legacyColors.BarCaptionInactiveBackground2, legacyColors.ߖ);
			}
		}
		else if (bar.MenuBar)
		{
			DisplayHelp.FillRectangle(graphics, rectangle, legacyColors.MenuBarBackground, legacyColors.MenuBarBackground2, legacyColors.MenuBarBackgroundGradientAngle);
		}
		else if (bar.GrabHandleStyle != eGrabHandleStyle.ResizeHandle && bar.BarType != eBarType.StatusBar)
		{
			if (bar.ItemsContainer.m_BackgroundColor.IsEmpty && ((Control)bar).BackColor != Color.Transparent)
			{
				if (bar.IsThemed)
				{
					Rectangle ल = new Rectangle(-bar.Location.X, -bar.Location.Y, ((Control)bar).Parent.Width, ((Control)bar).Parent.Height);
					_0A7A obj = ((ል)bar)._0A7A;
					obj._0EE5(graphics, ཤ.ཥ, ཫ.ཊ, ल);
				}
				else if (_24B1(bar))
				{
					DisplayHelp.FillRectangle(graphics, rectangle, legacyColors.BarBackground, legacyColors.BarBackground2, legacyColors.BarBackgroundGradientAngle, new float[3] { 0f, 0.12f, 1f }, new float[3] { 0f, 0.5f, 1f });
				}
				else
				{
					DisplayHelp.FillRectangle(graphics, rectangle, legacyColors.BarBackground);
				}
			}
			else if (!bar.ItemsContainer.BackColor.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, rectangle, bar.ItemsContainer.BackColor);
			}
			if (((Control)bar).Parent != null && ((Control)bar).Parent.BackgroundImage != null && ((Control)bar).Parent is DockSite)
			{
				Rectangle rectangle2 = new Rectangle(-bar.Location.X, -bar.Location.Y, ((Control)bar).Parent.Width, ((Control)bar).Parent.Height);
				DockSite dockSite = ((Control)bar).Parent as DockSite;
				BarFunctions._0982(graphics, rectangle2, ((Control)dockSite).BackgroundImage, dockSite.BackgroundImagePosition, dockSite.BackgroundImageAlpha);
			}
			else if (((Control)bar).BackgroundImage != null)
			{
				BarFunctions._0982(graphics, bounds, ((Control)bar).BackgroundImage, bar.BackgroundImagePosition, bar.BackgroundImageAlpha);
			}
			if (!bar.IsThemed && bar.LayoutType == eLayoutType.Toolbar && ((Control)bar).BackColor != Color.Transparent && က != null)
			{
				Pen val = new Pen(က.Colors.BarDockedBorder, 1f);
				try
				{
					graphics.DrawLine(val, 0, ((Control)bar).Height - 1, ((Control)bar).Width, ((Control)bar).Height - 1);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else
			{
				Rectangle ल2 = bounds;
				ल2.Inflate(-2, -2);
				BarFunctions._२(graphics, bar.DockedBorderStyle, ल2, bar.SingleLineColor);
			}
		}
		else if (!((Control)bar).BackColor.IsEmpty && bar.ShouldSerializeBackColor())
		{
			DisplayHelp.FillRectangle(graphics, bounds, ((Control)bar).BackColor);
		}
		else
		{
			Office2007BarColorTable bar2 = ຊ.Bar;
			Rectangle r = bounds;
			if (!က.ໟ)
			{
				r.Inflate(1, 1);
			}
			if (bar2.ToolbarBottomBackground == null || bar2.ToolbarBottomBackground.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, r, bar2.ToolbarTopBackground);
			}
			else
			{
				r.Height = (int)((float)r.Height * _24B0);
				r.Height++;
				DisplayHelp.FillRectangle(graphics, r, bar2.ToolbarTopBackground);
				r.Height--;
				r.Y += r.Height;
				r.Height = bounds.Height - r.Height + 1;
				DisplayHelp.FillRectangle(graphics, r, bar2.ToolbarBottomBackground);
			}
			if (bar.BarType == eBarType.StatusBar && bar.Items.Count > 0 && bar.Items[bar.Items.Count - 1] is ItemContainer && e.က != null && e.က.ໟ)
			{
				ItemContainer itemContainer = bar.Items[bar.Items.Count - 1] as ItemContainer;
				if (itemContainer.Visible && itemContainer.BackgroundStyle.Class == ElementStyleClassKeys.Office2007StatusBarBackground2Key)
				{
					Rectangle bounds2 = new Rectangle(itemContainer.Bounds.X, bounds.Y, itemContainer.Bounds.Width, ((Control)e.Bar).Height + 1);
					if (e.က.RightToLeft)
					{
						bounds2.Width += bounds2.X;
						bounds2.X = 0;
					}
					else
					{
						bounds2.X += e.က.ContainerControl.Left;
						bounds2.Width += bounds.Right - bounds2.Right;
					}
					ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(itemContainer.BackgroundStyle, graphics, bounds2));
				}
			}
			if (((Control)bar).Parent != null && ((Control)bar).Parent.BackgroundImage != null && ((Control)bar).Parent is DockSite)
			{
				Rectangle rectangle3 = new Rectangle(-bar.Location.X, -bar.Location.Y, ((Control)bar).Parent.Width, ((Control)bar).Parent.Height);
				DockSite dockSite2 = ((Control)bar).Parent as DockSite;
				BarFunctions._0982(graphics, rectangle3, ((Control)dockSite2).BackgroundImage, dockSite2.BackgroundImagePosition, dockSite2.BackgroundImageAlpha);
			}
			else if (((Control)bar).BackgroundImage != null)
			{
				BarFunctions._0982(graphics, ((Control)bar).ClientRectangle, ((Control)bar).BackgroundImage, bar.BackgroundImagePosition, bar.BackgroundImageAlpha);
			}
			if ((!bar2.ToolbarBottomBorder.IsEmpty && bar.BarType != eBarType.StatusBar) || (!bar2.StatusBarTopBorder.IsEmpty && bar.BarType == eBarType.StatusBar))
			{
				if (bar.BarType == eBarType.StatusBar)
				{
					Pen val2 = new Pen(bar2.StatusBarTopBorder, 1f);
					try
					{
						graphics.DrawLine(val2, bounds.X, bounds.Y, bounds.Right, bounds.Y);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					if (!bar2.StatusBarTopBorderLight.IsEmpty)
					{
						Pen val3 = new Pen(bar2.StatusBarTopBorderLight, 1f);
						try
						{
							graphics.DrawLine(val3, bounds.X, bounds.Y + 1, bounds.Right, bounds.Y + 1);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
				}
				else
				{
					Pen val4 = new Pen(bar2.ToolbarBottomBorder, 1f);
					try
					{
						graphics.DrawLine(val4, bounds.X, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
			}
		}
		if (က != null && !က.ໟ)
		{
			bar._0BD1(က);
		}
	}

	private bool _24B1(Bar ච)
	{
		if (ච.Style == eDotNetBarStyle.VS2005 && ච.LayoutType == eLayoutType.DockContainer)
		{
			return false;
		}
		return true;
	}

	public override void PaintFloatingBackground(ToolbarRendererEventArgs e)
	{
		Graphics graphics = e.Graphics;
		Bar bar = e.Bar;
		_ = e.က;
		Rectangle bounds = e.Bounds;
		ColorScheme legacyColors = ຊ.LegacyColors;
		DisplayHelp.FillRectangle(graphics, bounds, legacyColors.BarBackground, legacyColors.BarBackground2, legacyColors.BarBackgroundGradientAngle, new float[3] { 0f, 0.12f, 1f }, new float[3] { 0f, 0.5f, 1f });
		if (((Control)bar).BackgroundImage != null)
		{
			BarFunctions._0982(graphics, ((Control)bar).ClientRectangle, ((Control)bar).BackgroundImage, bar.BackgroundImagePosition, bar.BackgroundImageAlpha);
		}
	}

	public override void PaintPopupBackground(ToolbarRendererEventArgs e)
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Bar bar = e.Bar;
		_ = e.က;
		Office2007BarColorTable bar2 = ຊ.Bar;
		int ଖ = bar.ଖ;
		Rectangle bounds = e.Bounds;
		DisplayHelp.FillRectangle(graphics, bounds, bar2.PopupToolbarBackground.Start, bar2.PopupToolbarBackground.End, bar2.PopupToolbarBackground.GradientAngle);
		if (((Control)bar).BackgroundImage != null)
		{
			BarFunctions._0982(graphics, ((Control)bar).ClientRectangle, ((Control)bar).BackgroundImage, bar.BackgroundImagePosition, bar.BackgroundImageAlpha);
		}
		bar._0BDB(graphics);
		Rectangle bounds2 = bounds;
		if (bar.ଌ && !bar._0B0D)
		{
			bounds2 = new Rectangle(0, 0, ((Control)bar).ClientSize.Width - 2, ((Control)bar).ClientSize.Height - 2);
		}
		Pen val = new Pen(bar2.PopupToolbarBorder, 1f);
		try
		{
			DisplayHelp.DrawRoundedRectangle(graphics, val, bounds2, ଖ);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (bar.ଌ && !bar._0B0D)
		{
			Point[] array = new Point[3];
			array[0].X = 2;
			array[0].Y = bounds.Height - 1;
			array[1].X = bounds.Width - 1;
			array[1].Y = bounds.Height - 1;
			array[2].X = bounds.Width - 1;
			array[2].Y = 2;
			Pen val2 = new Pen(SystemColors.ControlDark, 2f);
			try
			{
				graphics.DrawLines(val2, array);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}
}
internal class _24B2
{
	public virtual void PaintDockedBackground(ToolbarRendererEventArgs e)
	{
	}

	public virtual void PaintFloatingBackground(ToolbarRendererEventArgs e)
	{
	}

	public virtual void PaintPopupBackground(ToolbarRendererEventArgs e)
	{
	}
}
internal class _24B3 : _၁, ᙓ
{
	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void PaintKeyTips(KeyTipsRendererEventArgs e)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		Color keyTipBorder = ຊ.KeyTips.KeyTipBorder;
		Color color = ຊ.KeyTips.KeyTipText;
		Color color2 = ຊ.KeyTips.KeyTipBackground;
		if (e.ReferenceObject is BaseItem && !((BaseItem)e.ReferenceObject).Enabled)
		{
			int alpha = 128;
			color2 = Color.FromArgb(alpha, color2);
			color = Color.FromArgb(alpha, color);
			keyTipBorder = Color.FromArgb(alpha, keyTipBorder);
		}
		Graphics graphics = e.Graphics;
		string keyTip = e.KeyTip;
		Font font = e.Font;
		SolidBrush val = new SolidBrush(color2);
		try
		{
			DisplayHelp.FillRoundedRectangle(graphics, (Brush)(object)val, bounds, 2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(color, 1f);
		try
		{
			DisplayHelp.DrawRoundedRectangle(graphics, val2, bounds, 2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		TextDrawing.DrawString(graphics, keyTip, font, color, bounds, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
	}
}
internal class _24B4 : _24B5, ᙓ
{
	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void PaintDialogLauncher(RibbonBarRendererEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		Rectangle bounds = e.Bounds;
		Graphics graphics = e.Graphics;
		bool flag = (int)((Control)e.RibbonBar).RightToLeft == 1;
		Office2007DialogLauncherStateColorTable office2007DialogLauncherStateColorTable = ጧ(e);
		if (!office2007DialogLauncherStateColorTable.TopBackground.IsEmpty && !office2007DialogLauncherStateColorTable.BottomBackground.IsEmpty)
		{
			ᙸ ᙸ = new ᙸ(new ᙔ(office2007DialogLauncherStateColorTable.OuterBorder), new ᙙ(office2007DialogLauncherStateColorTable.TopBackground));
			ᙸ.ᙟ = new ᙯ(1, 1, 1, 1);
			ᙸ ᙸ2 = new ᙸ(new ᙙ(office2007DialogLauncherStateColorTable.BottomBackground));
			ᙸ2.ᇐ._0711 = bounds.Height / 2;
			ᙸ.ᙠ._0CCB(ᙸ2);
			ᙸ2 = new ᙸ(new ᙔ(office2007DialogLauncherStateColorTable.InnerBorder));
			ᙸ.ᙠ._0CCB(ᙸ2);
			ᙰ p = new ᙰ(graphics, bounds);
			ᙸ.Paint(p);
		}
		Size size = new Size(Dpi.Width8, Dpi.Height8);
		bounds = ((!flag) ? new Rectangle(bounds.X + (bounds.Width - size.Width) / 2, bounds.Y + (bounds.Height - size.Height) / 2, size.Width, size.Height) : new Rectangle(bounds.X + (bounds.Width - size.Width) / 2, bounds.Y + (bounds.Height - size.Height) / 2, size.Width, size.Height));
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)0;
		ᙔ ᙔ = new ᙔ(office2007DialogLauncherStateColorTable.DialogLauncherShade, 1);
		ᙙ ᙙ = new ᙙ(office2007DialogLauncherStateColorTable.DialogLauncherShade);
		ᙝ ᙝ = new ᙝ();
		_0CF3 _166E = new _0CF3(new ᙞ(), new ᙞ(Dpi.Width6, 0), ᙔ);
		ᙝ.ᙠ._0CCB(_166E);
		_166E = new _0CF3(new ᙞ(), new ᙞ(0, Dpi.Width6), ᙔ);
		ᙝ.ᙠ._0CCB(_166E);
		ᙸ ᙸ3 = new ᙸ();
		ᙸ3.ᙵ = ᙙ;
		ᙸ3.ᙞ.ᒈ = Dpi.Width5;
		ᙸ3.ᙞ.ᒉ = Dpi.Height5;
		ᙸ3.ᇐ.ܐ = Dpi.Width3;
		ᙸ3.ᇐ._0711 = Dpi.Height3;
		ᙝ.ᙠ._0CCB(ᙸ3);
		_166E = new _0CF3(new ᙞ(Dpi.Width7, Dpi.Height4), new ᙞ(Dpi.Width7, Dpi.Height7), ᙔ);
		ᙝ.ᙠ._0CCB(_166E);
		_166E = new _0CF3(new ᙞ(Dpi.Width4, Dpi.Height7), new ᙞ(Dpi.Width7, Dpi.Height7), ᙔ);
		ᙝ.ᙠ._0CCB(_166E);
		_166E = new _0CF3(new ᙞ(Dpi.Width4, Dpi.Height4), new ᙞ(Dpi.Width5, Dpi.Height5), ᙔ);
		ᙝ.ᙠ._0CCB(_166E);
		bounds.Offset(Dpi.Width1, Dpi.Height1);
		ᙰ ᙰ = new ᙰ(graphics, bounds);
		ᙝ.Paint(ᙰ);
		ᙔ.ᙕ = office2007DialogLauncherStateColorTable.DialogLauncher;
		ᙙ.ᙕ = office2007DialogLauncherStateColorTable.DialogLauncher;
		bounds.Offset(-Dpi.Width1, -Dpi.Height1);
		ᙰ.ঋ = bounds;
		ᙝ.Paint(ᙰ);
		graphics.SmoothingMode = smoothingMode;
	}

	private Office2007DialogLauncherStateColorTable ጧ(RibbonBarRendererEventArgs ؿ)
	{
		if (ؿ.Pressed)
		{
			return ຊ.DialogLauncher.Pressed;
		}
		if (ؿ.MouseOver)
		{
			return ຊ.DialogLauncher.MouseOver;
		}
		return ຊ.DialogLauncher.Default;
	}
}
internal abstract class _24B5
{
	public abstract void PaintDialogLauncher(RibbonBarRendererEventArgs e);
}
internal class _24B7
{
	public virtual void Paint(SystemCaptionItemRendererEventArgs e)
	{
		if (e.SystemCaptionItem.IsSystemIcon)
		{
			PaintSystemIcon(e, e.SystemCaptionItem.GetEnabled());
		}
		else
		{
			PaintFormButtons(e);
		}
	}

	public virtual void PaintSystemIcon(SystemCaptionItemRendererEventArgs e, bool isEnabled)
	{
	}

	public virtual void PaintFormButtons(SystemCaptionItemRendererEventArgs e)
	{
	}
}
internal class _24B8 : _24BA, ᙓ
{
	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void Paint(MdiSystemItemRendererEventArgs e)
	{
		MDISystemItem mdiSystemItem = e.MdiSystemItem;
		Graphics graphics = e.Graphics;
		Rectangle rectangle = mdiSystemItem.DisplayRectangle;
		if (mdiSystemItem.IsSystemIcon)
		{
			rectangle.Offset((rectangle.Width - 16) / 2, (rectangle.Height - 16) / 2);
			if (mdiSystemItem.Icon != null)
			{
				graphics.DrawIconUnstretched(mdiSystemItem.Icon, rectangle);
			}
			return;
		}
		rectangle = new Rectangle(mdiSystemItem.DisplayRectangle.Location, mdiSystemItem.ᱼ());
		if (mdiSystemItem.Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(0, (mdiSystemItem.DisplayRectangle.Height - rectangle.Height) / 2);
		}
		else
		{
			rectangle.Offset((mdiSystemItem.WidthInternal - rectangle.Width) / 2, 0);
		}
		_1935(graphics, mdiSystemItem, _1886._1C8C, rectangle, ጧ(mdiSystemItem, _1886._1C8C), ຊ.LegacyColors);
		if (mdiSystemItem.Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(rectangle.Width, 0);
		}
		else
		{
			rectangle.Offset(0, rectangle.Height);
		}
		_1935(graphics, mdiSystemItem, _1886._1C8D, rectangle, ጧ(mdiSystemItem, _1886._1C8D), ຊ.LegacyColors);
		if (mdiSystemItem.Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(rectangle.Width + 2, 0);
		}
		else
		{
			rectangle.Offset(0, rectangle.Height + 2);
		}
		_1935(graphics, mdiSystemItem, _1886.ᆤ, rectangle, ጧ(mdiSystemItem, _1886.ᆤ), ຊ.LegacyColors);
	}

	private Office2007ButtonItemStateColorTable ጧ(MDISystemItem _24B9, _1886 ᤉ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ຊ.ButtonItemColors[eButtonColor.Orange.ToString()];
		Office2007ButtonItemStateColorTable result = office2007ButtonItemColorTable.Default;
		if (_24B9.ᲀ == ᤉ)
		{
			result = office2007ButtonItemColorTable.Pressed;
		}
		else if (_24B9._1C7F == ᤉ)
		{
			result = office2007ButtonItemColorTable.MouseOver;
		}
		return result;
	}

	private void _1935(Graphics _٥, MDISystemItem _24B9, _1886 ᤉ, Rectangle ल, Office2007ButtonItemStateColorTable ዅ, ColorScheme ᲇ)
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		Region clip = _٥.Clip;
		_٥.SetClip(ल);
		ᯊ.ᯓ(_٥, ዅ, ल, RoundRectangleShapeDescriptor.RoundCorner2);
		ल.Inflate(-1, -1);
		ल.Offset(1, 0);
		Bitmap val = _24B9.ᲆ(_٥, ᤉ, ल, ᲇ);
		try
		{
			if ((ᤉ == _1886._1C8C && !_24B9.MinimizeEnabled) || (ᤉ == _1886._1C8D && !_24B9.RestoreEnabled) || (ᤉ == _1886.ᆤ && !_24B9.CloseEnabled))
			{
				float[][] array = new float[5][];
				float[] array2 = new float[5];
				array[0] = array2;
				float[] array3 = new float[5];
				array[1] = array3;
				float[] array4 = new float[5];
				array[2] = array4;
				array[3] = new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0f };
				float[] array5 = new float[5];
				array[4] = array5;
				ColorMatrix colorMatrix = new ColorMatrix(array);
				ImageAttributes val2 = new ImageAttributes();
				val2.ClearColorKey();
				val2.SetColorMatrix(colorMatrix);
				_٥.DrawImage((Image)(object)val, ल, 0, 0, ((Image)val).Width, ((Image)val).Height, (GraphicsUnit)2, val2);
			}
			else
			{
				if (ᤉ == _24B9.ᲀ)
				{
					ल.Offset(1, 1);
				}
				_٥.DrawImageUnscaled((Image)(object)val, ल);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		_٥.Clip = clip;
	}
}
internal class _24BA
{
	public virtual void Paint(MdiSystemItemRendererEventArgs e)
	{
	}
}
internal class _24BB : _24BC, ᙓ
{
	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void PaintCaptionBackground(FormCaptionRendererEventArgs e)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		Office2007FormStateColorTable office2007FormStateColorTable = ຊ.Form.Active;
		Form form = e.Form;
		bool flag = false;
		if (form != null && form is RibbonForm)
		{
			if (!((RibbonForm)(object)form).Ứ)
			{
				office2007FormStateColorTable = ຊ.Form.Inactive;
			}
		}
		else if (form != null && ((form != Form.ActiveForm && form.MdiParent == null) || (form.MdiParent != null && form.MdiParent.ActiveMdiChild != form)))
		{
			office2007FormStateColorTable = ຊ.Form.Inactive;
		}
		if (form is OfficeForm)
		{
			flag = true;
		}
		Rectangle bounds = e.Bounds;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)0;
		Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width, (int)((double)bounds.Height * 0.3));
		DisplayHelp.FillRectangle(graphics, r, office2007FormStateColorTable.CaptionTopBackground);
		Rectangle r2 = new Rectangle(bounds.X, r.Bottom, bounds.Width, bounds.Height - r.Height);
		DisplayHelp.FillRectangle(graphics, r2, office2007FormStateColorTable.CaptionBottomBackground);
		if (flag && office2007FormStateColorTable.CaptionBottomBorder != null && office2007FormStateColorTable.CaptionBottomBorder.Length > 0)
		{
			int num = office2007FormStateColorTable.CaptionBottomBorder.Length;
			for (int i = 0; i < num; i++)
			{
				DisplayHelp.DrawLine(graphics, bounds.X, bounds.Bottom - num + i, bounds.Right, bounds.Bottom - num + i, office2007FormStateColorTable.CaptionBottomBorder[i], 1);
			}
		}
		graphics.SmoothingMode = smoothingMode;
	}
}
internal abstract class _24BC
{
	public abstract void PaintCaptionBackground(FormCaptionRendererEventArgs e);
}
internal class _24DF : ᙓ
{
	private Office2007ColorTable __137C;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
		}
	}

	public void _139A(ButtonItem _06EC, ItemPaintArgs _0BD2)
	{
		_139A(_06EC, _0BD2, __137C.CrumbBarItemView);
	}

	public void _139A(ButtonItem _06EC, ItemPaintArgs _0BD2, CrumbBarItemViewColorTable _24E0)
	{
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = _0BD2.Graphics;
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = _24E0.Default;
		CrumbBarItemViewStateColorTable _24E2 = null;
		bool _24E3 = false;
		if (_06EC.IsMouseDown || _06EC.Expanded)
		{
			crumbBarItemViewStateColorTable = _24E0.Pressed;
			_24E3 = true;
		}
		else if (_06EC.IsMouseOverExpand)
		{
			crumbBarItemViewStateColorTable = _24E0.MouseOverInactive;
			_24E2 = _24E0.MouseOver;
		}
		else if (_06EC.IsMouseOver)
		{
			crumbBarItemViewStateColorTable = _24E0.MouseOver;
		}
		Rectangle भ = _06EC.DisplayRectangle;
		भ.Width--;
		भ.Height--;
		Rectangle _24E4 = _06EC.ໜ();
		if (!_24E4.IsEmpty)
		{
			_24E4.Offset(भ.Location);
			_24E4.Width--;
			_24E4.Height--;
		}
		ᯓ(_06EC, graphics, crumbBarItemViewStateColorTable, _24E2, _24E3, ref भ, ref _24E4);
		Color color = crumbBarItemViewStateColorTable.Foreground;
		if (!_06EC.ForeColor.IsEmpty)
		{
			color = _06EC.ForeColor;
		}
		if (color.IsEmpty)
		{
			return;
		}
		Font val = _06EC._໘(_0BD2, _໙: false);
		bool rightToLeft = _0BD2.RightToLeft;
		भ = GetTextRectangle(_06EC);
		eTextFormat eTextFormat2 = eTextFormat.HidePrefix | eTextFormat.VerticalCenter;
		if (_06EC.TextMarkupBody == null)
		{
			TextDrawing.DrawString(graphics, _192F.ಱ(_06EC.Text), val, color, भ, eTextFormat2);
		}
		else
		{
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, val, color, rightToLeft);
			markupDrawContext.HotKeyPrefixVisible = (eTextFormat2 & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
			markupDrawContext.ContextObject = _06EC;
			Rectangle bounds = new Rectangle(भ.X, भ.Y + (भ.Height - _06EC.TextMarkupBody.Bounds.Height) / 2 + 1, _06EC.TextMarkupBody.Bounds.Width, _06EC.TextMarkupBody.Bounds.Height);
			_06EC.TextMarkupBody.Bounds = bounds;
			_06EC.TextMarkupBody.Render(markupDrawContext);
		}
		if ((_06EC.SubItems.Count <= 0 && _06EC.PopupType != ePopupType.Container) || !_06EC.ShowSubItems)
		{
			return;
		}
		GraphicsPath val2 = _24E5(_06EC, _24E4);
		if (val2 != null)
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)0;
			Brush val3 = (Brush)new SolidBrush(crumbBarItemViewStateColorTable.Foreground);
			try
			{
				graphics.FillPath(val3, val2);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			graphics.SmoothingMode = smoothingMode;
		}
	}

	private static void ᯓ(ButtonItem _06EC, Graphics _٥, CrumbBarItemViewStateColorTable _24E1, CrumbBarItemViewStateColorTable _24E2, bool _24E3, ref Rectangle भ, ref Rectangle _24E4)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		if (_24E1.Background != null && _24E1.Background.Count > 0)
		{
			Brush val = DisplayHelp.CreateBrush(भ, _24E1.Background, 90, eGradientType.Linear);
			try
			{
				_٥.FillRectangle(val, भ);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (_06EC.IsMouseOverExpand && _24E2 != null && _24E2.Background != null && _24E2.Background.Count > 0)
			{
				Brush val2 = DisplayHelp.CreateBrush(भ, _24E2.Background, 90, eGradientType.Linear);
				try
				{
					_٥.FillRectangle(val2, _24E4);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		if (!_24E1.Border.IsEmpty)
		{
			Pen val3 = new Pen(_24E1.Border, 1f);
			try
			{
				_٥.DrawLine(val3, भ.X, भ.Y, भ.X, भ.Bottom);
				Pen val4 = (Pen)((_24E2 == null) ? ((object)val3) : ((object)new Pen(_24E2.Border, 1f)));
				if (!_24E4.IsEmpty)
				{
					_٥.DrawLine(val4, _24E4.X, _24E4.Y, _24E4.X, _24E4.Bottom);
				}
				if (_24E3)
				{
					_٥.DrawLine(val3, भ.X, भ.Y, भ.Right, भ.Y);
				}
				_٥.DrawLine(val4, भ.Right, भ.Y, भ.Right, भ.Bottom);
				if (_24E2 != null)
				{
					val4.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		if (_24E1.BorderLight.IsEmpty)
		{
			return;
		}
		Rectangle rectangle = भ;
		if (!_24E4.IsEmpty)
		{
			rectangle.Width -= _24E4.Width;
		}
		rectangle.Inflate(-1, 0);
		Pen val5 = new Pen(_24E1.BorderLight, 1f);
		try
		{
			if (_24E3)
			{
				_٥.DrawLine(val5, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Y);
				_٥.DrawLine(val5, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom);
				if (!_24E4.IsEmpty)
				{
					rectangle = _24E4;
					rectangle.Inflate(-1, 0);
					_٥.DrawLine(val5, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Y);
					_٥.DrawLine(val5, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom);
				}
			}
			else
			{
				_٥.DrawRectangle(val5, rectangle);
				if (!_24E4.IsEmpty)
				{
					rectangle = _24E4;
					rectangle.Inflate(-1, 0);
					_٥.DrawRectangle(val5, rectangle);
				}
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
	}

	private GraphicsPath _24E5(ButtonItem _06EC, Rectangle _24E4)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		if (_06EC.Expanded)
		{
			Point point = new Point(_24E4.X + (_24E4.Width - Dpi.Width6) / 2, _24E4.Y + (_24E4.Height - 1) / 2);
			if (_06EC.IsMouseDown)
			{
				point.Offset(1, 1);
			}
			val.AddLine(point.X, point.Y, point.X + Dpi.Width8, point.Y);
			val.AddLine(point.X + Dpi.Width8, point.Y, point.X + Dpi.Width4, point.Y + Dpi.Height4);
			val.CloseAllFigures();
		}
		else
		{
			Point point2 = new Point(_24E4.X + (_24E4.Width - Dpi.Width2) / 2, _24E4.Y + (_24E4.Height - Dpi.Height7) / 2);
			if (_06EC.IsMouseDown)
			{
				point2.Offset(1, 1);
			}
			val.AddLine(point2.X, point2.Y, point2.X, point2.Y + Dpi.Height8);
			val.AddLine(point2.X, point2.Y + Dpi.Height8, point2.X + Dpi.Width4, point2.Y + Dpi.Height4);
			val.CloseAllFigures();
		}
		return val;
	}

	protected virtual Rectangle GetTextRectangle(ButtonItem button)
	{
		Rectangle displayRectangle = button.DisplayRectangle;
		Rectangle textDrawRect = button.TextDrawRect;
		textDrawRect.Offset(displayRectangle.Left, displayRectangle.Top);
		if (textDrawRect.Right > displayRectangle.Right)
		{
			textDrawRect.Width = displayRectangle.Right - textDrawRect.Left;
		}
		textDrawRect.X += 2;
		textDrawRect.Y--;
		if (button.IsMouseDown)
		{
			textDrawRect.Offset(1, 1);
		}
		return textDrawRect;
	}

	public void _24E6(ButtonItem _06EC, ItemPaintArgs _0BD2)
	{
		_24E6(_06EC, _0BD2, __137C.CrumbBarItemView);
	}

	public void _24E6(ButtonItem _06EC, ItemPaintArgs _0BD2, CrumbBarItemViewColorTable _24E0)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		Graphics graphics = _0BD2.Graphics;
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = _24E0.Default;
		CrumbBarItemViewStateColorTable _24E2 = null;
		bool flag = false;
		if (_06EC.IsMouseDown || _06EC.Expanded)
		{
			crumbBarItemViewStateColorTable = _24E0.Pressed;
			flag = true;
		}
		else if (_06EC.IsMouseOver)
		{
			crumbBarItemViewStateColorTable = _24E0.MouseOver;
		}
		Rectangle भ = _06EC.DisplayRectangle;
		भ.Width--;
		भ.Height--;
		Rectangle _24E4 = Rectangle.Empty;
		ᯓ(_06EC, graphics, crumbBarItemViewStateColorTable, _24E2, flag, ref भ, ref _24E4);
		Color foreground = crumbBarItemViewStateColorTable.Foreground;
		if (foreground.IsEmpty)
		{
			return;
		}
		_ = graphics.SmoothingMode;
		Point point = new Point(भ.X + (भ.Width - 7) / 2, भ.Y + (भ.Height - 5) / 2);
		if (flag)
		{
			point.Offset(1, 1);
		}
		Pen val = new Pen(foreground, 1f);
		try
		{
			for (int i = 0; i < 2; i++)
			{
				graphics.DrawLine(val, point.X + 3, point.Y, point.X, point.Y + 2);
				graphics.DrawLine(val, point.X, point.Y + 2, point.X + 3, point.Y + 4);
				point.X += 4;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
internal abstract class _250D
{
	public abstract void PaintTabGroup(RibbonTabGroupRendererEventArgs e);
}
internal class _26DB : IDisposable
{
	private BaseItem _ᕹ;

	private Control __1AAD;

	private bool _ኗ;

	private BaseRenderer ਛ;

	private BaseRenderer ਜ;

	private eRenderMode ԁ = eRenderMode.Global;

	public BaseItem _0CCA => _ᕹ;

	public _26DB(BaseItem item, Control parentControl)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		_ᕹ = item;
		__1AAD = parentControl;
		__1AAD.MouseDown += new MouseEventHandler(_26E1);
		__1AAD.MouseEnter += _26E0;
		__1AAD.MouseMove += new MouseEventHandler(_26DF);
		__1AAD.MouseUp += new MouseEventHandler(_26DE);
		__1AAD.MouseLeave += _26DD;
		__1AAD.Paint += new PaintEventHandler(_26DC);
		_ᕹ.ContainerControl = __1AAD;
	}

	private void _26DC(object _٠, PaintEventArgs ؿ)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (_ᕹ.NeedRecalcSize)
		{
			_ᕹ.RecalcSize();
		}
		_ᕹ.Displayed = true;
		Graphics graphics = ؿ.Graphics;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
		graphics.SmoothingMode = (SmoothingMode)4;
		graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		_ᕹ.Paint(_0A92(graphics));
		graphics.SmoothingMode = smoothingMode;
		graphics.TextRenderingHint = textRenderingHint;
	}

	private void _26DD(object _٠, EventArgs ؿ)
	{
		if (_ኗ)
		{
			_ᕹ.InternalMouseLeave();
			_ኗ = false;
		}
	}

	private void _26DE(object _٠, MouseEventArgs ؿ)
	{
		_ᕹ.InternalMouseUp(ؿ);
	}

	private void _26DF(object _٠, MouseEventArgs ؿ)
	{
		if (_ᕹ.Bounds.Contains(ؿ.Location))
		{
			if (!_ኗ)
			{
				_ᕹ.InternalMouseEnter();
				_ኗ = true;
			}
			_ᕹ.InternalMouseMove(ؿ);
		}
		else if (_ኗ)
		{
			_ᕹ.InternalMouseLeave();
			_ኗ = false;
		}
	}

	private void _26E0(object _٠, EventArgs ؿ)
	{
		Point pt = __1AAD.PointToClient(Control.MousePosition);
		if (_ᕹ.Bounds.Contains(pt))
		{
			_ᕹ.InternalMouseEnter();
		}
	}

	private void _26E1(object _٠, MouseEventArgs ؿ)
	{
		if (_ᕹ.Bounds.Contains(ؿ.Location))
		{
			_ᕹ.InternalMouseDown(ؿ);
		}
	}

	private ItemPaintArgs _0A92(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(null, __1AAD, _٥, _2105());
		itemPaintArgs.ໝ = GetRenderer();
		itemPaintArgs.ButtonStringFormat &= ~(itemPaintArgs.ButtonStringFormat & eTextFormat.SingleLine);
		itemPaintArgs.ButtonStringFormat |= eTextFormat.EndEllipsis | eTextFormat.WordBreak;
		return itemPaintArgs;
	}

	private ColorScheme _2105()
	{
		BaseRenderer renderer = GetRenderer();
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		return new ColorScheme(eDotNetBarStyle.StyleManagerControlled);
	}

	public virtual BaseRenderer GetRenderer()
	{
		return GlobalManager.Renderer;
	}

	public void Dispose()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		if (__1AAD != null)
		{
			__1AAD.MouseDown -= new MouseEventHandler(_26E1);
			__1AAD.MouseEnter -= _26E0;
			__1AAD.MouseMove -= new MouseEventHandler(_26DF);
			__1AAD.MouseUp -= new MouseEventHandler(_26DE);
			__1AAD.MouseLeave -= _26DD;
			__1AAD.Paint -= new PaintEventHandler(_26DC);
			_ᕹ.ContainerControl = null;
			_ᕹ = null;
			__1AAD = null;
		}
	}
}
internal static class _26E6
{
	internal static string _26E7(string ٮ)
	{
		if (ٮ.Length > 1)
		{
			return ٮ.Substring(0, 1).ToUpper() + ٮ.Substring(1);
		}
		return ٮ;
	}

	internal static string _26E8(string _06E3)
	{
		_06E3 = _06E3.Replace('_', ' ');
		if (_06E3.Contains(" "))
		{
			string[] array = _06E3.Split(new char[1] { ' ' });
			StringBuilder stringBuilder = new StringBuilder(_06E3.Length);
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(_26E7(array[i]));
				if (i < array.Length - 1)
				{
					stringBuilder.Append(' ');
				}
			}
			return stringBuilder.ToString();
		}
		if (_06E3.Length > 2)
		{
			bool flag = false;
			int num = -1;
			StringBuilder stringBuilder2 = new StringBuilder(_06E3.Length + 10);
			for (int j = 0; j < _06E3.Length; j++)
			{
				if (_26E9(_06E3[j]))
				{
					if (flag && j - num > 1)
					{
						stringBuilder2.Append(' ');
						stringBuilder2.Append(_06E3[j]);
						num = j;
					}
					else if (!flag)
					{
						num = j;
						flag = true;
						if (j > 0)
						{
							stringBuilder2.Append(' ');
						}
						stringBuilder2.Append(_06E3[j]);
					}
					else
					{
						num = j;
						stringBuilder2.Append(_06E3[j].ToString().ToLower());
						flag = true;
					}
				}
				else if (flag)
				{
					stringBuilder2.Append(_06E3[j]);
				}
				else
				{
					stringBuilder2.Append(_06E3[j].ToString().ToUpper());
					flag = true;
					num = j;
				}
			}
			return stringBuilder2.ToString();
		}
		return _06E3;
	}

	internal static bool _26E9(char _0BDE)
	{
		return _26E9(_0BDE.ToString());
	}

	internal static bool _26E9(string ٮ)
	{
		return ٮ.ToUpper() == ٮ;
	}

	internal static string _26EA(char _26EB, int _2041)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_26EB, _2041);
		return stringBuilder.ToString();
	}
}
[ToolboxItem(false)]
internal class _282B : Control
{
	private BubbleBar _282C;

	private Timer _282D;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			createParams.ExStyle = createParams.ExStyle | 0x80000 | 0x80;
			createParams.Style = -2046820352;
			createParams.Caption = "";
			return createParams;
		}
	}

	public _282B(BubbleBar parent)
	{
		if (parent == null)
		{
			throw new InvalidOperationException("Parent BubbleBar object for BubbleBarOverlay cannot be null.");
		}
		_282C = parent;
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)1, false);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).BackColor = Color.Transparent;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
	}

	internal void _282E()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		Bitmap val = new Bitmap(((Control)this).Width, ((Control)this).Height, (PixelFormat)2498570);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.Clear(Color.Transparent);
			val2.CompositingMode = (CompositingMode)1;
			val2.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			val2.SmoothingMode = (SmoothingMode)4;
			PaintEventArgs ؿ = new PaintEventArgs(val2, ((Control)this).DisplayRectangle);
			_282C._27F2(ؿ);
			IntPtr dC = _089F.GetDC(IntPtr.Zero);
			IntPtr intPtr = _0EFE.CreateCompatibleDC(dC);
			IntPtr hbitmap = val.GetHbitmap(Color.FromArgb(0));
			IntPtr hObject = _0EFE.SelectObject(intPtr, hbitmap);
			_089F.SIZE psize = default(_089F.SIZE);
			psize.cx = ((Control)this).Width;
			psize.cy = ((Control)this).Height;
			_089F.POINT pptDst = default(_089F.POINT);
			pptDst.x = ((Control)this).Left;
			pptDst.y = ((Control)this).Top;
			_089F.POINT pprSrc = default(_089F.POINT);
			pprSrc.x = 0;
			pprSrc.y = 0;
			_089F.BLENDFUNCTION pblend = default(_089F.BLENDFUNCTION);
			pblend.BlendOp = 0;
			pblend.BlendFlags = 0;
			pblend.SourceConstantAlpha = byte.MaxValue;
			pblend.AlphaFormat = 1;
			_089F.UpdateLayeredWindow(((Control)this).Handle, dC, ref pptDst, ref psize, intPtr, ref pprSrc, 0, ref pblend, 2);
			_0EFE.SelectObject(intPtr, hObject);
			_0EFE.ReleaseDC(IntPtr.Zero, dC);
			_0EFE.DeleteObject(hbitmap);
			_0EFE.DeleteDC(intPtr);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).Capture = true;
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (!((Control)this).DisplayRectangle.Contains(e.X, e.Y))
		{
			((Control)this).Capture = false;
		}
		_282C._27F8(e);
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		_282C._27F9(e);
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		_282C._27FD(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		_282C._27FF(e);
		((Control)this).OnMouseUp(e);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33)
		{
			((Message)(ref m)).Result = new IntPtr(3);
		}
		else
		{
			((Control)this).WndProc(ref m);
		}
	}

	private void ᆱ()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (_282D == null)
		{
			_282D = new Timer();
			_282D.Tick += _282F;
			_282D.Interval = 500;
			_282D.Enabled = true;
			_282D.Start();
		}
	}

	private void ᆵ()
	{
		if (_282D != null)
		{
			_282D.Stop();
			_282D.Enabled = false;
			((Component)(object)_282D).Dispose();
			_282D = null;
		}
	}

	private void _282F(object _٠, EventArgs ؿ)
	{
		try
		{
			BubbleBar bubbleBar = _282C;
			if (bubbleBar == null || ((Control)bubbleBar).IsDisposed)
			{
				ᆵ();
				return;
			}
			Form val = ((Control)bubbleBar).FindForm();
			IntPtr foregroundWindow = _089F.GetForegroundWindow();
			Control val2 = null;
			if (foregroundWindow != IntPtr.Zero)
			{
				val2 = Control.FromHandle(foregroundWindow);
			}
			if (val2 == null)
			{
				_2830();
				return;
			}
			if (val != null)
			{
				if (Form.ActiveForm == val)
				{
					return;
				}
				Form val3 = val;
				while (((ContainerControl)val3).ParentForm != null)
				{
					if (((ContainerControl)val3).ParentForm == Form.ActiveForm)
					{
						return;
					}
					val3 = ((ContainerControl)val3).ParentForm;
				}
				if (val.IsMdiChild && val.MdiParent != null && val.MdiParent.ActiveMdiChild == val)
				{
					return;
				}
			}
			_089F.POINT p = default(_089F.POINT);
			Point mousePosition = Control.MousePosition;
			p.x = mousePosition.X;
			p.x = mousePosition.Y;
			IntPtr intPtr = IntPtr.Zero;
			if (((Control)bubbleBar).Parent != null)
			{
				_089F.ChildWindowFromPoint(((Control)bubbleBar).Parent.Handle, p);
			}
			if (intPtr == IntPtr.Zero)
			{
				intPtr = _089F.WindowFromPoint(p);
			}
			if (intPtr != IntPtr.Zero && intPtr != ((Control)this).Handle && intPtr != ((Control)bubbleBar).Handle && ((Control)bubbleBar).Parent != null && ((Control)bubbleBar).Parent.Handle != intPtr)
			{
				_2830();
				return;
			}
			Point pt = ((Control)this).PointToClient(mousePosition);
			if (!((Control)this).DisplayRectangle.Contains(pt))
			{
				bubbleBar._27F9(ؿ);
			}
		}
		catch (NullReferenceException)
		{
			ᆵ();
		}
	}

	private void _2830()
	{
		((Control)this).Capture = false;
		ᆵ();
		_282C._27F5();
	}

	internal void _2831()
	{
		ᆱ();
	}

	internal void _2832()
	{
		ᆵ();
	}
}
internal class _2833
{
	private static int _2834 = 3;

	public static void _139A(BubbleButtonDisplayInfo _108F)
	{
		if (!_108F.Button.Visible)
		{
			return;
		}
		ᇎ ᇎ2 = null;
		Rectangle empty = Rectangle.Empty;
		if (_108F.Magnified)
		{
			empty = _108F.Button.MagnifiedDisplayRectangle;
			ᇎ2 = _2835(_108F.Button, empty.Size);
		}
		else
		{
			empty = _108F.Button.DisplayRectangle;
			ᇎ2 = _2835(_108F.Button, empty.Size);
		}
		if (empty.Width < 2 || empty.Height < 2)
		{
			return;
		}
		if (ᇎ2 != null)
		{
			if (!_108F.Button.Enabled)
			{
				ᇎ2.ᇗ(_108F.Graphics, empty, 0, 0, ᇎ2.ᇕ, ᇎ2.ᇔ, (GraphicsUnit)2, _2838());
			}
			else if (_108F.Button.MouseDown)
			{
				ᇎ2.ᇗ(_108F.Graphics, empty, 0, 0, ᇎ2.ᇕ, ᇎ2.ᇔ, (GraphicsUnit)2, _2837());
			}
			else
			{
				ᇎ2.ᇗ(_108F.Graphics, empty);
			}
		}
		else
		{
			_108F.Graphics.DrawRectangle(SystemPens.Highlight, empty);
		}
		if (_108F.Button.Focus)
		{
			empty.Inflate(1, 1);
			_1A6B._1A6C(_108F.Graphics, empty, Color.Navy);
		}
		_2836(_108F);
	}

	public static ᇎ _2835(BubbleButton ᤉ, Size ߒ)
	{
		ᇎ result = null;
		if (ᤉ.Image != null)
		{
			result = ((!(ᤉ.Image.Size == ߒ) && ᤉ.ImageLarge != null) ? ((ᤉ.ImageLarge == null || (!(ᤉ.ImageLarge.Size == ߒ) && ᤉ.ImageLarge.Size.Height / ߒ.Height >= 2)) ? new ᇎ(ᤉ.Image, dispose: false, ߒ) : new ᇎ(ᤉ.ImageLarge, dispose: false, ߒ)) : new ᇎ(ᤉ.Image, dispose: false, ߒ));
		}
		else if (ᤉ.ImageCached != null)
		{
			result = ((ᤉ.ImageCached.Size == ߒ || ᤉ.ImageLargeCached == null) ? new ᇎ(ᤉ.ImageCached, dispose: false, ߒ) : ((ᤉ.ImageLargeCached == null || ᤉ.ImageLargeCached.Size.Height / ߒ.Height > 2) ? new ᇎ(ᤉ.ImageCached, dispose: false, ߒ) : new ᇎ(ᤉ.ImageLargeCached, dispose: false, ߒ)));
		}
		return result;
	}

	private static void _2836(BubbleButtonDisplayInfo _108F)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		if (_108F.Button.MouseOver && _108F.BubbleBar.ShowTooltips && _108F.Button.TooltipText != "")
		{
			Color tooltipTextColor = _108F.BubbleBar.TooltipTextColor;
			Color tooltipOutlineColor = _108F.BubbleBar.TooltipOutlineColor;
			StringFormat stringFormat = TextDrawing.GetStringFormat(eTextFormat.Default);
			Graphics graphics = _108F.Graphics;
			CompositingMode val = (CompositingMode)0;
			graphics.CompositingMode = (CompositingMode)0;
			CompositingMode compositingMode = val;
			Font val2 = _108F.BubbleBar.TooltipFont;
			if (val2 == null)
			{
				val2 = ((Control)_108F.BubbleBar).Font;
			}
			Rectangle rectangle = _108F.Button.DisplayRectangle;
			Size size = TextDrawing.MeasureString(_108F.Graphics, _108F.Button.TooltipText, val2);
			if (_108F.Magnified)
			{
				rectangle = _108F.Button.MagnifiedDisplayRectangle;
			}
			if (_108F.Alignment == eBubbleButtonAlignment.Bottom)
			{
				rectangle.Y -= Math.Max(val2.Height, size.Height) + _2834;
			}
			else
			{
				rectangle.Y = rectangle.Bottom + _2834;
			}
			rectangle.Offset(-(size.Width - rectangle.Width) / 2, 0);
			Size size2 = TextDrawing.MeasureString(_108F.Graphics, _108F.Button.TooltipText, val2);
			Rectangle bounds = rectangle;
			bounds.Size = size2;
			bounds.Inflate(2, 2);
			SolidBrush val3 = new SolidBrush(Color.FromArgb(200, tooltipOutlineColor));
			try
			{
				DisplayHelp.FillRoundedRectangle(_108F.Graphics, (Brush)(object)val3, bounds, 3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			TextDrawing.DrawString(_108F.Graphics, _108F.Button.TooltipText, val2, tooltipTextColor, rectangle.X, rectangle.Y, eTextFormat.Default);
			_108F.Graphics.CompositingMode = compositingMode;
			stringFormat.Dispose();
		}
	}

	private static ImageAttributes _2837()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
		{
			new float[5] { 0.65f, 0f, 0f, 0f, 0f },
			new float[5] { 0f, 0.65f, 0f, 0f, 0f },
			new float[5] { 0f, 0f, 0.65f, 0f, 0f },
			new float[5] { 0f, 0f, 0f, 1f, 0f },
			new float[5] { 0f, 0f, 0f, 0f, 0.65f }
		});
		ImageAttributes val = new ImageAttributes();
		val.ClearColorKey();
		val.SetColorMatrix(colorMatrix);
		return val;
	}

	private static ImageAttributes _2838()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		float[][] array = new float[5][]
		{
			new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
			new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
			new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
			new float[5] { 0f, 0f, 0f, 1f, 0f },
			new float[5] { 0f, 0f, 0f, 0f, 1f }
		};
		ColorMatrix val = new ColorMatrix();
		val.Matrix00 = 1f / 3f;
		val.Matrix01 = 1f / 3f;
		val.Matrix02 = 1f / 3f;
		val.Matrix10 = 1f / 3f;
		val.Matrix11 = 1f / 3f;
		val.Matrix12 = 1f / 3f;
		val.Matrix20 = 1f / 3f;
		val.Matrix21 = 1f / 3f;
		val.Matrix22 = 1f / 3f;
		val.Matrix33 = 0.5f;
		ImageAttributes val2 = new ImageAttributes();
		val2.ClearColorKey();
		val2.SetColorMatrix(val);
		return val2;
	}
}
internal class _2841 : _2848
{
	public _2841()
	{
		base._2849 = true;
	}

	public override void Paint(Graphics g, TabStrip tabStrip)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		base.Paint(g, tabStrip);
		TabColorScheme colorScheme = tabStrip.ColorScheme;
		Rectangle clientRectangle = ((Control)tabStrip).ClientRectangle;
		if (((Control)tabStrip).BackColor != Color.Transparent || colorScheme.TabBackground != Color.Transparent)
		{
			if (colorScheme.TabPanelBackground2.IsEmpty)
			{
				if (!colorScheme.TabPanelBackground.IsEmpty)
				{
					SolidBrush val = new SolidBrush(colorScheme.TabPanelBackground);
					try
					{
						g.FillRectangle((Brush)(object)val, clientRectangle);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			else
			{
				SolidBrush val2 = new SolidBrush(Color.White);
				try
				{
					g.FillRectangle((Brush)(object)val2, clientRectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				LinearGradientBrush val3 = CreateTabGradientBrush(clientRectangle, colorScheme.TabPanelBackground, colorScheme.TabPanelBackground2, colorScheme.TabPanelBackgroundGradientAngle);
				try
				{
					g.FillRectangle((Brush)(object)val3, clientRectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		tabStrip.ᔲ(g);
		_2843(clientRectangle, g, colorScheme, new Region(((Control)tabStrip).DisplayRectangle), tabStrip.TabAlignment, tabStrip.ᓚ);
		for (int num = tabStrip.Tabs.Count - 1; num >= 0; num--)
		{
			TabItem tabItem = tabStrip.Tabs[num];
			if (tabItem.Visible && !tabItem.IsSelected && tabItem.DisplayRectangle.IntersectsWith(clientRectangle))
			{
				PaintTab(g, tabItem, first: false, last: false);
			}
		}
		if (tabStrip.SelectedTab != null && tabStrip.Tabs.Contains(tabStrip.SelectedTab))
		{
			PaintTab(g, tabStrip.SelectedTab, first: false, last: false);
		}
		g.ResetClip();
		tabStrip.ᔬ(g);
	}

	protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		Rectangle displayRectangle = tab.DisplayRectangle;
		GraphicsPath val = new GraphicsPath();
		if (!tab.IsSelected && !tab.IsMouseOver)
		{
			if (tab.TabAlignment == eTabStripAlignment.Bottom)
			{
				displayRectangle.Height--;
			}
			else if (tab.TabAlignment == eTabStripAlignment.Top)
			{
				displayRectangle.Height--;
				displayRectangle.Y++;
			}
			else if (tab.TabAlignment == eTabStripAlignment.Left)
			{
				displayRectangle.Width--;
				displayRectangle.X++;
			}
			else if (tab.TabAlignment == eTabStripAlignment.Right)
			{
				displayRectangle.Width--;
			}
			val.AddRectangle(displayRectangle);
		}
		else
		{
			if (tab.TabAlignment == eTabStripAlignment.Bottom)
			{
				val.AddLine(displayRectangle.X + 2, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom - 2);
				val.AddLine(displayRectangle.X, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y);
				val.AddLine(displayRectangle.Right, displayRectangle.Bottom - 2, displayRectangle.Right - 2, displayRectangle.Bottom);
				val.AddLine(displayRectangle.Right - 2, displayRectangle.Bottom, displayRectangle.X + 2, displayRectangle.Bottom);
			}
			else if (tab.TabAlignment == eTabStripAlignment.Top)
			{
				val.AddLine(displayRectangle.X, displayRectangle.Y + 2, displayRectangle.X + 2, displayRectangle.Y);
				val.AddLine(displayRectangle.Right - 2, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y + 2);
				val.AddLine(displayRectangle.Right, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom);
			}
			else if (tab.TabAlignment == eTabStripAlignment.Left)
			{
				val.AddLine(displayRectangle.Right, displayRectangle.Bottom, displayRectangle.X + 2, displayRectangle.Bottom);
				val.AddLine(displayRectangle.X + 2, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom - 2);
				val.AddLine(displayRectangle.X, displayRectangle.Bottom - 2, displayRectangle.X, displayRectangle.Y + 2);
				val.AddLine(displayRectangle.X, displayRectangle.Y + 2, displayRectangle.X + 2, displayRectangle.Y);
				val.AddLine(displayRectangle.Right - 1, displayRectangle.Y, displayRectangle.Right - 1, displayRectangle.Bottom);
			}
			else if (tab.TabAlignment == eTabStripAlignment.Right)
			{
				val.AddLine(displayRectangle.X, displayRectangle.Y, displayRectangle.Right - 2, displayRectangle.Y);
				val.AddLine(displayRectangle.Right - 2, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y + 2);
				val.AddLine(displayRectangle.Right, displayRectangle.Y + 2, displayRectangle.Right, displayRectangle.Bottom - 2);
				val.AddLine(displayRectangle.Right, displayRectangle.Bottom - 2, displayRectangle.Right - 2, displayRectangle.Bottom);
				val.AddLine(displayRectangle.X, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Y);
			}
			val.CloseAllFigures();
		}
		return val;
	}

	private GraphicsPath _2842(TabItem ᆰ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle displayRectangle = ᆰ.DisplayRectangle;
		if (ᆰ.TabAlignment == eTabStripAlignment.Bottom)
		{
			displayRectangle.Height++;
			val.AddLine(displayRectangle.Right, displayRectangle.Bottom - 2, displayRectangle.Right - 2, displayRectangle.Bottom);
			val.AddLine(displayRectangle.Right - 2, displayRectangle.Bottom, displayRectangle.X + 2, displayRectangle.Bottom);
			val.AddLine(displayRectangle.X + 2, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom - 2);
		}
		else if (ᆰ.TabAlignment == eTabStripAlignment.Top)
		{
			displayRectangle.Height++;
			displayRectangle.Y--;
			val.AddLine(displayRectangle.X, displayRectangle.Y + 2, displayRectangle.X + 2, displayRectangle.Y);
			val.AddLine(displayRectangle.X + 2, displayRectangle.Y, displayRectangle.Right - 2, displayRectangle.Y);
			val.AddLine(displayRectangle.Right - 2, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y + 2);
		}
		else if (ᆰ.TabAlignment == eTabStripAlignment.Left)
		{
			displayRectangle.Width++;
			displayRectangle.X--;
			val.AddLine(displayRectangle.X + 2, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom - 2);
			val.AddLine(displayRectangle.X, displayRectangle.Bottom - 2, displayRectangle.X, displayRectangle.Y + 2);
			val.AddLine(displayRectangle.X, displayRectangle.Y + 2, displayRectangle.X + 2, displayRectangle.Y);
		}
		else if (ᆰ.TabAlignment == eTabStripAlignment.Right)
		{
			displayRectangle.Width++;
			val.AddLine(displayRectangle.Right - 2, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y + 2);
			val.AddLine(displayRectangle.Right, displayRectangle.Y + 2, displayRectangle.Right, displayRectangle.Bottom - 2);
			val.AddLine(displayRectangle.Right, displayRectangle.Bottom - 2, displayRectangle.Right - 2, displayRectangle.Bottom);
		}
		return val;
	}

	private void _2843(Rectangle _2844, Graphics _٥, TabColorScheme ᛪ, Region _2845, eTabStripAlignment _2846, Rectangle _2847)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Expected O, but got Unknown
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Expected O, but got Unknown
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Expected O, but got Unknown
		int num = 3;
		GraphicsPath val = new GraphicsPath();
		GraphicsPath val2 = null;
		if (!_2847.IsEmpty)
		{
			Rectangle bounds;
			switch (_2846)
			{
			case eTabStripAlignment.Top:
			{
				_2847.Width += 3;
				bounds = new Rectangle(_2844.X, _2847.Y + 1, (_2847.Right > _2844.Right) ? _2844.Width : _2847.Right, _2847.Height - 1);
				val.AddLine(bounds.X, bounds.Bottom, bounds.X, bounds.Top + num);
				ᒇ cornerArc3 = GetCornerArc(bounds, num, eCornerArc.TopLeft);
				val.AddArc(cornerArc3.ᒈ, cornerArc3.ᒉ, cornerArc3.ܐ, cornerArc3._0711, cornerArc3.ᒊ, cornerArc3.ᒋ);
				val.AddLine(bounds.X + num, bounds.Y, bounds.Right - num, bounds.Y);
				cornerArc3 = GetCornerArc(bounds, num, eCornerArc.TopRight);
				val.AddArc(cornerArc3.ᒈ, cornerArc3.ᒉ, cornerArc3.ܐ, cornerArc3._0711, cornerArc3.ᒊ, cornerArc3.ᒋ);
				val.AddLine(bounds.Right, bounds.Y + num, bounds.Right, bounds.Bottom);
				object obj3 = val.Clone();
				val2 = (GraphicsPath)((obj3 is GraphicsPath) ? obj3 : null);
				val2.CloseAllFigures();
				if (_2847.Right < _2844.Right)
				{
					val.AddLine(bounds.Right, bounds.Bottom, _2844.Right, bounds.Bottom);
				}
				break;
			}
			case eTabStripAlignment.Bottom:
			{
				_2847.Width += 3;
				bounds = new Rectangle(_2844.X, _2844.Y, (_2847.Right > _2844.Right) ? _2844.Width : _2847.Right, _2847.Height - 1);
				val.AddLine(bounds.Right, bounds.Y, bounds.Right, bounds.Bottom - num);
				ᒇ cornerArc4 = GetCornerArc(bounds, num, eCornerArc.BottomRight);
				val.AddArc(cornerArc4.ᒈ, cornerArc4.ᒉ, cornerArc4.ܐ, cornerArc4._0711, cornerArc4.ᒊ, cornerArc4.ᒋ);
				val.AddLine(bounds.Right - num, bounds.Bottom, bounds.X + num, bounds.Bottom);
				cornerArc4 = GetCornerArc(bounds, num, eCornerArc.BottomLeft);
				val.AddArc(cornerArc4.ᒈ, cornerArc4.ᒉ, cornerArc4.ܐ, cornerArc4._0711, cornerArc4.ᒊ, cornerArc4.ᒋ);
				val.AddLine(bounds.X, bounds.Bottom - num, bounds.X, bounds.Y);
				object obj4 = val.Clone();
				val2 = (GraphicsPath)((obj4 is GraphicsPath) ? obj4 : null);
				val2.CloseAllFigures();
				if (_2847.Right < _2844.Right)
				{
					val.AddLine(bounds.Right, bounds.Y, _2844.Right, bounds.Y);
				}
				break;
			}
			case eTabStripAlignment.Left:
			{
				_2847.Height += 3;
				bounds = new Rectangle(_2847.X + 1, _2844.Y, _2847.Width - 1, (_2847.Bottom > _2844.Bottom) ? _2844.Height : _2847.Bottom);
				val.AddLine(bounds.Right - 1, bounds.Bottom, bounds.X + num, bounds.Bottom);
				ᒇ cornerArc2 = GetCornerArc(bounds, num, eCornerArc.BottomLeft);
				val.AddArc(cornerArc2.ᒈ, cornerArc2.ᒉ, cornerArc2.ܐ, cornerArc2._0711, cornerArc2.ᒊ, cornerArc2.ᒋ);
				val.AddLine(bounds.X, bounds.Bottom - num, bounds.X, bounds.Y + num);
				cornerArc2 = GetCornerArc(bounds, num, eCornerArc.TopLeft);
				val.AddArc(cornerArc2.ᒈ, cornerArc2.ᒉ, cornerArc2.ܐ, cornerArc2._0711, cornerArc2.ᒊ, cornerArc2.ᒋ);
				val.AddLine(bounds.X + num, bounds.Y, bounds.Right - 1, bounds.Y);
				object obj2 = val.Clone();
				val2 = (GraphicsPath)((obj2 is GraphicsPath) ? obj2 : null);
				val2.CloseAllFigures();
				if (_2847.Bottom < _2844.Bottom)
				{
					val.AddLine(bounds.Right - 1, bounds.Bottom, bounds.Right - 1, _2844.Bottom);
				}
				break;
			}
			case eTabStripAlignment.Right:
			{
				_2847.Height += 3;
				bounds = new Rectangle(_2844.X, _2844.Y, _2847.Width - 1, (_2847.Bottom > _2844.Bottom) ? _2844.Height : _2847.Bottom);
				val.AddLine(bounds.X, bounds.Y, bounds.Right - num, bounds.Y);
				ᒇ cornerArc = GetCornerArc(bounds, num, eCornerArc.TopRight);
				val.AddArc(cornerArc.ᒈ, cornerArc.ᒉ, cornerArc.ܐ, cornerArc._0711, cornerArc.ᒊ, cornerArc.ᒋ);
				val.AddLine(bounds.Right, bounds.Y + num, bounds.Right, bounds.Bottom - num);
				cornerArc = GetCornerArc(bounds, num, eCornerArc.BottomRight);
				val.AddArc(cornerArc.ᒈ, cornerArc.ᒉ, cornerArc.ܐ, cornerArc._0711, cornerArc.ᒊ, cornerArc.ᒋ);
				val.AddLine(bounds.Right - num, bounds.Bottom, bounds.X, bounds.Bottom);
				object obj = val.Clone();
				val2 = (GraphicsPath)((obj is GraphicsPath) ? obj : null);
				val2.CloseAllFigures();
				if (_2847.Bottom < _2844.Bottom)
				{
					val.AddLine(bounds.X, bounds.Bottom, bounds.X, _2844.Bottom);
				}
				break;
			}
			}
		}
		else
		{
			val.AddRectangle(_2844);
		}
		if (ᛪ.TabPanelBackground2.IsEmpty)
		{
			if (!ᛪ.TabPanelBackground.IsEmpty)
			{
				SolidBrush val3 = new SolidBrush(ᛪ.TabPanelBackground);
				try
				{
					_٥.FillPath((Brush)(object)val3, val);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		else
		{
			SolidBrush val4 = new SolidBrush(Color.White);
			try
			{
				_٥.FillPath((Brush)(object)val4, val);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			LinearGradientBrush val5 = CreateTabGradientBrush(_2844, ᛪ.TabPanelBackground, ᛪ.TabPanelBackground2, ᛪ.TabPanelBackgroundGradientAngle);
			try
			{
				_٥.FillPath((Brush)(object)val5, val);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		if (!ᛪ.TabBorder.IsEmpty)
		{
			val.CloseAllFigures();
			Pen val6 = new Pen(ᛪ.TabBorder, 1f);
			try
			{
				_٥.DrawPath(val6, val);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		if (val2 != null)
		{
			val2.Dispose();
		}
		if (val != null)
		{
			val.Dispose();
		}
	}

	protected override void DrawTabItemBackground(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Expected O, but got Unknown
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Expected O, but got Unknown
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Expected O, but got Unknown
		RectangleF bounds = path.GetBounds();
		Rectangle r = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
		if (colors.BackColor2.IsEmpty)
		{
			if (!colors.BackColor.IsEmpty)
			{
				SolidBrush val = new SolidBrush(colors.BackColor);
				try
				{
					g.FillPath((Brush)(object)val, path);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(Color.White);
			try
			{
				g.FillPath((Brush)(object)val2, path);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			LinearGradientBrush val3 = CreateTabGradientBrush(r, colors.BackColor, colors.BackColor2, colors.BackColorGradientAngle);
			try
			{
				g.FillPath((Brush)(object)val3, path);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		Pen val4 = new Pen(colors.BorderColor, 1f);
		try
		{
			if (!colors.BorderColor.IsEmpty && (tab.IsSelected || tab.IsMouseOver))
			{
				g.DrawPath(val4, path);
			}
			else if (tab.TabAlignment == eTabStripAlignment.Top || tab.TabAlignment == eTabStripAlignment.Bottom)
			{
				if (!colors.LightBorderColor.IsEmpty)
				{
					Pen val5 = new Pen(colors.LightBorderColor, 1f);
					try
					{
						g.DrawLine(val5, r.Right - 1, r.Y + 4, r.Right - 1, r.Bottom - 4);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				if (!colors.DarkBorderColor.IsEmpty)
				{
					Pen val6 = new Pen(colors.DarkBorderColor, 1f);
					try
					{
						g.DrawLine(val6, r.Right, r.Y + 4, r.Right, r.Bottom - 4);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				g.DrawLine(val4, r.X, r.Y, r.Right, r.Y);
				g.DrawLine(val4, r.X, r.Bottom, r.Right, r.Bottom);
			}
			else if (tab.TabAlignment == eTabStripAlignment.Left || tab.TabAlignment == eTabStripAlignment.Right)
			{
				if (!colors.LightBorderColor.IsEmpty)
				{
					Pen val7 = new Pen(colors.LightBorderColor, 1f);
					try
					{
						g.DrawLine(val7, r.X + 4, r.Bottom - 1, r.Right - 4, r.Bottom - 1);
					}
					finally
					{
						((IDisposable)val7)?.Dispose();
					}
				}
				if (!colors.DarkBorderColor.IsEmpty)
				{
					Pen val8 = new Pen(colors.DarkBorderColor, 1f);
					try
					{
						g.DrawLine(val8, r.X + 4, r.Bottom, r.Right - 4, r.Bottom);
					}
					finally
					{
						((IDisposable)val8)?.Dispose();
					}
				}
				g.DrawLine(val4, r.X, r.Y, r.X, r.Bottom);
				if (tab.TabAlignment == eTabStripAlignment.Left)
				{
					g.DrawLine(val4, r.Right - 1, r.Y, r.Right - 1, r.Bottom);
				}
				else
				{
					g.DrawLine(val4, r.Right, r.Y, r.Right, r.Bottom);
				}
			}
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		if (!tab.IsSelected && !tab.IsMouseOver)
		{
			return;
		}
		GraphicsPath val9 = _2842(tab);
		if (!colors.LightBorderColor.IsEmpty)
		{
			object obj = val9.Clone();
			GraphicsPath val10 = (GraphicsPath)((obj is GraphicsPath) ? obj : null);
			val10.CloseAllFigures();
			SolidBrush val11 = new SolidBrush(colors.LightBorderColor);
			try
			{
				g.FillPath((Brush)(object)val11, val10);
			}
			finally
			{
				((IDisposable)val11)?.Dispose();
			}
			val10.Dispose();
		}
		if (!colors.DarkBorderColor.IsEmpty)
		{
			Pen val12 = new Pen(colors.DarkBorderColor, 1f);
			try
			{
				g.DrawPath(val12, val9);
			}
			finally
			{
				((IDisposable)val12)?.Dispose();
			}
		}
		val9.Dispose();
	}
}
internal class _2848
{
	private bool ե = true;

	private bool ᵑ;

	private bool ᔝ;

	private Size _284B = new Size(11, 11);

	private int _284C = 3;

	public virtual bool AntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			ե = value;
		}
	}

	public bool _2849
	{
		get
		{
			return ᵑ;
		}
		set
		{
			ᵑ = value;
		}
	}

	public bool ῷ
	{
		get
		{
			return ᔝ;
		}
		set
		{
			ᔝ = value;
		}
	}

	public int _284A
	{
		get
		{
			return _284C;
		}
		set
		{
			_284C = value;
		}
	}

	public virtual void Paint(Graphics g, TabStrip tabStrip)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (AntiAlias)
		{
			g.SmoothingMode = (SmoothingMode)4;
			g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
	}

	protected virtual void PaintTab(Graphics g, TabItem tab, bool first, bool last)
	{
		if (!tab.Visible)
		{
			return;
		}
		if (tab.Parent != null && tab.Parent.ᓞ)
		{
			RenderTabItemEventArgs renderTabItemEventArgs = new RenderTabItemEventArgs(tab, g);
			tab.Parent.ᕉ(renderTabItemEventArgs);
			if (renderTabItemEventArgs.Cancel)
			{
				return;
			}
		}
		GraphicsPath tabItemPath = GetTabItemPath(tab, first, last);
		try
		{
			TabColors colors = tab.Parent.ᕝ(tab);
			DrawTabItemBackground(tab, tabItemPath, colors, g);
			DrawTabText(tab, colors, g);
		}
		finally
		{
			((IDisposable)tabItemPath)?.Dispose();
		}
		if (tab.Parent != null && tab.Parent.ᓟ)
		{
			RenderTabItemEventArgs ؿ = new RenderTabItemEventArgs(tab, g);
			tab.Parent.ᕊ(ؿ);
		}
	}

	protected virtual Region GetTabsRegion(TabsCollection tabs, TabItem lastTab)
	{
		return null;
	}

	protected virtual void DrawTabItemBackground(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		RectangleF bounds = path.GetBounds();
		Rectangle r = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		ቸ ቸ2 = colors.BackgroundColorBlend.ት();
		int num = colors.BackColorGradientAngle;
		if (tab.TabAlignment == eTabStripAlignment.Left || tab.TabAlignment == eTabStripAlignment.Right)
		{
			num -= 90;
		}
		switch (ቸ2)
		{
		case ቸ.ቺ:
			if (colors.BackColor.IsEmpty && colors.BackColor2.IsEmpty)
			{
				break;
			}
			try
			{
				Rectangle r2 = Rectangle.Ceiling(bounds);
				r2.Inflate(1, 1);
				LinearGradientBrush val2 = DisplayHelp.CreateLinearGradientBrush(r2, colors.BackColor, colors.BackColor2, num);
				try
				{
					val2.InterpolationColors = colors.BackgroundColorBlend.GetColorBlend();
					g.FillPath((Brush)(object)val2, path);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			catch
			{
				ቸ2 = ቸ.ቹ;
			}
			break;
		default:
		{
			Rectangle rectangle = Rectangle.Ceiling(bounds);
			BackgroundColorBlendCollection backgroundColorBlend = colors.BackgroundColorBlend;
			Region clip = g.Clip;
			g.SetClip(path, (CombineMode)1);
			for (int i = 0; i < backgroundColorBlend.Count; i += 2)
			{
				BackgroundColorBlend backgroundColorBlend2 = backgroundColorBlend[i];
				BackgroundColorBlend backgroundColorBlend3 = null;
				if (i < backgroundColorBlend.Count)
				{
					backgroundColorBlend3 = backgroundColorBlend[i + 1];
				}
				if (backgroundColorBlend2 != null && backgroundColorBlend3 != null)
				{
					Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Y + (int)backgroundColorBlend2.Position, rectangle.Width, ((backgroundColorBlend3.Position == 1f) ? rectangle.Height : ((int)backgroundColorBlend3.Position)) - (int)backgroundColorBlend2.Position);
					LinearGradientBrush val = DisplayHelp.CreateLinearGradientBrush(rectangle2, backgroundColorBlend2.Color, backgroundColorBlend3.Color, num);
					try
					{
						g.FillRectangle((Brush)(object)val, rectangle2);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			g.Clip = clip;
			break;
		}
		case ቸ.ቹ:
			break;
		}
		if (ቸ2 == ቸ.ቹ)
		{
			if (colors.BackColor2.IsEmpty)
			{
				if (!colors.BackColor.IsEmpty)
				{
					SolidBrush val3 = new SolidBrush(colors.BackColor);
					try
					{
						g.FillPath((Brush)(object)val3, path);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
			}
			else
			{
				SolidBrush val4 = new SolidBrush(Color.White);
				try
				{
					g.FillPath((Brush)(object)val4, path);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				LinearGradientBrush val5 = CreateTabGradientBrush(r, colors.BackColor, colors.BackColor2, num);
				try
				{
					g.FillPath((Brush)(object)val5, path);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
		}
		g.SmoothingMode = smoothingMode;
		DrawTabBorder(tab, path, colors, g);
	}

	protected virtual void DrawTabBorder(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		if (!colors.BorderColor.IsEmpty)
		{
			Pen val = new Pen(colors.BorderColor, 1f);
			try
			{
				g.DrawPath(val, path);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	protected virtual LinearGradientBrush CreateTabGradientBrush(Rectangle r, Color color1, Color color2, int gradientAngle)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		if (r.Width <= 0)
		{
			r.Width = 1;
		}
		if (r.Height <= 0)
		{
			r.Height = 1;
		}
		return new LinearGradientBrush(new Rectangle(r.X, r.Y - 1, r.Width, r.Height + 1), color1, color2, (float)gradientAngle);
	}

	private Rectangle _284D(Graphics _٥, bool ᔵ, bool ᔶ, bool _᪇, ref Rectangle ᔸ, TabStrip _284E)
	{
		return _284D(_٥, ᔵ, ᔶ, _᪇, ref ᔸ, _284E, _284F: true);
	}

	private Rectangle _284D(Graphics _٥, bool ᔵ, bool ᔶ, bool _᪇, ref Rectangle ᔸ, TabStrip _284E, bool _284F)
	{
		Size size = Dpi.Size(_284B);
		int num = _284C;
		bool flag = _284E.CloseButtonPosition == eTabCloseButtonPosition.Left;
		Rectangle empty = Rectangle.Empty;
		empty = (flag ? ((!ᔵ) ? new Rectangle(ᔸ.X + num, ᔸ.Y + (ᔸ.Height - size.Height) / 2, size.Width, size.Height) : new Rectangle(ᔸ.X + (ᔸ.Width - size.Width) / 2, ᔸ.Y + num, size.Width, size.Height)) : ((!ᔵ) ? new Rectangle(ᔸ.Right - num - size.Width, ᔸ.Y + (ᔸ.Height - size.Height) / 2, size.Width, size.Height) : new Rectangle(ᔸ.X + (ᔸ.Width - size.Width) / 2, ᔸ.Bottom - num - size.Height, size.Width, size.Height)));
		AdjustCloseButtonRectangle(ref empty, flag, ᔵ);
		if (_284F)
		{
			ᔴ(_٥, empty, ᔶ, _᪇, _284E);
		}
		if (ᔵ)
		{
			if (flag)
			{
				ᔸ.Y += empty.Height + num * 2;
			}
			ᔸ.Height -= empty.Height + num * 2;
		}
		else
		{
			if (flag)
			{
				ᔸ.X += empty.Width + num * 2;
			}
			ᔸ.Width -= empty.Width + num * 2;
		}
		return empty;
	}

	protected virtual void AdjustCloseButtonRectangle(ref Rectangle close, bool closeOnLeftSide, bool vertical)
	{
	}

	protected virtual void DrawTabText(TabItem tab, TabColors colors, Graphics g)
	{
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		int num = 4;
		eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		eTabStripAlignment tabAlignment = tab.Parent.TabAlignment;
		Rectangle ᔸ = tab.DisplayRectangle;
		bool flag = (tabAlignment == eTabStripAlignment.Left || tabAlignment == eTabStripAlignment.Right) && !ᵑ;
		if (ᔝ && tab.CloseButtonVisible)
		{
			if (flag)
			{
				ᔸ.Y++;
				ᔸ.Height--;
			}
			else
			{
				ᔸ.X += 2;
				ᔸ.Width -= 2;
			}
			bool _284F = tab.IsSelected || tab.IsMouseOver || tab.Parent.CloseButtonOnTabsAlwaysDisplayed;
			tab.CloseButtonBounds = _284D(g, flag, tab.CloseButtonMouseOver, tab.IsSelected, ref ᔸ, tab.Parent, _284F);
		}
		ᇎ tabImage = GetTabImage(tab);
		if (tabImage != null && tabImage.ܐ + 4 <= ᔸ.Width)
		{
			if (tabAlignment == eTabStripAlignment.Top || tabAlignment == eTabStripAlignment.Bottom || ᵑ)
			{
				tabImage.ᇗ(g, new Rectangle(ᔸ.X + 3, ᔸ.Y + (ᔸ.Height - tabImage._0711) / 2, tabImage.ܐ, tabImage._0711));
				int num2 = tabImage.ܐ + 2;
				ᔸ.X += num2;
				ᔸ.Width -= num2;
			}
			else
			{
				tabImage.ᇗ(g, new Rectangle(ᔸ.X + (ᔸ.Width - tabImage.ܐ) / 2, ᔸ.Y + 3, tabImage.ܐ, tabImage._0711));
				int num3 = tabImage._0711 + 2;
				ᔸ.Y += num3;
				ᔸ.Height -= num3;
			}
		}
		tabImage?.Dispose();
		bool flag2 = tab == tab.Parent.SelectedTab;
		if (tab.Parent.DisplaySelectedTextOnly && !flag2)
		{
			return;
		}
		if (!tab.Parent.AntiAlias)
		{
			g.TextRenderingHint = (TextRenderingHint)0;
		}
		Font font = ((Control)tab.Parent).Font;
		if (flag2 && tab.Parent.SelectedTabFont != null)
		{
			font = tab.Parent.SelectedTabFont;
		}
		AdjustTextRectangle(ref ᔸ, tabAlignment);
		if (flag)
		{
			g.RotateTransform(90f);
			ᔸ = new Rectangle(ᔸ.Top, -ᔸ.Right, ᔸ.Height, ᔸ.Width);
		}
		if (ᔸ.Width > num)
		{
			if ((tabAlignment == eTabStripAlignment.Left || tabAlignment == eTabStripAlignment.Right) && !ᵑ)
			{
				TextDrawing.DrawStringLegacy(g, tab.Text, font, colors.TextColor, ᔸ, format);
			}
			else
			{
				TextDrawing.DrawString(g, tab.Text, font, colors.TextColor, ᔸ, format);
			}
		}
		if (flag)
		{
			g.ResetTransform();
		}
		if (!tab.Parent.AntiAlias)
		{
			g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		if (tab.Parent.ShowFocusRectangle && ((Control)tab.Parent).Focused && flag2)
		{
			ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(tab.DisplayRectangle));
		}
	}

	protected virtual void AdjustTextRectangle(ref Rectangle rText, eTabStripAlignment tabAlignment)
	{
	}

	protected virtual ᇎ GetTabImage(TabItem tab)
	{
		Image image = tab.GetImage();
		if (image != null)
		{
			return new ᇎ(image, dispose: false);
		}
		Icon icon = tab.Icon;
		if (icon != null)
		{
			return new ᇎ(icon, dispose: false, tab.IconSize);
		}
		return null;
	}

	protected virtual Rectangle GetFocusRectangle(Rectangle rText)
	{
		rText.Inflate(-1, -1);
		return rText;
	}

	protected virtual TabItem GetLastVisibleTab(TabsCollection tabs)
	{
		int num = tabs.Count - 1;
		for (int num2 = num; num2 >= 0; num2--)
		{
			if (tabs[num2].Visible)
			{
				return tabs[num2];
			}
		}
		return null;
	}

	protected virtual void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		if (colors.TabBackground2.IsEmpty)
		{
			if (!colors.TabBackground.IsEmpty)
			{
				SolidBrush val = new SolidBrush(colors.TabBackground);
				try
				{
					g.FillRegion((Brush)(object)val, tabsRegion);
					return;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			return;
		}
		LinearGradientBrush val2 = BarFunctions.ॴ(tabsRegion.GetBounds(g), colors.TabBackground, colors.TabBackground2, colors.TabBackgroundGradientAngle);
		try
		{
			g.FillRegion((Brush)(object)val2, tabsRegion);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	protected virtual GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
	{
		return null;
	}

	protected virtual ᒇ GetCornerArc(Rectangle bounds, int cornerDiameter, eCornerArc corner)
	{
		int num = cornerDiameter * 2;
		return corner switch
		{
			eCornerArc.TopLeft => new ᒇ(bounds.X, bounds.Y, num, num, 180f, 90f), 
			eCornerArc.TopRight => new ᒇ(bounds.Right - num, bounds.Y, num, num, 270f, 90f), 
			eCornerArc.BottomLeft => new ᒇ(bounds.X, bounds.Bottom - num, num, num, 90f, 90f), 
			_ => new ᒇ(bounds.Right - num, bounds.Bottom - num, num, num, 0f, 90f), 
		};
	}

	public static void ᔴ(Graphics _٥, Rectangle ल, bool ᔶ, bool _᪇, TabStrip _284E)
	{
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		if (_284E.TabCloseButtonNormal != null)
		{
			Image val = _284E.TabCloseButtonNormal;
			if (ᔶ && _284E.TabCloseButtonHot != null)
			{
				val = _284E.TabCloseButtonHot;
			}
			_٥.DrawImageUnscaled(val, ल);
			return;
		}
		Color color = _284E.ColorScheme.TabItemBorder;
		Color color2 = _284E.ColorScheme.TabItemBorderLight;
		if (!color.IsEmpty && (double)Math.Abs(color.GetBrightness() - color2.GetBrightness()) <= 0.38)
		{
			color2 = BarFunctions.Ligten(color2, 60);
			color = BarFunctions.Darken(color, 40);
		}
		if (color.IsEmpty)
		{
			if (_284E.Style == eTabStripStyle.Office2003)
			{
				color = _284E.ColorScheme.TabPanelBackground;
				color2 = _284E.ColorScheme.TabItemText;
			}
			else if (!_284E.ColorScheme.TabItemSelectedBorder.IsEmpty)
			{
				color = _284E.ColorScheme.TabItemSelectedBorder;
				if (!_284E.ColorScheme.TabItemSelectedBorderLight.IsEmpty)
				{
					color2 = _284E.ColorScheme.TabItemSelectedBorderLight;
				}
				else if (_᪇ && color == _284E.ColorScheme.TabItemSelectedBackground && !_284E.ColorScheme.TabItemSelectedText.IsEmpty)
				{
					color2 = _284E.ColorScheme.TabItemSelectedText;
				}
				else if (_284E.Style == eTabStripStyle.Metro)
				{
					color2 = _284E.ColorScheme.TabBackground;
				}
			}
			else
			{
				color = SystemColors.ControlDark;
				color2 = SystemColors.ControlLightLight;
			}
		}
		if (color2.IsEmpty)
		{
			color2 = SystemColors.ControlLightLight;
		}
		color = Color.FromArgb(ᔶ ? 255 : 200, color);
		if ((_284E.Style == eTabStripStyle.Metro || _284E.Style == eTabStripStyle.Office2003) && !ᔶ)
		{
			if (StyleManager.Style == eStyle.Metro || StyleManager.Style == eStyle.OfficeMobile2014 || StyleManager.Style == eStyle.Office2016)
			{
				color2 = color;
			}
			color = Color.Empty;
		}
		if (!color.IsEmpty)
		{
			SmoothingMode smoothingMode = _٥.SmoothingMode;
			_٥.SmoothingMode = (SmoothingMode)4;
			Pen val2 = new Pen(Color.White, (float)Dpi.Width1);
			try
			{
				_٥.DrawEllipse(val2, ल);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			Pen val3 = new Pen(color, (float)Dpi.Width1);
			try
			{
				_٥.DrawEllipse(val3, ल);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			SolidBrush val4 = new SolidBrush(Color.White);
			try
			{
				_٥.FillEllipse((Brush)(object)val4, ल);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			SolidBrush val5 = new SolidBrush(color);
			try
			{
				_٥.FillEllipse((Brush)(object)val5, ल);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			_٥.SmoothingMode = smoothingMode;
		}
		Pen val6 = new Pen(color2, (float)Dpi.Width1);
		try
		{
			Rectangle rectangle = ल;
			rectangle.Inflate(-Dpi.Width3, -Dpi.Height3);
			rectangle.Width--;
			_٥.DrawLine(val6, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
			_٥.DrawLine(val6, rectangle.X, rectangle.Bottom, rectangle.Right, rectangle.Y);
			rectangle.Offset(1, 0);
			_٥.DrawLine(val6, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
			_٥.DrawLine(val6, rectangle.X, rectangle.Bottom, rectangle.Right, rectangle.Y);
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
	}
}
internal class _2850 : _2848
{
	public override void Paint(Graphics g, TabStrip tabStrip)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		base.Paint(g, tabStrip);
		TabsCollection tabs = tabStrip.Tabs;
		TabColorScheme colorScheme = tabStrip.ColorScheme;
		Rectangle clientRectangle = ((Control)tabStrip).ClientRectangle;
		Rectangle displayRectangle = ((Control)tabStrip).DisplayRectangle;
		displayRectangle.Inflate(2, 2);
		SolidBrush val = new SolidBrush(((Control)tabStrip).BackColor);
		try
		{
			g.FillRectangle((Brush)(object)val, displayRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		TabItem lastVisibleTab = GetLastVisibleTab(tabs);
		Rectangle selectedTabRect = Rectangle.Empty;
		if (tabStrip.SelectedTab != null)
		{
			selectedTabRect = tabStrip.SelectedTab.DisplayRectangle;
		}
		DrawBackground(tabStrip, clientRectangle, g, colorScheme, GetTabsRegion(tabs, lastVisibleTab), tabStrip.TabAlignment, selectedTabRect);
		bool first = true;
		foreach (TabItem item in tabs)
		{
			if (item.Visible)
			{
				if (item.DisplayRectangle.IntersectsWith(clientRectangle))
				{
					PaintTab(g, item, first, item == lastVisibleTab);
				}
				first = false;
			}
		}
	}

	protected override Region GetTabsRegion(TabsCollection tabs, TabItem lastTab)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		bool bFirst = true;
		Region val = new Region();
		val.MakeEmpty();
		foreach (TabItem tab in tabs)
		{
			if (tab.Visible)
			{
				GraphicsPath tabItemPath = GetTabItemPath(tab, bFirst, tab == lastTab);
				val.Union(tabItemPath);
				bFirst = false;
			}
		}
		return val;
	}

	protected override LinearGradientBrush CreateTabGradientBrush(Rectangle r, Color color1, Color color2, int gradientAngle)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		LinearGradientBrush val = base.CreateTabGradientBrush(r, color1, color2, gradientAngle);
		Blend val2 = new Blend(5);
		val2.Factors = new float[5] { 0f, 0.6f, 1f, 0.6f, 0f };
		val2.Positions = new float[5] { 0f, 0.3f, 0.5f, 0.7f, 1f };
		val.Blend = val2;
		return val;
	}

	protected override void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		int num = 5;
		GraphicsPath val = new GraphicsPath();
		switch (tabAlignment)
		{
		case eTabStripAlignment.Top:
		{
			Rectangle bounds = new Rectangle(tabStripRect.X, tabStripRect.Y + tabStripRect.Height / 2, tabStripRect.Width - 1, tabStripRect.Height - tabStripRect.Height / 2);
			val.AddLine(bounds.X, bounds.Bottom, bounds.X, bounds.Top + num);
			ᒇ cornerArc4 = GetCornerArc(bounds, num, eCornerArc.TopLeft);
			val.AddArc(cornerArc4.ᒈ, cornerArc4.ᒉ, cornerArc4.ܐ, cornerArc4._0711, cornerArc4.ᒊ, cornerArc4.ᒋ);
			val.AddLine(bounds.X + num, bounds.Y, bounds.Right - num, bounds.Y);
			cornerArc4 = GetCornerArc(bounds, num, eCornerArc.TopRight);
			val.AddArc(cornerArc4.ᒈ, cornerArc4.ᒉ, cornerArc4.ܐ, cornerArc4._0711, cornerArc4.ᒊ, cornerArc4.ᒋ);
			val.AddLine(bounds.Right, bounds.Y + num, bounds.Right, bounds.Bottom);
			break;
		}
		case eTabStripAlignment.Bottom:
		{
			Rectangle bounds = new Rectangle(tabStripRect.X, tabStripRect.Y, tabStripRect.Width - 1, tabStripRect.Height / 2);
			val.AddLine(bounds.Right, bounds.Y, bounds.Right, bounds.Bottom - num);
			ᒇ cornerArc3 = GetCornerArc(bounds, num, eCornerArc.BottomRight);
			val.AddArc(cornerArc3.ᒈ, cornerArc3.ᒉ, cornerArc3.ܐ, cornerArc3._0711, cornerArc3.ᒊ, cornerArc3.ᒋ);
			val.AddLine(bounds.Right - num, bounds.Bottom, bounds.X + num, bounds.Bottom);
			cornerArc3 = GetCornerArc(bounds, num, eCornerArc.BottomLeft);
			val.AddArc(cornerArc3.ᒈ, cornerArc3.ᒉ, cornerArc3.ܐ, cornerArc3._0711, cornerArc3.ᒊ, cornerArc3.ᒋ);
			val.AddLine(bounds.X, bounds.Bottom - num, bounds.X, bounds.Y);
			break;
		}
		case eTabStripAlignment.Left:
		{
			Rectangle bounds = new Rectangle(tabStripRect.X + tabStripRect.Width / 2, tabStripRect.Y, tabStripRect.Width / 2 + 1, tabStripRect.Height - 1);
			val.AddLine(bounds.Right, bounds.Bottom, bounds.X + num, bounds.Bottom);
			ᒇ cornerArc2 = GetCornerArc(bounds, num, eCornerArc.BottomLeft);
			val.AddArc(cornerArc2.ᒈ, cornerArc2.ᒉ, cornerArc2.ܐ, cornerArc2._0711, cornerArc2.ᒊ, cornerArc2.ᒋ);
			val.AddLine(bounds.X, bounds.Bottom - num, bounds.X, bounds.Y + num);
			cornerArc2 = GetCornerArc(bounds, num, eCornerArc.TopLeft);
			val.AddArc(cornerArc2.ᒈ, cornerArc2.ᒉ, cornerArc2.ܐ, cornerArc2._0711, cornerArc2.ᒊ, cornerArc2.ᒋ);
			val.AddLine(bounds.X + num, bounds.Y, bounds.Right, bounds.Y);
			break;
		}
		case eTabStripAlignment.Right:
		{
			Rectangle bounds = new Rectangle(tabStripRect.X, tabStripRect.Y, tabStripRect.Width / 2, tabStripRect.Height - 1);
			val.AddLine(bounds.X, bounds.Y, bounds.Right - num, bounds.Y);
			ᒇ cornerArc = GetCornerArc(bounds, num, eCornerArc.TopRight);
			val.AddArc(cornerArc.ᒈ, cornerArc.ᒉ, cornerArc.ܐ, cornerArc._0711, cornerArc.ᒊ, cornerArc.ᒋ);
			val.AddLine(bounds.Right, bounds.Y + num, bounds.Right, bounds.Bottom - num);
			cornerArc = GetCornerArc(bounds, num, eCornerArc.BottomRight);
			val.AddArc(cornerArc.ᒈ, cornerArc.ᒉ, cornerArc.ܐ, cornerArc._0711, cornerArc.ᒊ, cornerArc.ᒋ);
			val.AddLine(bounds.Right - num, bounds.Bottom, bounds.X, bounds.Bottom);
			break;
		}
		}
		object obj = val.Clone();
		GraphicsPath val2 = (GraphicsPath)((obj is GraphicsPath) ? obj : null);
		val2.CloseAllFigures();
		g.SetClip(val2, (CombineMode)0);
		g.SetClip(tabsRegion, (CombineMode)4);
		g.Clear(colors.TabPanelBackground);
		g.ResetClip();
		val2.Dispose();
		g.SetClip(tabsRegion, (CombineMode)4);
		Pen val3 = new Pen(colors.TabBorder, 1f);
		try
		{
			g.DrawPath(val3, val);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		val.Dispose();
		g.ResetClip();
	}

	protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		int num = 3;
		Rectangle displayRectangle = tab.DisplayRectangle;
		if (!bFirst && !bLast)
		{
			val.AddRectangle(displayRectangle);
		}
		else
		{
			if (bFirst)
			{
				if (tab.Parent.TabAlignment == eTabStripAlignment.Top || tab.Parent.TabAlignment == eTabStripAlignment.Bottom)
				{
					val.AddLine(displayRectangle.Right - (bLast ? num : 0), displayRectangle.Bottom, displayRectangle.X + num, displayRectangle.Bottom);
					ᒇ cornerArc = GetCornerArc(displayRectangle, num, eCornerArc.BottomLeft);
					val.AddArc(cornerArc.ᒈ, cornerArc.ᒉ, cornerArc.ܐ, cornerArc._0711, cornerArc.ᒊ, cornerArc.ᒋ);
					val.AddLine(displayRectangle.X, displayRectangle.Bottom - num, displayRectangle.X, displayRectangle.Y + num);
					cornerArc = GetCornerArc(displayRectangle, num, eCornerArc.TopLeft);
					val.AddArc(cornerArc.ᒈ, cornerArc.ᒉ, cornerArc.ܐ, cornerArc._0711, cornerArc.ᒊ, cornerArc.ᒋ);
					val.AddLine(displayRectangle.X + num, displayRectangle.Y, displayRectangle.Right - (bLast ? num : 0), displayRectangle.Y);
				}
				else
				{
					ᒇ cornerArc2 = GetCornerArc(displayRectangle, num, eCornerArc.TopLeft);
					val.AddArc(cornerArc2.ᒈ, cornerArc2.ᒉ, cornerArc2.ܐ, cornerArc2._0711, cornerArc2.ᒊ, cornerArc2.ᒋ);
					val.AddLine(displayRectangle.X + num, displayRectangle.Y, displayRectangle.Right - num, displayRectangle.Y);
					cornerArc2 = GetCornerArc(displayRectangle, num, eCornerArc.TopRight);
					val.AddArc(cornerArc2.ᒈ, cornerArc2.ᒉ, cornerArc2.ܐ, cornerArc2._0711, cornerArc2.ᒊ, cornerArc2.ᒋ);
					val.AddLine(displayRectangle.Right, displayRectangle.Y + num, displayRectangle.Right, displayRectangle.Bottom - (bLast ? num : 0));
				}
			}
			else
			{
				val.AddLine(displayRectangle.X, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y);
			}
			if (bLast)
			{
				if (tab.Parent.TabAlignment == eTabStripAlignment.Top || tab.Parent.TabAlignment == eTabStripAlignment.Bottom)
				{
					if (!bFirst)
					{
						val.AddLine(displayRectangle.X, displayRectangle.Y, displayRectangle.Right - num, displayRectangle.Y);
					}
					ᒇ cornerArc3 = GetCornerArc(displayRectangle, num, eCornerArc.TopRight);
					val.AddArc(cornerArc3.ᒈ, cornerArc3.ᒉ, cornerArc3.ܐ, cornerArc3._0711, cornerArc3.ᒊ, cornerArc3.ᒋ);
					val.AddLine(displayRectangle.Right, displayRectangle.Y + num, displayRectangle.Right, displayRectangle.Bottom - num);
					cornerArc3 = GetCornerArc(displayRectangle, num, eCornerArc.BottomRight);
					val.AddArc(cornerArc3.ᒈ, cornerArc3.ᒉ, cornerArc3.ܐ, cornerArc3._0711, cornerArc3.ᒊ, cornerArc3.ᒋ);
					if (!bFirst)
					{
						val.AddLine(displayRectangle.Right - num, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom);
					}
				}
				else
				{
					if (!bFirst)
					{
						val.AddLine(displayRectangle.Right, displayRectangle.Y, displayRectangle.Right, displayRectangle.Bottom - num);
					}
					ᒇ cornerArc4 = GetCornerArc(displayRectangle, num, eCornerArc.BottomRight);
					val.AddArc(cornerArc4.ᒈ, cornerArc4.ᒉ, cornerArc4.ܐ, cornerArc4._0711, cornerArc4.ᒊ, cornerArc4.ᒋ);
					val.AddLine(displayRectangle.Right - num, displayRectangle.Bottom, displayRectangle.X + num, displayRectangle.Bottom);
					cornerArc4 = GetCornerArc(displayRectangle, num, eCornerArc.BottomLeft);
					val.AddArc(cornerArc4.ᒈ, cornerArc4.ᒉ, cornerArc4.ܐ, cornerArc4._0711, cornerArc4.ᒊ, cornerArc4.ᒋ);
					if (!bFirst)
					{
						val.AddLine(displayRectangle.X, displayRectangle.Bottom - num, displayRectangle.X, displayRectangle.Y);
					}
				}
			}
			else
			{
				val.AddLine(displayRectangle.Right, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom);
			}
			val.CloseAllFigures();
		}
		return val;
	}
}
internal class _2851 : _2848
{
	private int ன = 1;

	public override void Paint(Graphics g, TabStrip tabStrip)
	{
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		base.Paint(g, tabStrip);
		TabColorScheme colorScheme = tabStrip.ColorScheme;
		Rectangle displayRectangle = ((Control)tabStrip).DisplayRectangle;
		if (colorScheme.TabBackgroundImage != null)
		{
			BarFunctions._0982(g, displayRectangle, colorScheme.TabBackgroundImage, eBackgroundImagePosition.Tile, 255);
		}
		if (colorScheme.TabBackground != Color.Transparent && !colorScheme.TabBackground.IsEmpty)
		{
			if (colorScheme.TabBackground2.IsEmpty)
			{
				if (!colorScheme.TabBackground.IsEmpty)
				{
					SolidBrush val = new SolidBrush(colorScheme.TabBackground);
					try
					{
						g.FillRectangle((Brush)(object)val, displayRectangle);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			else
			{
				SolidBrush val2 = new SolidBrush(Color.White);
				try
				{
					g.FillRectangle((Brush)(object)val2, displayRectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				LinearGradientBrush val3 = CreateTabGradientBrush(displayRectangle, colorScheme.TabBackground, colorScheme.TabBackground2, colorScheme.TabBackgroundGradientAngle);
				try
				{
					g.FillRectangle((Brush)(object)val3, displayRectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		Rectangle selectedTabRect = Rectangle.Empty;
		if (tabStrip.SelectedTab != null)
		{
			selectedTabRect = tabStrip.SelectedTab.DisplayRectangle;
		}
		Region val4 = new Region(((Control)tabStrip).DisplayRectangle);
		try
		{
			DrawBackground(tabStrip, displayRectangle, g, colorScheme, val4, tabStrip.TabAlignment, selectedTabRect);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		Rectangle systemBoxRectangle = tabStrip.GetSystemBoxRectangle();
		for (int num = tabStrip.Tabs.Count - 1; num >= 0; num--)
		{
			TabItem tabItem = tabStrip.Tabs[num];
			if (tabItem.Visible && tabItem != tabStrip.SelectedTab)
			{
				if (!tabItem.DisplayRectangle.IntersectsWith(systemBoxRectangle))
				{
					if (tabItem.DisplayRectangle.IntersectsWith(displayRectangle))
					{
						PaintTab(g, tabItem, first: false, last: false);
					}
				}
				else
				{
					Region clip = g.Clip;
					g.SetClip(systemBoxRectangle, (CombineMode)4);
					PaintTab(g, tabItem, first: false, last: false);
					g.Clip = clip;
				}
			}
		}
		if (tabStrip.SelectedTab != null && tabStrip.Tabs.Contains(tabStrip.SelectedTab))
		{
			if (!tabStrip.SelectedTab.DisplayRectangle.IntersectsWith(systemBoxRectangle))
			{
				PaintTab(g, tabStrip.SelectedTab, first: false, last: false);
			}
			else
			{
				Region clip2 = g.Clip;
				g.SetClip(systemBoxRectangle, (CombineMode)4);
				PaintTab(g, tabStrip.SelectedTab, first: false, last: false);
				g.Clip = clip2;
			}
		}
		g.ResetClip();
		tabStrip.ᔬ(g);
	}

	protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
	{
		return ἕ(tab.DisplayRectangle, tab.TabAlignment, _2852: true);
	}

	private GraphicsPath ἕ(Rectangle ᔸ, eTabStripAlignment _0736, bool _2852)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		switch (_0736)
		{
		case eTabStripAlignment.Top:
			if (_2852)
			{
				ᔸ.Height += 2;
			}
			val.AddLine(ᔸ.X, ᔸ.Bottom, ᔸ.X, ᔸ.Y + ன);
			val.AddLine(ᔸ.X, ᔸ.Y + ன, ᔸ.X + ன, ᔸ.Y);
			val.AddLine(ᔸ.X + ன, ᔸ.Y, ᔸ.Right - ன, ᔸ.Y);
			val.AddLine(ᔸ.Right - ன, ᔸ.Y, ᔸ.Right, ᔸ.Y + ன);
			val.AddLine(ᔸ.Right, ᔸ.Y + ன, ᔸ.Right, ᔸ.Bottom);
			break;
		case eTabStripAlignment.Bottom:
			if (!_2852)
			{
				ᔸ.Y++;
			}
			ᔸ.Height--;
			val.AddLine(ᔸ.X, ᔸ.Y, ᔸ.X, ᔸ.Bottom - ன);
			val.AddLine(ᔸ.X, ᔸ.Bottom - ன, ᔸ.X + ன, ᔸ.Bottom);
			val.AddLine(ᔸ.X + ன, ᔸ.Bottom, ᔸ.Right - ன, ᔸ.Bottom);
			val.AddLine(ᔸ.Right - ன, ᔸ.Bottom, ᔸ.Right, ᔸ.Bottom - ன);
			val.AddLine(ᔸ.Right, ᔸ.Bottom - ன, ᔸ.Right, ᔸ.Y);
			break;
		case eTabStripAlignment.Left:
			ᔸ.X--;
			ᔸ.Height--;
			if (_2852)
			{
				ᔸ.Width++;
			}
			val.AddLine(ᔸ.Right, ᔸ.Y, ᔸ.X + ன, ᔸ.Y);
			val.AddLine(ᔸ.X + ன, ᔸ.Y, ᔸ.X, ᔸ.Y + ன);
			val.AddLine(ᔸ.X, ᔸ.Y + ன, ᔸ.X, ᔸ.Bottom - ன);
			val.AddLine(ᔸ.X, ᔸ.Bottom - ன, ᔸ.X + ன, ᔸ.Bottom);
			val.AddLine(ᔸ.X + ன, ᔸ.Bottom, ᔸ.Right, ᔸ.Bottom);
			break;
		case eTabStripAlignment.Right:
			if (!_2852)
			{
				ᔸ.X++;
			}
			val.AddLine(ᔸ.X, ᔸ.Y, ᔸ.Right - ன, ᔸ.Y);
			val.AddLine(ᔸ.Right - ன, ᔸ.Y, ᔸ.Right, ᔸ.Y + ன);
			val.AddLine(ᔸ.Right, ᔸ.Y + ன, ᔸ.Right, ᔸ.Bottom - ன);
			val.AddLine(ᔸ.Right, ᔸ.Bottom - ன, ᔸ.Right - ன, ᔸ.Bottom);
			val.AddLine(ᔸ.Right - ன, ᔸ.Bottom, ᔸ.X, ᔸ.Bottom);
			break;
		}
		if (_2852)
		{
			val.CloseAllFigures();
		}
		return val;
	}

	protected override void DrawTabBorder(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		Rectangle displayRectangle = tab.DisplayRectangle;
		eTabStripAlignment tabAlignment = tab.TabAlignment;
		if (!colors.BorderColor.IsEmpty)
		{
			GraphicsPath val = ἕ(displayRectangle, tabAlignment, _2852: false);
			try
			{
				Pen val2 = new Pen(colors.BorderColor, (float)Dpi.Width1);
				try
				{
					g.DrawPath(val2, val);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		switch (tabAlignment)
		{
		case eTabStripAlignment.Top:
			displayRectangle.Offset(1, 1);
			displayRectangle.Width -= 2;
			displayRectangle.Height--;
			break;
		case eTabStripAlignment.Bottom:
			displayRectangle.Offset(1, 0);
			displayRectangle.Height--;
			displayRectangle.Width -= 2;
			break;
		case eTabStripAlignment.Left:
			displayRectangle.Offset(1, 1);
			displayRectangle.Height -= 2;
			displayRectangle.Width -= 2;
			break;
		case eTabStripAlignment.Right:
			displayRectangle.Offset(1, 1);
			displayRectangle.Height -= 2;
			displayRectangle.Width -= 2;
			break;
		}
		if (colors.LightBorderColor.IsEmpty)
		{
			return;
		}
		GraphicsPath val3 = ἕ(displayRectangle, tabAlignment, _2852: false);
		try
		{
			Pen val4 = new Pen(colors.LightBorderColor, (float)Dpi.Width1);
			try
			{
				g.DrawPath(val4, val3);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	protected override void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		base.DrawBackground(tabStrip, tabStripRect, g, colors, tabsRegion, tabAlignment, selectedTabRect);
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		if (!colors.TabItemBorder.IsEmpty)
		{
			Pen val = new Pen(colors.TabItemBorder, (float)Dpi.Width1);
			try
			{
				switch (tabAlignment)
				{
				case eTabStripAlignment.Top:
				{
					if (selectedTabRect.X > tabStripRect.Right)
					{
						selectedTabRect = Rectangle.Empty;
					}
					Rectangle bounds3 = new Rectangle(tabStripRect.X, tabStripRect.Bottom - 1, tabStripRect.Width, 2);
					DisplayHelp.FillRectangle(g, bounds3, colors.TabItemBackground, Color.Empty);
					g.DrawLine(val, bounds3.X, bounds3.Y, selectedTabRect.X - 1, bounds3.Y);
					g.DrawLine(val, selectedTabRect.Right, bounds3.Y, bounds3.Right, bounds3.Y);
					g.DrawLine(val, bounds3.X, bounds3.Y, bounds3.X, bounds3.Bottom);
					g.DrawLine(val, bounds3.Right - 1, bounds3.Y, bounds3.Right - 1, bounds3.Bottom);
					break;
				}
				case eTabStripAlignment.Bottom:
				{
					if (selectedTabRect.X > tabStripRect.Right)
					{
						selectedTabRect = Rectangle.Empty;
					}
					Rectangle bounds2 = new Rectangle(tabStripRect.X, tabStripRect.Y, tabStripRect.Width, 1);
					DisplayHelp.FillRectangle(g, bounds2, colors.TabItemBackground, Color.Empty);
					g.DrawLine(val, bounds2.X, bounds2.Bottom - 1, selectedTabRect.X - 1, bounds2.Bottom - 1);
					g.DrawLine(val, selectedTabRect.Right, bounds2.Bottom - 1, bounds2.Right, bounds2.Bottom - 1);
					g.DrawLine(val, bounds2.X, bounds2.Y, bounds2.X, bounds2.Bottom - 1);
					g.DrawLine(val, bounds2.Right - 1, bounds2.Y, bounds2.Right - 1, bounds2.Bottom - 1);
					break;
				}
				case eTabStripAlignment.Left:
				{
					if (selectedTabRect.Y > tabStripRect.Bottom)
					{
						selectedTabRect = Rectangle.Empty;
					}
					Rectangle bounds4 = new Rectangle(tabStripRect.Right - 1, tabStripRect.Y, 1, tabStripRect.Height);
					DisplayHelp.FillRectangle(g, bounds4, colors.TabItemBackground, Color.Empty);
					g.DrawLine(val, bounds4.X, bounds4.Y, bounds4.X, selectedTabRect.Y - 1);
					g.DrawLine(val, bounds4.X, selectedTabRect.Bottom, bounds4.X, bounds4.Bottom);
					g.DrawLine(val, bounds4.X, bounds4.Y, bounds4.Right, bounds4.Y);
					g.DrawLine(val, bounds4.X, bounds4.Bottom - 1, bounds4.Right, bounds4.Bottom - 1);
					break;
				}
				case eTabStripAlignment.Right:
				{
					if (selectedTabRect.Y > tabStripRect.Bottom)
					{
						selectedTabRect = Rectangle.Empty;
					}
					Rectangle bounds = new Rectangle(tabStripRect.X, tabStripRect.Y, 1, tabStripRect.Height);
					DisplayHelp.FillRectangle(g, bounds, colors.TabItemBackground, Color.Empty);
					g.DrawLine(val, bounds.Right - 1, bounds.Y, bounds.Right - 1, selectedTabRect.Y - 1);
					g.DrawLine(val, bounds.Right - 1, selectedTabRect.Bottom, bounds.Right - 1, bounds.Bottom);
					g.DrawLine(val, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
					g.DrawLine(val, bounds.X, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
					break;
				}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		g.SmoothingMode = smoothingMode;
	}
}
internal class _2853 : _2848
{
	private int _2854 = 8;

	public override void Paint(Graphics g, TabStrip tabStrip)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		base.Paint(g, tabStrip);
		TabColorScheme colorScheme = tabStrip.ColorScheme;
		Rectangle displayRectangle = ((Control)tabStrip).DisplayRectangle;
		if (((Control)tabStrip).BackColor != Color.Transparent || colorScheme.TabBackground != Color.Transparent)
		{
			if (colorScheme.TabPanelBackground2.IsEmpty)
			{
				if (!colorScheme.TabPanelBackground.IsEmpty)
				{
					SolidBrush val = new SolidBrush(colorScheme.TabPanelBackground);
					try
					{
						g.FillRectangle((Brush)(object)val, displayRectangle);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			else
			{
				SolidBrush val2 = new SolidBrush(Color.White);
				try
				{
					g.FillRectangle((Brush)(object)val2, displayRectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				LinearGradientBrush val3 = CreateTabGradientBrush(displayRectangle, colorScheme.TabPanelBackground, colorScheme.TabPanelBackground2, colorScheme.TabPanelBackgroundGradientAngle);
				try
				{
					g.FillRectangle((Brush)(object)val3, displayRectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		Rectangle selectedTabRect = Rectangle.Empty;
		if (tabStrip.SelectedTab != null)
		{
			selectedTabRect = tabStrip.SelectedTab.DisplayRectangle;
		}
		DrawBackground(tabStrip, displayRectangle, g, colorScheme, new Region(((Control)tabStrip).DisplayRectangle), tabStrip.TabAlignment, selectedTabRect);
		tabStrip.ᔲ(g);
		for (int num = tabStrip.Tabs.Count - 1; num >= 0; num--)
		{
			TabItem tabItem = tabStrip.Tabs[num];
			if (tabItem.Visible && tabItem != tabStrip.SelectedTab && tabItem.DisplayRectangle.IntersectsWith(displayRectangle))
			{
				PaintTab(g, tabItem, first: false, last: false);
			}
		}
		if (tabStrip.SelectedTab != null && tabStrip.Tabs.Contains(tabStrip.SelectedTab))
		{
			PaintTab(g, tabStrip.SelectedTab, first: false, last: false);
		}
		g.ResetClip();
		tabStrip.ᔬ(g);
	}

	protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		Rectangle ल = tab.DisplayRectangle;
		if (tab.TabAlignment == eTabStripAlignment.Right)
		{
			ल = new Rectangle(ल.X, ल.Y, ल.Height, ल.Width);
		}
		else if (tab.TabAlignment == eTabStripAlignment.Left)
		{
			ल = new Rectangle(ल.X, ल.Y, ल.Height, ल.Width);
		}
		ल.Offset(0, 1);
		GraphicsPath val = new GraphicsPath();
		val.AddPath(_2855(ल), true);
		val.AddLine(ल.X + 6, ल.Y, ल.Right - 5, ल.Y);
		val.AddPath(_2856(ल), true);
		val.AddLine(ल.Right + _2854, ल.Bottom, ल.X - _2854, ल.Bottom);
		val.CloseAllFigures();
		if (tab.TabAlignment == eTabStripAlignment.Bottom)
		{
			Matrix val2 = new Matrix();
			val2.RotateAt(180f, new PointF(ल.X + ल.Width / 2, ल.Y + ल.Height / 2));
			val.Transform(val2);
		}
		else if (tab.TabAlignment == eTabStripAlignment.Left)
		{
			Matrix val3 = new Matrix();
			val3.RotateAt(-90f, new PointF(ल.X, ल.Bottom));
			val3.Translate((float)ल.Height, (float)(ल.Width - ल.Height), (MatrixOrder)1);
			val.Transform(val3);
		}
		else if (tab.TabAlignment == eTabStripAlignment.Right)
		{
			Matrix val4 = new Matrix();
			val4.RotateAt(90f, new PointF(ल.Right, ल.Bottom));
			val4.Translate((float)(-ल.Width), (float)(ल.Width - (ल.Height - 1)), (MatrixOrder)1);
			val.Transform(val4);
		}
		return val;
	}

	private GraphicsPath _2855(Rectangle ल)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddLine(ल.X - _2854, ल.Bottom, ल.X, ल.Y + 5);
		val.AddCurve(new Point[3]
		{
			new Point(ल.X, ल.Y + 5),
			new Point(ल.X + 2, ल.Y + 2),
			new Point(ल.X + 5, ल.Y)
		}, 0.9f);
		return val;
	}

	private GraphicsPath _2856(Rectangle ल)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddCurve(new Point[3]
		{
			new Point(ल.Right - 5, ल.Y),
			new Point(ल.Right - 2, ल.Y + 2),
			new Point(ल.Right, ल.Y + 5)
		}, 0.9f);
		val.AddLine(ल.Right, ल.Y + 5, ल.Right + _2854, ल.Bottom);
		return val;
	}

	protected override void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		base.DrawBackground(tabStrip, tabStripRect, g, colors, tabsRegion, tabAlignment, selectedTabRect);
		if (colors.TabItemBorder.IsEmpty)
		{
			return;
		}
		Pen val = new Pen(colors.TabItemBorder, 1f);
		try
		{
			switch (tabAlignment)
			{
			case eTabStripAlignment.Bottom:
				g.DrawLine(val, tabStripRect.X, tabStripRect.Y + 1, tabStripRect.Right, tabStripRect.Y + 1);
				break;
			case eTabStripAlignment.Left:
				g.DrawLine(val, tabStripRect.Right - 1, tabStripRect.Y, tabStripRect.Right - 1, tabStripRect.Bottom);
				break;
			case eTabStripAlignment.Right:
				g.DrawLine(val, tabStripRect.X, tabStripRect.Y, tabStripRect.X, tabStripRect.Bottom);
				break;
			case eTabStripAlignment.Top:
				g.DrawLine(val, tabStripRect.X, tabStripRect.Bottom - 1, tabStripRect.Right, tabStripRect.Bottom - 1);
				break;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
internal class _2857 : _2848
{
	private int Ừ = 3;

	public override void Paint(Graphics g, TabStrip tabStrip)
	{
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		base.Paint(g, tabStrip);
		TabColorScheme colorScheme = tabStrip.ColorScheme;
		Rectangle displayRectangle = ((Control)tabStrip).DisplayRectangle;
		if (colorScheme.TabBackgroundImage != null)
		{
			BarFunctions._0982(g, displayRectangle, colorScheme.TabBackgroundImage, eBackgroundImagePosition.Tile, 255);
		}
		if (colorScheme.TabBackground != Color.Transparent && !colorScheme.TabBackground.IsEmpty)
		{
			if (colorScheme.TabBackground2.IsEmpty)
			{
				if (!colorScheme.TabBackground.IsEmpty)
				{
					SolidBrush val = new SolidBrush(colorScheme.TabBackground);
					try
					{
						g.FillRectangle((Brush)(object)val, displayRectangle);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			else
			{
				SolidBrush val2 = new SolidBrush(Color.White);
				try
				{
					g.FillRectangle((Brush)(object)val2, displayRectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				LinearGradientBrush val3 = CreateTabGradientBrush(displayRectangle, colorScheme.TabBackground, colorScheme.TabBackground2, colorScheme.TabBackgroundGradientAngle);
				try
				{
					g.FillRectangle((Brush)(object)val3, displayRectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		Rectangle selectedTabRect = Rectangle.Empty;
		if (tabStrip.SelectedTab != null)
		{
			selectedTabRect = tabStrip.SelectedTab.DisplayRectangle;
		}
		Region val4 = new Region(((Control)tabStrip).DisplayRectangle);
		try
		{
			DrawBackground(tabStrip, displayRectangle, g, colorScheme, val4, tabStrip.TabAlignment, selectedTabRect);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		Rectangle systemBoxRectangle = tabStrip.GetSystemBoxRectangle();
		systemBoxRectangle.Inflate(-2, -2);
		for (int num = tabStrip.Tabs.Count - 1; num >= 0; num--)
		{
			TabItem tabItem = tabStrip.Tabs[num];
			if (tabItem.Visible && tabItem != tabStrip.SelectedTab && !tabItem.DisplayRectangle.IntersectsWith(systemBoxRectangle) && tabItem.DisplayRectangle.IntersectsWith(displayRectangle))
			{
				PaintTab(g, tabItem, first: false, last: false);
			}
		}
		if (tabStrip.SelectedTab != null && tabStrip.Tabs.Contains(tabStrip.SelectedTab) && !_2858(tabStrip.SelectedTab).IntersectsWith(systemBoxRectangle))
		{
			PaintTab(g, tabStrip.SelectedTab, first: false, last: false);
		}
		g.ResetClip();
		tabStrip.ᔬ(g);
	}

	private Rectangle _2858(TabItem _06EC)
	{
		Rectangle displayRectangle = _06EC.DisplayRectangle;
		if (_06EC.TabAlignment == eTabStripAlignment.Top || _06EC.TabAlignment == eTabStripAlignment.Bottom)
		{
			displayRectangle.Width -= displayRectangle.Height - 6;
		}
		else
		{
			displayRectangle.Height -= displayRectangle.Width - 6;
		}
		return displayRectangle;
	}

	protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
	{
		return ᔹ(tab.DisplayRectangle, tab.TabAlignment, _2852: true);
	}

	private GraphicsPath ᔹ(Rectangle _2859, eTabStripAlignment _0736, bool _2852)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		Rectangle rectangle = _2859;
		switch (_0736)
		{
		case eTabStripAlignment.Right:
			rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Height, rectangle.Width);
			break;
		case eTabStripAlignment.Left:
			rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Height, rectangle.Width);
			break;
		}
		if (_0736 == eTabStripAlignment.Bottom || _0736 == eTabStripAlignment.Top)
		{
			rectangle.Offset(0, 1);
		}
		else
		{
			rectangle.Offset(1, 0);
		}
		GraphicsPath val = new GraphicsPath();
		if (_0736 == eTabStripAlignment.Bottom || _0736 == eTabStripAlignment.Left)
		{
			val.AddLine(rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom - Ừ);
			val.AddLine(rectangle.X, rectangle.Bottom - Ừ, rectangle.X + Ừ, rectangle.Bottom);
			val.AddLine(rectangle.X + Ừ, rectangle.Bottom, rectangle.Right - rectangle.Height / 2, rectangle.Bottom);
			Point[] array = new Point[4];
			array[0].X = rectangle.Right - rectangle.Height / 2;
			array[0].Y = rectangle.Bottom;
			array[1].X = array[0].X + 5;
			array[1].Y = array[0].Y - 3;
			array[2].X = array[1].X + rectangle.Height / 2;
			array[2].Y = rectangle.Y + 3;
			array[3].X = array[2].X + 4;
			array[3].Y = rectangle.Y;
			val.AddCurve(array, 0, 3, 0.1f);
			if (_2852)
			{
				val.AddLine(rectangle.Right, rectangle.Y, rectangle.X, rectangle.Y);
			}
		}
		else
		{
			val.AddLine(rectangle.X, rectangle.Bottom, rectangle.X, rectangle.Y + Ừ);
			val.AddLine(rectangle.X, rectangle.Y + Ừ, rectangle.X + Ừ, rectangle.Y);
			val.AddLine(rectangle.X + Ừ, rectangle.Y, rectangle.Right - rectangle.Height / 2, rectangle.Y);
			Point[] array2 = new Point[4];
			array2[0].X = rectangle.Right - rectangle.Height / 2;
			array2[0].Y = rectangle.Y;
			array2[1].X = array2[0].X + 5;
			array2[1].Y = array2[0].Y + 3;
			array2[2].X = array2[1].X + rectangle.Height / 2;
			array2[2].Y = rectangle.Bottom - 3;
			array2[3].X = array2[2].X + 4;
			array2[3].Y = rectangle.Bottom;
			val.AddCurve(array2, 0, 3, 0.1f);
			if (_2852)
			{
				val.AddLine(rectangle.Right, rectangle.Bottom, rectangle.X, rectangle.Bottom);
			}
		}
		switch (_0736)
		{
		case eTabStripAlignment.Left:
		{
			Matrix val3 = new Matrix();
			val3.RotateAt(90f, new PointF(rectangle.Right, rectangle.Bottom));
			val3.Translate((float)(-rectangle.Width - 2), (float)(rectangle.Width - rectangle.Height), (MatrixOrder)1);
			val.Transform(val3);
			break;
		}
		case eTabStripAlignment.Right:
		{
			Matrix val2 = new Matrix();
			val2.RotateAt(90f, new PointF(rectangle.Right, rectangle.Bottom));
			val2.Translate((float)(-rectangle.Width), (float)(rectangle.Width - rectangle.Height), (MatrixOrder)1);
			val.Transform(val2);
			break;
		}
		}
		if (_2852)
		{
			val.CloseAllFigures();
		}
		return val;
	}

	protected override void DrawTabBorder(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		Rectangle displayRectangle = tab.DisplayRectangle;
		eTabStripAlignment tabAlignment = tab.TabAlignment;
		Region clip = g.Clip;
		Rectangle clip2 = Rectangle.Ceiling(path.GetBounds());
		if (tab.TabAlignment == eTabStripAlignment.Right || tab.TabAlignment == eTabStripAlignment.Bottom)
		{
			clip2.Inflate(1, 1);
		}
		else if (tab.TabAlignment == eTabStripAlignment.Left && !tab.IsSelected)
		{
			clip2.Width--;
		}
		g.SetClip(clip2);
		if (!colors.BorderColor.IsEmpty)
		{
			GraphicsPath val = ᔹ(displayRectangle, tabAlignment, _2852: false);
			try
			{
				Pen val2 = new Pen(colors.BorderColor);
				try
				{
					g.DrawPath(val2, val);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		switch (tabAlignment)
		{
		case eTabStripAlignment.Top:
			displayRectangle.Offset(1, 1);
			displayRectangle.Width--;
			break;
		case eTabStripAlignment.Bottom:
			displayRectangle.Offset(1, -1);
			displayRectangle.Width--;
			break;
		case eTabStripAlignment.Left:
			displayRectangle.Offset(1, 1);
			displayRectangle.Height--;
			break;
		case eTabStripAlignment.Right:
			displayRectangle.Offset(-1, 1);
			break;
		}
		if (!colors.LightBorderColor.IsEmpty)
		{
			GraphicsPath val3 = ᔹ(displayRectangle, tabAlignment, _2852: false);
			try
			{
				clip2 = Rectangle.Ceiling(path.GetBounds());
				if (tabAlignment == eTabStripAlignment.Left)
				{
					clip2.Width--;
				}
				g.SetClip(clip2);
				Pen val4 = new Pen(colors.LightBorderColor);
				try
				{
					g.DrawPath(val4, val3);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		g.Clip = clip;
	}

	protected override void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		base.DrawBackground(tabStrip, tabStripRect, g, colors, tabsRegion, tabAlignment, selectedTabRect);
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Pen val = new Pen(colors.TabItemBorder, 1f);
		try
		{
			int num = 9;
			if (selectedTabRect.IsEmpty)
			{
				num = 0;
			}
			Color color = colors.TabItemSelectedBackground2;
			if (tabStrip.SelectedTab != null)
			{
				TabColors tabColors = tabStrip.ᕝ(tabStrip.SelectedTab);
				color = tabColors.BackColor2;
			}
			switch (tabAlignment)
			{
			case eTabStripAlignment.Top:
			{
				if (selectedTabRect.X > tabStripRect.Right || selectedTabRect.Bottom < tabStripRect.Bottom - 4)
				{
					selectedTabRect = Rectangle.Empty;
					num = 0;
				}
				Rectangle bounds3 = new Rectangle(tabStripRect.X, tabStripRect.Bottom - 1, tabStripRect.Width, 1);
				DisplayHelp.FillRectangle(g, bounds3, color, Color.Empty);
				g.DrawLine(val, bounds3.X, bounds3.Y, selectedTabRect.X, bounds3.Y);
				g.DrawLine(val, selectedTabRect.Right + num, bounds3.Y, bounds3.Right, bounds3.Y);
				g.DrawLine(val, bounds3.X, bounds3.Y, bounds3.X, bounds3.Bottom);
				g.DrawLine(val, bounds3.Right - 1, bounds3.Y, bounds3.Right - 1, bounds3.Bottom);
				break;
			}
			case eTabStripAlignment.Bottom:
			{
				if (selectedTabRect.X > tabStripRect.Right || selectedTabRect.Y > 4)
				{
					selectedTabRect = Rectangle.Empty;
					num = 0;
				}
				Rectangle bounds2 = new Rectangle(tabStripRect.X, tabStripRect.Y, tabStripRect.Width, 1);
				DisplayHelp.FillRectangle(g, bounds2, color, Color.Empty);
				g.DrawLine(val, bounds2.X, bounds2.Bottom - 1, selectedTabRect.X, bounds2.Bottom - 1);
				g.DrawLine(val, selectedTabRect.Right + num, bounds2.Bottom - 1, bounds2.Right, bounds2.Bottom - 1);
				g.DrawLine(val, bounds2.X, bounds2.Y, bounds2.X, bounds2.Bottom - 1);
				g.DrawLine(val, bounds2.Right - 1, bounds2.Y, bounds2.Right - 1, bounds2.Bottom - 1);
				break;
			}
			case eTabStripAlignment.Left:
			{
				if (selectedTabRect.Y > tabStripRect.Bottom || selectedTabRect.Right < tabStripRect.Right - 4)
				{
					selectedTabRect = Rectangle.Empty;
					num = 0;
				}
				Rectangle bounds4 = new Rectangle(tabStripRect.Right - 1, tabStripRect.Y, 1, tabStripRect.Height);
				DisplayHelp.FillRectangle(g, bounds4, color, Color.Empty);
				g.DrawLine(val, bounds4.X, bounds4.Y, bounds4.X, selectedTabRect.Y);
				g.DrawLine(val, bounds4.X, selectedTabRect.Bottom + num, bounds4.X, bounds4.Bottom);
				g.DrawLine(val, bounds4.X, bounds4.Y, bounds4.Right, bounds4.Y);
				g.DrawLine(val, bounds4.X, bounds4.Bottom - 1, bounds4.Right, bounds4.Bottom - 1);
				break;
			}
			case eTabStripAlignment.Right:
			{
				if (selectedTabRect.Y > tabStripRect.Bottom || selectedTabRect.X > 4)
				{
					selectedTabRect = Rectangle.Empty;
					num = 0;
				}
				Rectangle bounds = new Rectangle(tabStripRect.X, tabStripRect.Y, 1, tabStripRect.Height);
				DisplayHelp.FillRectangle(g, bounds, color, Color.Empty);
				g.DrawLine(val, bounds.Right - 1, bounds.Y, bounds.Right - 1, selectedTabRect.Y);
				g.DrawLine(val, bounds.Right - 1, selectedTabRect.Bottom + num, bounds.Right - 1, bounds.Bottom);
				g.DrawLine(val, bounds.X, bounds.Y, bounds.Right, bounds.Y);
				g.DrawLine(val, bounds.X, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
				break;
			}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		g.SmoothingMode = smoothingMode;
	}

	protected override void AdjustTextRectangle(ref Rectangle rText, eTabStripAlignment tabAlignment)
	{
		if (tabAlignment == eTabStripAlignment.Top || tabAlignment == eTabStripAlignment.Bottom)
		{
			rText.Width -= 3;
		}
		base.AdjustTextRectangle(ref rText, tabAlignment);
	}

	protected override void AdjustCloseButtonRectangle(ref Rectangle close, bool closeOnLeftSide, bool vertical)
	{
		if (!closeOnLeftSide)
		{
			if (!vertical)
			{
				close.X -= base._284A + close.Height / 6;
			}
			else
			{
				close.Y -= base._284A + close.Height / 6;
			}
		}
	}
}
internal class _28F9
{
	public ISimpleElement _28FA;

	public Graphics ܓ;

	public Font _0745;

	public int _0749;

	public int _074A;

	public ElementStyle _074B;

	public bool _074C = true;

	public bool ݍ;
}
internal class _28FB
{
	private enum _28FF
	{
		_0749,
		ስ,
		ሪ
	}

	private enum _2900
	{
		_074A,
		ሷ,
		ራ
	}

	internal static void _28FC(_28F9 _108F)
	{
		Size textSize = Size.Empty;
		Font val = _108F._0745;
		int num = 0;
		if (_108F._074B.Font != null)
		{
			val = _108F._074B.Font;
		}
		string text = _108F._28FA.Text;
		if (_108F._28FA.FixedWidth == 0)
		{
			if (_108F._28FA.TextVisible && !string.IsNullOrEmpty(text))
			{
				textSize = TextDrawing.MeasureString(_108F.ܓ, text, val);
				if (_108F._074B != null && !_108F._074B.TextShadowColor.IsEmpty && !_108F._074B.TextShadowOffset.IsEmpty)
				{
					textSize.Height += _108F._074B.TextShadowOffset.Y;
				}
				_108F._28FA.TextSize = textSize;
			}
		}
		else
		{
			int num2 = _108F._28FA.FixedWidth - ᝈ.ᝍ(_108F._074B);
			if (_108F._28FA.ImageVisible)
			{
				num2 -= _108F._28FA.ImageLayoutSize.Width;
			}
			if (_108F._28FA.TextVisible)
			{
				int num3 = val.Height;
				if (_108F._074B != null && !_108F._074B.TextShadowColor.IsEmpty && !_108F._074B.TextShadowOffset.IsEmpty)
				{
					num3 += _108F._074B.TextShadowOffset.Y;
				}
				if (_108F._074B.WordWrap)
				{
					num3 = _108F._074B.MaximumHeight - _108F._074B.MarginTop - _108F._074B.MarginBottom - _108F._074B.PaddingTop - _108F._074B.PaddingBottom;
					if (num2 > 0)
					{
						textSize = ((num3 <= 0) ? TextDrawing.MeasureString(_108F.ܓ, text, val, num2, _108F._074B.ݽ) : TextDrawing.MeasureString(_108F.ܓ, text, val, new Size(num2, num3), _108F._074B.ݽ));
						_108F._28FA.TextSize = textSize;
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(text))
					{
						_108F._28FA.TextSize = TextDrawing.MeasureString(_108F.ܓ, text, val);
					}
					textSize = new Size(num2, num3);
				}
			}
		}
		if (_108F._28FA.TextVisible && !_108F._074B.TextShadowColor.IsEmpty)
		{
			textSize.Height += _108F._074B.TextShadowOffset.Y;
		}
		num = textSize.Height;
		if (_108F.ݍ)
		{
			if (_108F._28FA.ImageVisible && _108F._28FA.ImageLayoutSize.Height > 0)
			{
				num += _108F._28FA.ImageLayoutSize.Height;
			}
		}
		else if (_108F._28FA.ImageVisible && _108F._28FA.ImageLayoutSize.Height > num)
		{
			num = _108F._28FA.ImageLayoutSize.Height;
		}
		Rectangle _0730 = new Rectangle(_108F._0749 + ᝈ.ᝐ(_108F._074B), _108F._074A + ᝈ._1753(_108F._074B), _108F._28FA.FixedWidth - ᝈ.ᝍ(_108F._074B), num);
		if (_0730.Width == 0)
		{
			if (_108F.ݍ)
			{
				if (_108F._28FA.TextVisible)
				{
					_0730.Width = textSize.Width;
				}
				if (_108F._28FA.ImageVisible && _108F._28FA.ImageLayoutSize.Width > _0730.Width)
				{
					_0730.Width = _108F._28FA.ImageLayoutSize.Width + _108F._28FA.ImageTextSpacing;
				}
			}
			else
			{
				if (_108F._28FA.TextVisible)
				{
					_0730.Width = textSize.Width;
				}
				if (_108F._28FA.ImageVisible && _108F._28FA.ImageLayoutSize.Width > 0)
				{
					_0730.Width += _108F._28FA.ImageLayoutSize.Width + _108F._28FA.ImageTextSpacing;
				}
			}
		}
		Rectangle bounds = new Rectangle(_108F._0749, _108F._074A, _108F._28FA.FixedWidth, _0730.Height + _108F._074B.MarginTop + _108F._074B.MarginBottom + _108F._074B.PaddingTop + _108F._074B.PaddingBottom);
		if (bounds.Width == 0)
		{
			bounds.Width = _0730.Width + ᝈ.ᝍ(_108F._074B);
		}
		_108F._28FA.Bounds = bounds;
		if (_108F._28FA.ImageVisible && !_108F._28FA.ImageLayoutSize.IsEmpty)
		{
			_2900 _2900 = _28FE(_108F._28FA.ImageAlignment);
			_28FF _28FF = _28FD(_108F._28FA.ImageAlignment, _108F._074C);
			if (_108F.ݍ)
			{
				_108F._28FA.ImageBounds = _0734(_108F._28FA.ImageLayoutSize, ref _0730, _28FF, _2900, _108F._28FA.ImageTextSpacing);
			}
			else
			{
				_108F._28FA.ImageBounds = ܮ(_108F._28FA.ImageLayoutSize, ref _0730, _28FF, _2900, _108F._28FA.ImageTextSpacing);
			}
		}
		else
		{
			_108F._28FA.ImageBounds = Rectangle.Empty;
		}
		if (!textSize.IsEmpty)
		{
			_108F._28FA.TextBounds = _0730;
		}
		else
		{
			_108F._28FA.TextBounds = Rectangle.Empty;
		}
	}

	private static Rectangle ܮ(Size ܯ, ref Rectangle _0730, _28FF _0731, _2900 _0732, int _0733)
	{
		Rectangle result = new Rectangle(Point.Empty, ܯ);
		if (_0731 == _28FF.ሪ)
		{
			result.X = _0730.Right - result.Width;
			_0730.Width -= result.Width + _0733;
		}
		else
		{
			result.X = _0730.X;
			_0730.X = result.Right + _0733;
			_0730.Width -= result.Width + _0733;
		}
		switch (_0732)
		{
		case _2900._074A:
			result.Y = _0730.Y;
			break;
		case _2900.ሷ:
			result.Y = _0730.Y + (_0730.Height - result.Height) / 2;
			break;
		case _2900.ራ:
			result.Y = _0730.Bottom - result.Height;
			break;
		}
		return result;
	}

	private static Rectangle _0734(Size ܯ, ref Rectangle _0730, _28FF _0731, _2900 _0732, int _0733)
	{
		Rectangle result = new Rectangle(Point.Empty, ܯ);
		switch (_0731)
		{
		case _28FF._0749:
			result.X = _0730.X;
			break;
		case _28FF.ሪ:
			result.X = _0730.Right - result.Width;
			break;
		case _28FF.ስ:
			result.X = _0730.X + (_0730.Width - result.Width) / 2;
			break;
		}
		if (_0732 == _2900.ራ)
		{
			result.Y = _0730.Bottom - result.Height;
			_0730.Height -= result.Height + _0733;
		}
		else
		{
			result.Y = _0730.Y;
			_0730.Y = result.Bottom + _0733;
			_0730.Height -= result.Height + _0733;
		}
		return result;
	}

	private static _28FF _28FD(eSimplePartAlignment _0736, bool _0737)
	{
		if (((_0736 == eSimplePartAlignment.NearBottom || _0736 == eSimplePartAlignment.NearCenter || _0736 == eSimplePartAlignment.NearTop) && _0737) || ((_0736 == eSimplePartAlignment.FarBottom || _0736 == eSimplePartAlignment.FarCenter || _0736 == eSimplePartAlignment.FarTop) && !_0737))
		{
			return _28FF._0749;
		}
		if (_0736 == eSimplePartAlignment.CenterBottom || _0736 == eSimplePartAlignment.CenterTop)
		{
			return _28FF.ስ;
		}
		return _28FF.ሪ;
	}

	private static _2900 _28FE(eSimplePartAlignment _0736)
	{
		_2900 result = _2900.ሷ;
		switch (_0736)
		{
		case eSimplePartAlignment.NearBottom:
		case eSimplePartAlignment.CenterBottom:
		case eSimplePartAlignment.FarBottom:
			result = _2900.ራ;
			break;
		case eSimplePartAlignment.NearTop:
		case eSimplePartAlignment.CenterTop:
		case eSimplePartAlignment.FarTop:
			result = _2900._074A;
			break;
		}
		return result;
	}
}
internal class _2901
{
	public static _23D7 _2902(ButtonItem _06EC)
	{
		_2904 _29042 = new _2904();
		_29042._0CCA = _06EC;
		return _29042;
	}

	public static _2903 _2902(ItemContainer _06EC)
	{
		_2903 _29032 = new _2903();
		_29032._0CCA = _06EC;
		return _29032;
	}
}
internal class _2903 : _23D7
{
	private bool ೠ;

	private eOrientation ᅡ;

	private bool ᄛ;

	public override void RecordSetting(BaseItem item)
	{
		if (!base._23D8)
		{
			ItemContainer itemContainer = item as ItemContainer;
			ೠ = itemContainer.MultiLine;
			ᅡ = itemContainer.LayoutOrientation;
			ᄛ = itemContainer.TitleVisible;
			base.RecordSetting(item);
		}
	}

	public override void RestoreSettings()
	{
		if (base._23D8)
		{
			ItemContainer itemContainer = _0CCA as ItemContainer;
			itemContainer.MultiLine = ೠ;
			itemContainer.LayoutOrientation = ᅡ;
			itemContainer.TitleVisible = ᄛ;
			base.RestoreSettings();
		}
	}
}
internal class _2904 : _23D7
{
	private eButtonStyle _0E70;

	private eImagePosition _0E71;

	private Size _2905 = Size.Empty;

	private string ө;

	public override void RecordSetting(BaseItem item)
	{
		if (!base._23D8)
		{
			ButtonItem buttonItem = item as ButtonItem;
			_0E70 = buttonItem.ButtonStyle;
			_0E71 = buttonItem.ImagePosition;
			_2905 = buttonItem.ImageFixedSize;
			if (buttonItem.TextMarkupBody != null && buttonItem.TextMarkupBody._0CBA)
			{
				ө = buttonItem.Text;
			}
			base.RecordSetting(item);
		}
	}

	public override void RestoreSettings()
	{
		if (base._23D8)
		{
			ButtonItem buttonItem = _0CCA as ButtonItem;
			bool globalItem = buttonItem.GlobalItem;
			buttonItem.GlobalItem = false;
			buttonItem.ButtonStyle = _0E70;
			buttonItem.ImagePosition = _0E71;
			buttonItem.ImageFixedSize = _2905;
			if (ө != null)
			{
				buttonItem.Text = ө;
			}
			buttonItem.GlobalItem = globalItem;
			base.RestoreSettings();
		}
	}
}
internal class _2906
{
	public ElementStyle ߕ;

	public Graphics ܓ;

	public ISimpleElement _28FA;

	public Font _0745;

	public Rectangle _175A = Rectangle.Empty;

	public bool _2907;

	public _2906(ElementStyle style, Graphics g, ISimpleElement elem, Font font, bool rightToLeft)
	{
		ߕ = style;
		ܓ = g;
		_28FA = elem;
		_0745 = font;
		_2907 = rightToLeft;
	}
}
internal class _2908
{
	public static void _139A(_2906 ឡ)
	{
		Rectangle bounds = ឡ._28FA.Bounds;
		Region clip = ឡ.ܓ.Clip;
		if (clip != null)
		{
			ឡ.ܓ.SetClip(bounds, (CombineMode)1);
		}
		else
		{
			ឡ.ܓ.SetClip(bounds, (CombineMode)0);
		}
		ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(ឡ.ߕ, ឡ.ܓ, bounds);
		ElementStyleDisplay.Paint(elementStyleDisplayInfo);
		ឡ.ܓ.ResetClip();
		if (ឡ._28FA.ImageVisible)
		{
			_25B3(ឡ);
		}
		if (ឡ._28FA.TextVisible)
		{
			elementStyleDisplayInfo.Bounds = (ឡ._175A.IsEmpty ? ឡ._28FA.TextBounds : ឡ._175A);
			eTextFormat eTextFormat2 = ឡ.ߕ.ݽ;
			if (ឡ._2907)
			{
				eTextFormat2 |= eTextFormat.RightToLeft;
			}
			ElementStyleDisplay.PaintText(elementStyleDisplayInfo, ឡ._28FA.Text, ឡ._0745, useDefaultFont: false, eTextFormat2);
		}
		if (clip != null)
		{
			ឡ.ܓ.Clip = clip;
		}
		else
		{
			ឡ.ܓ.ResetClip();
		}
	}

	private static void _25B3(_2906 ឡ)
	{
		if (!ឡ._28FA.ImageLayoutSize.IsEmpty && ឡ._28FA.Image != null)
		{
			Rectangle imageBounds = ឡ._28FA.ImageBounds;
			ឡ.ܓ.DrawImage(ឡ._28FA.Image, imageBounds.X + (imageBounds.Width - ឡ._28FA.Image.Width) / 2, imageBounds.Y + (imageBounds.Height - ឡ._28FA.Image.Height) / 2);
		}
	}
}
internal class _290D
{
	public static string _290E = "documents";

	public static string _290F = "dockcontainer";

	public static string _2910 = "barcontainer";

	public static string ᡥ = "orientation";

	public static string ܐ = "w";

	public static string _0711 = "h";

	public static string _2911 = "docksite";

	public static string _2912 = "dockingside";

	public static string _2913 = "size";

	public static string _2914 = "version";

	public static string _2915 = "originaldocksitesize";

	public static DocumentBaseContainer _2916(string _2917)
	{
		if (_2917 == _290F)
		{
			return new DocumentDockContainer();
		}
		if (_2917 == _2910)
		{
			return new DocumentBarContainer();
		}
		throw new InvalidOperationException("Document type not recognized: " + _2917);
	}
}
internal class _2918
{
	public static string _0FD9 = "bar";

	public static string _0D01 = "name";

	public static string ᖸ = "custom";
}
[ToolboxItem(false)]
internal class _2924 : Form
{
	private Container Պ;

	private _295D _2936 = _295D._26AF;

	private _295F _2937;

	private bool _2938;

	private GraphicsPath _2939;

	private GraphicsPath _293A;

	private GraphicsPath _293B;

	private GraphicsPath _293C;

	private GraphicsPath _293D;

	private bool _293E;

	private static Bitmap _293F;

	private static Bitmap _2940;

	private static Bitmap _2941;

	private static Bitmap _2942;

	private static Bitmap _2943;

	private static Bitmap _2944;

	private static Bitmap _2945;

	private static Bitmap _2946;

	private static Bitmap _2947;

	private static Bitmap _2948;

	private static Bitmap _2949;

	private static Bitmap _294A;

	private static Bitmap _294B;

	private static Bitmap _294C;

	private eDotNetBarStyle _ᎌ = eDotNetBarStyle.Office2003;

	public static Bitmap _2925
	{
		get
		{
			if (_293F == null)
			{
				_293F = BarFunctions.ই("SystemImages.DockHintTop.png");
			}
			return _293F;
		}
	}

	public static Bitmap _2926
	{
		get
		{
			if (_2945 == null)
			{
				_2945 = BarFunctions.ই("SystemImages.DockHintTop2010.png");
			}
			return _2945;
		}
	}

	public static Bitmap _2927
	{
		get
		{
			if (_2940 == null)
			{
				_2940 = BarFunctions.ই("SystemImages.DockHintBottom.png");
			}
			return _2940;
		}
	}

	public static Bitmap _2928
	{
		get
		{
			if (_2946 == null)
			{
				_2946 = BarFunctions.ই("SystemImages.DockHintBottom2010.png");
			}
			return _2946;
		}
	}

	public static Bitmap _2929
	{
		get
		{
			if (_2941 == null)
			{
				_2941 = BarFunctions.ই("SystemImages.DockHintLeft.png");
			}
			return _2941;
		}
	}

	public static Bitmap _292A
	{
		get
		{
			if (_2947 == null)
			{
				_2947 = BarFunctions.ই("SystemImages.DockHintLeft2010.png");
			}
			return _2947;
		}
	}

	public static Bitmap _292B
	{
		get
		{
			if (_2942 == null)
			{
				_2942 = BarFunctions.ই("SystemImages.DockHintRight.png");
			}
			return _2942;
		}
	}

	public static Bitmap _292C
	{
		get
		{
			if (_2948 == null)
			{
				_2948 = BarFunctions.ই("SystemImages.DockHintRight2010.png");
			}
			return _2948;
		}
	}

	public static Bitmap _292D
	{
		get
		{
			if (_2943 == null)
			{
				_2943 = BarFunctions.ই("SystemImages.DockHintTab.png");
			}
			return _2943;
		}
	}

	public static Bitmap _292E
	{
		get
		{
			if (_2949 == null)
			{
				_2949 = BarFunctions.ই("SystemImages.DockHintTab2010.png");
			}
			return _2949;
		}
	}

	public static Bitmap _292F
	{
		get
		{
			if (_294A == null)
			{
				_294A = BarFunctions.ই("SystemImages.DockHintTopBottom2010.png");
			}
			return _294A;
		}
	}

	public static Bitmap _2930
	{
		get
		{
			if (_294B == null)
			{
				_294B = BarFunctions.ই("SystemImages.DockHintLeftRight2010.png");
			}
			return _294B;
		}
	}

	public static Bitmap _2931
	{
		get
		{
			if (_2944 == null)
			{
				_2944 = BarFunctions.ই("SystemImages.DockHintAllSides.png");
			}
			return _2944;
		}
	}

	public static Bitmap _2932
	{
		get
		{
			if (_294C == null)
			{
				_294C = BarFunctions.ই("SystemImages.DockHintAllSides2010.png");
			}
			return _294C;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Form)this).CreateParams;
			createParams.ExStyle |= 0x18;
			return createParams;
		}
	}

	private _295F _2933
	{
		get
		{
			return _2937;
		}
		set
		{
			if (_2937 != value)
			{
				_2937 = value;
				_2938 = true;
			}
		}
	}

	public _295D _2934
	{
		get
		{
			return _2936;
		}
		set
		{
			if (_2936 != value)
			{
				_2936 = value;
				_2950();
				if (((Control)this).IsHandleCreated)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	public bool _2935
	{
		get
		{
			return _293E;
		}
		set
		{
			_293E = value;
		}
	}

	public _2924(_295D hintSide, eDotNetBarStyle style)
		: this(hintSide, middleDockingHint: false, style)
	{
	}

	public _2924(_295D hintSide, bool middleDockingHint, eDotNetBarStyle style)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		ـ();
		_ᎌ = style;
		_293E = middleDockingHint;
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		_2936 = hintSide;
		_2950();
	}

	public void _294D()
	{
		Size size = ((Form)this).Size;
		_089F.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, size.Width, size.Height, 82);
		if (!BarFunctions.ThemedOS)
		{
			_2950();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void _294E()
	{
		_294F(ref _2939);
		_294F(ref _293A);
		_294F(ref _293C);
		_294F(ref _293B);
	}

	private void _294F(ref GraphicsPath _1A6D)
	{
		if (_1A6D != null)
		{
			_1A6D.Dispose();
		}
		_1A6D = null;
	}

	public void _2950()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Expected O, but got Unknown
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Expected O, but got Unknown
		Region val = null;
		Size empty = Size.Empty;
		if (_293E)
		{
			GraphicsPath val2 = new GraphicsPath();
			_293D = _2959(_295D.ᵌ);
			_294E();
			if (_2936 == (_295D._0749 | _295D.ሪ) && (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ)))
			{
				_2939 = _2959(_295D._0749);
				_293A = _2959(_295D.ሪ);
				val2.AddRectangle(new Rectangle(0, 36, 112, 40));
			}
			else if (_2936 == (_295D._074A | _295D.ራ) && (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ)))
			{
				_293C = _2959(_295D._074A);
				_293B = _2959(_295D.ራ);
				val2.AddRectangle(new Rectangle(36, 0, 40, 112));
			}
			else
			{
				if ((_2936 & _295D._0749) != 0)
				{
					_2939 = _2959(_295D._0749);
					val2.AddPath(_2939, true);
				}
				else if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
				{
					val2.AddLine(26, 36, 26, 76);
				}
				else
				{
					val2.AddLine(23, 57, 23, 29);
				}
				if ((_2936 & _295D._074A) != 0)
				{
					_293C = _2959(_295D._074A);
					val2.AddPath(_293C, true);
				}
				else if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
				{
					val2.AddLine(26, 36, 88, 36);
				}
				else
				{
					val2.AddLine(29, 23, 57, 23);
				}
				if ((_2936 & _295D.ሪ) != 0)
				{
					_293A = _2959(_295D.ሪ);
					val2.AddPath(_293A, true);
				}
				else if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
				{
					val2.AddLine(88, 36, 88, 76);
				}
				else
				{
					val2.AddLine(64, 29, 64, 57);
				}
				if ((_2936 & _295D.ራ) != 0)
				{
					_293B = _2959(_295D.ራ);
					val2.AddPath(_293B, true);
				}
				else if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
				{
					val2.AddLine(88, 76, 26, 76);
				}
				else
				{
					val2.AddLine(57, 64, 29, 64);
				}
				val2.CloseAllFigures();
			}
			empty = ((_ᎌ != eDotNetBarStyle.Office2010 && !StyleManager.IsMetro(_ᎌ)) ? new Size(88, 88) : new Size(112, 112));
			val = new Region(val2);
		}
		else
		{
			empty = ((_ᎌ != eDotNetBarStyle.Office2010 && !StyleManager.IsMetro(_ᎌ)) ? (((_2936 & _295D._0749) == 0 && (_2936 & _295D.ሪ) == 0) ? new Size(29, 31) : new Size(31, 29)) : new Size(40, 40));
			if ((_2936 & _295D._0749) != 0)
			{
				_2939 = new GraphicsPath();
				_2939.AddRectangle(new Rectangle(Point.Empty, empty));
			}
			if ((_2936 & _295D.ሪ) != 0)
			{
				_293A = new GraphicsPath();
				_293A.AddRectangle(new Rectangle(Point.Empty, empty));
			}
			if ((_2936 & _295D._074A) != 0)
			{
				_293C = new GraphicsPath();
				_293C.AddRectangle(new Rectangle(Point.Empty, empty));
			}
			if ((_2936 & _295D.ራ) != 0)
			{
				_293B = new GraphicsPath();
				_293B.AddRectangle(new Rectangle(Point.Empty, empty));
			}
			val = new Region(new Rectangle(Point.Empty, empty));
		}
		((Form)this).Size = empty;
		if (val != null)
		{
			((Control)this).Region = val;
		}
	}

	private Rectangle _2951()
	{
		Size size = Size.Empty;
		if ((_2936 & _295D.ራ) == _295D.ራ)
		{
			SizeF size2 = _293B.GetBounds().Size;
			size = new Size((int)size2.Width, (int)size2.Height);
		}
		if ((_2936 & _295D._074A) == _295D._074A)
		{
			SizeF size3 = _293C.GetBounds().Size;
			size = new Size((int)size3.Width, (int)size3.Height);
		}
		if ((_2936 & _295D.ሪ) == _295D.ሪ)
		{
			SizeF size4 = _293A.GetBounds().Size;
			size = new Size((int)size4.Width, (int)size4.Height);
		}
		if ((_2936 & _295D.ᵌ) == _295D.ᵌ)
		{
			SizeF size5 = _293D.GetBounds().Size;
			size = new Size((int)size5.Width, (int)size5.Height);
		}
		if ((_2936 & _295D._0749) == _295D._0749)
		{
			SizeF size6 = _2939.GetBounds().Size;
			size = new Size((int)size6.Width, (int)size6.Height);
		}
		if (_2936 == _295D._295E)
		{
			size = new Size(88, 88);
		}
		return new Rectangle(Point.Empty, size);
	}

	private Image _2952()
	{
		if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
		{
			return (Image)(object)_292E;
		}
		return (Image)(object)_292D;
	}

	private Image _2953()
	{
		if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
		{
			if (_2936 == (_295D._0749 | _295D.ሪ))
			{
				return (Image)(object)_2930;
			}
			if (_2936 == (_295D._074A | _295D.ራ))
			{
				return (Image)(object)_292F;
			}
			return (Image)(object)_2932;
		}
		return (Image)(object)_2931;
	}

	private Image _2954()
	{
		if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
		{
			return (Image)(object)_292A;
		}
		return (Image)(object)_2929;
	}

	private Image _2955()
	{
		if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
		{
			return (Image)(object)_292C;
		}
		return (Image)(object)_292B;
	}

	private Image _2956()
	{
		if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
		{
			return (Image)(object)_2926;
		}
		return (Image)(object)_2925;
	}

	private Image _2957()
	{
		if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
		{
			return (Image)(object)_2928;
		}
		return (Image)(object)_2927;
	}

	private SolidBrush _2958()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		return new SolidBrush(Color.FromArgb(48, Color.Navy));
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Color color = Color.FromArgb(65, 112, 202);
		Color color2 = Color.FromArgb(228, 228, 228);
		SolidBrush val = new SolidBrush(color2);
		Pen val2 = new Pen(color, 1f);
		try
		{
			if (_293E)
			{
				graphics.DrawImageUnscaled(_2953(), 0, 0);
				if ((_2936 & _295D.ᵌ) == _295D.ᵌ)
				{
					Image val3 = _2952();
					if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
					{
						graphics.DrawImageUnscaled(val3, (((Control)this).Width - val3.Width) / 2, (((Control)this).Height - val3.Height) / 2);
					}
					else
					{
						graphics.DrawImageUnscaled(val3, (((Control)this).Width - val3.Width) / 2 + 2, (((Control)this).Height - val3.Height) / 2 + 2);
					}
				}
				if (_ᎌ != eDotNetBarStyle.Office2010 && !StyleManager.IsMetro(_ᎌ))
				{
					if ((_2936 & _295D.ራ) != _295D.ራ)
					{
						graphics.FillRectangle((Brush)(object)val, 33, 60, 23, 26);
					}
					if ((_2936 & _295D._074A) != _295D._074A)
					{
						graphics.FillRectangle((Brush)(object)val, 33, 4, 23, 26);
					}
					if ((_2936 & _295D._0749) != _295D._0749)
					{
						graphics.FillRectangle((Brush)(object)val, 4, 33, 26, 23);
					}
					if ((_2936 & _295D.ሪ) != _295D.ሪ)
					{
						graphics.FillRectangle((Brush)(object)val, 60, 33, 26, 23);
					}
				}
			}
			else
			{
				if ((_2936 & _295D._0749) == _295D._0749)
				{
					graphics.DrawImage(_2954(), _2939.GetBounds());
				}
				if ((_2936 & _295D.ሪ) == _295D.ሪ)
				{
					graphics.DrawImage(_2955(), _293A.GetBounds());
				}
				if ((_2936 & _295D._074A) == _295D._074A)
				{
					graphics.DrawImage(_2956(), _293C.GetBounds());
				}
				if ((_2936 & _295D.ራ) == _295D.ራ)
				{
					graphics.DrawImage(_2957(), _293B.GetBounds());
				}
				else
				{
					graphics.FillRectangle((Brush)(object)val, 33, 84, 23, 26);
				}
			}
			if (_2937 == _295F._0749)
			{
				if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
				{
					Rectangle r = ((!_293E) ? new Rectangle(4, 4, 32, 32) : new Rectangle(4, 40, 32, 32));
					GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(r, 1);
					try
					{
						SolidBrush val4 = _2958();
						try
						{
							graphics.FillPath((Brush)(object)val4, roundedRectanglePath);
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)roundedRectanglePath)?.Dispose();
					}
				}
				else
				{
					RectangleF bounds = _2939.GetBounds();
					if (_293E)
					{
						bounds.Inflate(0f, -6f);
						bounds.Width -= 6f;
					}
					graphics.DrawLine(val2, bounds.X, bounds.Y, bounds.Right, bounds.Y);
					graphics.DrawLine(val2, bounds.X, bounds.Y, bounds.X, bounds.Bottom);
					graphics.DrawLine(val2, bounds.X, bounds.Bottom - 1f, bounds.Right, bounds.Bottom - 1f);
				}
			}
			if (_2937 == _295F.ሪ)
			{
				if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
				{
					Rectangle r2 = ((!_293E) ? new Rectangle(4, 4, 32, 32) : new Rectangle(76, 40, 32, 32));
					GraphicsPath roundedRectanglePath2 = DisplayHelp.GetRoundedRectanglePath(r2, 1);
					try
					{
						SolidBrush val5 = _2958();
						try
						{
							graphics.FillPath((Brush)(object)val5, roundedRectanglePath2);
						}
						finally
						{
							((IDisposable)val5)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)roundedRectanglePath2)?.Dispose();
					}
				}
				else
				{
					RectangleF bounds2 = _293A.GetBounds();
					if (_293E)
					{
						bounds2.Inflate(0f, -6f);
						bounds2.Width -= 6f;
						bounds2.X += 6f;
					}
					graphics.DrawLine(val2, bounds2.X, bounds2.Y, bounds2.Right, bounds2.Y);
					graphics.DrawLine(val2, bounds2.Right - 1f, bounds2.Y, bounds2.Right - 1f, bounds2.Bottom);
					graphics.DrawLine(val2, bounds2.X, bounds2.Bottom - 1f, bounds2.Right, bounds2.Bottom - 1f);
				}
			}
			if (_2937 == _295F._074A)
			{
				if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
				{
					Rectangle r3 = ((!_293E) ? new Rectangle(4, 4, 32, 32) : new Rectangle(40, 4, 32, 32));
					GraphicsPath roundedRectanglePath3 = DisplayHelp.GetRoundedRectanglePath(r3, 1);
					try
					{
						SolidBrush val6 = _2958();
						try
						{
							graphics.FillPath((Brush)(object)val6, roundedRectanglePath3);
						}
						finally
						{
							((IDisposable)val6)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)roundedRectanglePath3)?.Dispose();
					}
				}
				else
				{
					RectangleF bounds3 = _293C.GetBounds();
					if (_293E)
					{
						bounds3.Inflate(-6f, 0f);
						bounds3.Height -= 6f;
					}
					graphics.DrawLine(val2, bounds3.X, bounds3.Y, bounds3.Right, bounds3.Y);
					graphics.DrawLine(val2, bounds3.Right - 1f, bounds3.Y, bounds3.Right - 1f, bounds3.Bottom);
					graphics.DrawLine(val2, bounds3.X, bounds3.Y, bounds3.X, bounds3.Bottom);
				}
			}
			if (_2937 == _295F.ራ)
			{
				if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
				{
					Rectangle r4 = ((!_293E) ? new Rectangle(4, 4, 32, 32) : new Rectangle(40, 76, 32, 32));
					GraphicsPath roundedRectanglePath4 = DisplayHelp.GetRoundedRectanglePath(r4, 1);
					try
					{
						SolidBrush val7 = _2958();
						try
						{
							graphics.FillPath((Brush)(object)val7, roundedRectanglePath4);
						}
						finally
						{
							((IDisposable)val7)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)roundedRectanglePath4)?.Dispose();
					}
				}
				else
				{
					RectangleF bounds4 = _293B.GetBounds();
					if (_293E)
					{
						bounds4.Inflate(-6f, 0f);
						bounds4.Height -= 6f;
						bounds4.Y += 6f;
					}
					graphics.DrawLine(val2, bounds4.X, bounds4.Bottom - 1f, bounds4.Right, bounds4.Bottom - 1f);
					graphics.DrawLine(val2, bounds4.Right - 1f, bounds4.Y, bounds4.Right - 1f, bounds4.Bottom);
					graphics.DrawLine(val2, bounds4.X, bounds4.Y, bounds4.X, bounds4.Bottom);
				}
			}
			if (_2937 != _295F.ᵌ || (_ᎌ != eDotNetBarStyle.Office2010 && !StyleManager.IsMetro(_ᎌ)) || !_293E)
			{
				return;
			}
			Rectangle r5 = new Rectangle(40, 40, 32, 32);
			GraphicsPath roundedRectanglePath5 = DisplayHelp.GetRoundedRectanglePath(r5, 1);
			try
			{
				SolidBrush val8 = _2958();
				try
				{
					graphics.FillPath((Brush)(object)val8, roundedRectanglePath5);
				}
				finally
				{
					((IDisposable)val8)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)roundedRectanglePath5)?.Dispose();
			}
		}
		finally
		{
			((Brush)val).Dispose();
			val2.Dispose();
		}
	}

	public GraphicsPath _2959(_295D _295A)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		if (_295A == _295D.ᵌ)
		{
			if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
			{
				val.AddRectangle(new Rectangle(40, 40, 32, 32));
			}
			else
			{
				val.AddRectangle(new Rectangle(29, 29, 29, 29));
			}
		}
		else
		{
			val.StartFigure();
			if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
			{
				val.AddLine(27, 35, 36, 26);
				val.AddLine(36, 0, 76, 0);
				val.AddLine(76, 26, 86, 35);
			}
			else
			{
				val.AddLine(23, 28, 28, 23);
				val.AddLine(29, 23, 29, 0);
				val.AddLine(57, 0, 57, 23);
				val.AddLine(58, 23, 63, 28);
			}
		}
		switch (_295A)
		{
		case _295D.ራ:
		{
			Matrix val4 = new Matrix();
			try
			{
				val4.Rotate(180f);
				if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
				{
					val4.Translate((0f - val.GetBounds().Width) * 2f + 6f, (0f - val.GetBounds().Height) * 2f - 42f);
				}
				else
				{
					val4.Translate((0f - val.GetBounds().Width) * 2f - 6f, (0f - val.GetBounds().Height) * 2f - 30f);
				}
				val.Transform(val4);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			break;
		}
		case _295D._0749:
		{
			Matrix val3 = new Matrix();
			try
			{
				val3.Rotate(-90f);
				if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
				{
					val3.Translate(-53f, 0f);
				}
				else
				{
					val3.Translate(-46f, 0f);
				}
				val.Transform(val3);
				val3.Reset();
				val3.Translate(0f, val.GetBounds().Height);
				val.Transform(val3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			break;
		}
		case _295D.ሪ:
		{
			Matrix val2 = new Matrix();
			try
			{
				val2.Rotate(90f);
				val.Transform(val2);
				val2.Reset();
				if (_ᎌ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_ᎌ))
				{
					val2.Translate(val.GetBounds().Width * 2f + 42f, 0f);
				}
				else
				{
					val2.Translate(val.GetBounds().Width * 2f + 30f, 0f);
				}
				val.Transform(val2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			break;
		}
		}
		return val;
	}

	internal _295F _295B(int و, int ى)
	{
		if (((Control)this).Bounds.Contains(و, ى))
		{
			Point point = ((Control)this).PointToClient(new Point(و, ى));
			if (_2939 != null && _2939.IsVisible(point))
			{
				_2933 = _295F._0749;
			}
			else if (_293A != null && _293A.IsVisible(point))
			{
				_2933 = _295F.ሪ;
			}
			else if (_293C != null && _293C.IsVisible(point))
			{
				_2933 = _295F._074A;
			}
			else if (_293B != null && _293B.IsVisible(point))
			{
				_2933 = _295F.ራ;
			}
			else if (_293D != null && _293D.IsVisible(point) && (_2936 & _295D.ᵌ) == _295D.ᵌ)
			{
				_2933 = _295F.ᵌ;
			}
			else
			{
				_2933 = _295F.ᘶ;
			}
		}
		else
		{
			_2933 = _295F.ᘶ;
		}
		_295C();
		return _2933;
	}

	private void _295C()
	{
		if (_2938)
		{
			_2938 = false;
			((Control)this).Refresh();
		}
	}

	private void ـ()
	{
		((Form)this).ControlBox = false;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).MinimizeBox = false;
		((Form)this).MaximizeBox = false;
		((Form)this).ControlBox = false;
		((Control)this).Name = "DockingHint";
		((Control)this).Text = "";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)0;
	}
}
[Flags]
internal enum _295D
{
	_0749 = 1,
	ሪ = 2,
	_074A = 4,
	ራ = 8,
	ᵌ = 0x10,
	_26AF = 0x1F,
	_295E = 0xF
}
internal enum _295F
{
	ᘶ,
	_0749,
	ሪ,
	_074A,
	ራ,
	ᵌ
}
internal static class _2A76
{
	public static double _2A77(double _2A78)
	{
		return _2A78 * (180.0 / Math.PI);
	}

	public static double _2A79(double _2A7A)
	{
		return _2A7A * (Math.PI / 180.0);
	}
}
internal struct _2CE7
{
	public Rectangle _2CE8;

	public Rectangle _2CE9;

	public Color _22AC;

	public double _༡;

	public static readonly _2CE7 _2CEA = default(_2CE7);

	public int Ⳬ;

	public double ᒊ;

	public double ᒋ;

	public bool ᮄ => _2CE8.IsEmpty;

	public _2CE7(Rectangle hotPoint, Color hotPointColor, double value, int dataPointIndex)
	{
		_2CE8 = hotPoint;
		_22AC = hotPointColor;
		_༡ = value;
		ᒊ = 0.0;
		ᒋ = 0.0;
		_2CE9 = Rectangle.Empty;
		Ⳬ = dataPointIndex;
	}

	public _2CE7(Rectangle hotPointBounds, Rectangle chartSliceBounds, Color color, double value, int dataPointIndex)
	{
		_2CE8 = hotPointBounds;
		_2CE9 = chartSliceBounds;
		_22AC = color;
		_༡ = value;
		ᒊ = 0.0;
		ᒋ = 0.0;
		Ⳬ = dataPointIndex;
	}

	public _2CE7(Rectangle bounds, Color color, double value, double startAngle, double sweepAngle, int dataPointIndex)
	{
		_2CE8 = bounds;
		_22AC = color;
		_༡ = value;
		ᒊ = startAngle;
		ᒋ = sweepAngle;
		_2CE9 = Rectangle.Empty;
		Ⳬ = dataPointIndex;
	}
}
internal abstract class _2DEF
{
	private int _2DF1 = 6;

	protected int _2DF0
	{
		get
		{
			return _2DF1;
		}
		set
		{
			_2DF1 = value;
		}
	}

	public _2DEF()
	{
	}

	public virtual void Paint(_2DF3 info)
	{
	}

	protected virtual void PaintArrow(Point p, _2DF3 info)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		GraphicsPath val = ឱ.ឲ(p, _2DF0, _2DF2(info));
		if (!info._2DF4._2DFD.IsEmpty)
		{
			SolidBrush val2 = new SolidBrush(info._2DF4._2DFD);
			try
			{
				info.ܓ.FillPath((Brush)(object)val2, val);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (!info._2DF4._2DFC.IsEmpty)
		{
			Pen val3 = new Pen(info._2DF4._2DFC, 1f);
			try
			{
				info.ܓ.DrawPath(val3, val);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		val.Dispose();
	}

	protected _17B6 _2DF2(_2DF3 _108F)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		if (((int)_108F._2DF7 == 1 && _108F._2DF6) || ((int)_108F._2DF7 == 2 && !_108F._2DF6))
		{
			return _17B6._074A;
		}
		if (((int)_108F._2DF7 == 1 && !_108F._2DF6) || ((int)_108F._2DF7 == 2 && _108F._2DF6))
		{
			return _17B6.ራ;
		}
		if (((int)_108F._2DF7 == 3 && _108F._2DF6) || ((int)_108F._2DF7 == 4 && !_108F._2DF6))
		{
			return _17B6._0749;
		}
		return _17B6.ሪ;
	}

	protected LinearGradientBrush ॴ(Rectangle ल, Color ॵ, Color ॶ, float ॷ)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		return new LinearGradientBrush(new Rectangle(ल.X - 1, ल.Y - 1, ल.Width + 1, ल.Height + 1), ॵ, ॶ, ॷ);
	}

	protected virtual void PaintBackground(_2DF3 info)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		int num = info._2DF4._2DFA;
		Rectangle ᆽ = info.ᆽ;
		if ((int)info._2DF7 == 1 || (int)info._2DF7 == 2)
		{
			num += 90;
		}
		if (info._2DF4._2DF9.IsEmpty)
		{
			SolidBrush val = new SolidBrush(info._2DF4.ᖳ);
			try
			{
				info.ܓ.FillRectangle((Brush)(object)val, ᆽ);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		LinearGradientBrush val2 = ॴ(ᆽ, info._2DF4.ᖳ, info._2DF4._2DF9, num);
		try
		{
			info.ܓ.FillRectangle((Brush)(object)val2, ᆽ);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
internal class _2DF3
{
	public Graphics ܓ;

	public Rectangle ᆽ;

	public _2DF8 _2DF4;

	public bool _2DF5;

	public bool _2DF6;

	public DockStyle _2DF7;
}
internal class _2DF8
{
	public Color ᖳ = Color.Empty;

	public Color _2DF9 = Color.Empty;

	public int _2DFA;

	public Color _2DFB = Color.Empty;

	public Color _26D9 = Color.Empty;

	public Color _2DFC = Color.Empty;

	public Color _2DFD = Color.Empty;
}
internal class _2DFE : _2DEF
{
	public override void Paint(_2DF3 info)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		int num = 8;
		Rectangle ᆽ = info.ᆽ;
		Graphics ܓ = info.ܓ;
		_2DF8 obj = info._2DF4;
		PaintBackground(info);
		if ((int)info._2DF7 == 1 || (int)info._2DF7 == 2)
		{
			Point point = new Point(ᆽ.X + (ᆽ.Width - 34) / 2 - 1, ᆽ.Y + (ᆽ.Height - 4) / 2);
			SolidBrush val = new SolidBrush(obj._26D9);
			try
			{
				int num2 = point.X + 1;
				int num3 = point.Y + 1;
				for (int i = 0; i < 9; i++)
				{
					ܓ.FillRectangle((Brush)(object)val, num2, num3, 2, 2);
					num2 += 4;
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			SolidBrush val2 = new SolidBrush(obj._2DFB);
			try
			{
				int num4 = point.X;
				int y = point.Y;
				for (int j = 0; j < 9; j++)
				{
					ܓ.FillRectangle((Brush)(object)val2, num4, y, 2, 2);
					num4 += 4;
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			if (info._2DF5)
			{
				Point p = new Point(ᆽ.X + (ᆽ.Width - 36) / 2 - base._2DF0 - num, ᆽ.Y + (ᆽ.Height - base._2DF0 / 2) / 2);
				PaintArrow(p, info);
				p.Offset(36 + num * 2 + base._2DF0, 0);
				PaintArrow(p, info);
			}
			return;
		}
		Point point2 = new Point(ᆽ.X + (ᆽ.Width - 4) / 2, ᆽ.Y + (ᆽ.Height - 34) / 2);
		SolidBrush val3 = new SolidBrush(obj._26D9);
		try
		{
			int num5 = point2.Y;
			int num6 = point2.X + 1;
			for (int k = 0; k < 9; k++)
			{
				ܓ.FillRectangle((Brush)(object)val3, num6, num5, 2, 2);
				num5 += 4;
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		SolidBrush val4 = new SolidBrush(obj._2DFB);
		try
		{
			int num7 = point2.Y - 1;
			int x = point2.X;
			for (int l = 0; l < 9; l++)
			{
				ܓ.FillRectangle((Brush)(object)val4, x, num7, 2, 2);
				num7 += 4;
			}
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		if (info._2DF5)
		{
			Point p2 = new Point(ᆽ.X + (ᆽ.Width - base._2DF0 / 2) / 2, ᆽ.Y + (ᆽ.Height - 36) / 2 - base._2DF0 - num);
			PaintArrow(p2, info);
			p2.Offset(0, 36 + num * 2 + base._2DF0);
			PaintArrow(p2, info);
		}
	}
}
internal class _2DFF : _2DEF
{
	public override void Paint(_2DF3 info)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		int num = 42;
		int num2 = 8;
		Rectangle ᆽ = info.ᆽ;
		Graphics ܓ = info.ܓ;
		PaintBackground(info);
		if ((int)info._2DF7 == 1 || (int)info._2DF7 == 2)
		{
			if (info.ᆽ.Width - base._2DF0 * 2 < num)
			{
				num = info.ᆽ.Width - base._2DF0 * 2;
				if (num < 0)
				{
					num = info.ᆽ.Width;
				}
			}
			Point point = new Point(ᆽ.X + (ᆽ.Width - num) / 2, ᆽ.Y + (ᆽ.Height - 5) / 2);
			SolidBrush val = new SolidBrush(info._2DF4._26D9);
			SolidBrush val2 = new SolidBrush(info._2DF4._2DFB);
			for (int i = point.X; i < point.X + num; i += 3)
			{
				ܓ.FillRectangle((Brush)(object)val, i, point.Y + 3, 1, 1);
				ܓ.FillRectangle((Brush)(object)val2, i + 1, point.Y + 4, 1, 1);
				ܓ.FillRectangle((Brush)(object)val, i + 1, point.Y, 1, 1);
				ܓ.FillRectangle((Brush)(object)val2, i + 2, point.Y + 1, 1, 1);
			}
			((Brush)val).Dispose();
			((Brush)val2).Dispose();
			if (info._2DF5)
			{
				Point p = new Point(ᆽ.X + (ᆽ.Width - num) / 2 - base._2DF0 - num2, ᆽ.Y + (ᆽ.Height - base._2DF0 / 2) / 2);
				PaintArrow(p, info);
				p.Offset(num + num2 * 2 + base._2DF0, 0);
				PaintArrow(p, info);
			}
			return;
		}
		if (info.ᆽ.Height - base._2DF0 * 2 < num)
		{
			num = info.ᆽ.Height - base._2DF0 * 2;
			if (num < 0)
			{
				num = info.ᆽ.Height;
			}
		}
		Point point2 = new Point(ᆽ.X + (ᆽ.Width - 5) / 2, ᆽ.Y + (ᆽ.Height - num) / 2);
		SolidBrush val3 = new SolidBrush(info._2DF4._26D9);
		SolidBrush val4 = new SolidBrush(info._2DF4._2DFB);
		for (int j = point2.Y; j < point2.Y + num; j += 3)
		{
			ܓ.FillRectangle((Brush)(object)val3, point2.X + 3, j, 1, 1);
			ܓ.FillRectangle((Brush)(object)val4, point2.X + 4, j + 1, 1, 1);
			ܓ.FillRectangle((Brush)(object)val3, point2.X, j + 1, 1, 1);
			ܓ.FillRectangle((Brush)(object)val4, point2.X + 1, j + 2, 1, 1);
		}
		((Brush)val3).Dispose();
		((Brush)val4).Dispose();
		if (info._2DF5)
		{
			Point p2 = new Point(ᆽ.X + (ᆽ.Width - base._2DF0 / 2) / 2, ᆽ.Y + (ᆽ.Height - num) / 2 - base._2DF0 - num2);
			PaintArrow(p2, info);
			p2.Offset(0, num + num2 * 2 + base._2DF0);
			PaintArrow(p2, info);
		}
	}
}
internal class _2E8F
{
	private const string _2E91 = "Misc.";

	private object __2E92;

	private object[] __2D76;

	private TypeConverter __2E93;

	private Attribute[] __2E94;

	private _2EA5 __2E95;

	private List<string> __2D7A;

	private List<string> __2D7B;

	private IPropertyGridLocalizer __2E96;

	private PropertySettingsCollection __2D7C;

	private ITypeDescriptorContext __2E97;

	private static CategoryAttribute __2E98 = new CategoryAttribute();

	private ePropertyGridHelpType __2D7E;

	public ePropertyGridHelpType _2E90
	{
		get
		{
			return __2D7E;
		}
		set
		{
			__2D7E = value;
		}
	}

	public TypeConverter _2E30
	{
		get
		{
			return __2E93;
		}
		set
		{
			__2E93 = value;
		}
	}

	public _2E8F(object selectedObject, Attribute[] attributeFilter, _2EA5 nodeFactory, List<string> ignoredProperties, List<string> ignoredCategories, IPropertyGridLocalizer localizer, PropertySettingsCollection propertySettings)
	{
		__2E92 = selectedObject;
		__2E94 = attributeFilter;
		__2E95 = nodeFactory;
		__2D7A = ignoredProperties;
		__2D7B = ignoredCategories;
		__2E96 = localizer;
		__2D7C = propertySettings;
	}

	public _2E8F(ITypeDescriptorContext context, object selectedObject, Attribute[] attributeFilter, _2EA5 nodeFactory, List<string> ignoredProperties, List<string> ignoredCategories, IPropertyGridLocalizer localizer, PropertySettingsCollection propertySettings)
	{
		__2E97 = context;
		__2E92 = selectedObject;
		__2E94 = attributeFilter;
		__2E95 = nodeFactory;
		__2D7A = ignoredProperties;
		__2D7B = ignoredCategories;
		__2E96 = localizer;
		__2D7C = propertySettings;
	}

	public _2E8F(object[] selectedObjects, Attribute[] attributeFilter, _2EA5 nodeFactory, List<string> ignoredProperties, List<string> ignoredCategories, IPropertyGridLocalizer localizer, PropertySettingsCollection propertySettings)
	{
		__2D76 = selectedObjects;
		__2E94 = attributeFilter;
		__2E95 = nodeFactory;
		__2D7A = ignoredProperties;
		__2D7B = ignoredCategories;
		__2E96 = localizer;
		__2D7C = propertySettings;
	}

	public _2E8F(ITypeDescriptorContext context, object[] selectedObjects, Attribute[] attributeFilter, _2EA5 nodeFactory, List<string> ignoredProperties, List<string> ignoredCategories, IPropertyGridLocalizer localizer, PropertySettingsCollection propertySettings)
	{
		__2E97 = context;
		__2D76 = selectedObjects;
		__2E94 = attributeFilter;
		__2E95 = nodeFactory;
		__2D7A = ignoredProperties;
		__2D7B = ignoredCategories;
		__2E96 = localizer;
		__2D7C = propertySettings;
	}

	internal void ሕ(NodeCollection _2737, ePropertySort _2E99, SuperTooltip _2E9A)
	{
		PropertyDescriptorCollection propertyDescriptorCollection = _2E9E();
		bool flag = __2D7A.Count > 0;
		bool flag2 = __2D7B.Count > 0;
		if (_2E99 == ePropertySort.Categorized || _2E99 == ePropertySort.CategorizedAlphabetical)
		{
			Dictionary<string, Node> dictionary = new Dictionary<string, Node>();
			foreach (PropertyDescriptor item in propertyDescriptorCollection)
			{
				if (item == null || (flag && __2D7A.Contains(item.Name)))
				{
					continue;
				}
				string text = _23E1(item);
				PropertySettings propertySettings = __2D7C[item, item.Name];
				if (propertySettings != null && propertySettings.Category != null)
				{
					text = propertySettings.Category;
				}
				if (!flag2 || string.IsNullOrEmpty(text) || !__2D7B.Contains(text))
				{
					Node value = null;
					if (!dictionary.TryGetValue(text, out value))
					{
						value = __2E95._2EB0(text, __2E96);
						_2737.Add(value);
						dictionary.Add(text, value);
					}
					PropertyNode propertyNode = __2E95._2EAA(item, __2E92 ?? __2D76, __2E96, propertySettings, __2D76 != null);
					value.Nodes.Add(propertyNode);
					propertyNode._2E74();
					if (__2D7E == ePropertyGridHelpType.SuperTooltip && _2E9A != null)
					{
						propertyNode._2E6D(_2E9A);
					}
					propertyNode.UpdateDisplayedValue();
				}
			}
			if (_2E99 != ePropertySort.CategorizedAlphabetical)
			{
				return;
			}
			foreach (Node value2 in dictionary.Values)
			{
				value2.Nodes.Sort();
			}
			_2737.Sort();
			return;
		}
		foreach (PropertyDescriptor item2 in propertyDescriptorCollection)
		{
			if (item2 != null && !__2D7A.Contains(item2.Name))
			{
				PropertyNode propertyNode2 = __2E95._2EAA(item2, __2E92 ?? __2D76, __2E96, __2D7C, __2D76 != null);
				_2737.Add(propertyNode2);
				propertyNode2._2E74();
				if (__2D7E == ePropertyGridHelpType.SuperTooltip && _2E9A != null)
				{
					propertyNode2._2E6D(_2E9A);
				}
				propertyNode2.UpdateDisplayedValue();
			}
		}
		if (_2E99 == ePropertySort.Alphabetical)
		{
			_2737.Sort();
		}
	}

	internal static bool _2E9B(PropertyDescriptor _06EC, object Ⴅ)
	{
		bool flag = TypeDescriptor.GetAttributes(Ⴅ).Contains(InheritanceAttribute.InheritedReadOnly);
		if (TypeDescriptor.GetAttributes(Ⴅ).Contains(ReadOnlyAttribute.Yes))
		{
			flag = true;
		}
		ReadOnlyAttribute readOnlyAttribute = _06EC.Attributes[typeof(ReadOnlyAttribute)] as ReadOnlyAttribute;
		if (!flag && readOnlyAttribute != null && readOnlyAttribute.IsReadOnly && !readOnlyAttribute.IsDefaultAttribute())
		{
			flag = true;
		}
		if (!flag)
		{
			flag = _06EC.IsReadOnly;
		}
		return flag;
	}

	private string _23E1(PropertyDescriptor _06EC)
	{
		string category = _06EC.Category;
		if (!string.IsNullOrEmpty(category))
		{
			return category;
		}
		if (_06EC.Attributes[__2E98.GetType()] is CategoryAttribute categoryAttribute && !string.IsNullOrEmpty(categoryAttribute.Category))
		{
			return categoryAttribute.Category;
		}
		return "Misc.";
	}

	private TypeConverter _2E9C(object _2E9D)
	{
		if (__2E93 != null && __2E92 != null)
		{
			return __2E93;
		}
		TypeConverter converter = TypeDescriptor.GetConverter(_2E9D);
		if (converter.GetPropertiesSupported())
		{
			return converter;
		}
		return null;
	}

	private PropertyDescriptorCollection _2E9E()
	{
		Attribute[] _2EA4 = __2E94;
		if (__2E92 != null)
		{
			object _2E9D = __2E92;
			return _2E9E(__2E97, _2EA4, _2E9D, _2E9C(_2E9D));
		}
		int num = __2D76.Length;
		List<PropertyDescriptor> list = new List<PropertyDescriptor>();
		List<PropertyDescriptorCollection> list2 = new List<PropertyDescriptorCollection>(num);
		object[] array = __2D76;
		foreach (object _2E9D2 in array)
		{
			list2.Add(_2E9E(__2E97, _2EA4, _2E9D2, _2E9C(_2E9D2)));
		}
		PropertyDescriptorCollection propertyDescriptorCollection = list2[0];
		for (int j = 0; j < propertyDescriptorCollection.Count; j++)
		{
			PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[j];
			if (!propertyDescriptor.Attributes[typeof(MergablePropertyAttribute)].IsDefaultAttribute())
			{
				continue;
			}
			int num2 = 1;
			for (int k = 1; k < list2.Count; k++)
			{
				PropertyDescriptorCollection propertyDescriptorCollection2 = list2[k];
				foreach (PropertyDescriptor item in propertyDescriptorCollection2)
				{
					if (_2E9F(item, propertyDescriptor) && propertyDescriptor.Attributes[typeof(MergablePropertyAttribute)].IsDefaultAttribute())
					{
						num2++;
						break;
					}
				}
			}
			if (num2 == num)
			{
				list.Add(propertyDescriptor);
			}
		}
		return new PropertyDescriptorCollection(list.ToArray());
	}

	private bool _2E9F(PropertyDescriptor _2EA0, PropertyDescriptor _2EA1)
	{
		if (_2EA0.Name == _2EA1.Name)
		{
			return (object)_2EA0.PropertyType == _2EA1.PropertyType;
		}
		return false;
	}

	public static PropertyDescriptor _2EA2(string फ़, object _2E9D, TypeConverter _2EA3)
	{
		PropertyDescriptorCollection propertyDescriptorCollection = _2E9E(null, _2E9D, _2EA3);
		foreach (PropertyDescriptor item in propertyDescriptorCollection)
		{
			if (item.Name == फ़)
			{
				return item;
			}
		}
		return null;
	}

	public static PropertyDescriptorCollection _2E9E(Attribute[] _2EA4, object _2E9D, TypeConverter _2EA3)
	{
		return _2E9E(null, _2EA4, _2E9D, _2EA3);
	}

	public static PropertyDescriptorCollection _2E9E(ITypeDescriptorContext Ⴊ, Attribute[] _2EA4, object _2E9D, TypeConverter _2EA3)
	{
		if (_2E9D == null)
		{
			return null;
		}
		if (_2E9D is ICustomTypeDescriptor)
		{
			ICustomTypeDescriptor customTypeDescriptor = (ICustomTypeDescriptor)_2E9D;
			if (_2EA4 != null)
			{
				return customTypeDescriptor.GetProperties(_2EA4);
			}
			return customTypeDescriptor.GetProperties();
		}
		if (_2EA3 != null)
		{
			return _2EA3.GetProperties(Ⴊ, _2E9D, _2EA4);
		}
		_2E9D.GetType();
		if (_2EA4 != null)
		{
			return TypeDescriptor.GetProperties(_2E9D, _2EA4);
		}
		return TypeDescriptor.GetProperties(_2E9D);
	}
}
internal class _2EA5
{
	private int __2EA8 = 9;

	private IPropertyElementStyleProvider __2EA9;

	public int _2EA6
	{
		get
		{
			return __2EA8;
		}
		set
		{
			__2EA8 = value;
		}
	}

	public IPropertyElementStyleProvider _2EA7
	{
		get
		{
			return __2EA9;
		}
		set
		{
			__2EA9 = value;
		}
	}

	public _2EA5(IPropertyElementStyleProvider styleProvider)
	{
		__2EA9 = styleProvider;
	}

	public PropertyNode _2EAA(PropertyDescriptor _06EC, object _2EAB, IPropertyGridLocalizer _2EAC, PropertySettingsCollection _2EAD, bool _2EAE)
	{
		PropertySettings _2EAF = _2EAD[_06EC, _06EC.Name];
		return _2EAA(_06EC, _2EAB, _2EAC, _2EAF, _2EAE);
	}

	public PropertyNode _2EAA(PropertyDescriptor _06EC, object _2EAB, IPropertyGridLocalizer _2EAC, PropertySettings _2EAF, bool _2EAE)
	{
		PropertyNode propertyNode = null;
		propertyNode = (((object)_06EC.PropertyType == typeof(Color) && (_2EAF == null || _2EAF.ValueEditor == null)) ? new PropertyNodeColor(_06EC) : (((object)_06EC.PropertyType == typeof(bool) && (_2EAF == null || _2EAF.ValueEditor == null)) ? new PropertyCheckBoxNode(_06EC) : ((_2EAF == null || _2EAF.PropertyNodeType != ePropertyNodeType.RadioButtonList) ? new PropertyNode(_06EC) : new PropertyOptionListNode(_06EC))));
		propertyNode.IsMultiObject = _2EAE;
		propertyNode.NodesIndent = __2EA8;
		propertyNode.ImageAlignment = eCellPartAlignment.FarCenter;
		string text = _2EAC.GetPropertyName(_06EC.Name);
		if (string.IsNullOrEmpty(text))
		{
			text = _06EC.DisplayName;
		}
		propertyNode.Text = text;
		propertyNode._2E31 = _2EAB;
		Cell cell = new Cell();
		cell.Selectable = false;
		cell.TextMarkupEnabled = false;
		propertyNode.Cells.Add(cell);
		propertyNode.PropertySettings = _2EAF;
		return propertyNode;
	}

	public Node _2EB0(string _23E7, IPropertyGridLocalizer _2EAC)
	{
		PropertyCategoryNode propertyCategoryNode = new PropertyCategoryNode();
		if (__2EA9 != null)
		{
			propertyCategoryNode.Style = __2EA9.CategoryStyle;
		}
		string text = _2EAC.GetCategoryName(_23E7);
		if (string.IsNullOrEmpty(text))
		{
			text = _23E7;
		}
		propertyCategoryNode.Text = text;
		propertyCategoryNode.Expanded = true;
		propertyCategoryNode.FullRowBackground = true;
		propertyCategoryNode.Selectable = true;
		propertyCategoryNode.CellNavigationEnabled = false;
		return propertyCategoryNode;
	}
}
internal class _2EC9
{
	public struct tagHH_FTS_QUERY
	{
		private long _2ED8;

		private long _2ED9;

		private string _2EDA;

		private long _2EDB;

		private long _2EDC;

		private long _2EDD;

		private long _2EDE;

		private string _2EDF;
	}

	private const int _2ECB = 0;

	private const int _2ECC = 1;

	private const int _2ECD = 2;

	private const int _2ECE = 3;

	private const int _2ECF = 15;

	public const int _2ED0 = 16;

	private Control _2ED1;

	private string _2ED2 = "";

	private IntPtr _2ED3 = IntPtr.Zero;

	public Control ކ
	{
		get
		{
			return _2ED1;
		}
		set
		{
			_2ED1 = value;
		}
	}

	public string _2ECA
	{
		get
		{
			return _2ED2;
		}
		set
		{
			_2ED2 = value;
		}
	}

	[DllImport("hhctrl.ocx", CharSet = CharSet.Ansi, EntryPoint = "HtmlHelpA", SetLastError = true)]
	private static extern IntPtr HtmlHelp(IntPtr hwnd, string lpHelpFile, int wCommand, int dwData);

	[DllImport("hhctrl.ocx", CharSet = CharSet.Auto, EntryPoint = "HtmlHelp", SetLastError = true)]
	private static extern IntPtr HTMLHelpFtsQuery(IntPtr hwnd, string lpHelpFile, int wCommand, ref tagHH_FTS_QUERY dwData);

	[DllImport("user32")]
	private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32")]
	private static extern bool IsWindow(IntPtr hWnd);

	public _2EC9()
	{
	}

	public _2EC9(Control parent, string helpfile)
	{
		_2ED1 = parent;
		_2ED2 = helpfile;
	}

	public void _2ED4()
	{
		if (File.Exists(_2ED2))
		{
			_2ED3 = HtmlHelp(_2ED1.Handle, _2ED2, 1, 0);
			return;
		}
		throw new InvalidOperationException("Help file could not be found.");
	}

	public void _2ED5()
	{
		if (File.Exists(_2ED2))
		{
			_2ED3 = HtmlHelp(_2ED1.Handle, _2ED2, 2, 0);
			return;
		}
		throw new InvalidOperationException("Help file could not be found.");
	}

	public void _2ED6()
	{
		if (File.Exists(_2ED2))
		{
			_2ED3 = HtmlHelp(_2ED1.Handle, _2ED2, 3, 0);
			return;
		}
		throw new InvalidOperationException("Help file could not be found.");
	}

	public void _2ED7()
	{
		if (_2ED3 != IntPtr.Zero && IsWindow(_2ED3))
		{
			PostMessage(_2ED3, 16, 0, 0);
		}
		_2ED3 = IntPtr.Zero;
	}
}
internal class _2F05 : OfficeForm
{
	private eTaskDialogBackgroundColor __2F08;

	private eTaskDialogButton __2F09 = eTaskDialogButton.Yes;

	private static readonly int _2F0A = 420;

	private static readonly int _2EE2 = 200;

	private eTaskDialogResult __2F0B;

	private bool _᮰ = true;

	private IContainer Պ;

	private PictureBox _2F0C;

	private LabelX _2F0D;

	private LabelX _2F0E;

	private ItemPanel _2F0F;

	private PanelEx _2F10;

	private ButtonX _2F11;

	private ButtonX _2F12;

	private ButtonX _2F13;

	private ButtonX ẘ;

	private ButtonX _2F14;

	private ButtonX _2F15;

	private CheckBoxX _2F16;

	private LabelX _2F17;

	private PictureBox _2F18;

	private PanelEx _2F19;

	private FlowLayoutPanel _2F1A;

	[DefaultValue(eTaskDialogBackgroundColor.Default)]
	public eTaskDialogBackgroundColor _2F06
	{
		get
		{
			return __2F08;
		}
		set
		{
			__2F08 = value;
		}
	}

	public eTaskDialogResult _2F07 => __2F0B;

	public bool Ⲭ
	{
		get
		{
			return _᮰;
		}
		set
		{
			_᮰ = value;
			_2F0D.AntiAlias = value;
			_2F0E.AntiAlias = value;
			_2F0F.AntiAlias = value;
			_2F11.AntiAlias = value;
			ẘ.AntiAlias = value;
			_2F15.AntiAlias = value;
			_2F14.AntiAlias = value;
			_2F13.AntiAlias = value;
			_2F17.AntiAlias = value;
			_2F16.AntiAlias = value;
		}
	}

	public _2F05()
	{
		ـ();
		Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
		_2F10.Style.BackColor1.Color = colorTable.Form.BackColor;
		_2F0D.MarkupLinkClick += Ӟ;
		_2F0E.MarkupLinkClick += Ӟ;
		_2F17.MarkupLinkClick += Ӟ;
		((Form)this).ShowInTaskbar = false;
	}

	private void Ӟ(object _٠, MarkupLinkClickEventArgs ؿ)
	{
		TaskDialog.ۃ(_٠, ؿ);
	}

	private void _2F1B(object _٠, EventArgs ؿ)
	{
	}

	public eTaskDialogResult _2F1C(IWin32Window ॡ, TaskDialogInfo _2F1D)
	{
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		if (ॡ != null)
		{
			((Form)this).StartPosition = (FormStartPosition)4;
		}
		else
		{
			((Form)this).StartPosition = (FormStartPosition)1;
		}
		_2F1F(_2F1D);
		int num = Dpi.Width20;
		int num2 = 0;
		int num3 = Dpi.Height32;
		((Control)this).Text = _2F1D.Title;
		((Control)_2F0D).Text = _2F1D.Header;
		((Control)_2F0E).Text = _2F1D.Text;
		__2F09 = _2F1D.DefaultButton;
		if (_2F1D.TaskDialogIcon == eTaskDialogIcon.None)
		{
			int num4 = ((Control)this).Width - ((Control)_2F0D).Right;
			((Control)_2F0D).Left = ((Control)_2F0C).Left;
			((Control)_2F0D).Width = ((Control)this).Width - num4 - ((Control)_2F0D).Left;
			num4 = ((Control)this).Width - ((Control)_2F0E).Right;
			((Control)_2F0E).Left = ((Control)_2F0C).Left;
			((Control)_2F0E).Width = ((Control)this).Width - num4 - ((Control)_2F0E).Left;
			num4 = ((Control)this).Width - ((Control)_2F0F).Right;
			((Control)_2F0F).Left = ((Control)_2F0C).Left;
			((Control)_2F0F).Width = ((Control)this).Width - num4 - ((Control)_2F0F).Left;
			((Control)_2F0C).Visible = false;
		}
		else
		{
			_2F0C.Image = TaskDialog._0EC5(_2F1D.TaskDialogIcon);
		}
		if (_2F1D.CheckBoxCommand != null)
		{
			_2F16.Command = _2F1D.CheckBoxCommand;
			num += ((Control)_2F16).Width + Dpi.Width12;
		}
		else
		{
			((Control)_2F16).Visible = false;
		}
		if (!string.IsNullOrEmpty(_2F1D.FooterText))
		{
			((Control)_2F17).Text = _2F1D.FooterText;
			num2 += ((Control)_2F17).Width + Dpi.Width12;
		}
		else
		{
			((Control)_2F17).Visible = false;
		}
		if (_2F1D.FooterImage != null)
		{
			_2F18.Image = _2F1D.FooterImage;
			num2 += ((Control)_2F18).Width;
		}
		else
		{
			((Control)_2F18).Visible = false;
			((Control)_2F17).Left = ((Control)_2F18).Left;
		}
		if (_2F1D.RadioButtons != null && _2F1D.RadioButtons.Length > 0)
		{
			Command[] radioButtons = _2F1D.RadioButtons;
			foreach (Command command in radioButtons)
			{
				CheckBoxItem checkBoxItem = new CheckBoxItem();
				checkBoxItem.CheckBoxStyle = eCheckBoxStyle.RadioButton;
				checkBoxItem.Command = command;
				_2F0F.Items.Add(checkBoxItem);
			}
		}
		if (_2F1D.Buttons != null && _2F1D.Buttons.Length > 0)
		{
			Command[] buttons = _2F1D.Buttons;
			foreach (Command command2 in buttons)
			{
				ButtonItem buttonItem = new ButtonItem();
				if (command2.Image != null)
				{
					buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
					buttonItem.ImagePosition = eImagePosition.Left;
				}
				buttonItem.Command = command2;
				_2F0F.Items.Add(buttonItem);
				_20B1._20B7(buttonItem);
				num2 = Math.Max(num2, buttonItem.WidthInternal + ((Control)_2F0F).Left * 2);
			}
		}
		if ((_2F1D.DialogButtons & eTaskDialogButton.Ok) == 0)
		{
			((Control)_2F11).Visible = false;
		}
		else
		{
			num += ((Control)_2F11).Width + 3;
			if (_2F1D.DefaultButton == eTaskDialogButton.Ok)
			{
				((Form)this).AcceptButton = (IButtonControl)(object)_2F11;
			}
		}
		if ((_2F1D.DialogButtons & eTaskDialogButton.Cancel) == 0)
		{
			((Control)ẘ).Visible = false;
		}
		else
		{
			num += ((Control)ẘ).Width + 3;
			((Form)this).CancelButton = (IButtonControl)(object)ẘ;
			if (_2F1D.DefaultButton == eTaskDialogButton.Cancel)
			{
				((Form)this).AcceptButton = (IButtonControl)(object)ẘ;
			}
		}
		if ((_2F1D.DialogButtons & eTaskDialogButton.Yes) == 0)
		{
			((Control)_2F15).Visible = false;
		}
		else
		{
			num += ((Control)_2F15).Width + 3;
			if (_2F1D.DefaultButton == eTaskDialogButton.Yes)
			{
				((Form)this).AcceptButton = (IButtonControl)(object)_2F15;
			}
		}
		if ((_2F1D.DialogButtons & eTaskDialogButton.No) == 0)
		{
			((Control)_2F14).Visible = false;
		}
		else
		{
			num += ((Control)_2F14).Width + 3;
			if (((Form)this).CancelButton == null)
			{
				((Form)this).CancelButton = (IButtonControl)(object)_2F14;
			}
			if (_2F1D.DefaultButton == eTaskDialogButton.No)
			{
				((Form)this).AcceptButton = (IButtonControl)(object)_2F14;
			}
		}
		if ((_2F1D.DialogButtons & eTaskDialogButton.Close) == 0)
		{
			((Control)_2F12).Visible = false;
		}
		else
		{
			num += ((Control)_2F12).Width + 3;
			if (((Form)this).CancelButton == null)
			{
				((Form)this).CancelButton = (IButtonControl)(object)_2F12;
			}
			else if (_2F1D.DefaultButton == eTaskDialogButton.Close)
			{
				((Form)this).AcceptButton = (IButtonControl)(object)_2F12;
			}
		}
		if ((_2F1D.DialogButtons & eTaskDialogButton.Retry) == 0)
		{
			((Control)_2F13).Visible = false;
		}
		else
		{
			num += ((Control)_2F13).Width + Dpi.Width3;
			if (_2F1D.DefaultButton == eTaskDialogButton.Retry)
			{
				((Form)this).AcceptButton = (IButtonControl)(object)_2F13;
			}
		}
		if (_2F1D.DialogButtons == eTaskDialogButton.Ok)
		{
			((Form)this).CancelButton = (IButtonControl)(object)_2F11;
		}
		if (string.IsNullOrEmpty(_2F1D.FooterText) && _2F1D.FooterImage == null)
		{
			((Control)_2F19).Visible = false;
			((Control)_2F10).Height = ((Control)_2F1A).Height + Dpi.Height4;
		}
		else
		{
			num3 += ((Control)_2F18).Height;
		}
		((Control)this).Width = Math.Max(Dpi.Width(_2F0A), Math.Max(num2, num));
		Graphics val = _2F0D.CreateGraphics();
		try
		{
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		((Control)_2F0D).MaximumSize = new Size(((Control)_2F0D).Width, 1000);
		((Control)_2F0D).Height = ((Control)_2F0D).GetPreferredSize(Size.Empty).Height;
		num3 += ((Control)_2F0D).Height + Dpi.Height3;
		((Control)_2F0E).Top = ((Control)_2F0D).Bounds.Bottom + Dpi.Height3;
		((Control)_2F0E).MaximumSize = new Size(((Control)_2F0E).Width, 1000);
		Graphics val2 = _2F0E.CreateGraphics();
		try
		{
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		((Control)_2F0E).Height = ((Control)_2F0E).GetPreferredSize(Size.Empty).Height;
		num3 += ((Control)_2F0E).Height + Dpi.Height6;
		if (_2F0E._2287)
		{
			num3 += Dpi.Height8;
		}
		((Control)_2F0F).Top = ((Control)_2F0E).Bottom + Dpi.Height3;
		if (_2F0F.Items.Count == 0)
		{
			((Control)_2F0F).Visible = false;
		}
		else
		{
			Graphics val3 = _2F0F.CreateGraphics();
			try
			{
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			((Control)_2F0F).Height = _2F0F.GetAutoSizeHeight() + Dpi.Height2;
			num3 += ((Control)_2F0F).Height + Dpi.Height6;
			if (string.IsNullOrEmpty(_2F1D.FooterText) && _2F1D.FooterImage == null)
			{
				num3 += Dpi.Height8;
			}
		}
		num3 += ((Control)_2F10).Height;
		((Control)this).Height = Math.Max(Dpi.Height(_2EE2), num3);
		if (_2F1D.TaskDialogIcon == eTaskDialogIcon.Help)
		{
			SystemSounds.Question.Play();
		}
		else if (_2F1D.TaskDialogIcon == eTaskDialogIcon.Information)
		{
			SystemSounds.Asterisk.Play();
		}
		else
		{
			SystemSounds.Exclamation.Play();
		}
		_2F1E();
		((Form)this).ShowDialog(ॡ);
		return __2F0B;
	}

	protected override void OnShown(EventArgs e)
	{
		if (__2F09 == eTaskDialogButton.Cancel && ((Control)ẘ).Visible)
		{
			((Control)ẘ).Focus();
		}
		else if (__2F09 == eTaskDialogButton.Close && ((Control)_2F12).Visible)
		{
			((Control)_2F12).Focus();
		}
		else if (__2F09 == eTaskDialogButton.No && ((Control)_2F14).Visible)
		{
			((Control)_2F14).Focus();
		}
		else if (__2F09 == eTaskDialogButton.Ok && ((Control)_2F11).Visible)
		{
			((Control)_2F11).Focus();
		}
		else if (__2F09 == eTaskDialogButton.Retry && ((Control)_2F13).Visible)
		{
			((Control)_2F13).Focus();
		}
		else if (__2F09 == eTaskDialogButton.Yes && ((Control)_2F15).Visible)
		{
			((Control)_2F15).Focus();
		}
		((Form)this).OnShown(e);
	}

	private void _2F1E()
	{
		((Control)ẘ).Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxCancelButton, ((Control)ẘ).Text);
		((Control)_2F12).Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxCloseButton, ((Control)_2F12).Text);
		((Control)_2F14).Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxNoButton, ((Control)_2F14).Text);
		((Control)_2F11).Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxOkButton, ((Control)_2F11).Text);
		((Control)_2F13).Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxRetryButton, ((Control)_2F13).Text);
		((Control)_2F15).Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxYesButton, ((Control)_2F15).Text);
	}

	private void _2F1F(TaskDialogInfo _2F1D)
	{
		eTaskDialogBackgroundColor eTaskDialogBackgroundColor2 = _2F1D.DialogColor;
		if (StyleManager.IsMetro(StyleManager.Style))
		{
			eTaskDialogBackgroundColor2 = eTaskDialogBackgroundColor.Default;
		}
		switch (eTaskDialogBackgroundColor2)
		{
		case eTaskDialogBackgroundColor.Aqua:
			_2F10.Style.BackColor1.Color = ColorScheme.GetColor(14413555);
			((Control)_2F0D).ForeColor = ColorScheme.GetColor(2119783);
			((Control)_2F0E).ForeColor = ColorScheme.GetColor(2119783);
			break;
		case eTaskDialogBackgroundColor.Blue:
			_2F10.Style.BackColor1.Color = ColorScheme.GetColor(14411249);
			((Control)_2F0D).ForeColor = ColorScheme.GetColor(2375777);
			((Control)_2F0E).ForeColor = ColorScheme.GetColor(2375777);
			break;
		case eTaskDialogBackgroundColor.DarkBlue:
			_2F10.Style.BackColor1.Color = ColorScheme.GetColor(13031920);
			((Control)_2F0D).ForeColor = ColorScheme.GetColor(992318);
			((Control)_2F0E).ForeColor = ColorScheme.GetColor(992318);
			break;
		case eTaskDialogBackgroundColor.OliveGreen:
			_2F10.Style.BackColor1.Color = ColorScheme.GetColor(15462877);
			((Control)_2F0D).ForeColor = ColorScheme.GetColor(5202216);
			((Control)_2F0E).ForeColor = ColorScheme.GetColor(5202216);
			break;
		case eTaskDialogBackgroundColor.Orange:
			_2F10.Style.BackColor1.Color = ColorScheme.GetColor(16640730);
			((Control)_2F0D).ForeColor = ColorScheme.GetColor(9914374);
			((Control)_2F0E).ForeColor = ColorScheme.GetColor(9914374);
			break;
		case eTaskDialogBackgroundColor.Purple:
			_2F10.Style.BackColor1.Color = ColorScheme.GetColor(15065324);
			((Control)_2F0D).ForeColor = ColorScheme.GetColor(4141393);
			((Control)_2F0E).ForeColor = ColorScheme.GetColor(4141393);
			break;
		case eTaskDialogBackgroundColor.Red:
			_2F10.Style.BackColor1.Color = ColorScheme.GetColor(15916251);
			((Control)_2F0D).ForeColor = ColorScheme.GetColor(6497315);
			((Control)_2F0E).ForeColor = ColorScheme.GetColor(6497315);
			break;
		case eTaskDialogBackgroundColor.Silver:
			_2F10.Style.BackColor1.Color = ColorScheme.GetColor(15921906);
			((Control)_2F0D).ForeColor = ColorScheme.GetColor(789516);
			((Control)_2F0E).ForeColor = ColorScheme.GetColor(789516);
			break;
		case eTaskDialogBackgroundColor.Tan:
			_2F10.Style.BackColor1.Color = ColorScheme.GetColor(14539203);
			((Control)_2F0D).ForeColor = ColorScheme.GetColor(1907472);
			((Control)_2F0E).ForeColor = ColorScheme.GetColor(1907472);
			break;
		}
		if (eTaskDialogBackgroundColor2 != 0)
		{
			_2F10.Style.BorderColor.Color = ((Control)_2F0D).ForeColor;
		}
		if (StyleManager.IsMetro(StyleManager.Style))
		{
			((Control)this).BackColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.BackColor;
		}
	}

	private void _2F20(object _٠, EventArgs ؿ)
	{
		_2F26(eTaskDialogResult.Ok);
	}

	private void _2F21(object _٠, EventArgs ؿ)
	{
		_2F26(eTaskDialogResult.Yes);
	}

	private void _2F22(object _٠, EventArgs ؿ)
	{
		_2F26(eTaskDialogResult.No);
	}

	private void _2F23(object _٠, EventArgs ؿ)
	{
		_2F26(eTaskDialogResult.Cancel);
	}

	private void _2F24(object _٠, EventArgs ؿ)
	{
		_2F26(eTaskDialogResult.Retry);
	}

	private void _2F25(object _٠, EventArgs ؿ)
	{
		_2F26(eTaskDialogResult.Close);
	}

	internal void _2F26(eTaskDialogResult _2F27)
	{
		__2F0B = _2F27;
		((Form)this).Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		base.Dispose(disposing);
	}

	private void ـ()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_0917: Unknown result type (might be due to invalid IL or missing references)
		//IL_09de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c30: Unknown result type (might be due to invalid IL or missing references)
		_2F0C = new PictureBox();
		_2F0D = new LabelX();
		_2F0E = new LabelX();
		_2F0F = new ItemPanel();
		_2F10 = new PanelEx();
		_2F17 = new LabelX();
		_2F18 = new PictureBox();
		_2F16 = new CheckBoxX();
		_2F19 = new PanelEx();
		_2F1A = new FlowLayoutPanel();
		_2F12 = new ButtonX();
		_2F13 = new ButtonX();
		ẘ = new ButtonX();
		_2F11 = new ButtonX();
		_2F14 = new ButtonX();
		_2F15 = new ButtonX();
		((ISupportInitialize)_2F0C).BeginInit();
		((Control)_2F10).SuspendLayout();
		((ISupportInitialize)_2F18).BeginInit();
		((Control)_2F1A).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)_2F0C).Location = new Point(12, 12);
		((Control)_2F0C).Name = "headerImage";
		((Control)_2F0C).Size = new Size(32, 32);
		_2F0C.SizeMode = (PictureBoxSizeMode)4;
		_2F0C.TabIndex = 0;
		_2F0C.TabStop = false;
		((Control)_2F0D).Anchor = (AnchorStyles)13;
		_2F0D.BackgroundStyle.Class = "";
		_2F0D.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)_2F0D).Font = new Font("Microsoft Sans Serif", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_2F0D).Location = new Point(50, 12);
		((Control)_2F0D).Name = "headerLabel";
		((Control)_2F0D).Size = new Size(444, 32);
		((Control)_2F0D).TabIndex = 0;
		((Control)_2F0D).Text = "Task Dialog Header Instructions";
		_2F0D.TextLineAlignment = (StringAlignment)0;
		_2F0D.WordWrap = true;
		((Control)_2F0E).Anchor = (AnchorStyles)13;
		_2F0E.BackgroundStyle.Class = "";
		_2F0E.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)_2F0E).Location = new Point(50, 60);
		((Control)_2F0E).Name = "contentLabel";
		((Control)_2F0E).Size = new Size(444, 35);
		((Control)_2F0E).TabIndex = 1;
		((Control)_2F0E).Text = "Task Dialog actual content of the dialog with some instructions and more text with text-markup support and word wrapping.";
		_2F0E.TextLineAlignment = (StringAlignment)0;
		_2F0E.WordWrap = true;
		((Control)_2F0F).Anchor = (AnchorStyles)13;
		_2F0F.BackgroundStyle.Class = "";
		_2F0F.BackgroundStyle.CornerType = eCornerType.Square;
		_2F0F.ContainerControlProcessDialogKey = true;
		_2F0F.LayoutOrientation = eOrientation.Vertical;
		((Control)_2F0F).Location = new Point(50, 101);
		((Control)_2F0F).Name = "buttonsPanel";
		((Control)_2F0F).Size = new Size(444, 120);
		((Control)_2F0F).TabIndex = 2;
		((Control)_2F0F).Text = "itemPanel1";
		_2F10.CanvasColor = SystemColors.Control;
		_2F10.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)_2F10).Controls.Add((Control)(object)_2F17);
		((Control)_2F10).Controls.Add((Control)(object)_2F18);
		((Control)_2F10).Controls.Add((Control)(object)_2F16);
		((Control)_2F10).Controls.Add((Control)(object)_2F19);
		((Control)_2F10).Controls.Add((Control)(object)_2F1A);
		((Control)_2F10).Dock = (DockStyle)2;
		((Control)_2F10).Location = new Point(0, 240);
		((Control)_2F10).Name = "bottomPanel";
		((Control)_2F10).Padding = new Padding(3, 3, 3, 0);
		((Control)_2F10).Size = new Size(506, 70);
		_2F10.Style.Alignment = (StringAlignment)1;
		_2F10.Style.BackColor1.Color = Color.FromArgb(227, 239, 255);
		_2F10.Style.Border = eBorderType.SingleLine;
		_2F10.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		_2F10.Style.BorderSide = eBorderSide.Top;
		_2F10.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		_2F10.Style.GradientAngle = 90;
		((Control)_2F10).TabIndex = 0;
		((Control)_2F17).Anchor = (AnchorStyles)13;
		_2F17.BackgroundStyle.Class = "";
		_2F17.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)_2F17).Location = new Point(33, 49);
		((Control)_2F17).Name = "footerLabel";
		((Control)_2F17).Size = new Size(467, 15);
		((Control)_2F17).TabIndex = 2;
		((Control)_2F17).Text = "Footer Label with text-markup support";
		((Control)_2F18).Location = new Point(8, 48);
		((Control)_2F18).Name = "footerImage";
		((Control)_2F18).Size = new Size(16, 16);
		_2F18.TabIndex = 8;
		_2F18.SizeMode = (PictureBoxSizeMode)4;
		_2F18.TabStop = false;
		((Control)_2F16).AutoSize = true;
		_2F16.BackgroundStyle.Class = "";
		_2F16.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)_2F16).Location = new Point(3, 10);
		((Control)_2F16).Name = "taskCheckBox";
		((Control)_2F16).Size = new Size(75, 15);
		_2F16.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)_2F16).TabIndex = 10;
		((Control)_2F16).Text = "Check-box";
		_2F19.CanvasColor = SystemColors.Control;
		_2F19.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)_2F19).Dock = (DockStyle)2;
		((Control)_2F19).Location = new Point(3, 40);
		((Control)_2F19).Name = "footerPanel";
		((Control)_2F19).Size = new Size(500, 30);
		_2F19.Style.Alignment = (StringAlignment)1;
		_2F19.Style.Border = eBorderType.Raised;
		_2F19.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		_2F19.Style.BorderSide = eBorderSide.Top;
		_2F19.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		_2F19.Style.GradientAngle = 90;
		((Control)_2F19).TabIndex = 10;
		((Control)_2F1A).Controls.Add((Control)(object)_2F12);
		((Control)_2F1A).Controls.Add((Control)(object)ẘ);
		((Control)_2F1A).Controls.Add((Control)(object)_2F11);
		((Control)_2F1A).Controls.Add((Control)(object)_2F13);
		((Control)_2F1A).Controls.Add((Control)(object)_2F14);
		((Control)_2F1A).Controls.Add((Control)(object)_2F15);
		((Control)_2F1A).Dock = (DockStyle)1;
		_2F1A.FlowDirection = (FlowDirection)2;
		((Control)_2F1A).Location = new Point(3, 3);
		((Control)_2F1A).Name = "flowLayoutPanel1";
		((Control)_2F1A).Size = new Size(500, 35);
		((Control)_2F1A).TabIndex = 0;
		_2F1A.WrapContents = false;
		((Control)_2F12).AccessibleRole = (AccessibleRole)43;
		_2F12.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)_2F12).Location = new Point(436, 5);
		((Control)_2F12).Margin = new Padding(3, 5, 3, 3);
		((Control)_2F12).Name = "buttonClose";
		((Control)_2F12).Size = new Size(61, 24);
		((Control)_2F12).MinimumSize = new Size(61, 24);
		((Control)_2F12).AutoSize = true;
		_2F12.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)_2F12).TabIndex = 5;
		((Control)_2F12).Text = "&Close";
		((Control)_2F12).Click += _2F25;
		((Control)_2F13).AccessibleRole = (AccessibleRole)43;
		_2F13.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)_2F13).Location = new Point(369, 5);
		((Control)_2F13).Margin = new Padding(3, 5, 3, 3);
		((Control)_2F13).Name = "buttonRetry";
		((Control)_2F13).Size = new Size(61, 24);
		((Control)_2F13).MinimumSize = new Size(61, 24);
		((Control)_2F13).AutoSize = true;
		_2F13.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)_2F13).TabIndex = 4;
		((Control)_2F13).Text = "&Retry";
		((Control)_2F13).Click += _2F24;
		((Control)ẘ).AccessibleRole = (AccessibleRole)43;
		ẘ.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)ẘ).Location = new Point(302, 5);
		((Control)ẘ).Margin = new Padding(3, 5, 3, 3);
		((Control)ẘ).Name = "buttonCancel";
		((Control)ẘ).Size = new Size(61, 24);
		((Control)ẘ).MinimumSize = new Size(61, 24);
		((Control)ẘ).AutoSize = true;
		ẘ.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ẘ).TabIndex = 3;
		((Control)ẘ).Text = "&Cancel";
		((Control)ẘ).Click += _2F23;
		((Control)_2F11).AccessibleRole = (AccessibleRole)43;
		_2F11.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)_2F11).Location = new Point(235, 5);
		((Control)_2F11).Margin = new Padding(3, 5, 3, 3);
		((Control)_2F11).Name = "buttonOk";
		((Control)_2F11).Size = new Size(61, 24);
		((Control)_2F11).MinimumSize = new Size(61, 24);
		((Control)_2F11).AutoSize = true;
		_2F11.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)_2F11).TabIndex = 2;
		((Control)_2F11).Text = "&OK";
		((Control)_2F11).Click += _2F20;
		((Control)_2F14).AccessibleRole = (AccessibleRole)43;
		_2F14.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)_2F14).Location = new Point(168, 5);
		((Control)_2F14).Margin = new Padding(3, 5, 3, 3);
		((Control)_2F14).Name = "buttonNo";
		((Control)_2F14).Size = new Size(61, 24);
		((Control)_2F14).MinimumSize = new Size(61, 24);
		((Control)_2F14).AutoSize = true;
		_2F14.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)_2F14).TabIndex = 1;
		((Control)_2F14).Text = "&No";
		((Control)_2F14).Click += _2F22;
		((Control)_2F15).AccessibleRole = (AccessibleRole)43;
		_2F15.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)_2F15).Location = new Point(101, 5);
		((Control)_2F15).Margin = new Padding(3, 5, 3, 3);
		((Control)_2F15).Name = "buttonYes";
		((Control)_2F15).Size = new Size(61, 24);
		((Control)_2F15).MinimumSize = new Size(61, 24);
		((Control)_2F15).AutoSize = true;
		_2F15.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)_2F15).TabIndex = 0;
		((Control)_2F15).Text = "&Yes";
		((Control)_2F15).Click += _2F21;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(96f, 96f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)2;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(506, 310);
		((Control)this).Controls.Add((Control)(object)_2F10);
		((Control)this).Controls.Add((Control)(object)_2F0F);
		((Control)this).Controls.Add((Control)(object)_2F0E);
		((Control)this).Controls.Add((Control)(object)_2F0D);
		((Control)this).Controls.Add((Control)(object)_2F0C);
		((Control)this).DoubleBuffered = true;
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "TaskDialogForm";
		((Control)this).Text = "DotNetBar Task Dialog";
		((Form)this).Load += _2F1B;
		((ISupportInitialize)_2F0C).EndInit();
		((Control)_2F10).ResumeLayout(false);
		((Control)_2F10).PerformLayout();
		((ISupportInitialize)_2F18).EndInit();
		((Control)_2F1A).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
internal class _2F32 : _0EEE
{
	protected override string ThemeClass => "COMBOBOX";

	public _2F32(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2F33 _070B, _2F34 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2F33 : _༠
{
	public static _2F33 _0F71 = new _2F33(1);

	protected _2F33(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2F34 : _༢
{
	public static _2F34 ཊ = new _2F34(1);

	public static _2F34 _0F77 = new _2F34(2);

	public static _2F34 _0F78 = new _2F34(3);

	public static _2F34 ड़ = new _2F34(4);

	protected _2F34(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2F35 : _0EEE
{
	protected override string ThemeClass => "EDIT";

	public _2F35(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2F36 _070B, _2F39 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2F36 : _༠
{
	public static _2F36 _2F37 = new _2F36(1);

	public static _2F36 _2F38 = new _2F36(2);

	protected _2F36(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2F39 : _༢
{
	public static _2F39 ཊ = new _2F39(1);

	public static _2F39 _0F77 = new _2F39(2);

	public static _2F39 ᒅ = new _2F39(3);

	public static _2F39 ड़ = new _2F39(4);

	public static _2F39 ᒆ = new _2F39(5);

	public static _2F39 _2F3A = new _2F39(6);

	public static _2F39 _2F3B = new _2F39(7);

	protected _2F39(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2F3C : _0EEE
{
	protected override string ThemeClass => "LISTVIEW";

	public _2F3C(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2F3D _070B, _2F43 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2F3D : _༠
{
	public static _2F3D _2F3E = new _2F3D(1);

	public static _2F3D _2F3F = new _2F3D(2);

	public static _2F3D _2F40 = new _2F3D(3);

	public static _2F3D _2F41 = new _2F3D(4);

	public static _2F3D _2F42 = new _2F3D(5);

	protected _2F3D(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2F43 : _༢
{
	public static _2F43 _2F44 = new _2F43(1);

	public static _2F43 _2F45 = new _2F43(1);

	public static _2F43 _2F46 = new _2F43(1);

	public static _2F43 _2F47 = new _2F43(1);

	public static _2F43 _2F48 = new _2F43(1);

	public static _2F43 _2F49 = new _2F43(2);

	public static _2F43 _2F4A = new _2F43(3);

	public static _2F43 _2F4B = new _2F43(4);

	public static _2F43 _2F4C = new _2F43(5);

	protected _2F43(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2F4D : _0EEE
{
	protected override string ThemeClass => "MENU";

	public _2F4D(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2F4E _070B, _2F53 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2F4E : _༠
{
	public static _2F4E _2F4F = new _2F4E(1);

	public static _2F4E _2F50 = new _2F4E(2);

	public static _2F4E _2F51 = new _2F4E(3);

	public static _2F4E _2F52 = new _2F4E(4);

	public static _2F4E ཀྵ = new _2F4E(5);

	public static _2F4E _0F74 = new _2F4E(6);

	protected _2F4E(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2F53 : _༢
{
	public static _2F53 ཊ = new _2F53(1);

	public static _2F53 ᒅ = new _2F53(2);

	public static _2F53 _2F54 = new _2F53(3);

	protected _2F53(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2F55 : _0EEE
{
	protected override string ThemeClass => "MENUBAND";

	public _2F55(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2F56 _070B, _2F58 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2F56 : _༠
{
	public static _2F56 _2F57 = new _2F56(1);

	public static _2F56 _0F74 = new _2F56(2);

	protected _2F56(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2F58 : _༢
{
	public static _2F58 ཊ = new _2F58(1);

	public static _2F58 _0F77 = new _2F58(2);

	public static _2F58 _0F78 = new _2F58(3);

	public static _2F58 ड़ = new _2F58(4);

	public static _2F58 _0F79 = new _2F58(5);

	public static _2F58 _0F7A = new _2F58(6);

	protected _2F58(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2F59 : _0EEE
{
	protected override string ThemeClass => "PAGE";

	public _2F59(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2F5A _070B, _2F5F య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2F5A : _༠
{
	public static _2F5A _2F5B = new _2F5A(1);

	public static _2F5A _2F5C = new _2F5A(2);

	public static _2F5A _2F5D = new _2F5A(3);

	public static _2F5A _2F5E = new _2F5A(4);

	protected _2F5A(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2F5F : _༢
{
	public static _2F5F ཊ = new _2F5F(1);

	public static _2F5F _0F77 = new _2F5F(2);

	public static _2F5F _0F78 = new _2F5F(3);

	public static _2F5F ड़ = new _2F5F(4);

	protected _2F5F(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2F60 : _0EEE
{
	protected override string ThemeClass => "SPIN";

	public _2F60(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2F61 _070B, _2F62 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2F61 : _༠
{
	public static _2F61 _2F5B = new _2F61(1);

	public static _2F61 _2F5C = new _2F61(2);

	public static _2F61 _2F5D = new _2F61(3);

	public static _2F61 _2F5E = new _2F61(4);

	protected _2F61(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2F62 : _༢
{
	public static _2F62 ཊ = new _2F62(1);

	public static _2F62 _0F77 = new _2F62(2);

	public static _2F62 _0F78 = new _2F62(3);

	public static _2F62 ड़ = new _2F62(4);

	protected _2F62(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2F63 : _0EEE
{
	protected override string ThemeClass => "STARTPANEL";

	public _2F63(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2F64 _070B, _2F70 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2F64 : _༠
{
	public static _2F64 _2F65 = new _2F64(1);

	public static _2F64 _2F66 = new _2F64(2);

	public static _2F64 _2F67 = new _2F64(3);

	public static _2F64 _2F68 = new _2F64(4);

	public static _2F64 _2F69 = new _2F64(5);

	public static _2F64 _2F6A = new _2F64(6);

	public static _2F64 _2F6B = new _2F64(7);

	public static _2F64 _2F6C = new _2F64(8);

	public static _2F64 _2F6D = new _2F64(9);

	public static _2F64 _2F6E = new _2F64(10);

	public static _2F64 _2F6F = new _2F64(11);

	protected _2F64(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2F70 : _༢
{
	public static _2F70 _2F71 = new _2F70(0);

	public static _2F70 _2F72 = new _2F70(0);

	public static _2F70 _2F73 = new _2F70(1);

	public static _2F70 _2F74 = new _2F70(2);

	public static _2F70 _2F75 = new _2F70(3);

	public static _2F70 _2F76 = new _2F70(0);

	public static _2F70 _2F77 = new _2F70(0);

	public static _2F70 _2F78 = new _2F70(0);

	public static _2F70 _2F79 = new _2F70(0);

	public static _2F70 _2F7A = new _2F70(0);

	public static _2F70 _2F7B = new _2F70(1);

	public static _2F70 _2F7C = new _2F70(2);

	public static _2F70 _2F7D = new _2F70(3);

	public static _2F70 _2F7E = new _2F70(0);

	public static _2F70 _2F7F = new _2F70(0);

	protected _2F70(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2F80 : _0EEE
{
	protected override string ThemeClass => "STATUS";

	public _2F80(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2F81 _070B, _2F83 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2F81 : _༠
{
	public static _2F81 ᒀ = new _2F81(1);

	public static _2F81 _2F82 = new _2F81(2);

	public static _2F81 ས = new _2F81(3);

	protected _2F81(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2F83 : _༢
{
	public static _2F83 ཊ = new _2F83(0);

	protected _2F83(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2F84 : _0EEE
{
	protected override string ThemeClass => "TASKBAND";

	public _2F84(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2F85 _070B, _2F89 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2F85 : _༠
{
	public static _2F85 _2F86 = new _2F85(1);

	public static _2F85 _2F87 = new _2F85(2);

	public static _2F85 _2F88 = new _2F85(3);

	protected _2F85(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2F89 : _༢
{
	public static _2F89 ཊ = new _2F89(0);

	protected _2F89(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2F8A : _0EEE
{
	protected override string ThemeClass => "TASKBAR";

	public _2F8A(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2F8B _070B, _2F94 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2F8B : _༠
{
	public static _2F8B _2F8C = new _2F8B(1);

	public static _2F8B _2F8D = new _2F8B(2);

	public static _2F8B _2F8E = new _2F8B(3);

	public static _2F8B _2F8F = new _2F8B(4);

	public static _2F8B _2F90 = new _2F8B(5);

	public static _2F8B _2F91 = new _2F8B(6);

	public static _2F8B _2F92 = new _2F8B(7);

	public static _2F8B _2F93 = new _2F8B(8);

	protected _2F8B(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2F94 : _༢
{
	public static _2F94 ཊ = new _2F94(0);

	protected _2F94(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2F95 : _0EEE
{
	protected override string ThemeClass => "TOOLTIP";

	public _2F95(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2F96 _070B, _2F9B య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2F96 : _༠
{
	public static _2F96 _2F97 = new _2F96(1);

	public static _2F96 _2F98 = new _2F96(2);

	public static _2F96 _2F99 = new _2F96(3);

	public static _2F96 _2F9A = new _2F96(4);

	public static _2F96 ᆤ = new _2F96(5);

	protected _2F96(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2F9B : _༢
{
	public static _2F9B _2F9C = new _2F9B(1);

	public static _2F9B _2F9D = new _2F9B(2);

	public static _2F9B _2F9E = new _2F9B(1);

	public static _2F9B _2F9F = new _2F9B(2);

	public static _2F9B _2FA0 = new _2F9B(1);

	public static _2F9B _2FA1 = new _2F9B(2);

	public static _2F9B _2FA2 = new _2F9B(3);

	protected _2F9B(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2FA3 : _0EEE
{
	protected override string ThemeClass => "TRACKBAR";

	public _2FA3(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2FA4 _070B, _2FAE య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2FA4 : _༠
{
	public static _2FA4 ᘴ = new _2FA4(1);

	public static _2FA4 _2FA5 = new _2FA4(2);

	public static _2FA4 _2FA6 = new _2FA4(3);

	public static _2FA4 _2FA7 = new _2FA4(4);

	public static _2FA4 _2FA8 = new _2FA4(5);

	public static _2FA4 _2FA9 = new _2FA4(6);

	public static _2FA4 _2FAA = new _2FA4(7);

	public static _2FA4 _2FAB = new _2FA4(8);

	public static _2FA4 _2FAC = new _2FA4(9);

	public static _2FA4 _2FAD = new _2FA4(10);

	protected _2FA4(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2FAE : _༢
{
	public static _2FAE _0FA3 = new _2FAE(1);

	public static _2FAE _2FAF = new _2FAE(1);

	public static _2FAE _0FA7 = new _2FAE(1);

	public static _2FAE _0FA8 = new _2FAE(2);

	public static _2FAE _0FA9 = new _2FAE(3);

	public static _2FAE _2FB0 = new _2FAE(4);

	public static _2FAE _0FAA = new _2FAE(5);

	protected _2FAE(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2FB1 : _0EEE
{
	protected override string ThemeClass => "TRAYNOTIFY";

	public _2FB1(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2FB2 _070B, _2FB4 య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2FB2 : _༠
{
	public static _2FB2 ཥ = new _2FB2(1);

	public static _2FB2 _2FB3 = new _2FB2(2);

	protected _2FB2(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2FB4 : _༢
{
	public static _2FB4 ཊ = new _2FB4(0);

	protected _2FB4(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
internal class _2FB5 : _0EEE
{
	protected override string ThemeClass => "TREEVIEW";

	public _2FB5(Control parent)
		: base(parent)
	{
	}

	public void _0EE5(Graphics _٥, _2FB6 _070B, _2FBA య, Rectangle ल)
	{
		InternalDrawBackground(_٥, _070B, య, ल);
	}
}
internal class _2FB6 : _༠
{
	public static _2FB6 _2FB7 = new _2FB6(1);

	public static _2FB6 _2FB8 = new _2FB6(1);

	public static _2FB6 _2FB9 = new _2FB6(1);

	protected _2FB6(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
internal class _2FBA : _༢
{
	public static _2FBA _2FBB = new _2FBA(0);

	public static _2FBA _2FBC = new _2FBA(1);

	public static _2FBA _2FBD = new _2FBA(2);

	public static _2FBA _2FBE = new _2FBA(1);

	public static _2FBA _2FBF = new _2FBA(2);

	public static _2FBA _2FC0 = new _2FBA(3);

	public static _2FBA _2FC1 = new _2FBA(4);

	public static _2FBA _2FC2 = new _2FBA(5);

	protected _2FBA(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
