using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Drawing.Painters.Table;

internal class StiIndicatorCellPainter
{
	internal static void Draw(Graphics g, RectangleD rect, StiTableElement table, object[] rowValues, string[] columnKeys, StiIndicatorColumn column, double zoom, decimal? value, StiTableElementStyle style, bool drawText = true)
	{
		string cellText = GetCellText(table, column, value);
		Draw(g, rect, table, rowValues, columnKeys, column, zoom, value, style, cellText, drawText);
	}

	internal static void Draw(Graphics g, RectangleD rect, StiTableElement table, object[] rowValues, string[] columnKeys, StiIndicatorColumn column, double zoom, decimal? value, StiTableElementStyle style, string str, bool drawText = true)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		double textWidth = MeasureText(g, zoom, str, table.Font);
		DrawIndicator(g, table, rowValues, columnKeys, column, CalculateIndicatorRect(rect, column, zoom, textWidth, table.IsExporting).ToRectangleF(), value, style);
		if (drawText)
		{
			RectangleD rect2 = CalculateTextRect(rect, column, zoom, textWidth, table.IsExporting);
			Color color = GetColor(column.ForeColor, value, style);
			color = StiTableElementConditionHelper.ProcessForeColor(color, column.Key, table, rowValues, columnKeys);
			FontStyle val = StiTableElementConditionHelper.ProcessFontStyle(table.Font, column.Key, table, rowValues, columnKeys);
			Font val2 = new Font(table.Font, val);
			try
			{
				DrawText(g, rect2, column, zoom, str, val2, color);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}

	private static void DrawIndicator(Graphics g, StiTableElement table, object[] rowValues, string[] columnKeys, StiIndicatorColumn column, RectangleF rect, decimal? value, StiTableElementStyle style)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		double num = 1.0;
		if (!table.IsExporting)
		{
			num = StiScale.Factor;
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)4;
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
				Color defaultColor = (((num3.GetValueOrDefault() > default(decimal)) & num3.HasValue) ? style.CellIndicatorPositive : style.CellIndicatorNegative);
				defaultColor = StiTableElementConditionHelper.ProcessForeColor(defaultColor, column.Key, table, rowValues, columnKeys);
				SolidBrush val = new SolidBrush(defaultColor);
				try
				{
					g.FillPolygon((Brush)(object)val, new PointF[3] { pointF, pointF2, pointF3 });
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				goto IL_0249;
			}
		}
		StiDrawing.FillRectangle(g, style.CellIndicatorNeutral, new RectangleD(rect.X + (rect.Width - num2) / 2f, rect.Y + (rect.Height - 4f) / 2f, num2, num * 5.0).ToRectangleF());
		goto IL_0249;
		IL_0249:
		g.SmoothingMode = smoothingMode;
	}

	internal static void DrawText(Graphics g, RectangleD rect, object textObj, double zoom, string text, Font baseFont, Color color)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		rect.Width -= 2.0;
		StringFormat val = new StringFormat(StringFormat.GenericTypographic);
		try
		{
			Font gdiFont = StiElementConsts.Table.Font.GetGdiFont(zoom, null, baseFont);
			try
			{
				val.LineAlignment = (StringAlignment)1;
				val.Alignment = ToAlignment(textObj);
				val.Trimming = (StringTrimming)0;
				val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 0x1000);
				StiTextDrawing.DrawString(g, text, gdiFont, color, rect, val);
			}
			finally
			{
				((IDisposable)gdiFont)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal static double MeasureText(Graphics g, double zoom, string text, Font baseFont)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return 0.0;
		}
		Font gdiFont = StiElementConsts.Table.Font.GetGdiFont(zoom, null, baseFont);
		try
		{
			return StiTextDrawing.MeasureString(g, text, gdiFont).Width;
		}
		finally
		{
			((IDisposable)gdiFont)?.Dispose();
		}
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
		rect.Inflate(-4.0 * zoom, -4.0 * zoom);
		return rect;
	}

	internal static string GetCellText(StiTableElement table, StiIndicatorColumn column, decimal? value)
	{
		if (!value.HasValue)
		{
			return "";
		}
		return StiTextFormatHelper.FormatAsPercentage(table?.Report, column.TextFormat, value);
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

	private static StringAlignment ToAlignment(object textObj)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (textObj is StringAlignment)
		{
			return (StringAlignment)textObj;
		}
		if (!(textObj is IStiHorAlignment stiHorAlignment))
		{
			return (StringAlignment)1;
		}
		return ToAlignment(stiHorAlignment.HorAlignment);
	}

	internal static StringAlignment ToAlignment(StiHorAlignment value)
	{
		return (StringAlignment)(value switch
		{
			StiHorAlignment.Left => 0, 
			StiHorAlignment.Center => 1, 
			StiHorAlignment.Right => 2, 
			_ => 1, 
		});
	}

	private static StiHorAlignment GetHorAlignment(object textObj)
	{
		if (!(textObj is IStiHorAlignment stiHorAlignment))
		{
			return StiHorAlignment.Right;
		}
		return stiHorAlignment.HorAlignment;
	}
}
