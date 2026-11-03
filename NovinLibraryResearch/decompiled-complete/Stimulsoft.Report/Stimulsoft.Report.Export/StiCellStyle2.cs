using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Export;

public class StiCellStyle2 : ICloneable
{
	public bool AbsolutePosition;

	public Color Color;

	public Font Font;

	public StiTextHorAlignment HorAlignment;

	public StiVertAlignment VertAlignment;

	public StiTextOptions TextOptions;

	public Color TextColor;

	public bool WordWrap;

	public string Format;

	private string internalStyleName;

	private string styleName;

	public string InternalStyleName
	{
		get
		{
			return internalStyleName;
		}
		set
		{
			if (value != null)
			{
				internalStyleName = value.Trim().Replace(" ", "");
			}
		}
	}

	public string StyleName
	{
		get
		{
			if (styleName == null)
			{
				styleName = InternalStyleName;
				if (styleName == null)
				{
					styleName = Guid.NewGuid().ToString().Replace("-", "")
						.Substring(0, 8);
				}
			}
			return styleName;
		}
		set
		{
			styleName = value;
		}
	}

	public object Clone()
	{
		StiCellStyle stiCellStyle = MemberwiseClone() as StiCellStyle;
		if (Font != null)
		{
			ref Font font = ref stiCellStyle.Font;
			object obj = Font.Clone();
			font = (Font)((obj is Font) ? obj : null);
		}
		if (TextOptions != null)
		{
			ref StiTextOptions textOptions = ref stiCellStyle.TextOptions;
			object obj2 = TextOptions.Clone();
			textOptions = (StiTextOptions)((obj2 is StiTextOptions) ? obj2 : null);
		}
		return stiCellStyle;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		StiCellStyle2 stiCellStyle = obj as StiCellStyle2;
		if (Color != stiCellStyle.Color)
		{
			return false;
		}
		if (HorAlignment != stiCellStyle.HorAlignment)
		{
			return false;
		}
		if (VertAlignment != stiCellStyle.VertAlignment)
		{
			return false;
		}
		if (TextColor != stiCellStyle.TextColor)
		{
			return false;
		}
		if (Font.Name != stiCellStyle.Font.Name)
		{
			return false;
		}
		if (Font.Bold != stiCellStyle.Font.Bold)
		{
			return false;
		}
		if (Font.Italic != stiCellStyle.Font.Italic)
		{
			return false;
		}
		if (Font.Strikeout != stiCellStyle.Font.Strikeout)
		{
			return false;
		}
		if (Font.Underline != stiCellStyle.Font.Underline)
		{
			return false;
		}
		if (Font.Size != stiCellStyle.Font.Size)
		{
			return false;
		}
		if (WordWrap != stiCellStyle.WordWrap)
		{
			return false;
		}
		if (TextOptions == null && stiCellStyle.TextOptions != null)
		{
			return false;
		}
		if (TextOptions != null && stiCellStyle.TextOptions == null)
		{
			return false;
		}
		if (TextOptions != null && stiCellStyle.TextOptions != null)
		{
			if (TextOptions.Angle != stiCellStyle.TextOptions.Angle)
			{
				return false;
			}
			if (TextOptions.WordWrap != stiCellStyle.TextOptions.WordWrap)
			{
				return false;
			}
			if (TextOptions.RightToLeft != stiCellStyle.TextOptions.RightToLeft)
			{
				return false;
			}
		}
		if (Format != stiCellStyle.Format)
		{
			return false;
		}
		if (InternalStyleName != stiCellStyle.InternalStyleName)
		{
			return false;
		}
		return true;
	}

	public StiCellStyle2(Color color, Color textColor, Font font, StiTextHorAlignment horAlignment, StiVertAlignment vertAlignment, StiTextOptions textOptions, bool wordWrap, string format)
		: this(color, textColor, font, horAlignment, vertAlignment, textOptions, wordWrap, format, null)
	{
	}//IL_0004: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)


	public StiCellStyle2(Color color, Color textColor, Font font, StiTextHorAlignment horAlignment, StiVertAlignment vertAlignment, StiTextOptions textOptions, bool wordWrap, string format, string styleName)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		Color = color;
		TextColor = textColor;
		Font = font;
		HorAlignment = horAlignment;
		VertAlignment = vertAlignment;
		TextOptions = textOptions;
		WordWrap = wordWrap;
		Format = format;
		InternalStyleName = styleName;
	}
}
