using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Components.Cards;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Visuals.Cards;

internal static class StiCardsCellDrawer
{
	internal static void DataBarsDraw(StiContext context, RectangleD rect, StiCardsElement cards, StiCardsElementStyle style, Color foreColor, StiDataBarsCardsColumn column, double zoom, decimal? value, decimal min, decimal max)
	{
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Expected O, but got Unknown
		string text = StiTextFormatHelper.FormatBasedOnColumnType(cards, column.TextFormat, column, value);
		if (!value.HasValue)
		{
			return;
		}
		decimal? num = value;
		if ((num.GetValueOrDefault() == default(decimal)) & num.HasValue)
		{
			return;
		}
		if (min > 0m)
		{
			min = default(decimal);
		}
		if (max < 0m)
		{
			max = default(decimal);
		}
		RectangleD rectangleD = rect;
		rectangleD.Inflate(-2.0 * zoom, -2.0 * zoom);
		context.FillRectangle(Color.FromArgb(40, style.CellDataBarsPositive), rectangleD.ToRectangle());
		double num2 = (double)max + Math.Abs((double)min);
		double num3 = rectangleD.Width * Math.Abs((double)min) / num2;
		double num4 = rectangleD.Width * (double)max / num2;
		double num5 = rectangleD.Width * Math.Abs((double)value.Value) / num2;
		bool flag = num5 > num4;
		if (flag)
		{
			num5 = num4;
		}
		double x = rectangleD.X;
		num = value;
		rectangleD.X = x + (((num.GetValueOrDefault() < default(decimal)) & num.HasValue) ? (num3 - num5) : num3);
		rectangleD.Width = num5;
		if (rectangleD.Width > 0.0 && rectangleD.Height > 0.0)
		{
			num = value;
			Color color = (((num.GetValueOrDefault() < default(decimal)) & num.HasValue) ? style.CellDataBarsNegative : style.CellDataBarsPositive);
			context.FillRectangle(color, rectangleD.ToRectangle());
			if (flag)
			{
				RectangleD rectangleD2 = new RectangleD(rectangleD.Right - 6.0 * zoom, rectangleD.Y, 6.0 * zoom, rectangleD.Height);
				context.FillRectangle(style.CellDataBarsOverlapped, rectangleD2.ToRectangle());
			}
		}
		Font font = new Font(column.Font.FontFamily, column.Font.Size * (float)zoom);
		SizeF rectSize = context.MeasureString(text, new StiFontGeom(font));
		DrawText(context, text, column.Font, foreColor, column.HorAlignment, rect.ToRectangle(), rectSize, zoom);
	}

	internal static void ColorScaleDraw(StiContext context, RectangleD rect, StiCardsElement cards, StiCardsElementStyle style, Color foreColor, StiColorScaleCardsColumn column, double zoom, decimal? value, decimal min, decimal max)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		string text = StiTextFormatHelper.FormatBasedOnColumnType(cards, column.TextFormat, column, value);
		Color scaleColor = GetScaleColor(StiValueHelper.TryToDecimal(value), min, max, style.CellBackColor, column.MinimumColor, column.MaximumColor);
		context.FillRectangle(scaleColor, rect.ToRectangle());
		Font font = new Font(column.Font.FontFamily, column.Font.Size * (float)zoom);
		SizeF rectSize = context.MeasureString(text, new StiFontGeom(font));
		DrawText(context, text, column.Font, foreColor, column.HorAlignment, rect.ToRectangle(), rectSize, zoom);
	}

	private static Color GetScaleColor(decimal value, decimal min, decimal max, Color baseColor, Color minimumColor, Color maximumColor)
	{
		decimal num = (max - min) / 2m;
		value = Math.Min(value, max);
		value = Math.Max(value, min);
		decimal num2 = default(decimal);
		Color color;
		Color color2;
		if (value > num)
		{
			decimal num3 = max - num;
			if (num3 != 0m)
			{
				num2 = (value - num) / num3;
				color = Color.FromArgb(255, baseColor);
				color2 = Color.FromArgb(255, maximumColor);
			}
			else
			{
				color = Color.LightGray;
				color2 = Color.LightGray;
			}
		}
		else
		{
			decimal num4 = num - min;
			if (num4 != 0m)
			{
				num2 = value / num4;
				color = Color.FromArgb(255, minimumColor);
				color2 = Color.FromArgb(255, baseColor);
			}
			else
			{
				color = Color.LightGray;
				color2 = Color.LightGray;
			}
		}
		decimal val = (decimal)(color2.A - color.A) * num2 + (decimal)color.A;
		decimal val2 = (decimal)(color2.R - color.R) * num2 + (decimal)color.R;
		decimal val3 = (decimal)(color2.G - color.G) * num2 + (decimal)color.G;
		decimal val4 = (decimal)(color2.B - color.B) * num2 + (decimal)color.B;
		val = Math.Min(val, 255m);
		val2 = Math.Min(val2, 255m);
		val3 = Math.Min(val3, 255m);
		val4 = Math.Min(val4, 255m);
		val = Math.Max(val, 0m);
		val2 = Math.Max(val2, 0m);
		val3 = Math.Max(val3, 0m);
		val4 = Math.Max(val4, 0m);
		return Color.FromArgb((byte)val, (byte)val2, (byte)val3, (byte)val4);
	}

	internal static void SparklinesDraw(StiContext context, RectangleD rect, StiSparklinesCardsColumn column, object value, double zoom, StiCardsElementStyle style)
	{
		object[] array = CastToArray(value);
		switch (column.Type)
		{
		case StiSparklinesType.Column:
			StiColumnSparklinesCellPainter.Draw(context, rect, array, column.AllowCustomColors ? column.PositiveColor : style.CellSparkline, column.AllowCustomColors ? column.NegativeColor : style.CellSparkline);
			break;
		case StiSparklinesType.Line:
			StiLineSparklinesCellPainter.Draw(context, rect, array, (float)zoom, column.AllowCustomColors ? column.PositiveColor : style.CellSparkline, showArea: false, column.ShowFirstLastPoints, column.ShowHighLowPoints);
			break;
		case StiSparklinesType.Area:
			StiLineSparklinesCellPainter.Draw(context, rect, array, (float)zoom, column.AllowCustomColors ? column.PositiveColor : style.CellSparkline, showArea: true, column.ShowFirstLastPoints, column.ShowHighLowPoints);
			break;
		case StiSparklinesType.WinLoss:
			StiWinLossSparklinesCellPainter.Draw(context, rect, array, column.AllowCustomColors ? column.PositiveColor : style.CellWinLossPositive, column.AllowCustomColors ? column.NegativeColor : style.CellWinLossNegative);
			break;
		default:
			throw new StiTypeNotRecognizedException(column.Type);
		}
	}

	private static object[] CastToArray(object value)
	{
		if (!(value is IEnumerable))
		{
			return null;
		}
		return ((IEnumerable)value).Cast<object>().ToArray();
	}

	internal static void BubbleDraw(StiContext context, RectangleD rect, StiCardsElement cards, StiCardsElementStyle style, Color foreColor, StiBubbleCardsColumn bubbleColumn, double zoom, decimal value, decimal min, decimal max, bool drawText = true)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		if (!(value == 0m))
		{
			string text = StiTextFormatHelper.FormatBasedOnColumnType(cards, bubbleColumn.TextFormat, bubbleColumn, value);
			if (min > 0m)
			{
				min = default(decimal);
			}
			if (max < 0m)
			{
				max = default(decimal);
			}
			StiHorAlignment horAlignment = bubbleColumn.HorAlignment;
			RectangleD rect2 = rect;
			rect2.Inflate(-StiScale.I4, -StiScale.I4);
			rect2 = CalculateBubbleRect(rect2, bubbleColumn, Math.Abs(value), min, max);
			if (drawText && horAlignment != StiHorAlignment.Center)
			{
				StiHorAlignment horAlignment2 = ((bubbleColumn.HorAlignment != StiHorAlignment.Left) ? StiHorAlignment.Left : StiHorAlignment.Right);
				Font font = new Font(bubbleColumn.Font.FontFamily, bubbleColumn.Font.Size * (float)zoom);
				SizeF rectSize = context.MeasureString(text, new StiFontGeom(font));
				DrawText(context, text, bubbleColumn.Font, foreColor, horAlignment2, rect.ToRectangle(), rectSize, zoom);
			}
			if (rect2.Width > 0.0 && rect2.Height > 0.0)
			{
				Color color = (bubbleColumn.AllowCustomColors ? bubbleColumn.PositiveColor : style.CellDataBarsPositive);
				Color color2 = (bubbleColumn.AllowCustomColors ? bubbleColumn.NegativeColor : style.CellDataBarsNegative);
				Color color3 = ((value < 0m) ? color2 : color);
				context.FillEllipse(color3, (float)rect2.X, (float)rect2.Y, (float)rect2.Width, (float)rect2.Height);
			}
		}
	}

	private static RectangleD CalculateBubbleRect(RectangleD rect, object textObj, decimal value, decimal min, decimal max)
	{
		if (!(textObj is IStiHorAlignment stiHorAlignment))
		{
			return rect;
		}
		double num = rect.Height - 2.0;
		decimal num2 = Math.Max(Math.Abs(min), Math.Abs(max));
		double num3 = (double)Math.Abs((decimal)num * value / num2);
		return stiHorAlignment.HorAlignment switch
		{
			StiHorAlignment.Right => new RectangleD(rect.Right - num + (num - num3) / 2.0, rect.Y + (rect.Height - num3) / 2.0, num3, num3), 
			StiHorAlignment.Center => new RectangleD(rect.X + (rect.Width - num3) / 2.0, rect.Y + (rect.Height - num3) / 2.0, num3, num3), 
			StiHorAlignment.Left => new RectangleD(rect.X + (num - num3) / 2.0, rect.Y + (rect.Height - num3) / 2.0, num3, num3), 
			_ => rect, 
		};
	}

	internal static void IndicatorDraw(StiContext context, RectangleD rect, StiCardsElement cards, StiCardsElementStyle style, StiIndicatorCardsColumn column, double zoom, decimal? value)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		string cellText = GetCellText(cards, column, value);
		StiFontGeom fontGeom = StiCardsVisual.GetFontGeom(column.Font, (float)zoom);
		float width = context.MeasureString(cellText, fontGeom).Width;
		DrawIndicator(context, CalculateIndicatorRect(rect, column, zoom, width, cards.IsExporting).ToRectangleF(), value, style, cards.IsExporting);
		RectangleD rectangleD = CalculateTextRect(rect, column, zoom, width, cards.IsExporting);
		Color color = GetColor(column.ForeColor, value, style);
		Font font = new Font(column.Font.FontFamily, column.Font.Size * (float)zoom);
		SizeF rectSize = context.MeasureString(cellText, new StiFontGeom(font));
		DrawText(context, cellText, column.Font, color, column.HorAlignment, rectangleD.ToRectangle(), rectSize, zoom);
	}

	internal static RectangleD CalculateTextRect(RectangleD rect, object column, double zoom, double textWidth, bool isExporting)
	{
		double num = CalculateIndicatorRect(rect, column, zoom, textWidth, isExporting).Width * 2.0;
		return GetHorAlignment(column) switch
		{
			StiHorAlignment.Left => new RectangleD(rect.X + num, rect.Y, rect.Width - num, rect.Height), 
			StiHorAlignment.Right => new RectangleD(rect.X, rect.Y, rect.Width - num, rect.Height), 
			StiHorAlignment.Center => new RectangleD(rect.X, rect.Y, rect.Width, rect.Height), 
			_ => rect, 
		};
	}

	internal static Color GetColor(Color foreColor, decimal? value, IStiCellIndicatorStyle style)
	{
		if (foreColor != Color.Transparent)
		{
			return foreColor;
		}
		decimal? num = value;
		if ((num.GetValueOrDefault() > default(decimal)) & num.HasValue)
		{
			return style.CellIndicatorPositive;
		}
		num = value;
		if ((num.GetValueOrDefault() < default(decimal)) & num.HasValue)
		{
			return style.CellIndicatorNegative;
		}
		return style.CellIndicatorNeutral;
	}

	internal static RectangleD CalculateIndicatorRect(RectangleD rect, object column, double zoom, double textWidth, bool isExporting)
	{
		if (!isExporting)
		{
			zoom *= StiScale.Factor;
		}
		double num = 16.0 * zoom;
		switch (GetHorAlignment(column))
		{
		case StiHorAlignment.Left:
			rect = new RectangleD(rect.X, rect.Y, num, rect.Height);
			break;
		case StiHorAlignment.Right:
			rect = new RectangleD(rect.Right - num, rect.Y, num, rect.Height);
			break;
		case StiHorAlignment.Center:
		{
			double num2 = textWidth + num;
			rect = new RectangleD(rect.Right - (rect.Width - num2) / 2.0 - num, rect.Y, num, rect.Height);
			break;
		}
		}
		rect.Inflate(-2.0 * zoom, -2.0 * zoom);
		return rect;
	}

	private static StiHorAlignment GetHorAlignment(object textObj)
	{
		if (!(textObj is IStiHorAlignment stiHorAlignment))
		{
			return StiHorAlignment.Right;
		}
		return stiHorAlignment.HorAlignment;
	}

	internal static string GetCellText(StiCardsElement cards, StiIndicatorCardsColumn column, decimal? value)
	{
		if (!value.HasValue)
		{
			return "";
		}
		return StiTextFormatHelper.FormatAsPercentage(cards?.Report, column.TextFormat, value);
	}

	private static void DrawIndicator(StiContext context, RectangleF rect, decimal? value, StiCardsElementStyle style, bool isExporting)
	{
		double num = 1.0;
		if (!isExporting)
		{
			num = StiScale.Factor;
		}
		float num2 = Math.Min(rect.Width, rect.Height);
		if (value.HasValue)
		{
			decimal? num3 = value;
			if (!((num3.GetValueOrDefault() == default(decimal)) & num3.HasValue))
			{
				float num4 = num2 * 0.75f;
				rect = new RectangleF(rect.X + (rect.Width - num2) / 2f, rect.Y + (rect.Height - num4) / 2f, num2, num4);
				float num5 = rect.Width / 2f;
				num3 = value;
				PointF pointF;
				PointF pointF2;
				PointF pointF3;
				if ((num3.GetValueOrDefault() > default(decimal)) & num3.HasValue)
				{
					pointF = new PointF(rect.Left + num5, rect.Top);
					pointF2 = new PointF(rect.Left, rect.Bottom);
					pointF3 = new PointF(rect.Right, rect.Bottom);
				}
				else
				{
					pointF = new PointF(rect.Left + num5, rect.Bottom);
					pointF2 = new PointF(rect.Left, rect.Top);
					pointF3 = new PointF(rect.Right, rect.Top);
				}
				num3 = value;
				Color color = (((num3.GetValueOrDefault() > default(decimal)) & num3.HasValue) ? style.CellIndicatorPositive : style.CellIndicatorNegative);
				List<StiSegmentGeom> list = new List<StiSegmentGeom>();
				StiLineSegmentGeom item = new StiLineSegmentGeom(pointF, pointF2);
				StiLineSegmentGeom item2 = new StiLineSegmentGeom(pointF2, pointF3);
				StiLineSegmentGeom item3 = new StiLineSegmentGeom(pointF3, pointF);
				list.Add(item);
				list.Add(item2);
				list.Add(item3);
				context.FillPath(color, list, rect);
				return;
			}
		}
		context.FillRectangle(style.CellIndicatorNeutral, new RectangleD(rect.X + (rect.Width - num2) / 2f, rect.Y + (rect.Height - 4f) / 2f, num2, num * 5.0).ToRectangle());
	}

	internal static void DrawText(StiContext context, string text, Font font, Color color, StiHorAlignment horAlignment, Rectangle rect, SizeF rectSize, double zoom)
	{
		StiStringFormatGeom stringFormatGeom = StiStringFormatHelper.GetStringFormatGeom(context);
		stringFormatGeom.Alignment = (StringAlignment)2;
		RectangleF rect2 = RectangleF.Empty;
		switch (horAlignment)
		{
		case StiHorAlignment.Left:
			rect2 = new RectangleF(rect.X, rect.Y, rectSize.Width, rect.Height);
			break;
		case StiHorAlignment.Center:
			rect2 = new RectangleF((float)(rect.X + rect.Width / 2) - rectSize.Width / 2f, rect.Y, rectSize.Width, rect.Height);
			break;
		case StiHorAlignment.Right:
			rect2 = new RectangleF((float)(rect.X + rect.Width) - rectSize.Width, rect.Y, rectSize.Width, rect.Height);
			break;
		}
		StiFontGeom fontGeom = StiCardsVisual.GetFontGeom(font, (float)zoom);
		string measureText = GetMeasureText(context, text, fontGeom, rect.Width);
		context.DrawRotatedString(measureText, fontGeom, color, rect2, stringFormatGeom, StiRotationMode.CenterCenter, 0f, antialiasing: true);
	}

	internal static string GetMeasureText(StiContext context, string text, StiFontGeom font, double maxWidth)
	{
		if (!string.IsNullOrEmpty(text))
		{
			for (int i = 0; text.Length > i; i++)
			{
				string text2 = text.Substring(0, i);
				if (maxWidth < (double)context.MeasureString(text2, font).Width)
				{
					if (text2.Length > 4)
					{
						return text2.Substring(0, text2.Length - 3) + "...";
					}
					if (text2.Length > 1)
					{
						return text2.Substring(0, 1) + "...";
					}
				}
			}
		}
		return text;
	}
}
