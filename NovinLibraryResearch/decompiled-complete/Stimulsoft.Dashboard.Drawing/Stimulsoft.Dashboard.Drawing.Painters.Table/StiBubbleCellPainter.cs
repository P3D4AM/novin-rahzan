using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Drawing.Painters.Table;

internal class StiBubbleCellPainter
{
	internal static void Draw(Graphics g, RectangleD rect, StiTableElement table, StiTableColumn column, double zoom, decimal value, decimal min, decimal max, bool isInterlaced, bool isSelected, bool drawText = true)
	{
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		if (value == 0m)
		{
			return;
		}
		string text = StiTextFormatHelper.FormatBasedOnColumnType(table, column, value);
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
		RectangleD rect3 = CalculateTextRect(table.Font, rect2, column, table.Zoom);
		rect2 = CalculateBubbleRect(table.Font, rect2, column, Math.Abs(value), min, max, table.Zoom);
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(table);
		Color foreColor = GetForeColor(table, column, tableStyle, isInterlaced, isSelected);
		if (drawText && horAlignment != StiHorAlignment.Center)
		{
			StiTextCellPainter.Draw(g, rect3, zoom, (object)(StringAlignment)2, text, table.Font, foreColor, wordWrap: false);
		}
		if (!(rect2.Width > 0.0) || !(rect2.Height > 0.0))
		{
			return;
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			StiBubbleColumn stiBubbleColumn = column as StiBubbleColumn;
			Color color = (stiBubbleColumn.AllowCustomColors ? stiBubbleColumn.PositiveColor : tableStyle.CellDataBarsPositive);
			Color color2 = (stiBubbleColumn.AllowCustomColors ? stiBubbleColumn.NegativeColor : tableStyle.CellDataBarsNegative);
			SolidBrush val = new SolidBrush((value < 0m) ? color2 : color);
			try
			{
				g.FillEllipse((Brush)(object)val, (float)rect2.X, (float)rect2.Y, (float)rect2.Width, (float)rect2.Height);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
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

	internal static RectangleD CalculateBubbleRect(Font font, RectangleD rect, object textObj, decimal value, decimal min, decimal max, double zoom)
	{
		if (!(textObj is IStiHorAlignment stiHorAlignment))
		{
			return rect;
		}
		int height = StiElementConsts.Table.GetHeight(font, zoom * StiScale.Factor);
		height -= 2;
		decimal num = Math.Max(Math.Abs(min), Math.Abs(max));
		double num2 = (double)Math.Abs((decimal)height * value / num);
		return stiHorAlignment.HorAlignment switch
		{
			StiHorAlignment.Left => new RectangleD(rect.Right - (double)height + ((double)height - num2) / 2.0, rect.Y + (rect.Height - num2) / 2.0, num2, num2), 
			StiHorAlignment.Center => new RectangleD(rect.X + (rect.Width - num2) / 2.0, rect.Y + (rect.Height - num2) / 2.0, num2, num2), 
			StiHorAlignment.Right => new RectangleD(rect.X + ((double)height - num2) / 2.0, rect.Y + (rect.Height - num2) / 2.0, num2, num2), 
			_ => rect, 
		};
	}

	internal static RectangleD CalculateTextRect(Font font, RectangleD rect, object textObj, double zoom)
	{
		if (textObj is IStiHorAlignment stiHorAlignment)
		{
			int height = StiElementConsts.Table.GetHeight(font, zoom * StiScale.Factor);
			switch (stiHorAlignment.HorAlignment)
			{
			case StiHorAlignment.Left:
				rect = new RectangleD(rect.X, rect.Y, rect.Width - (double)height, rect.Height);
				break;
			case StiHorAlignment.Right:
				rect = new RectangleD(rect.X + (double)height, rect.Y, rect.Width - (double)height, rect.Height);
				break;
			}
		}
		rect.Inflate(-2.0, -2.0);
		return rect;
	}

	internal static string GetCellText(StiTableElement table, StiTableColumn column, decimal value)
	{
		return StiTextFormatHelper.FormatBasedOnColumnType(table, column, value);
	}
}
