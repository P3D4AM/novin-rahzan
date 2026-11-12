using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public class FontInfos
{
	private Hashtable stylesTable;

	private FontInfo fontInfo;

	private bool isMonoSpaced;

	private int fontHeight;

	private Font font;

	protected IntPtr dc;

	public FontInfo CurrentInfo => fontInfo;

	public bool IsMonoSpaced => isMonoSpaced;

	public int FontHeight => fontHeight;

	public FontInfos(Font font, IntPtr dc)
	{
		this.font = font;
		this.dc = dc;
		stylesTable = new Hashtable();
	}

	~FontInfos()
	{
		stylesTable.Clear();
	}

	private bool UpdateMonoSpaced()
	{
		bool flag = isMonoSpaced;
		int num = fontHeight;
		isMonoSpaced = false;
		fontHeight = 0;
		if (fontInfo != null)
		{
			isMonoSpaced = fontInfo.IsMonoSpaced;
			int fontWidth = fontInfo.FontWidth;
			IDictionaryEnumerator enumerator = stylesTable.GetEnumerator();
			enumerator.Reset();
			while (enumerator.MoveNext())
			{
				if (isMonoSpaced && ((FontInfo)enumerator.Value).FontWidth != fontWidth)
				{
					isMonoSpaced = false;
				}
				fontHeight = Math.Max(fontHeight, ((FontInfo)enumerator.Value).FontHeight);
			}
		}
		if (flag == isMonoSpaced)
		{
			return num != fontHeight;
		}
		return true;
	}

	protected virtual FontInfo CreateFontInfo(Font font)
	{
		return new FontInfo(font, dc);
	}

	protected virtual void SelectFontInfo(FontInfo fontInfo)
	{
	}

	public FontInfo InitStyle(FontStyle style)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		if (font == null)
		{
			return null;
		}
		fontInfo = (FontInfo)stylesTable[style];
		if (fontInfo == null)
		{
			Font val = font;
			if (font.Style != style)
			{
				val = new Font(font.FontFamily, font.Size, style);
			}
			fontInfo = CreateFontInfo(val);
			stylesTable.Add(style, fontInfo);
			UpdateMonoSpaced();
		}
		else
		{
			SelectFontInfo(fontInfo);
		}
		return fontInfo;
	}
}
