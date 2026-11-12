using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public class Painter
{
	private Hashtable fontTable;

	private Hashtable brushTable;

	private Hashtable penTable;

	private FontInfos fontInfos;

	private IntPtr oldMeasureFont;

	private IntPtr measureHFont;

	private Font font;

	private FontStyle fontStyle;

	private Color textColor;

	private Color backColor;

	private Color foreColor;

	private StringFormat stringFormat;

	private bool opaque = true;

	protected IntPtr measureDC;

	protected FontInfo CurrentInfo
	{
		get
		{
			if (fontInfos == null)
			{
				return null;
			}
			return fontInfos.CurrentInfo;
		}
	}

	protected Hashtable FontTable => fontTable;

	protected Hashtable BrushTable => brushTable;

	protected Hashtable PenTable => penTable;

	public Font Font
	{
		get
		{
			return font;
		}
		set
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (font != value)
			{
				font = value;
				if (value != null)
				{
					SelectFont(font, font.Style);
				}
			}
		}
	}

	public FontStyle FontStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return fontStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (fontStyle != value)
			{
				SelectFont(value);
			}
		}
	}

	public Color TextColor
	{
		get
		{
			return textColor;
		}
		set
		{
			if (textColor != value)
			{
				textColor = value;
				SelectTextColor(value);
			}
		}
	}

	public Color ForeColor
	{
		get
		{
			return foreColor;
		}
		set
		{
			if (foreColor != value)
			{
				foreColor = value;
				SelectPen(foreColor, select: true);
			}
		}
	}

	public Color BackColor
	{
		get
		{
			return backColor;
		}
		set
		{
			if (backColor != value)
			{
				backColor = value;
				SelectBrush(backColor, select: true);
			}
		}
	}

	public bool Opaque
	{
		get
		{
			return opaque;
		}
		set
		{
			if (opaque != value)
			{
				opaque = value;
				SelectOpaque(value);
			}
		}
	}

	public StringFormat StringFormat
	{
		get
		{
			return stringFormat;
		}
		set
		{
			if (stringFormat != value)
			{
				stringFormat = value;
				SelectStringFormat(value);
			}
		}
	}

	public bool IsMonoSpaced => fontInfos.IsMonoSpaced;

	public int FontWidth
	{
		get
		{
			if (CurrentInfo == null)
			{
				return 0;
			}
			return CurrentInfo.FontWidth;
		}
	}

	public int FontHeight => fontInfos.FontHeight;

	public Painter()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		fontTable = new Hashtable();
		brushTable = new Hashtable();
		penTable = new Hashtable();
		IntPtr dC = Win32.GetDC(IntPtr.Zero);
		Font val = new Font(FontFamily.GenericMonospace, 10f);
		try
		{
			measureDC = Win32.CreateCompatibleDC(dC);
			measureHFont = val.ToHfont();
			oldMeasureFont = Win32.SelectObject(measureDC, measureHFont);
		}
		finally
		{
			Win32.ReleaseDC(IntPtr.Zero, dC);
		}
		Init();
	}

	~Painter()
	{
		ClearBrushes();
		ClearPens();
		ClearFonts();
		Win32.SelectObject(measureDC, oldMeasureFont);
		if (measureHFont != IntPtr.Zero)
		{
			Win32.DeleteObject(measureHFont);
		}
		Win32.DeleteDC(measureDC);
	}

	protected void Init()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		Font = new Font(FontFamily.GenericMonospace, 10f);
		TextColor = Consts.DefaultControlForeColor;
		BackColor = Consts.DefaultControlBackColor;
		ForeColor = Consts.DefaultControlForeColor;
		Opaque = true;
		stringFormat = new StringFormat(StringFormat.GenericTypographic);
		stringFormat.Trimming = (StringTrimming)0;
		StringFormat obj = stringFormat;
		obj.FormatFlags = (StringFormatFlags)(obj.FormatFlags | 0x1800);
		SelectStringFormat(stringFormat);
	}

	public virtual void Clear()
	{
		ClearPens();
		ClearBrushes();
		ClearColors();
		ClearFonts();
		font = null;
		fontInfos = null;
		Init();
	}

	protected virtual object GetFontKey(Font font)
	{
		return font.Name + "|" + font.Size;
	}

	protected virtual void ClearPens()
	{
		penTable.Clear();
	}

	protected virtual void ClearBrushes()
	{
		brushTable.Clear();
	}

	protected virtual void ClearColors()
	{
		backColor = Color.Empty;
		textColor = Color.Empty;
		foreColor = Color.Empty;
	}

	protected virtual void ClearFonts()
	{
		fontTable.Clear();
	}

	protected virtual object CreatePen(Color color)
	{
		return null;
	}

	protected virtual object CreateBrush(Color color)
	{
		return null;
	}

	protected virtual void SelectTextColor(Color color)
	{
	}

	protected virtual void SelectStringFormat(StringFormat format)
	{
	}

	protected virtual void SelectPen(Color color, bool select)
	{
	}

	protected virtual void SelectBrush(Color color, bool select)
	{
	}

	protected virtual void SelectOpaque(bool value)
	{
	}

	protected virtual FontInfos CreateFontInfos(Font font)
	{
		return new FontInfos(font, measureDC);
	}

	protected virtual void SelectFont(Font font, FontStyle fontStyle)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (font != null)
		{
			this.font = font;
			this.fontStyle = fontStyle;
			object fontKey = GetFontKey(font);
			fontInfos = (FontInfos)fontTable[fontKey];
			if (fontInfos == null)
			{
				fontInfos = CreateFontInfos(font);
				fontTable.Add(fontKey, fontInfos);
			}
			fontInfos.InitStyle(font.Style);
		}
	}

	protected virtual void SelectFont(FontStyle fontStyle)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		this.fontStyle = fontStyle;
		fontInfos.InitStyle(fontStyle);
		font = ((CurrentInfo != null) ? CurrentInfo.Font : null);
	}

	protected virtual object SelectBrush(Color color)
	{
		object obj = brushTable[color];
		if (obj == null)
		{
			obj = CreateBrush(color);
			brushTable.Add(color, obj);
		}
		return obj;
	}

	protected virtual object SelectPen(Color color)
	{
		object obj = penTable[color];
		if (obj == null)
		{
			obj = CreatePen(color);
			penTable.Add(color, obj);
		}
		return obj;
	}
}
