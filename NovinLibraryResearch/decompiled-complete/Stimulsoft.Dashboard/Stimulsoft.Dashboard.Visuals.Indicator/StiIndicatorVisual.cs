using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Visuals.Indicator;

public class StiIndicatorVisual : StiVisual
{
	private float minSide = StiScale.I20;

	public List<StiIndicatorIteration> Iterations { get; set; }

	public StiIndicatorElement Element { get; }

	public override void Draw(StiContext context, RectangleF rect)
	{
		float num = rect.Width * 0.06f;
		if (num < 4f)
		{
			num = 4f;
		}
		if (num > 12f)
		{
			num = 12f;
		}
		RectangleF rect2 = new RectangleF(rect.X + num / 2f, rect.Y + num / 2f, rect.Width - num, rect.Height - num);
		try
		{
			context.PushClip(rect);
			if (Iterations.Count == 1)
			{
				DrawSingleMode(context, rect, rect2);
			}
			else if (Iterations.Count > 1)
			{
				DrawMultiMode(context, rect2);
			}
			context.PopClip();
		}
		catch
		{
		}
	}

	internal RectangleF GetContentRectangle(RectangleF rect)
	{
		if (Iterations == null || Iterations.Count <= 1)
		{
			return rect;
		}
		float num = rect.Height / (float)Iterations.Count;
		if (num < minSide)
		{
			num = minSide;
		}
		return new RectangleF(rect.X, rect.Y, rect.Width, num * (float)Iterations.Count);
	}

	private void DrawSingleMode(StiContext context, RectangleF rootRect, RectangleF rect)
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Expected O, but got Unknown
		if (Iterations.Count == 0)
		{
			return;
		}
		PointD? mouseOverPoint = StiIndicatorElementMouseOverHelper.GetMouseOverPoint(Element);
		bool hasValue = Iterations[0].Target.HasValue;
		GetRectanglesSingleMode(context, StiTextFormatHelper.Format(Element.Report, Element.TextFormat, Iterations[0].Value), rect, out var rootRectangle, out var variationRectangle, out var seriesTitleRectangle);
		string text = ProcessFontName(Element.Font.FontFamily.Name, Iterations[0]);
		FontStyle val = ProcessFontStyle(Element.Font, Iterations[0]);
		Font val2 = new Font(text, 8f, val);
		Color? color = ProcessBackColor(Iterations[0]);
		Color color2 = ProcessForeColor(Iterations[0]);
		Color iconColor = ProcessGlyphColor(Iterations[0]);
		StiFontIcons value = ProcessIcon(Element.Icon, Iterations[0]);
		StiIconAlignment iconAlignment = ProcessIconAlignment(Iterations[0]);
		byte[] customIcon = ProcessCustomIcon(Iterations[0]);
		if (color.HasValue)
		{
			context.FillRectangle((object)new SolidBrush(color.GetValueOrDefault()), Rectangle.Ceiling(rootRect));
		}
		if (mouseOverPoint.HasValue && rootRect.Contains(mouseOverPoint.Value.ToPointF()))
		{
			Color hotBackColor = StiDashboardStyleHelper.GetHotBackColor(Element);
			context.FillRectangle(hotBackColor, Rectangle.Ceiling(rootRect));
			StiIndicatorElementMouseOverData data = new StiIndicatorElementMouseOverData
			{
				Value = Iterations[0].Value,
				Series = Iterations[0].Series,
				Target = Iterations[0].Target
			};
			StiIndicatorElementMouseOverHelper.SetMouseOverData(Element, data);
		}
		if (StiOptions.Configuration.IsWeb && !Element.IsDesigning)
		{
			Color hotBackColor2 = StiDashboardStyleHelper.GetHotBackColor(Element);
			if (!color.HasValue)
			{
				color = Color.Transparent;
			}
			context.FillRectangle(color.GetValueOrDefault(), hotBackColor2, Rectangle.Ceiling(rootRect), GetInteractionDataGeom(Iterations[0]));
		}
		Color textColor = color2;
		if (Iterations[0].Value < 0m && StiNegativeColorChecker.IsNegativeInRed(Element.TextFormat))
		{
			textColor = StiOptions.Engine.NegativeColor;
		}
		DrawTextIcon(context, StiTextFormatHelper.Format(Element.Report, Element.TextFormat, Iterations[0].Value), val2, StiFontIconsHelper.GetContent(value), textColor, iconColor, rootRectangle, iconAlignment, customIcon);
		if (hasValue)
		{
			decimal variation = GetVariation(Iterations[0]);
			StiIndicatorElementStyle indicatorStyle = StiDashboardStyleHelper.GetIndicatorStyle(Element);
			Color color3 = ((variation >= 0m) ? indicatorStyle.PositiveColor : indicatorStyle.NegativeColor);
			color3 = ProcessTargetIconColor(color3, Iterations[0]);
			StiFontIcons icon = ((variation >= 0m) ? StiFontIcons.ArrowUp : StiFontIcons.ArrowDown);
			icon = ProcessTargetIcon(icon, Iterations[0]);
			StiPercentageFormatService format = new StiPercentageFormatService
			{
				DecimalDigits = 3
			};
			string text2 = StiTextFormatHelper.FormatAsPercentage(Element.Report, format, variation);
			StiIconAlignment iconAlignment2 = ((!(variation == 0m)) ? StiIconAlignment.Right : StiIconAlignment.None);
			StiIconAlignment iconAlignment3 = ProcessTargetIconAlignment(iconAlignment2, Iterations[0]);
			DrawTextIcon(context, text2, val2, StiFontIconsHelper.GetContent(icon), color3, color3, variationRectangle, iconAlignment3, null);
		}
		float? fontSize = null;
		MeasureFontSize(context, seriesTitleRectangle, Iterations[0].Series, val2, ref fontSize);
		if (fontSize.GetValueOrDefault() > 0f)
		{
			Font font = new Font(text, fontSize.GetValueOrDefault() * 0.6f, val);
			StiFontGeom font2 = new StiFontGeom(font);
			context.DrawRotatedString(Iterations[0].Series, font2, color2, Rectangle.Round(seriesTitleRectangle), StiStringFormatHelper.GetStringFormatGeom(context), StiRotationMode.CenterCenter, 0f, antialiasing: true);
		}
	}

	private decimal GetVariation(StiIndicatorIteration iteration)
	{
		decimal value = iteration.Value;
		decimal valueOrDefault = iteration.Target.GetValueOrDefault();
		decimal num = default(decimal);
		if (valueOrDefault != 0m)
		{
			switch (Element.TargetMode)
			{
			case StiTargetMode.Variation:
				num = value / valueOrDefault - 1m;
				if (valueOrDefault < 0m)
				{
					num = -num;
				}
				break;
			case StiTargetMode.Percentage:
				num = value / valueOrDefault;
				break;
			}
		}
		return num;
	}

	private float GetFactorByFontSize(StiContext context, string value, StiIndicatorElement element, RectangleF rect)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		switch (element.FontSizeMode)
		{
		case StiFontSizeMode.Auto:
			return 0.7f;
		case StiFontSizeMode.Value:
		case StiFontSizeMode.Target:
			return context.MeasureString(value, new StiFontGeom(new Font(element.Font.FontFamily.Name, (float)((double)element.Font.Size * StiScale.Factor), element.Font.Style, (GraphicsUnit)3))).Height / rect.Height;
		default:
			return 0f;
		}
	}

	private void GetRectanglesSingleMode(StiContext context, string value, RectangleF rect, out RectangleF rootRectangle, out RectangleF variationRectangle, out RectangleF seriesTitleRectangle)
	{
		bool hasValue = Iterations[0].Target.HasValue;
		bool flag = !string.IsNullOrEmpty(Iterations[0].Series);
		float num = 0.7f;
		float num2 = (1f - num) / 2f;
		float num3 = 1f - num2 - num;
		float y = rect.Y;
		if (!flag && hasValue)
		{
			if (Element.FontSizeMode == StiFontSizeMode.Value)
			{
				num = GetFactorByFontSize(context, value, Element, rect);
				num2 = 1f - num;
			}
			else if (Element.FontSizeMode == StiFontSizeMode.Target)
			{
				num2 = GetFactorByFontSize(context, value, Element, rect);
				num = 1f - num2;
			}
			num3 = 0f;
		}
		else if (flag && !hasValue)
		{
			num2 = 0f;
			num3 = 1f - num;
		}
		else if (!flag && !hasValue)
		{
			if (Element.FontSizeMode != StiFontSizeMode.Value)
			{
				num = 1f;
			}
			else
			{
				num = GetFactorByFontSize(context, value, Element, rect);
				y = rect.Y + (rect.Height - rect.Height * num) / 2f;
			}
			num2 = 0f;
			num3 = 0f;
		}
		rootRectangle = new RectangleF(rect.X, y, rect.Width, rect.Height * num);
		variationRectangle = new RectangleF(rootRectangle.X, rootRectangle.Bottom, rect.Width, rect.Height * num2);
		seriesTitleRectangle = new RectangleF(rootRectangle.X, variationRectangle.Bottom, rect.Width, rect.Height * num3);
	}

	private FontStyle ProcessFontStyle(Font fontText, StiIndicatorIteration iteration)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		bool flag = ProcessFontBold(fontText.Bold, iteration);
		bool flag2 = ProcessFontItalic(fontText.Italic, iteration);
		bool flag3 = ProcessFontUnderline(fontText.Underline, iteration);
		bool flag4 = ProcessFontStrikeout(fontText.Strikeout, iteration);
		FontStyle val = (FontStyle)0;
		if (flag)
		{
			val = (FontStyle)(val ^ 1);
		}
		if (flag2)
		{
			val = (FontStyle)(val ^ 2);
		}
		if (flag3)
		{
			val = (FontStyle)(val ^ 4);
		}
		if (flag4)
		{
			val = (FontStyle)(val ^ 8);
		}
		return val;
	}

	private bool ProcessFontStrikeout(bool strikeout, StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.FontStyleStrikeout) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.Font.Strikeout;
			}
		}
		return strikeout;
	}

	private bool ProcessFontUnderline(bool underline, StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.FontStyleUnderline) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.Font.Underline;
			}
		}
		return underline;
	}

	private bool ProcessFontItalic(bool italic, StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.FontStyleItalic) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.Font.Italic;
			}
		}
		return italic;
	}

	private bool ProcessFontBold(bool bold, StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.FontStyleBold) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.Font.Bold;
			}
		}
		return bold;
	}

	private string ProcessFontName(string fontName, StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.Font) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.Font.Name;
			}
		}
		return fontName;
	}

	private StiFontIcons ProcessIcon(StiFontIcons icon, StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.Icon) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.Icon;
			}
		}
		return icon;
	}

	private StiFontIcons ProcessTargetIcon(StiFontIcons icon, StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.TargetIcon) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.TargetIcon;
			}
		}
		return icon;
	}

	private byte[] ProcessCustomIcon(StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.Icon) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.CustomIcon;
			}
		}
		return Element.CustomIcon;
	}

	private StiIconAlignment ProcessIconAlignment(StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.Icon) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.IconAlignment;
			}
		}
		return Element.IconAlignment;
	}

	private StiIconAlignment ProcessTargetIconAlignment(StiIconAlignment iconAlignment, StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.TargetIcon) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.TargetIconAlignment;
			}
		}
		return iconAlignment;
	}

	private Color? ProcessBackColor(StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.BackColor) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.BackColor;
			}
		}
		return null;
	}

	private Color ProcessForeColor(StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.TextColor) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.TextColor;
			}
		}
		return StiDashboardStyleHelper.GetForeColor(Element);
	}

	private Color ProcessGlyphColor(StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.Icon) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.IconColor;
			}
		}
		return StiDashboardStyleHelper.GetGlyphColor(Element);
	}

	private Color ProcessTargetIconColor(Color color, StiIndicatorIteration iteration)
	{
		foreach (StiIndicatorElementCondition indicatorCondition in Element.IndicatorConditions)
		{
			if ((indicatorCondition.Permissions & StiIndicatorConditionPermissions.TargetIcon) > StiIndicatorConditionPermissions.None && GetConditionResult(iteration, indicatorCondition))
			{
				return indicatorCondition.TargetIconColor;
			}
		}
		return color;
	}

	private bool GetConditionResult(StiIndicatorIteration iteration, StiIndicatorElementCondition condition)
	{
		object obj = null;
		switch (condition.Field)
		{
		case StiIndicatorFieldCondition.Value:
			obj = iteration.Value;
			break;
		case StiIndicatorFieldCondition.Series:
			obj = iteration.Series;
			break;
		case StiIndicatorFieldCondition.Target:
			obj = iteration.Target;
			break;
		case StiIndicatorFieldCondition.Variation:
		{
			decimal variation = GetVariation(iteration);
			StiPercentageFormatService format = new StiPercentageFormatService
			{
				DecimalDigits = 3
			};
			string text = StiTextFormatHelper.FormatAsPercentage(Element?.Report, format, variation);
			double result = 0.0;
			double.TryParse(text.Replace("%", ""), out result);
			obj = result;
			break;
		}
		}
		bool result2 = false;
		object obj2 = GetConditionValue(condition);
		if (obj != null)
		{
			if (condition.Field == StiIndicatorFieldCondition.Value || condition.Field == StiIndicatorFieldCondition.Target || condition.Field == StiIndicatorFieldCondition.Variation)
			{
				obj = Convert.ToDouble(obj);
			}
			IComparable comparable = obj as IComparable;
			if (comparable != null && obj2 != null)
			{
				if (obj.GetType() != obj2.GetType())
				{
					obj = obj.ToString();
					obj2 = obj2.ToString();
					comparable = obj as IComparable;
				}
				try
				{
					switch (condition.Condition)
					{
					case Stimulsoft.Report.Components.StiFilterCondition.EqualTo:
						return comparable.CompareTo(obj2) == 0;
					case Stimulsoft.Report.Components.StiFilterCondition.NotEqualTo:
						return comparable.CompareTo(obj2) != 0;
					case Stimulsoft.Report.Components.StiFilterCondition.GreaterThan:
						return comparable.CompareTo(obj2) == 1;
					case Stimulsoft.Report.Components.StiFilterCondition.GreaterThanOrEqualTo:
						return comparable.CompareTo(obj2) >= 0;
					case Stimulsoft.Report.Components.StiFilterCondition.LessThan:
						return comparable.CompareTo(obj2) == -1;
					case Stimulsoft.Report.Components.StiFilterCondition.LessThanOrEqualTo:
						return comparable.CompareTo(obj2) <= 0;
					case Stimulsoft.Report.Components.StiFilterCondition.Containing:
					{
						string text5 = obj.ToString();
						string value4 = obj2.ToString();
						return text5.IndexOf(value4, StringComparison.InvariantCulture) != -1;
					}
					case Stimulsoft.Report.Components.StiFilterCondition.NotContaining:
					{
						string text4 = obj.ToString();
						string value3 = obj2.ToString();
						return text4.IndexOf(value3, StringComparison.InvariantCulture) == -1;
					}
					case Stimulsoft.Report.Components.StiFilterCondition.BeginningWith:
					{
						string text3 = obj.ToString();
						string value2 = obj2.ToString();
						return text3.StartsWith(value2, StringComparison.InvariantCulture);
					}
					case Stimulsoft.Report.Components.StiFilterCondition.EndingWith:
					{
						string text2 = obj.ToString();
						string value = obj2.ToString();
						return text2.EndsWith(value, StringComparison.InvariantCulture);
					}
					case Stimulsoft.Report.Components.StiFilterCondition.Between:
					case Stimulsoft.Report.Components.StiFilterCondition.NotBetween:
						break;
					}
				}
				catch
				{
				}
			}
		}
		return result2;
	}

	private object GetConditionValue(StiIndicatorElementCondition condition)
	{
		if (string.IsNullOrEmpty(condition.Value))
		{
			return null;
		}
		StiIndicatorFieldCondition field = condition.Field;
		if (field == StiIndicatorFieldCondition.Value || (uint)(field - 2) <= 1u)
		{
			return double.Parse(condition.Value.Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator));
		}
		return condition.Value.ToString();
	}

	private void DrawMultiMode(StiContext context, RectangleF rect)
	{
		if (Iterations.Count != 0)
		{
			List<decimal?> targetValues = GetTargetValues();
			if (targetValues.Any((decimal? e) => e.HasValue))
			{
				DrawMultiModeWithVariation(context, rect, targetValues);
			}
			else
			{
				DrawMultiModePresent(context, rect);
			}
		}
	}

	private void DrawMultiModePresent(StiContext context, RectangleF rect)
	{
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Expected O, but got Unknown
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Expected O, but got Unknown
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Expected O, but got Unknown
		//IL_0765: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Unknown result type (might be due to invalid IL or missing references)
		PointD? mouseOverPoint = StiIndicatorElementMouseOverHelper.GetMouseOverPoint(Element);
		IEnumerable<decimal> source = Iterations.Select((StiIndicatorIteration i) => i.Value);
		decimal num = source.Max();
		decimal num2 = source.Min();
		decimal num3 = num - num2;
		float num4 = rect.Height / (float)Iterations.Count;
		if (num4 < minSide)
		{
			num4 = minSide;
		}
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<StiFontIcons?> list3 = new List<StiFontIcons?>();
		List<StiFontIcons> fontIcons = StiFontIconsHelper.GetFontIcons(Element.IconSet);
		decimal num5 = 1m / (decimal)fontIcons.Count;
		List<StiIndicatorIconRangeValue> list4 = new List<StiIndicatorIconRangeValue>();
		if (Element.IconMode == StiIndicatorIconMode.Auto)
		{
			decimal num6 = 1m / (decimal)fontIcons.Count;
			for (int j = 0; j < fontIcons.Count; j++)
			{
				decimal start = num2 + (decimal)j * num6 * num3;
				decimal end = num2 + (decimal)(1 + j) * num6 * num3;
				list4.Add(new StiIndicatorIconRangeValue(start, end, fontIcons[j]));
			}
		}
		else
		{
			foreach (StiIndicatorIconRange iconRange in Element.IconRanges)
			{
				decimal num7 = StiValueHelper.ParseDecimal(StiReportParser.Parse(iconRange.StartExpression, Element));
				decimal num8 = StiValueHelper.ParseDecimal(StiReportParser.Parse(iconRange.EndExpression, Element));
				if (Element.IconRangeMode == StiIndicatorIconRangeMode.Percentage)
				{
					num7 = num2 + num7 / 100m * num3;
					num8 = num2 + num8 / 100m * num3;
				}
				list4.Add(new StiIndicatorIconRangeValue(num7, num8, iconRange.Icon));
			}
		}
		foreach (StiIndicatorIteration iteration in Iterations)
		{
			list.Add(iteration.Series);
			list2.Add(StiTextFormatHelper.Format(Element.Report, Element.TextFormat, iteration.Value));
			StiIndicatorIconRangeValue stiIndicatorIconRangeValue = list4.Where((StiIndicatorIconRangeValue i) => i.IsBetween(iteration.Value)).FirstOrDefault();
			if (stiIndicatorIconRangeValue != null)
			{
				list3.Add(stiIndicatorIconRangeValue.Icon);
			}
			else
			{
				list3.Add(null);
			}
		}
		Font font = Element.Font;
		float? num9 = null;
		Font val = new Font(font.FontFamily.Name, 1f, font.Style, font.Unit);
		try
		{
			for (int k = 0; k < Iterations.Count; k++)
			{
				string text = list[k] + list2[k] + "Icon";
				RectangleF rect2 = new RectangleF(rect.X, rect.Y, rect.Width, num4);
				float fontSize = StiStringFormatHelper.GetFontSize(context, rect2, text, val);
				if (!num9.HasValue)
				{
					num9 = fontSize;
				}
				else if (num9 > fontSize)
				{
					num9 = fontSize;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		StiStringFormatGeom stringFormatGeom = StiStringFormatHelper.GetStringFormatGeom(context);
		float num10 = Element.Font.Size / 13f * num9.GetValueOrDefault();
		if (num10 > num9.GetValueOrDefault())
		{
			num10 = num9.GetValueOrDefault();
		}
		if (num10 < 1f)
		{
			return;
		}
		float? num11 = null;
		for (int l = 0; l < Iterations.Count; l++)
		{
			Font val2 = new Font(fontFamilyIcons, num10 * 0.6f);
			StiFontGeom font2 = new StiFontGeom(val2.FontFamily, val2.FontFamily.Name, val2.Size, val2.Style, val2.Unit, val2.GdiCharSet, val2.GdiVerticalFont);
			StiFontIcons? fontIcons2 = list3[l];
			if (fontIcons2.HasValue)
			{
				SizeF sizeF = context.MeasureString(StiFontIconsHelper.GetContent(fontIcons2), font2);
				if (!num11.HasValue || num11.GetValueOrDefault() > rect.Right - sizeF.Width)
				{
					num11 = rect.Right - sizeF.Width;
				}
			}
		}
		for (int m = 0; m < Iterations.Count; m++)
		{
			string text2 = ProcessFontName(font.FontFamily.Name, Iterations[m]);
			FontStyle val3 = ProcessFontStyle(font, Iterations[m]);
			Font font3 = new Font(text2, num10, val3);
			StiFontGeom font4 = new StiFontGeom(font3);
			float num12 = rect.Y + num4 * (float)m;
			RectangleF value = new RectangleF(rect.X, num12, rect.Width, num4);
			if (!value.IntersectsWith(rect))
			{
				break;
			}
			Color? color = ProcessBackColor(Iterations[m]);
			if (color.HasValue)
			{
				context.FillRectangle((object)new SolidBrush(color.GetValueOrDefault()), Rectangle.Ceiling(value));
			}
			if (mouseOverPoint.HasValue && value.Contains(mouseOverPoint.Value.ToPointF()))
			{
				Color hotBackColor = StiDashboardStyleHelper.GetHotBackColor(Element);
				context.FillRectangle(hotBackColor, Rectangle.Ceiling(value));
				StiIndicatorElementMouseOverData data = new StiIndicatorElementMouseOverData
				{
					Value = Iterations[m].Value,
					Series = Iterations[m].Series,
					Target = Iterations[m].Target
				};
				StiIndicatorElementMouseOverHelper.SetMouseOverData(Element, data);
			}
			if ((StiOptions.Configuration.IsWeb || StiOptions.Configuration.IsWPF) && !Element.IsDesigning)
			{
				Color hotBackColor2 = StiDashboardStyleHelper.GetHotBackColor(Element);
				if (!color.HasValue)
				{
					color = Color.Transparent;
				}
				context.FillRectangle(color.GetValueOrDefault(), hotBackColor2, Rectangle.Ceiling(value), GetInteractionDataGeom(Iterations[m]));
			}
			PointF pos = new PointF(value.X, value.Y + num4 / 2f);
			Color color2 = ProcessForeColor(Iterations[m]);
			context.DrawRotatedString(list[m], font4, color2, pos, stringFormatGeom, StiRotationMode.LeftCenter, 0f, antialiasing: true);
			Font val4 = new Font(fontFamilyIcons, num10 * 0.6f);
			StiFontGeom font5 = new StiFontGeom(val4.FontFamily, val4.FontFamily.Name, val4.Size, val4.Style, val4.Unit, val4.GdiCharSet, val4.GdiVerticalFont);
			PointF pos2 = new PointF(num11.GetValueOrDefault(), num12 + num4 / 2f);
			Color color3 = ProcessGlyphColor(Iterations[m]);
			StiFontIcons? stiFontIcons = list3[m];
			if (stiFontIcons.HasValue)
			{
				StiFontIcons value2 = ProcessIcon(stiFontIcons.GetValueOrDefault(), Iterations[m]);
				context.DrawRotatedString(StiFontIconsHelper.GetContent(value2), font5, color3, pos2, StiStringFormatHelper.GetStringFormatGeom(context), StiRotationMode.LeftCenter, 0f, antialiasing: true);
			}
			context.DrawRotatedString(pos: new PointF(num11.GetValueOrDefault() - (float)StiScale.I10, num12 + num4 / 2f), text: list2[m], font: font4, brush: color2, sf: stringFormatGeom, mode: StiRotationMode.RightCenter, angle: 0f, antialiasing: true);
		}
	}

	private void DrawMultiModeWithVariation(StiContext context, RectangleF rect, List<decimal?> tagtesValues)
	{
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Expected O, but got Unknown
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		PointD? mouseOverPoint = StiIndicatorElementMouseOverHelper.GetMouseOverPoint(Element);
		float num = rect.Height / (float)Iterations.Count;
		if (num < minSide)
		{
			num = minSide;
		}
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		List<StiFontIcons> list4 = new List<StiFontIcons>();
		List<StiIconAlignment> list5 = new List<StiIconAlignment>();
		for (int i = 0; i < Iterations.Count; i++)
		{
			StiIndicatorIteration stiIndicatorIteration = Iterations[i];
			list.Add(stiIndicatorIteration.Series);
			list2.Add(StiTextFormatHelper.Format(Element.Report, Element.TextFormat, stiIndicatorIteration.Value));
			decimal variation = GetVariation(Iterations[i]);
			StiPercentageFormatService format = new StiPercentageFormatService
			{
				DecimalDigits = 3
			};
			string item = StiTextFormatHelper.FormatAsPercentage(Element?.Report, format, variation);
			StiFontIcons item2 = ((variation >= 0m) ? StiFontIcons.ArrowUp : StiFontIcons.ArrowDown);
			list3.Add(item);
			list4.Add(item2);
			list5.Add((!(variation == 0m)) ? StiIconAlignment.Right : StiIconAlignment.None);
		}
		float? num2 = null;
		Font font = Element.Font;
		Font val = new Font(font.FontFamily.Name, 1f, font.Style, font.Unit);
		try
		{
			for (int j = 0; j < Iterations.Count; j++)
			{
				string text = list[j] + list2[j] + list3[j] + "DeltaIcon";
				RectangleF rect2 = new RectangleF(rect.X, rect.Y, rect.Width, num);
				float fontSize = StiStringFormatHelper.GetFontSize(context, rect2, text, val);
				if (!num2.HasValue)
				{
					num2 = fontSize;
				}
				else if (num2 > fontSize)
				{
					num2 = fontSize;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		StiStringFormatGeom stringFormatGeom = StiStringFormatHelper.GetStringFormatGeom(context);
		float num3 = Element.Font.Size / 13f * num2.GetValueOrDefault();
		if (num3 > num2.GetValueOrDefault())
		{
			num3 = num2.GetValueOrDefault();
		}
		if (num3 < 1f)
		{
			return;
		}
		StiFontGeom font2 = new StiFontGeom(new Font(font.FontFamily.Name, num3, font.Style, font.Unit));
		float? num4 = null;
		float? num5 = null;
		for (int k = 0; k < Iterations.Count; k++)
		{
			Font val2 = new Font(fontFamilyIcons, num3 * 0.6f);
			StiFontGeom font3 = new StiFontGeom(val2.FontFamily, val2.FontFamily.Name, val2.Size, val2.Style, val2.Unit, val2.GdiCharSet, val2.GdiVerticalFont);
			SizeF sizeF = context.MeasureString(StiFontIconsHelper.GetContent(list4[k]), font3);
			if (!num5.HasValue || num5.GetValueOrDefault() > rect.Right - sizeF.Width)
			{
				num5 = rect.Right - sizeF.Width;
			}
			SizeF sizeF2 = context.MeasureString(list3[k], font2);
			if (!num4.HasValue || num4.GetValueOrDefault() > num5.GetValueOrDefault() - sizeF2.Width)
			{
				num4 = num5.GetValueOrDefault() - sizeF2.Width;
			}
		}
		StiIndicatorElementStyle indicatorStyle = StiDashboardStyleHelper.GetIndicatorStyle(Element);
		for (int l = 0; l < Iterations.Count; l++)
		{
			float y = rect.Y + num * (float)l;
			RectangleF value = new RectangleF(rect.X, y, rect.Width, num);
			if (!value.IntersectsWith(rect))
			{
				break;
			}
			Color? color = ProcessBackColor(Iterations[l]);
			if (color.HasValue)
			{
				context.FillRectangle((object)new SolidBrush(color.GetValueOrDefault()), Rectangle.Ceiling(value));
			}
			if (mouseOverPoint.HasValue && value.Contains(mouseOverPoint.Value.ToPointF()))
			{
				Color color2 = ((indicatorStyle is StiCustomIndicatorElementStyle) ? indicatorStyle.HotBackColor : StiDashboardStyleHelper.GetHotBackColor(Element));
				context.FillRectangle(color2, Rectangle.Ceiling(value));
				StiIndicatorElementMouseOverData data = new StiIndicatorElementMouseOverData
				{
					Value = Iterations[l].Value,
					Series = Iterations[l].Series,
					Target = Iterations[l].Target
				};
				StiIndicatorElementMouseOverHelper.SetMouseOverData(Element, data);
			}
			if (StiOptions.Configuration.IsWeb && !Element.IsDesigning)
			{
				Color hotBackColor = StiDashboardStyleHelper.GetHotBackColor(Element);
				if (!color.HasValue)
				{
					color = Color.Transparent;
				}
				context.FillRectangle(color.GetValueOrDefault(), hotBackColor, Rectangle.Ceiling(value), GetInteractionDataGeom(Iterations[l]));
			}
			Color color3 = ProcessForeColor(Iterations[l]);
			string text2 = ProcessFontName(font.FontFamily.Name, Iterations[l]);
			FontStyle val3 = ProcessFontStyle(font, Iterations[l]);
			Font font4 = new Font(text2, num3, val3);
			font2 = new StiFontGeom(font4);
			context.DrawString(rect: Rectangle.Round(new RectangleF(width: context.MeasureString(list[l], font2).Width, x: value.X, y: value.Y, height: num)), text: list[l], font: font2, brush: color3, sf: stringFormatGeom);
			Color color4 = ((list4[l] == StiFontIcons.ArrowUp) ? indicatorStyle.PositiveColor : indicatorStyle.NegativeColor);
			color4 = ProcessTargetIconColor(color4, Iterations[l]);
			Font val4 = new Font(fontFamilyIcons, num3 * 0.6f);
			StiFontGeom font5 = new StiFontGeom(val4.FontFamily, val4.FontFamily.Name, val4.Size, val4.Style, val4.Unit, val4.GdiCharSet, val4.GdiVerticalFont);
			SizeF sizeF3 = context.MeasureString(StiFontIconsHelper.GetContent(list4[l]), font5);
			RectangleF value2 = new RectangleF(num5.GetValueOrDefault(), y, sizeF3.Width, num);
			SizeF sizeF4 = context.MeasureString(list3[l], font2);
			RectangleF value3 = new RectangleF(num5.GetValueOrDefault() - sizeF4.Width, y, sizeF4.Width, num);
			StiFontIcons icon = list4[l];
			icon = ProcessTargetIcon(icon, Iterations[l]);
			context.DrawString(list3[l], font2, color4, Rectangle.Round(value3), stringFormatGeom);
			if (ProcessTargetIconAlignment(list5[l], Iterations[l]) != 0)
			{
				context.DrawString(StiFontIconsHelper.GetContent(icon), font5, color4, Rectangle.Round(value2), StiStringFormatHelper.GetStringFormatGeom(context));
			}
			if (Iterations[l].Value < 0m && StiNegativeColorChecker.IsNegativeInRed(Element.TextFormat))
			{
				color3 = StiOptions.Engine.NegativeColor;
			}
			SizeF sizeF5 = context.MeasureString(list2[l], font2);
			context.DrawString(rect: Rectangle.Round(new RectangleF(num4.GetValueOrDefault() - sizeF5.Width, y, sizeF5.Width, num)), text: list2[l], font: font2, brush: color3, sf: stringFormatGeom);
		}
	}

	private StiInteractionDataGeom GetInteractionDataGeom(StiIndicatorIteration iteration)
	{
		StiIndicatorInteractionData interactionData = new StiIndicatorInteractionData
		{
			Value = (double)iteration.Value,
			SeriesText = iteration.Series,
			Target = iteration.Target
		};
		return new StiInteractionDataGeom
		{
			InteractionToolTip = Element.Interaction.ToolTip,
			InteractionHyperlink = Element.Interaction.Hyperlink,
			InteractionData = interactionData
		};
	}

	private decimal GetMaxValue()
	{
		decimal num = default(decimal);
		foreach (StiIndicatorIteration iteration in Iterations)
		{
			if (num < iteration.Value)
			{
				num = iteration.Value;
			}
		}
		return num;
	}

	public List<decimal?> GetTargetValues()
	{
		List<decimal?> list = new List<decimal?>();
		foreach (StiIndicatorIteration iteration in Iterations)
		{
			list.Add(iteration.Target);
		}
		return list;
	}

	private void DrawTextIcon(StiContext context, string text, Font defaultFont, string icon, Color textColor, Color iconColor, RectangleF rect, StiIconAlignment iconAlignment, byte[] customIcon)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Expected O, but got Unknown
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		float? fontSize = null;
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		RectangleF rectangleF;
		RectangleF rectangleF2;
		if (iconAlignment == StiIconAlignment.None)
		{
			rectangleF = RectangleF.Empty;
			rectangleF2 = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
			MeasureFontSize(context, rectangleF2, text, defaultFont, ref fontSize);
		}
		else if (iconAlignment == StiIconAlignment.Right || iconAlignment == StiIconAlignment.Left)
		{
			MeasureFontSize(context, rect, text, defaultFont, ref fontSize);
			if (fontSize.GetValueOrDefault() <= 0f)
			{
				return;
			}
			Font val = new Font(fontFamilyIcons, fontSize.GetValueOrDefault());
			StiFontGeom font = new StiFontGeom(val.FontFamily, val.FontFamily.Name, val.Size, val.Style, val.Unit, val.GdiCharSet, val.GdiVerticalFont);
			StiFontGeom font2 = new StiFontGeom(new Font(defaultFont.FontFamily.Name, fontSize.GetValueOrDefault(), defaultFont.Style, (GraphicsUnit)3));
			SizeF sizeF = context.MeasureString(text, font2);
			SizeF sizeF2 = context.MeasureString(icon, font);
			float num = ((sizeF.Width + sizeF2.Width > rect.Width) ? (rect.Width / (rect.Width + sizeF2.Width)) : 1f);
			fontSize *= num;
			SizeF sizeF3 = new SizeF(sizeF.Width * num, sizeF.Height * num);
			SizeF sizeF4 = new SizeF(sizeF2.Width * num, sizeF2.Height * num);
			float x = ((iconAlignment == StiIconAlignment.Right) ? (rect.X + (rect.Width - sizeF3.Width - sizeF4.Width) / 2f) : (rect.X + (rect.Width - sizeF3.Width - sizeF4.Width) / 2f + sizeF4.Width));
			float y = rect.Y + (rect.Height - sizeF3.Height) / 2f;
			rectangleF2 = new RectangleF(x, y, sizeF3.Width, sizeF3.Height);
			float x2 = ((iconAlignment == StiIconAlignment.Right) ? rectangleF2.Right : (rectangleF2.X - sizeF4.Width));
			float y2 = rect.Y + (rect.Height - sizeF4.Height) / 2f;
			rectangleF = new RectangleF(x2, y2, sizeF4.Width, sizeF4.Height);
		}
		else
		{
			rectangleF2 = ((iconAlignment == StiIconAlignment.Bottom) ? new RectangleF(rect.X, rect.Y, rect.Width, rect.Height / 2f) : new RectangleF(rect.X, rect.Y + rect.Height / 2f, rect.Width, rect.Height / 2f));
			rectangleF = ((iconAlignment == StiIconAlignment.Bottom) ? new RectangleF(rect.X, rect.Y + rect.Height / 2f, rect.Width, rect.Height / 2f) : new RectangleF(rect.X, rect.Y, rect.Width, rect.Height / 2f));
			MeasureFontSize(context, rectangleF2, text, defaultFont, ref fontSize);
		}
		if (fontSize.GetValueOrDefault() > 0f)
		{
			StiFontGeom font3 = new StiFontGeom(new Font(defaultFont.FontFamily.Name, fontSize.GetValueOrDefault(), defaultFont.Style, (GraphicsUnit)3));
			context.DrawRotatedString(text, font3, textColor, Rectangle.Round(rectangleF2), StiStringFormatHelper.GetStringFormatGeom(context), StiRotationMode.CenterCenter, 0f, antialiasing: true);
		}
		if (fontSize.GetValueOrDefault() > 0f && iconAlignment != 0)
		{
			if (customIcon == null || customIcon.Length == 0)
			{
				Font val2 = new Font(fontFamilyIcons, fontSize.GetValueOrDefault() * 0.7f);
				StiFontGeom font4 = new StiFontGeom(val2.FontFamily, val2.FontFamily.Name, val2.Size, val2.Style, val2.Unit, val2.GdiCharSet, val2.GdiVerticalFont);
				context.DrawRotatedString(icon, font4, iconColor, Rectangle.Round(rectangleF), StiStringFormatHelper.GetStringFormatGeom(context), StiRotationMode.CenterCenter, 0f, antialiasing: true);
			}
			else
			{
				context.DrawImage(customIcon, rectangleF);
			}
		}
	}

	public void MeasureFontSize(StiContext context, RectangleF rect, string text, Font font, ref float? fontSize)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		SizeF sizeF = context.MeasureString(text, new StiFontGeom(new Font(font.FontFamily.Name, 1f, font.Style)));
		if (sizeF.Width == 0f || sizeF.Height == 0f)
		{
			fontSize = 0f;
			return;
		}
		float num = rect.Width / sizeF.Width;
		float num2 = rect.Height / sizeF.Height;
		float num3 = ((num > num2) ? num2 : num);
		if (!fontSize.HasValue)
		{
			fontSize = num3;
		}
		else if (fontSize > num3)
		{
			fontSize = num3;
		}
	}

	protected float GetElementSide(bool isVerticalOrientation, SizeF size)
	{
		float num = (isVerticalOrientation ? size.Height : size.Width);
		float num2 = ((!isVerticalOrientation) ? size.Height : size.Width);
		float num3 = num / (float)Iterations.Count;
		if (num3 > num2)
		{
			num3 = num2;
		}
		return num3;
	}

	public StiIndicatorVisual(StiIndicatorElement element, List<StiIndicatorIteration> iterations)
	{
		Element = element;
		Iterations = iterations;
	}
}
