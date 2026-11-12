using System;
using System.Drawing;

namespace Stimulsoft.Editor;

public class GdiFontInfos : FontInfos
{
	public GdiFontInfos(Font font, IntPtr dc)
		: base(font, dc)
	{
	}

	protected override FontInfo CreateFontInfo(Font font)
	{
		return new GdiFontInfo(font, dc);
	}

	protected override void SelectFontInfo(FontInfo fontInfo)
	{
		Win32.SelectObject(dc, fontInfo.HFont);
	}
}
