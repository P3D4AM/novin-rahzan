using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Drawing.Painters.Table;

internal class StiTableColumnSizeHelper
{
	public int Width { get; set; }

	public int MinWidth { get; set; } = 30;


	public int MaxWidth { get; set; } = 300;


	public int MaxHeight { get; set; }

	public bool WordWrap { get; set; }

	public int FontUniqueCode { get; set; }

	public StiTableColumn Column { get; set; }

	public bool IsMeasurable { get; set; }

	public int GetUniqueCode()
	{
		int num = 0;
		num = (num * 397) ^ Width;
		num = (num * 397) ^ MinWidth;
		num = (num * 397) ^ MaxWidth;
		return (num * 397) ^ WordWrap.GetHashCode();
	}

	public double GetColumnWidth(double width, bool skipDefaultWidth = false)
	{
		if (Width != 0 && !skipDefaultWidth)
		{
			return StiScale.I(Width);
		}
		if (!IsMeasurable)
		{
			return width;
		}
		int num = StiScale.I(MinWidth);
		int num2 = StiScale.I(MaxWidth);
		if (width < (double)num)
		{
			width = num;
		}
		if (MaxWidth != 0 && width > (double)num2)
		{
			width = num2;
		}
		return width;
	}

	public double GetColumnHeight(double height)
	{
		if (!IsMeasurable)
		{
			return height;
		}
		if ((double)MaxHeight < height)
		{
			return MaxHeight;
		}
		return height;
	}

	public static bool IsWordWrap(StiTableColumn column)
	{
		IStiTableColumnSize stiTableColumnSize = column as IStiTableColumnSize;
		if (stiTableColumnSize?.Size == null)
		{
			return false;
		}
		return stiTableColumnSize.Size.WordWrap;
	}

	public StiTableColumnSizeHelper(Font font, StiTableColumn column)
	{
		Column = column;
		IStiTableColumnSize stiTableColumnSize = column as IStiTableColumnSize;
		if (stiTableColumnSize?.Size != null)
		{
			IsMeasurable = true;
			FontUniqueCode = ((object)font).GetHashCode();
			StiTableColumnSize size = stiTableColumnSize.Size;
			Width = size.Width;
			MinWidth = size.MinWidth;
			MaxWidth = size.MaxWidth;
			MaxHeight = (size.WordWrap ? int.MaxValue : ((int)Math.Round(font.GetHeight(), 0) + 1));
			WordWrap = size.WordWrap;
		}
	}
}
