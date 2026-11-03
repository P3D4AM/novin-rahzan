using System;
using System.Collections;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Export;

public class StiCellStyle : ICloneable
{
	public StiBorderSide Border;

	public StiBorderSide BorderL;

	public StiBorderSide BorderR;

	public StiBorderSide BorderB;

	public bool AbsolutePosition;

	public bool AllowHtmlTags;

	public Color Color;

	public Font Font;

	public StiTextHorAlignment HorAlignment;

	public StiVertAlignment VertAlignment;

	public StiTextOptions TextOptions;

	public Color TextColor;

	public bool WordWrap;

	public string Format;

	public double LineSpacing = 1.0;

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
				internalStyleName = StiNameValidator.CorrectName(value.Trim());
			}
		}
	}

	public string StyleName
	{
		get
		{
			if (styleName != null)
			{
				return styleName;
			}
			styleName = InternalStyleName ?? Guid.NewGuid().ToString().Replace("-", "")
				.Substring(0, 8);
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
		if (Border != (StiBorderSide)null)
		{
			ref StiBorderSide border = ref stiCellStyle.Border;
			object obj = Border.Clone();
			border = (StiBorderSide)((obj is StiBorderSide) ? obj : null);
		}
		if (BorderL != (StiBorderSide)null)
		{
			ref StiBorderSide borderL = ref stiCellStyle.BorderL;
			object obj2 = BorderL.Clone();
			borderL = (StiBorderSide)((obj2 is StiBorderSide) ? obj2 : null);
		}
		if (BorderR != (StiBorderSide)null)
		{
			ref StiBorderSide borderR = ref stiCellStyle.BorderR;
			object obj3 = BorderR.Clone();
			borderR = (StiBorderSide)((obj3 is StiBorderSide) ? obj3 : null);
		}
		if (BorderB != (StiBorderSide)null)
		{
			ref StiBorderSide borderB = ref stiCellStyle.BorderB;
			object obj4 = BorderB.Clone();
			borderB = (StiBorderSide)((obj4 is StiBorderSide) ? obj4 : null);
		}
		if (Font != null)
		{
			ref Font font = ref stiCellStyle.Font;
			object obj5 = Font.Clone();
			font = (Font)((obj5 is Font) ? obj5 : null);
		}
		if (TextOptions != null)
		{
			ref StiTextOptions textOptions = ref stiCellStyle.TextOptions;
			object obj6 = TextOptions.Clone();
			textOptions = (StiTextOptions)((obj6 is StiTextOptions) ? obj6 : null);
		}
		return stiCellStyle;
	}

	public override int GetHashCode()
	{
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected I4, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected I4, but got Unknown
		int num = ((Border != (StiBorderSide)null) ? ((object)Border).GetHashCode() : 0);
		num = (num * 397) ^ ((BorderL != (StiBorderSide)null) ? ((object)BorderL).GetHashCode() : 0);
		num = (num * 397) ^ ((BorderR != (StiBorderSide)null) ? ((object)BorderR).GetHashCode() : 0);
		num = (num * 397) ^ ((BorderB != (StiBorderSide)null) ? ((object)BorderB).GetHashCode() : 0);
		num = (num * 397) ^ Color.GetHashCode();
		num = (num * 397) ^ ((Font != null) ? ((object)Font).GetHashCode() : 0);
		num = (num * 397) ^ HorAlignment;
		num = (num * 397) ^ VertAlignment;
		num = (num * 397) ^ ((TextOptions != null) ? ((object)TextOptions).GetHashCode() : 0);
		num = (num * 397) ^ TextColor.GetHashCode();
		num = (num * 397) ^ WordWrap.GetHashCode();
		num = (num * 397) ^ ((Format != null) ? Format.GetHashCode() : 0);
		num = (num * 397) ^ ((internalStyleName != null) ? internalStyleName.GetHashCode() : 0);
		num = (num * 397) ^ LineSpacing.GetHashCode();
		return (num * 397) ^ AllowHtmlTags.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		StiCellStyle stiCellStyle = obj as StiCellStyle;
		if (!Color.Equals((object?)stiCellStyle.Color))
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
		if (!TextColor.Equals((object?)stiCellStyle.TextColor))
		{
			return false;
		}
		if (WordWrap != stiCellStyle.WordWrap)
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
		if (Font.Name != stiCellStyle.Font.Name)
		{
			return false;
		}
		if (TextOptions == null)
		{
			if (stiCellStyle.TextOptions != null)
			{
				return false;
			}
		}
		else
		{
			if (stiCellStyle.TextOptions == null)
			{
				return false;
			}
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
		if (Border == (StiBorderSide)null)
		{
			if (stiCellStyle.Border != (StiBorderSide)null)
			{
				return false;
			}
		}
		else
		{
			if (stiCellStyle.Border == (StiBorderSide)null)
			{
				return false;
			}
			if (Border.Color != stiCellStyle.Border.Color)
			{
				return false;
			}
			if (Border.Size != stiCellStyle.Border.Size)
			{
				return false;
			}
			if (Border.Style != stiCellStyle.Border.Style)
			{
				return false;
			}
		}
		if (BorderL == (StiBorderSide)null)
		{
			if (stiCellStyle.BorderL != (StiBorderSide)null)
			{
				return false;
			}
		}
		else
		{
			if (stiCellStyle.BorderL == (StiBorderSide)null)
			{
				return false;
			}
			if (BorderL.Color != stiCellStyle.BorderL.Color)
			{
				return false;
			}
			if (BorderL.Size != stiCellStyle.BorderL.Size)
			{
				return false;
			}
			if (BorderL.Style != stiCellStyle.BorderL.Style)
			{
				return false;
			}
		}
		if (BorderR == (StiBorderSide)null)
		{
			if (stiCellStyle.BorderR != (StiBorderSide)null)
			{
				return false;
			}
		}
		else
		{
			if (stiCellStyle.BorderR == (StiBorderSide)null)
			{
				return false;
			}
			if (BorderR.Color != stiCellStyle.BorderR.Color)
			{
				return false;
			}
			if (BorderR.Size != stiCellStyle.BorderR.Size)
			{
				return false;
			}
			if (BorderR.Style != stiCellStyle.BorderR.Style)
			{
				return false;
			}
		}
		if (BorderB == (StiBorderSide)null)
		{
			if (stiCellStyle.BorderB != (StiBorderSide)null)
			{
				return false;
			}
		}
		else
		{
			if (stiCellStyle.BorderB == (StiBorderSide)null)
			{
				return false;
			}
			if (BorderB.Color != stiCellStyle.BorderB.Color)
			{
				return false;
			}
			if (BorderB.Size != stiCellStyle.BorderB.Size)
			{
				return false;
			}
			if (BorderB.Style != stiCellStyle.BorderB.Style)
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
		if (LineSpacing != stiCellStyle.LineSpacing)
		{
			return false;
		}
		if (AllowHtmlTags != stiCellStyle.AllowHtmlTags)
		{
			return false;
		}
		return true;
	}

	public static StiCellStyle GetStyleFromCache(Color color, Color textColor, Font font, StiTextHorAlignment horAlignment, StiVertAlignment vertAlignment, StiBorderSide border, StiBorderSide borderL, StiBorderSide borderR, StiBorderSide borderB, StiTextOptions textOptions, bool wordWrap, string format, string internalStyleName, double lineSpacing, bool allowHtmlTags, Hashtable hashStyles, ArrayList styles, Hashtable fontsCache, StiCellStyle cellStyle, bool simplyAdd)
	{
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected I4, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected I4, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		if (font != null)
		{
			object obj = fontsCache[font];
			if (obj == null)
			{
				num = (font.Bold ? 1231 : 1237);
				num = (num * 397) ^ (font.Italic ? 1231 : 1237);
				num = (num * 397) ^ (font.Strikeout ? 1231 : 1237);
				num = (num * 397) ^ (font.Underline ? 1231 : 1237);
				num = (num * 397) ^ font.Size.GetHashCode();
				num = (num * 397) ^ font.Name.GetHashCode();
				fontsCache[font] = num;
			}
			else
			{
				num = (int)obj;
			}
		}
		num = (num * 397) ^ ((border != (StiBorderSide)null) ? ((object)border).GetHashCode() : 0);
		num = (num * 397) ^ ((borderL != (StiBorderSide)null) ? ((object)borderL).GetHashCode() : 0);
		num = (num * 397) ^ ((borderR != (StiBorderSide)null) ? ((object)borderR).GetHashCode() : 0);
		num = (num * 397) ^ ((borderB != (StiBorderSide)null) ? ((object)borderB).GetHashCode() : 0);
		num = (num * 397) ^ color.GetHashCode();
		num = (num * 397) ^ horAlignment;
		num = (num * 397) ^ vertAlignment;
		num = (num * 397) ^ (((object)textOptions)?.GetHashCode() ?? 0);
		num = (num * 397) ^ textColor.GetHashCode();
		num = (num * 397) ^ wordWrap.GetHashCode();
		num = (num * 397) ^ (format?.GetHashCode() ?? 0);
		num = (num * 397) ^ (internalStyleName?.GetHashCode() ?? 0);
		num = (num * 397) ^ lineSpacing.GetHashCode();
		num = (num * 397) ^ allowHtmlTags.GetHashCode();
		if (simplyAdd && cellStyle != null)
		{
			hashStyles[num] = cellStyle;
			return cellStyle;
		}
		object obj2 = hashStyles[num];
		if (obj2 == null)
		{
			StiCellStyle stiCellStyle = new StiCellStyle(color, textColor, font, horAlignment, vertAlignment, border, borderL, borderR, borderB, textOptions, wordWrap, format, internalStyleName, lineSpacing, allowHtmlTags);
			if (cellStyle != null)
			{
				stiCellStyle.styleName = cellStyle.styleName;
			}
			hashStyles[num] = stiCellStyle;
			styles.Add(stiCellStyle);
			return stiCellStyle;
		}
		return (StiCellStyle)obj2;
	}

	public StiCellStyle(Color color, Color textColor, Font font, StiTextHorAlignment horAlignment, StiVertAlignment vertAlignment, StiBorderSide border, StiBorderSide borderL, StiBorderSide borderR, StiBorderSide borderB, StiTextOptions textOptions, bool wordWrap, string format, string styleName, double lineSpacing, bool allowHtmlTags)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Color = color;
		TextColor = textColor;
		Font = font;
		HorAlignment = horAlignment;
		VertAlignment = vertAlignment;
		Border = border;
		BorderL = borderL;
		BorderR = borderR;
		BorderB = borderB;
		TextOptions = textOptions;
		WordWrap = wordWrap;
		InternalStyleName = styleName;
		Format = format;
		LineSpacing = lineSpacing;
		AllowHtmlTags = allowHtmlTags;
	}
}
