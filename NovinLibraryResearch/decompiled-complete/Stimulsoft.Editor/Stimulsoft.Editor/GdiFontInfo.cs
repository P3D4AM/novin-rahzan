using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public class GdiFontInfo : FontInfo
{
	private Hashtable chartable;

	private int[] asciTable = new int[256];

	public GdiFontInfo(Font font, IntPtr dc)
		: base(font, dc)
	{
		if (!IsFontMonoSpaced())
		{
			InitChartable();
		}
	}

	protected virtual bool IsFontMonoSpaced()
	{
		return false;
	}

	protected int GetCharWidth(char ch)
	{
		Size size = new Size(0, 0);
		Win32.SelectObject(dc, base.HFont);
		Win32.GetTextExtentPoint32(dc, ch.ToString(), 1, ref size);
		return size.Width;
	}

	protected void InitChartable()
	{
		chartable = new Hashtable();
		Win32.GetCharABCWidths(dc, 0, 255, ref asciTable);
	}

	public int CharWidth(char ch)
	{
		if (IsFontMonoSpaced())
		{
			return base.FontWidth;
		}
		if (ch <= 'ÿ')
		{
			return asciTable[(uint)ch];
		}
		object obj = chartable[ch];
		if (obj == null)
		{
			obj = GetCharWidth(ch);
			chartable.Add(ch, obj);
		}
		return (int)obj;
	}
}
