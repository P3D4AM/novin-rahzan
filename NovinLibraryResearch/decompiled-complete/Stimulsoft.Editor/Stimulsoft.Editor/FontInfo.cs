using System;
using System.Drawing;

namespace Stimulsoft.Editor;

public class FontInfo
{
	private IntPtr hFont;

	private Win32.TextMetrics fontMetrics;

	private bool isMonoSpaced;

	protected IntPtr dc;

	private Font font;

	public Font Font => font;

	public IntPtr HFont => hFont;

	public int FontWidth => fontMetrics.AveCharWidth;

	public int FontHeight => fontMetrics.Height;

	public bool IsMonoSpaced => isMonoSpaced;

	public FontInfo(Font font, IntPtr dc)
	{
		this.font = font;
		hFont = font.ToHfont();
		this.dc = dc;
		Win32.SelectObject(dc, hFont);
		Win32.GetTextMetrics(dc, ref fontMetrics);
		isMonoSpaced = (fontMetrics.PitchAndFamily & 1) == 0;
	}

	~FontInfo()
	{
		if (hFont != IntPtr.Zero)
		{
			Win32.DeleteObject(hFont);
		}
	}
}
