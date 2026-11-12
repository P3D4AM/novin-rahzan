using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Drawing.Painters.Table;

internal class StiColorScaleCellPainter
{
	internal static void Draw(Graphics g, RectangleD rect, StiTableElement table, string[] columnKeys, object[] rowValues, StiTableColumn column, decimal value, double zoom, decimal min, decimal max, StiTableElementStyle style, Color minimumColor, Color maximumColor)
	{
		string text = StiTextFormatHelper.FormatBasedOnColumnType(table, column, value);
		Color scaleColor = GetScaleColor(StiValueHelper.TryToDecimal(value), min, max, style.CellBackColor, minimumColor, maximumColor);
		scaleColor = StiTableElementConditionHelper.ProcessBackColor(scaleColor, column.Key, table, rowValues, columnKeys).GetValueOrDefault();
		StiDrawing.FillRectangle(g, scaleColor, rect);
		StiTextCellPainter.Draw(g, rect, table, rowValues, columnKeys, column, value, zoom, text, style);
	}

	internal static Color GetScaleColor(decimal value, decimal min, decimal max, Color baseColor, Color minimumColor, Color maximumColor)
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
}
