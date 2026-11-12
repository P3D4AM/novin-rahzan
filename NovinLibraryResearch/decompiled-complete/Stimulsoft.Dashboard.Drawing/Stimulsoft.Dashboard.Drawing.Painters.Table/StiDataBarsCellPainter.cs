using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Drawing.Painters.Table;

internal class StiDataBarsCellPainter
{
	internal static void Draw(Graphics g, RectangleD rect, StiTableElement table, object[] rowValues, string[] columnKeys, StiTableColumn column, double zoom, decimal value, decimal min, decimal max, bool isInterlaced, bool isSelected, bool drawText = true, StiDataBarsDirection direction = StiDataBarsDirection.LeftToRight, StiDataBarsBrushType brushType = StiDataBarsBrushType.Solid)
	{
		string text = StiTextFormatHelper.FormatBasedOnColumnType(table, column, value);
		Draw(g, rect, value, min, max, zoom, text, table, rowValues, columnKeys, column, isInterlaced, isSelected, drawText, direction, brushType);
	}

	internal static void Draw(Graphics g, RectangleD rect, decimal? value, decimal min, decimal max, double zoom, string text, StiTableElement table, object[] rowValues, string[] columnKeys, StiTableColumn column, bool isInterlaced, bool isSelected, bool drawText = true, StiDataBarsDirection direction = StiDataBarsDirection.LeftToRight, StiDataBarsBrushType brushType = StiDataBarsBrushType.Solid)
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Expected O, but got Unknown
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Expected O, but got Unknown
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
		StiHorAlignment horAlignment = column.HorAlignment;
		RectangleD rect2 = rect;
		rect2.Inflate(-2.0 * zoom, -2.0 * zoom);
		RectangleD rect3 = CalculateTextRect(rect2, column);
		rect2 = CalculateDataBarsRect(rect2, column);
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(table);
		Color foreColor = GetForeColor(table, column, tableStyle, isInterlaced, isSelected);
		foreColor = StiTableElementConditionHelper.ProcessForeColor(foreColor, column.Key, table, rowValues, columnKeys);
		if (drawText)
		{
			FontStyle val = StiTableElementConditionHelper.ProcessFontStyle(table.Font, column.Key, table, rowValues, columnKeys);
			Font val2 = new Font(table.Font, val);
			try
			{
				StiTextCellPainter.Draw(g, rect3, zoom, (object)(StringAlignment)2, text, val2, foreColor, wordWrap: false);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (horAlignment != StiHorAlignment.Center)
		{
			SolidBrush val3 = new SolidBrush(Color.FromArgb(40, tableStyle.CellDataBarsPositive));
			try
			{
				StiDrawing.FillRectangle(g, (Brush)(object)val3, rect2);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		double num2 = (double)max + Math.Abs((double)min);
		double num3 = rect2.Width * Math.Abs((double)min) / num2;
		double num4 = rect2.Width * (double)max / num2;
		double num5 = rect2.Width * Math.Abs((double)value.Value) / num2;
		bool flag = num5 > num4;
		if (flag)
		{
			num5 = num4;
		}
		float num6 = 0f;
		if (direction == StiDataBarsDirection.LeftToRight)
		{
			double x = rect2.X;
			num = value;
			rect2.X = x + (((num.GetValueOrDefault() < default(decimal)) & num.HasValue) ? (num3 - num5) : num3);
			rect2.Width = num5;
		}
		else
		{
			num6 = 180f;
			num = value;
			rect2.X = (((num.GetValueOrDefault() < default(decimal)) & num.HasValue) ? (rect2.Right - num3) : (rect2.Right - num3 - num5));
			rect2.Width = num5;
		}
		num = value;
		if ((num.GetValueOrDefault() < default(decimal)) & num.HasValue)
		{
			num6 += 180f;
		}
		if (!(rect2.Width > 0.0) || !(rect2.Height > 0.0))
		{
			return;
		}
		if (brushType == StiDataBarsBrushType.Gradient)
		{
			RectangleD rectangleD;
			if (direction == StiDataBarsDirection.LeftToRight)
			{
				num = value;
				rectangleD = (((num.GetValueOrDefault() > default(decimal)) & num.HasValue) ? new RectangleD(rect.Left + num3, rect.Top, num4, rect.Height) : new RectangleD(rect.Left, rect.Top, num3, rect.Height));
			}
			else
			{
				num = value;
				rectangleD = (((num.GetValueOrDefault() < default(decimal)) & num.HasValue) ? new RectangleD(rect.Left + num4, rect.Top, num3, rect.Height) : new RectangleD(rect.Left, rect.Top, num4, rect.Height));
			}
			if (rect2.Width > 0.0 && rect2.Width < 1.0)
			{
				rect2.Width = 1.0;
			}
			if (rectangleD.Width > 0.0 && rectangleD.Width < 1.0)
			{
				rectangleD.Width = 1.0;
			}
			if (rectangleD.Width > 0.0 && rect2.Width > 0.0)
			{
				num = value;
				Color color = (((num.GetValueOrDefault() < default(decimal)) & num.HasValue) ? tableStyle.CellDataBarsNegative : tableStyle.CellDataBarsPositive);
				Color color2 = StiColorUtils.Light(color, 200);
				rectangleD.X -= rectangleD.Width * 0.1;
				rectangleD.Width += rectangleD.Width * 0.2;
				LinearGradientBrush val4 = new LinearGradientBrush(rectangleD.ToRectangleF(), color, color2, num6);
				try
				{
					StiDrawing.FillRectangle(g, (Brush)(object)val4, rect2);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
		}
		else
		{
			num = value;
			Color defaultColor = (((num.GetValueOrDefault() < default(decimal)) & num.HasValue) ? tableStyle.CellDataBarsNegative : tableStyle.CellDataBarsPositive);
			defaultColor = StiTableElementConditionHelper.ProcessForeColor(defaultColor, column.Key, table, rowValues, columnKeys);
			SolidBrush val5 = new SolidBrush(defaultColor);
			try
			{
				StiDrawing.FillRectangle(g, (Brush)(object)val5, rect2);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		if (flag)
		{
			SolidBrush val6 = new SolidBrush(tableStyle.CellDataBarsOverlapped);
			try
			{
				RectangleD rect4 = new RectangleD(rect2.Right - 6.0 * zoom, rect2.Y, 6.0 * zoom, rect2.Height);
				StiDrawing.FillRectangle(g, (Brush)(object)val6, rect4);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		GraphicsState val7 = g.Save();
		g.SetClip(rect2.ToRectangleF());
		if (horAlignment == StiHorAlignment.Center)
		{
			StiTextCellPainter.Draw(g, rect3, zoom, (object)(StringAlignment)2, text, table.Font, Color.White, wordWrap: false);
		}
		g.Restore(val7);
	}

	internal static Color GetForeColor(StiTableElement table, StiTableColumn column, StiTableElementStyle style, bool isInterlaced, bool isSelected)
	{
		if (column.ForeColor != Color.Transparent)
		{
			return column.ForeColor;
		}
		Color foreColor = StiDashboardExpressionHelper.GetForeColor(table, table.ForeColor);
		if (foreColor != Color.Transparent)
		{
			return foreColor;
		}
		if (isInterlaced)
		{
			return style.AlternatingCellForeColor;
		}
		if (isSelected)
		{
			return style?.SelectedCellForeColor ?? StiElementConsts.Table.Font.Color;
		}
		return style?.CellForeColor ?? StiElementConsts.Table.Font.Color;
	}

	internal static RectangleD CalculateDataBarsRect(RectangleD rect, object textObj)
	{
		if (!(textObj is IStiHorAlignment { HorAlignment: var horAlignment }))
		{
			return rect;
		}
		return horAlignment switch
		{
			StiHorAlignment.Left => new RectangleD(rect.Right - rect.Width / 2.0, rect.Y, rect.Width / 2.0, rect.Height), 
			StiHorAlignment.Right => new RectangleD(rect.X, rect.Y, rect.Width / 2.0, rect.Height), 
			_ => rect, 
		};
	}

	internal static RectangleD CalculateTextRect(RectangleD rect, object textObj)
	{
		if (textObj is IStiHorAlignment { HorAlignment: var horAlignment })
		{
			switch (horAlignment)
			{
			case StiHorAlignment.Left:
				rect = new RectangleD(rect.X, rect.Y, rect.Width / 2.0, rect.Height);
				break;
			case StiHorAlignment.Right:
				rect = new RectangleD(rect.Right - rect.Width / 2.0, rect.Y, rect.Width / 2.0, rect.Height);
				break;
			}
		}
		rect.Inflate(-2.0, -2.0);
		return rect;
	}
}
