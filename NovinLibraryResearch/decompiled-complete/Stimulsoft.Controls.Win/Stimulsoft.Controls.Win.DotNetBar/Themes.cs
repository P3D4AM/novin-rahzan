using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal abstract class Themes : IDisposable
{
	public struct RECT
	{
		private int left;

		private int top;

		private int right;

		private int bottom;

		public RECT(Rectangle r)
		{
			left = r.Left;
			top = r.Top;
			right = r.Right;
			bottom = r.Bottom;
		}

		public void Offset(int x, int y)
		{
			left += x;
			top += y;
		}
	}

	public struct MARGINS
	{
		private int cxLeftWidth;

		private int cxRightWidth;

		private int cyTopHeight;

		private int cyBottomHeight;
	}

	public struct SIZE
	{
		public int Width;

		public int Height;
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

	public struct DTTOPTS
	{
		public int dwSize;

		public int dwFlags;

		public COLORREF crText;

		public COLORREF crBorder;

		public COLORREF crShadow;

		public int iTextShadowType;

		public WinApi.POINT ptShadowOffset;

		public int iBorderSize;

		public int iFontPropId;

		public int iColorPropId;

		public int iStateId;

		public bool fApplyOverlay;

		public int iGlowSize;

		public int pfnDrawTextCallback;

		public int lParam;
	}

	protected Control m_Parent;

	protected IntPtr m_hTheme = IntPtr.Zero;

	public static bool ThemesActive = _IsThemeActive();

	protected abstract string ThemeClass { get; }

	public static int DrawShadowText(Graphics g, Rectangle bounds, string text, ThemeTextFormat format, Color textColor, Color shadowColor, int shadowOffsetX, int shadowOffsetY)
	{
		int num = 0;
		IntPtr hdc = g.GetHdc();
		try
		{
			RECT r = new RECT(bounds);
			return DrawShadowText(hdc, text, (uint)text.Length, ref r, (int)format, new COLORREF(textColor), new COLORREF(shadowColor), shadowOffsetX, shadowOffsetY);
		}
		finally
		{
			g.ReleaseHdc(hdc);
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

	public Themes(Control parent)
	{
		m_Parent = parent;
		m_hTheme = OpenThemeData(parent.Handle, ThemeClass);
	}

	~Themes()
	{
		if (m_hTheme != IntPtr.Zero)
		{
			Dispose();
		}
	}

	private static bool _IsThemeActive()
	{
		if (!BarFunctions.ThemedOS)
		{
			return false;
		}
		return IsThemeActive();
	}

	public static void RefreshIsThemeActive()
	{
		ThemesActive = _IsThemeActive();
	}

	public static int SetWindowTheme(Control ctrl, string sTheme)
	{
		return SetWindowTheme(ctrl.Handle, IntPtr.Zero, sTheme);
	}

	protected virtual void InternalDrawBackground(Graphics g, ThemePart part, ThemeState state, Rectangle r)
	{
		RECT pRect = new RECT(r);
		IntPtr hdc = g.GetHdc();
		try
		{
			int num = DrawThemeBackground(m_hTheme, hdc, part.Value, state.Value, ref pRect, IntPtr.Zero);
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
	}

	protected virtual void InternalDrawBackground(Graphics g, ThemePart part, ThemeState state, Rectangle r, Rectangle clip)
	{
		RECT pRect = new RECT(r);
		RECT pClipRect = new RECT(clip);
		IntPtr hdc = g.GetHdc();
		try
		{
			int num = DrawThemeBackground(m_hTheme, hdc, part.Value, state.Value, ref pRect, ref pClipRect);
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
	}

	protected virtual IntPtr InternalGetThemeBackgroundRegion(Graphics g, ThemePart part, ThemeState state, Rectangle r)
	{
		RECT pRect = new RECT(r);
		IntPtr hdc = g.GetHdc();
		IntPtr pRegion = IntPtr.Zero;
		try
		{
			GetThemeBackgroundRegion(m_hTheme, hdc, part.Value, state.Value, ref pRect, ref pRegion);
			return pRegion;
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
	}

	protected virtual void InternalDrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemePart part, ThemeState state, ThemeTextFormat format, bool drawdisabled)
	{
		RECT pRect = new RECT(layoutRect);
		IntPtr hdc = g.GetHdc();
		IntPtr intPtr = font.ToHfont();
		IntPtr hgdiobj = SelectObject(hdc, intPtr);
		int num = DrawThemeText(m_hTheme, hdc, part.Value, state.Value, text, text.Length, (int)format, drawdisabled ? 1 : 0, ref pRect);
		SelectObject(hdc, hgdiobj);
		WinApi.DeleteObject(intPtr);
		g.ReleaseHdc(hdc);
	}

	protected virtual void InternalDrawTextEx(Graphics g, string text, Font font, Rectangle layoutRect, ThemePart part, ThemeState state, ThemeTextFormat format, DTTOPTS options)
	{
		RECT pRect = new RECT(layoutRect);
		IntPtr hdc = g.GetHdc();
		IntPtr intPtr = font.ToHfont();
		IntPtr hgdiobj = SelectObject(hdc, intPtr);
		options.dwSize = Marshal.SizeOf(options);
		int num = DrawThemeTextEx(m_hTheme, hdc, part.Value, state.Value, text, text.Length, (int)format, ref pRect, ref options);
		SelectObject(hdc, hgdiobj);
		WinApi.DeleteObject(intPtr);
		g.ReleaseHdc(hdc);
	}

	public virtual bool IsPartDefined(ThemePart part, ThemeState state)
	{
		return IsThemePartDefined(m_hTheme, part.Value, state.Value);
	}

	public void Dispose()
	{
		if (m_hTheme != IntPtr.Zero)
		{
			CloseThemeData(m_hTheme);
			m_hTheme = IntPtr.Zero;
		}
	}

	public virtual Size ThemeMinSize(Graphics g, ThemePart part, ThemeState state)
	{
		SIZE psz = default(SIZE);
		IntPtr hdc = g.GetHdc();
		try
		{
			int themePartSize = GetThemePartSize(m_hTheme, hdc, part.Value, state.Value, IntPtr.Zero, 0, ref psz);
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
		return new Size(psz.Width, psz.Height);
	}

	public virtual Size ThemeTrueSize(Graphics g, ThemePart part, ThemeState state)
	{
		SIZE psz = default(SIZE);
		IntPtr hdc = g.GetHdc();
		try
		{
			int themePartSize = GetThemePartSize(m_hTheme, hdc, part.Value, state.Value, IntPtr.Zero, 1, ref psz);
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
		return new Size(psz.Width, psz.Height);
	}

	public virtual Size ThemeDrawSize(Graphics g, ThemePart part, ThemeState state)
	{
		SIZE psz = default(SIZE);
		IntPtr hdc = g.GetHdc();
		try
		{
			int themePartSize = GetThemePartSize(m_hTheme, hdc, part.Value, state.Value, IntPtr.Zero, 2, ref psz);
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
		return new Size(psz.Width, psz.Height);
	}
}
