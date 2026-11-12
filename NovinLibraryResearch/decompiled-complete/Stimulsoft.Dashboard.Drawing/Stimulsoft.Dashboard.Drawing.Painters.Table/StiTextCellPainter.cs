using System;
using System.Drawing;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Drawing.Painters.Table;

internal class StiTextCellPainter
{
	internal static void Draw(Graphics g, RectangleD rect, StiTableElement table, object[] rowValues, string[] columnKeys, StiTableColumn column, object value, double zoom, string text, StiTableElementStyle style, bool isSelected = false, bool? isInterlaced = null)
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		rect.Inflate(-2.0 * zoom, -2.0 * zoom);
		if (column is StiDimensionColumn && ((StiDimensionColumn)column).ShowHyperlink)
		{
			Color defaultColor = ((column.ForeColor != Color.Transparent) ? column.ForeColor : StiColor.Get("165dc8"));
			defaultColor = StiTableElementConditionHelper.ProcessForeColor(defaultColor, column.Key, table, rowValues, columnKeys);
			DrawHyperlink(g, rect, zoom, column, text, table.Font, defaultColor, StiTableColumnSizeHelper.IsWordWrap(column));
			return;
		}
		Color defaultColor2 = GetForeColor(table, column, style, isSelected, isInterlaced);
		decimal num = StiValueHelper.TryToDecimal(value);
		if (num < 0m && StiNegativeColorChecker.IsNegativeInRed(column.TextFormat))
		{
			defaultColor2 = StiOptions.Engine.NegativeColor;
		}
		defaultColor2 = StiTableElementConditionHelper.ProcessForeColor(defaultColor2, column.Key, table, rowValues, columnKeys);
		FontStyle val = StiTableElementConditionHelper.ProcessFontStyle(table.Font, column.Key, table, rowValues, columnKeys);
		Font val2 = new Font(table.Font, val);
		try
		{
			Draw(g, rect, zoom, column, text, val2, defaultColor2, StiTableColumnSizeHelper.IsWordWrap(column));
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private static void DrawHyperlink(Graphics g, RectangleD rect, double zoom, object textObj, string text, Font baseFont, Color foreColor, bool wordWrap)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		Font val = new Font(baseFont.Name, baseFont.SizeInPoints, (FontStyle)4);
		try
		{
			Draw(g, rect, zoom, textObj, text, val, foreColor, wordWrap);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal static void Draw(Graphics g, RectangleD rect, double zoom, object textObj, string text, Font baseFont, Color color, bool wordWrap)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		StringFormat val = new StringFormat(StringFormat.GenericTypographic);
		try
		{
			Font gdiFont = StiElementConsts.Table.Font.GetGdiFont(zoom, null, baseFont);
			try
			{
				val.LineAlignment = (StringAlignment)1;
				val.Alignment = ToAlignment(textObj);
				val.Trimming = (StringTrimming)3;
				val.FormatFlags = (StringFormatFlags)(wordWrap ? 8192 : 4096);
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

	public static Color GetForeColor(StiTableElement table, StiTableColumn column, StiTableElementStyle style, bool isSelected, bool? isInterlaced = null)
	{
		if (isSelected)
		{
			return style?.SelectedCellForeColor ?? StiElementConsts.Table.Font.SelectedColor;
		}
		if (column.ForeColor != Color.Transparent)
		{
			return column.ForeColor;
		}
		if (table.ForeColor != Color.Transparent)
		{
			return table.ForeColor;
		}
		if (style != null)
		{
			if (isInterlaced.HasValue)
			{
				if (!isInterlaced.Value)
				{
					return style.CellForeColor;
				}
				return style.AlternatingCellForeColor;
			}
			return style.CellForeColor;
		}
		return StiElementConsts.Table.Font.Color;
	}

	internal static StringAlignment ToAlignment(object textObj)
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
			_ => throw new StiTypeNotRecognizedException(value), 
		});
	}
}
